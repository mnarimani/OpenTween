// ReSharper disable StaticMemberInGenericType

using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
#if UNITY_COLLECTIONS
using NativeListInt = Unity.Collections.NativeList<int>;

#else
using NativeListInt = OpenTween.Jobs.NativeList<int>;
#endif

namespace OpenTween.Jobs
{
    internal delegate JobHandle LerpScheduleFunc<T>(NativeArray<TweenInternal<T>> tweens, NativeArray<TweenOptions<T>> options, NativeListInt activeIndices, JobHandle deps);

    internal static class TweenRegistry<T, TReferences> where TReferences : ManagedReferences, new()
    {
        private static NativeArray<TweenInternal<T>> _all;
        private static NativeArray<TweenOptions<T>> _allOptions;
        private static NativeListInt _activeIndices;
        private static Stack<int> _freeIndices;

        private static TReferences[] _references;

        public static LerpScheduleFunc<T> LerpScheduler;
        private static JobHandle _jobHandle;

        private static bool IsInitialized => _all.IsCreated;

        private static void Initialize()
        {
            _all = new NativeArray<TweenInternal<T>>(64000, Allocator.Persistent);
            _allOptions = new NativeArray<TweenOptions<T>>(_all.Length, Allocator.Persistent);
            _activeIndices = new NativeListInt(_all.Length, Allocator.Persistent);

            _freeIndices = new Stack<int>(_all.Length);
            for (int i = 0; i < _all.Length; i++)
            {
                _freeIndices.Push(i);
            }

            _references = new TReferences[_all.Length];

#if UNITY_EDITOR
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainOnDomainUnload;
            EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
#endif
        }


        private static void IncreaseCapacity()
        {
            var newAll = new NativeArray<TweenInternal<T>>(_all.Length * 2, Allocator.Persistent);
            new CopyArrayJob<TweenInternal<T>>(_all, newAll)
                .Schedule(_all.Length, 32)
                .Complete();
            _all.Dispose();
            _all = newAll;

            var newOptions = new NativeArray<TweenOptions<T>>(newAll.Length, Allocator.Persistent);
            new CopyArrayJob<TweenOptions<T>>(_allOptions, newOptions)
                .Schedule(_allOptions.Length, 32)
                .Complete();
            _allOptions.Dispose();
            _allOptions = newOptions;

            var newReferences = new TReferences[newAll.Length];
            Array.Copy(_references, newReferences, _references.Length);
            _references = newReferences;

            for (int i = _all.Length / 2; i < _all.Length; i++)
            {
                _freeIndices.Push(i);
            }
        }

        public static int New()
        {
            if (!IsInitialized)
            {
                Initialize();
            }

            if (_freeIndices.Count == 0)
            {
                IncreaseCapacity();
            }

            int index = _freeIndices.Pop();

            ref TweenInternal<T> tween = ref _all.GetRef(index);
            tween.ResetToDefaults();
            tween.Index = index;

            ref TweenOptions<T> options = ref _allOptions.GetRef(index);
            options.ResetToDefaults();

            TReferences refs = _references[index] ?? new TReferences();
            refs.ResetToDefaults();
            _references[index] = refs;

            _activeIndices.Add(index);
            return index;
        }

        public static ref TweenInternal<T> GetByRef(int index)
        {
            return ref _all.GetRef(index);
        }

        public static ref TweenOptions<T> GetOptionsByRef(int index)
        {
            return ref _allOptions.GetRef(index);
        }

        public static TReferences GetManagedReferences(int index)
        {
            return _references[index];
        }

