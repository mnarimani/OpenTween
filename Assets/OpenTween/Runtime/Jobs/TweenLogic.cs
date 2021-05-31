using System;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
#if UNITASK
using Task = Cysharp.Threading.Tasks.UniTask;
using TaskCompletionSource = Cysharp.Threading.Tasks.AutoResetUniTaskCompletionSource;

#else
using Task = System.Threading.Tasks.Task;
using TaskCompletionSource = System.Threading.Tasks.TaskCompletionSource<bool>;

#endif

namespace OpenTween.Jobs
{
    [BurstCompile]
    internal static class TweenLogic
    {
        [BurstCompile]
        public static float Pow(float value, int p)
        {
            if (!Constant.IsConstantExpression(p))
                return math.pow(value, p);

            switch (p)
            {
                case 0: return 1;
                case 1: return value;
                case 2: return value * value;
                case 3: return value * value * value;
                case 4: return value * value * value * value;
                case 5: return value * value * value * value * value;
                default: return math.pow(value, p);
            }
        }

        [BurstCompile]
        public static bool UpdateTime<T>(ref TweenInternal<T> t, ref TweenOptions<T> options, float dt)
        {
            if (Hint.Likely(options.AutoPlay) && t.State == TweenState.NotPlayed)
            {
                t.State = TweenState.Running;
            }
            
            if (Hint.Likely(t.State == TweenState.Running))
            {
                t.CurrentTime += dt;

                if (t.CurrentTime >= options.Duration)
                {
                    t.CurrentTime = options.Duration;
                    t.State = TweenState.Completed;
                }

                if (t.State == TweenState.Completed)
                {
                    t.IsCompletedInLastFrame = true;
                }

                return true;
            }

            if (t.State == TweenState.RewindRunning)
            {
                t.CurrentTime -= dt;

                if (t.CurrentTime <= 0)
                {
                    t.State = TweenState.RewindCompleted;
                    t.CurrentTime = 0;
                }

                if (t.State == TweenState.RewindCompleted)
                {
                    t.IsRewindCompletedInLastFrame = true;
                }

                return true;
            }

            return false;
        }

        private static bool PlayBase<T, TRef>(int index, bool restart) where TRef : ManagedReferences, new()
        {
            TRef refs = TweenRegistry<T, TRef>.GetManagedReferences(index);

            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                TweenRegistry<T, TRef>.Return(index);
                return false;
            }

            ref TweenInternal<T> tween = ref TweenRegistry<T, TRef>.GetByRef(index);

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

        public static bool PlayTween<T>(int index, bool restart)
        {
            TweenManagedReferences<T> refs = TweenRegistry<T, TweenManagedReferences<T>>.GetManagedReferences(index);

            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                TweenRegistry<T, TweenManagedReferences<T>>.Return(index);
                return false;
            }

            ref TweenInternal<T> tween = ref TweenRegistry<T, TweenManagedReferences<T>>.GetByRef(index);
            ref TweenOptions<T> options = ref TweenRegistry<T, TweenManagedReferences<T>>.GetOptionsByRef(index);

            if ((tween.State == TweenState.NotPlayed || restart) && options.DynamicStartEvaluation)
            {
                options.Start = refs.StartEvalFunc();
            }

            return PlayBase<T, TweenManagedReferences<T>>(index, restart);
        }

        public static void Rewind<T, TRef>(int index, bool restart) where TRef : ManagedReferences, new()
        {
            TRef refs = TweenRegistry<T, TRef>.GetManagedReferences(index);

            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                TweenRegistry<T, TRef>.Return(index);
                return;
            }

            ref TweenOptions<T> opt = ref TweenRegistry<T, TRef>.GetOptionsByRef(index);
            ref TweenInternal<T> tween = ref TweenRegistry<T, TRef>.GetByRef(index);

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

        public static void ForceComplete<T, TRef>(int index) where TRef : ManagedReferences, new()
        {
            TRef refs = TweenRegistry<T, TRef>.GetManagedReferences(index);

            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                TweenRegistry<T, TRef>.Return(index);
                return;
            }

            ref TweenOptions<T> opt = ref TweenRegistry<T, TRef>.GetOptionsByRef(index);
            ref TweenInternal<T> tween = ref TweenRegistry<T, TRef>.GetByRef(index);

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

        public static void Pause<T, TRef>(int index) where TRef : ManagedReferences, new()
        {
            TRef refs = TweenRegistry<T, TRef>.GetManagedReferences(index);

            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                TweenRegistry<T, TRef>.Return(index);
                return;
            }

            ref TweenInternal<T> tween = ref TweenRegistry<T, TRef>.GetByRef(index);

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

        public static Task AwaitPlayStart<T, TRef>(int index) where TRef : ManagedReferences, new()
        {
            TRef refs = TweenRegistry<T, TRef>.GetManagedReferences(index);
            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                TweenRegistry<T, TRef>.Return(index);
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

        public static Task AwaitPause<T, TRef>(int index) where TRef : ManagedReferences, new()
        {
            TRef refs = TweenRegistry<T, TRef>.GetManagedReferences(index);
            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                TweenRegistry<T, TRef>.Return(index);
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

        public static Task AwaitCompletion<T, TRef>(int index) where TRef : ManagedReferences, new()
        {
            TRef refs = TweenRegistry<T, TRef>.GetManagedReferences(index);
            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                TweenRegistry<T, TRef>.Return(index);
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

        public static Task AwaitRewindStart<T, TRef>(int index) where TRef : ManagedReferences, new()
        {
            TRef refs = TweenRegistry<T, TRef>.GetManagedReferences(index);
            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                TweenRegistry<T, TRef>.Return(index);
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

        public static Task AwaitRewindCompletion<T, TRef>(int index) where TRef : ManagedReferences, new()
        {
            TRef refs = TweenRegistry<T, TRef>.GetManagedReferences(index);
            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                TweenRegistry<T, TRef>.Return(index);
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

        public static Task AwaitDispose<T, TRef>(int index) where TRef : ManagedReferences, new()
        {
            TRef refs = TweenRegistry<T, TRef>.GetManagedReferences(index);
            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                TweenRegistry<T, TRef>.Return(index);
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