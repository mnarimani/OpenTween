using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;
#if UNITY_COLLECTIONS
using NativeListInt = Unity.Collections.NativeList<int>;
#else
using NativeListInt = OpenTween.Jobs.NativeList<int>;
#endif
#if UNITASK
using Task = Cysharp.Threading.Tasks.UniTask;
using TaskCompletionSource = Cysharp.Threading.Tasks.AutoResetUniTaskCompletionSource;

#else
using Task = System.Threading.Tasks.Task;
using TaskCompletionSource = System.Threading.Tasks.TaskCompletionSource<bool>;

#endif

namespace OpenTween.Jobs
{
    internal abstract class RegistryBase<TTween, TOptions, TReferences, TInherited>
        where TReferences : ManagedReferences, new()
        where TOptions : struct, IOptionsBaseInternal
        where TTween : struct, ITweenBaseInternal
        where TInherited : RegistryBase<TTween, TOptions, TReferences, TInherited>, new()
    {
        protected NativeArray<TTween> All;
        protected NativeArray<TOptions> AllOptions;
        protected NativeListInt ActiveIndices;
        protected Stack<int> FreeIndices;
        protected TReferences[] References;
        protected JobHandle JobHandle;

        private static TInherited _instance;
        public static TInherited Instance
        {
            get
            {
                _instance ??= new TInherited();
                return _instance;
            }
        }

        protected bool IsInitialized => All.IsCreated;

        protected RegistryBase()
        {
            TweenScheduleMaster.RegisterSchedule(Schedule);
            TweenScheduleMaster.RegisterComplete(Complete);
        }

        protected virtual void Initialize()
        {
            All = new NativeArray<TTween>(OpenTweenSettings.InitialCapacity, Allocator.Persistent);
            AllOptions = new NativeArray<TOptions>(All.Length, Allocator.Persistent);
            ActiveIndices = new NativeListInt(All.Length, Allocator.Persistent);

            FreeIndices = new Stack<int>(All.Length);
            for (int i = 0; i < All.Length; i++)
            {
                FreeIndices.Push(i);
            }

            References = new TReferences[All.Length];

#if UNITY_EDITOR
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainOnDomainUnload;
            EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
#endif
        }

        protected virtual void IncreaseCapacity()
        {
            var newAll = new NativeArray<TTween>(All.Length * 2, Allocator.Persistent);
            new CopyArrayJob<TTween>(All, newAll)
                .Schedule(All.Length, OpenTweenSettings.InnerLoopBatchCount)
                .Complete();
            All.Dispose();
            All = newAll;

            var newOptions = new NativeArray<TOptions>(newAll.Length, Allocator.Persistent);
            new CopyArrayJob<TOptions>(AllOptions, newOptions)
                .Schedule(AllOptions.Length, OpenTweenSettings.InnerLoopBatchCount)
                .Complete();
            AllOptions.Dispose();
            AllOptions = newOptions;

            var newReferences = new TReferences[newAll.Length];
            Array.Copy(References, newReferences, References.Length);
            References = newReferences;

            for (int i = All.Length / 2; i < All.Length; i++)
            {
                FreeIndices.Push(i);
            }
        }

        public virtual int New()
        {
            if (!IsInitialized)
            {
                Initialize();
            }

            if (FreeIndices.Count == 0)
            {
                IncreaseCapacity();
            }

            int index = FreeIndices.Pop();

            ref TTween tween = ref All.GetRef(index);
            tween.ResetToDefaults();
            tween.Index = index;

            ref TOptions options = ref AllOptions.GetRef(index);
            options.ResetToDefaults();

            TReferences refs = References[index] ?? new TReferences();
            refs.ResetToDefaults();
            References[index] = refs;

            ActiveIndices.Add(index);
            return index;
        }

        public ITweenBaseInternal GetByInterface(int index)
        {
            return All[index];
        } 
        
        public ref TTween GetByRef(int index)
        {
            return ref All.GetRef(index);
        }

        public ref TOptions GetOptionsByRef(int index)
        {
            return ref AllOptions.GetRef(index);
        }

        public TReferences GetManagedReferences(int index)
        {
            return References[index];
        }

        protected virtual void Schedule(float dt)
        {
            if (!IsInitialized)
                return;

            for (int i = ActiveIndices.Length - 1; i >= 0; i--)
            {
                int index = ActiveIndices[i];
                ref TTween tween = ref All.GetRef(index);
                ref TOptions options = ref AllOptions.GetRef(index);
                TReferences refs = References[index];

                if (refs.HasBoundComponent && refs.BoundComponent == null)
                {
                    refs.OnDisposing();

                    ActiveIndices.RemoveAtSwapBack(i);
                    FreeIndices.Push(index);
                    
                    refs.Version++;
                    GetByRef(index).Version++;
                    GetOptionsByRef(index).Version++;
                    continue;
                }
                
                if (options.AutoPlay && tween.State == TweenState.NotPlayed)
                {
                    tween.State = TweenState.Running;
                }
            }
        }

        public void Complete()
        {
            if (!ActiveIndices.IsCreated)
                return;

            JobHandle.Complete();

            for (int k = ActiveIndices.Length - 1; k >= 0; k--)
            {
                int index = ActiveIndices[k];
                ref TTween tween = ref All.GetRef(index);
                ref TOptions options = ref AllOptions.GetRef(index);
                TReferences refs = References[index];

                ProcessPostComplete(index, ref tween, ref options, refs);

                if (tween.IsCompletedInLastFrame)
                {
                    refs.OnCompleted();
                    if (options.DisposeOnComplete)
                    {
                        refs.OnDisposing();

                        ActiveIndices.RemoveAtSwapBack(k);
                        FreeIndices.Push(index);
                        refs.Version++;
                        tween.Version++;
                        options.Version++;
                    }
                }

                if (tween.IsRewindCompletedInLastFrame)
                {
                    refs.OnRewindCompleted();
                }

                tween.IsCompletedInLastFrame = false;
                tween.IsUpdatedInLastFrame = false;
                tween.IsRewindCompletedInLastFrame = false;
            }
        }

        protected abstract void ProcessPostComplete(int index, ref TTween tween, ref TOptions options, TReferences refs);

        public void Return(int tweenIndex)
        {
            TReferences refs = References[tweenIndex];
            refs.OnDisposing();

            for (int i = ActiveIndices.Length - 1; i >= 0; i--)
            {
                if (ActiveIndices[i] == tweenIndex)
                    ActiveIndices.RemoveAtSwapBack(i);
            }

            All.GetRef(tweenIndex).Version++;
            AllOptions.GetRef(tweenIndex).Version++;
            refs.Version++;
            FreeIndices.Push(tweenIndex);
        }

#if UNITY_EDITOR

        private void EditorApplicationOnplayModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.ExitingPlayMode && All.IsCreated)
            {
                All.Dispose();
                AllOptions.Dispose();
                ActiveIndices.Dispose();
            }
        }

        private void CurrentDomainOnDomainUnload(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.DomainUnload -= CurrentDomainOnDomainUnload;
            if (All.IsCreated)
            {
                All.Dispose();
                AllOptions.Dispose();
                ActiveIndices.Dispose();
            }
        }