        public static void Schedule(float dt)
        {
            if (!IsInitialized)
                return;

            for (int i = _activeIndices.Length - 1; i >= 0; i--)
            {
                int index = _activeIndices[i];
                TReferences refs = _references[index];

                if (refs.HasBoundComponent && refs.BoundComponent == null)
                {
                    refs.OnDisposing();

                    _activeIndices.RemoveAtSwapBack(i);
                    _freeIndices.Push(index);
                    refs.Version++;
                    GetByRef(index).Version++;
                    GetOptionsByRef(index).Version++;
                }
            }

            _jobHandle = new Job
            {
                DT = dt,
                Tweens = _all,
                Options = _allOptions,
                Indices = _activeIndices
            }.Schedule(_activeIndices.Length, 64);

            _jobHandle = LerpScheduler(_all, _allOptions, _activeIndices, _jobHandle);
        }

        public static void Complete()
        {
            if (!_activeIndices.IsCreated)
                return;

            _jobHandle.Complete();

            for (int k = _activeIndices.Length - 1; k >= 0; k--)
            {
                int index = _activeIndices[k];
                ref TweenInternal<T> tween = ref _all.GetRef(index);
                ref TweenOptions<T> options = ref _allOptions.GetRef(index);
                TReferences refs = _references[index];

                if (tween.ValueChangedInLastFrame)
                {
                    if (refs is TweenManagedReferences<T> tweenRefs)
                    {
                        tweenRefs.OnValueUpdated(tween.CurrentValue);
                    }
                }

                if (tween.IsCompletedInLastFrame)
                {
                    refs.OnCompleted();
                    if (options.DisposeOnComplete)
                    {
                        refs.OnDisposing();

                        _activeIndices.RemoveAtSwapBack(k);
                        _freeIndices.Push(index);
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
                tween.ValueChangedInLastFrame = false;
                tween.IsRewindCompletedInLastFrame = false;
            }
        }

        public static void Return(int tweenIndex)
        {
            TReferences refs = _references[tweenIndex];
            refs.OnDisposing();

            for (int i = _activeIndices.Length - 1; i >= 0; i--)
            {
                if (_activeIndices[i] == tweenIndex)
                    _activeIndices.RemoveAtSwapBack(i);
            }

            _all.GetRef(tweenIndex).Version++;
            _allOptions.GetRef(tweenIndex).Version++;
            refs.Version++;
            _freeIndices.Push(tweenIndex);
        }

#if UNITY_EDITOR

        private static void EditorApplicationOnplayModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.ExitingPlayMode && _all.IsCreated)
            {
                _all.Dispose();
                _allOptions.Dispose();
                _activeIndices.Dispose();
            }
        }

        private static void CurrentDomainOnDomainUnload(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.DomainUnload -= CurrentDomainOnDomainUnload;
            if (_all.IsCreated)
            {
                _all.Dispose();
                _allOptions.Dispose();
                _activeIndices.Dispose();
            }
        }
#endif


        [BurstCompile]
        private struct Job : IJobParallelFor
        {
            public float DT;

            [ReadOnly, NativeDisableParallelForRestriction]
            public NativeListInt Indices;

            [ReadOnly, NativeDisableParallelForRestriction]
            public NativeArray<TweenOptions<T>> Options;

            [NativeDisableParallelForRestriction] public NativeArray<TweenInternal<T>> Tweens;

            [BurstCompile]
            public void Execute(int i)
            {
                int index = Indices[i];
                TweenInternal<T> t = Tweens[index];
                TweenOptions<T> options = Options[index];

                // ReSharper disable once InvertIf
                if (Hint.Likely(TweenLogic.UpdateTime(ref t, ref options, DT)))
                {
                    if (Hint.Unlikely(t.CurrentTime <= 0))
                    {
                        t.LerpParameter = 0;
                    }
                    else if (t.CurrentTime >= options.Duration)
                    {
                        t.LerpParameter = 1;
                    }
                    else
                    {
                        t.LerpParameter = JobEaseMap.Evaluate(options.Ease, t.CurrentTime, options.Duration, 0, 0);
                    }

                    t.ValueChangedInLastFrame = true;
                    Tweens[index] = t;
                }
            }
        }
    }
}