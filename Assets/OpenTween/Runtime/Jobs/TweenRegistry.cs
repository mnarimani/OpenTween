// ReSharper disable StaticMemberInGenericType

using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Jobs;
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

        public override void Schedule(float dt)
        {
            if (!IsInitialized)
                return;
            base.Schedule(dt);
            JobHandle = LerpScheduler(All, AllOptions, ActiveIndices, JobHandle);
        }

        protected override void ProcessUpdate(float dt)
        {
            JobHandle = new Job
            {
                DT = dt,
                Tweens = All,
                Options = AllOptions,
                Indices = ActiveIndices
            }.Schedule(ActiveIndices.Length, 64);
        }

        protected override void ProcessPostComplete(int index, ref TweenInternal<T> tween, ref TweenOptions<T> options, TweenManagedReferences<T> refs)
        {
            if (tween.IsUpdatedInLastFrame)
            {
                refs.OnValueUpdated(tween.CurrentValue);
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
                if (Hint.Likely(TweenLogic.UpdateTime(ref t, options.AutoPlay, options.Duration, DT)))
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

                    t.IsUpdatedInLastFrame = true;
                    Tweens[index] = t;
                }
            }
        }
    }
}