#endif

        public virtual bool Play(int index, bool restart)
        {
            TReferences refs = GetManagedReferences(index);

            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                Return(index);
                return false;
            }

            ref TTween tween = ref GetByRef(index);

            if (restart)
            {
                tween.CurrentTime = 0;
            }
            else
            {
                if (tween.State != TweenState.NotPlayed &&
                    tween.State != TweenState.Paused &&
                    tween.State != TweenState.RewindPaused &&
                    tween.State != TweenState.RewindCompleted)
                {
                    return false;
                }
            }

            tween.State = TweenState.Running;
            refs.OnPlayStarted();
            return true;
        }
        
        public void Rewind(int index, bool restart)
        {
            var refs = GetManagedReferences(index);

            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                Return(index);
                return;
            }

            ref TOptions opt = ref GetOptionsByRef(index);
            ref TTween tween = ref GetByRef(index);

            if (restart)
            {
                tween.CurrentTime = opt.Duration;
            }
            else
            {
                if (tween.State != TweenState.NotPlayed &&
                    tween.State != TweenState.Paused &&
                    tween.State != TweenState.RewindPaused &&
                    tween.State != TweenState.Completed)
                {
                    return;
                }
            }

            tween.State = TweenState.RewindRunning;
            refs.OnRewindStarted();
        }

        public void ForceComplete(int index)
        {
            TReferences refs = GetManagedReferences(index);

            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                Return(index);
                return;
            }

            ref TOptions opt = ref GetOptionsByRef(index);
            ref TTween tween = ref GetByRef(index);

            if (tween.State == TweenState.Running)
            {
                tween.CurrentTime = opt.Duration;
                tween.State = TweenState.Completed;
            }
            else if (tween.State == TweenState.RewindRunning)
            {
                tween.CurrentTime = 0;
                tween.State = TweenState.RewindCompleted;
            }
        }

        public void Pause(int index)
        {
            var refs = GetManagedReferences(index);

            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                Return(index);
                return;
            }

            ref TTween tween = ref GetByRef(index);

            if (tween.State == TweenState.Running)
            {
                tween.State = TweenState.Paused;
                refs.OnPaused();
            }
            else if (tween.State == TweenState.RewindRunning)
            {
                tween.State = TweenState.RewindPaused;
                refs.OnRewindPaused();
            }
        }

        public Task AwaitPlayStart(int index)
        {
            var refs = GetManagedReferences(index);
            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                Return(index);
                return Task.CompletedTask;
            }

            TaskCompletionSource source = CreateTaskCompletionSource();

            refs.PlayStarted.Add(SetResult);

            return source.Task;

            void SetResult()
            {
                SetTaskCompletionResult(source);
                refs.PlayStarted.Remove(SetResult);
            }
        }

        public Task AwaitPause(int index)
        {
            var refs = GetManagedReferences(index);
            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                Return(index);
                return Task.CompletedTask;
            }

            TaskCompletionSource source = CreateTaskCompletionSource();

            refs.Paused.Add(SetResult);

            return source.Task;

            void SetResult()
            {
                SetTaskCompletionResult(source);
                refs.Paused.Remove(SetResult);
            }
        }

        public Task AwaitCompletion(int index)
        {
            var refs = GetManagedReferences(index);
            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                Return(index);
                return Task.CompletedTask;
            }

            TaskCompletionSource source = CreateTaskCompletionSource();

            refs.Completed.Add(SetResult);

            return source.Task;

            void SetResult()
            {
                SetTaskCompletionResult(source);
                refs.Completed.Remove(SetResult);
            }
        }

        public Task AwaitRewindStart(int index)
        {
            var refs = GetManagedReferences(index);
            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                Return(index);
                return Task.CompletedTask;
            }

            TaskCompletionSource source = CreateTaskCompletionSource();

            refs.RewindStarted.Add(SetResult);

            return source.Task;

            void SetResult()
            {
                SetTaskCompletionResult(source);
                refs.RewindStarted.Remove(SetResult);
            }
        }

        public Task AwaitRewindCompletion(int index)
        {
            var refs = GetManagedReferences(index);
            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                Return(index);
                return Task.CompletedTask;
            }

            TaskCompletionSource source = CreateTaskCompletionSource();

            refs.RewindCompleted.Add(SetResult);

            return source.Task;

            void SetResult()
            {
                SetTaskCompletionResult(source);
                refs.RewindCompleted.Remove(SetResult);
            }
        }

        public Task AwaitDispose(int index)
        {
            var refs = GetManagedReferences(index);
            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                Return(index);
                return Task.CompletedTask;
            }

            TaskCompletionSource source = CreateTaskCompletionSource();

            refs.Disposing.Add(SetResult);

            return source.Task;

            void SetResult()
            {
                SetTaskCompletionResult(source);
                refs.Disposing.Remove(SetResult);
            }
        }

        private static TaskCompletionSource CreateTaskCompletionSource()
        {
#if UNITASK
            return TaskCompletionSource.Create();
#else
            return new TaskCompletionSource<bool>();
#endif
        }

        private static void SetTaskCompletionResult(TaskCompletionSource source)
        {
            try
            {
#if UNITASK
                source.TrySetResult();
#else
                source.TrySetResult(false);
#endif
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}