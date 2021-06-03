// ReSharper disable StaticMemberInGenericType

using System;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
#if UNITY_COLLECTIONS
using NativeListInt = Unity.Collections.NativeList<int>;

#else
using NativeListInt = OpenTween.Jobs.NativeList<int>;
#endif

namespace OpenTween.Jobs
{
    internal delegate JobHandle LerpScheduleFunc<T>(NativeArray<TweenInternal<T>> tweens, NativeArray<TweenOptions<T>> options, NativeListInt activeIndices, JobHandle deps);

    internal class TweenRegistry<T> : RegistryBase<TweenInternal<T>, TweenOptions<T>, TweenManagedReferences<T>, TweenRegistry<T>>
    {
        public static LerpScheduleFunc<T> LerpScheduler;

        protected override void Schedule(float dt)
        {
            if (!IsInitialized)
                return;
            base.Schedule(dt);
            JobHandle = new Job
            {
                DelaTime = dt,
                Tweens = All,
                Options = AllOptions,
                Indices = ActiveIndices
            }.Schedule(ActiveIndices.Length, 64);
            JobHandle = LerpScheduler(All, AllOptions, ActiveIndices, JobHandle);
        }

        protected override void ProcessPostComplete(int index, ref TweenInternal<T> tween, ref TweenOptions<T> options, TweenManagedReferences<T> refs)
        {
            if (tween.IsUpdatedInLastFrame)
            {
                refs.OnValueUpdated(tween.CurrentValue);
            }

            if ((tween.IsCompletedInLastFrame || tween.IsRewindCompletedInLastFrame) && (options.LoopCount == -1 || tween.CurrentLoopCount < options.LoopCount))
            {
                switch (options.LoopType)
                {
                    case LoopType.Restart when tween.State == TweenState.Completed:
                    {
                        tween.State = TweenState.Running;
                        tween.CurrentTime = 0;
                        break;
                    }
                    case LoopType.Restart when tween.State == TweenState.RewindCompleted:
                    {
                        tween.State = TweenState.RewindRunning;
                        tween.CurrentTime = options.Duration;
                        break;
                    }
                    case LoopType.YoYo when tween.State == TweenState.Completed:
                    {
                        tween.State = TweenState.RewindRunning;
                        break;
                    }
                    case LoopType.YoYo when tween.State == TweenState.RewindCompleted:
                    {
                        tween.State = TweenState.Running;
                        break;
                    }
                    case LoopType.Incremental when tween.State == TweenState.Completed:
                    {
                        tween.State = TweenState.Running;
                        tween.CurrentTime = 0;
                        T diff = TweenValueOp<T>.Sub(options.End, options.Start);
                        options.Start = options.End;
                        options.End = TweenValueOp<T>.Add(options.Start, diff);
                        break;
                    }
                    case LoopType.Incremental when tween.State == TweenState.RewindCompleted:
                    {
                        tween.State = TweenState.RewindRunning;
                        tween.CurrentTime = options.Duration;
                        T diff = TweenValueOp<T>.Sub(options.End, options.Start);
                        options.Start = options.End;
                        options.End = TweenValueOp<T>.Add(options.Start, diff);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                tween.IsCompletedInLastFrame = false;
                tween.IsRewindCompletedInLastFrame = false;
            }
        }

        public override bool Play(int index, bool restart)
        {
            TweenManagedReferences<T> refs = GetManagedReferences(index);

            if (refs.HasBoundComponent && refs.BoundComponent == null)
            {
                Return(index);
                return false;
            }

            ref TweenInternal<T> tween = ref GetByRef(index);
            ref TweenOptions<T> options = ref GetOptionsByRef(index);

            if ((tween.State == TweenState.NotPlayed || restart) && options.DynamicStartEvaluation)
            {
                options.Start = refs.StartEvalFunc();
            }

            return base.Play(index, restart);
        }

        [BurstCompile]
        private struct Job : IJobParallelFor
        {
            public float DelaTime;

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

                if (Hint.Likely(TweenLogic.UpdateTweenTime(ref t, ref options, DelaTime)))
                {
                    if (t.CurrentTime >= options.PrePlayDelay)
                    {
                        float time = t.CurrentTime - options.PrePlayDelay - options.PostPlayDelay;

                        if (time <= 0)
                        {
                            t.LerpParameter = 0;
                        }
                        else if (time >= options.Duration)
                        {
                            t.LerpParameter = 1;
                        }
                        else
                        {
                            t.LerpParameter = JobEaseMap.Evaluate(options.Ease, time, options.Duration, options.OvershootOrAmplitude, options.Period);
                        }

                        t.IsUpdatedInLastFrame = true;
                    }
                }

                Tweens[index] = t;
            }
        }
    }
}