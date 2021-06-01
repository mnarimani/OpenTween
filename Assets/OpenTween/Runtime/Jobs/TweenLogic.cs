using System;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;


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
        public static bool UpdateTime<TTween>(ref TTween t, bool autoPlay, float duration, float dt)
            where TTween : ITweenBaseInternal
        {
            if (Hint.Likely(autoPlay) && t.State == TweenState.NotPlayed)
            {
                t.State = TweenState.Running;
            }

            if (Hint.Likely(t.State == TweenState.Running))
            {
                t.CurrentTime += dt;

                if (t.CurrentTime >= duration)
                {
                    t.CurrentTime = duration;
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
    }
}