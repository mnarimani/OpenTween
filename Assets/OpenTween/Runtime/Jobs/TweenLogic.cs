using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;

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
        public static bool UpdateTweenTime<T>(ref TweenInternal<T> t, ref TweenOptions<T> options, float dt)
        {
            float duration = options.Duration + options.PrePlayDelay + options.PostPlayDelay;
            if (Hint.Likely(t.State == TweenState.Running))
            {
                t.CurrentTime += dt;

                if (t.CurrentTime >= duration)
                {
                    t.CurrentLoopCount++;
                    t.CurrentTime = duration;
                    t.State = TweenState.Completed;
                    t.IsCompletedInLastFrame = true;
                }

                return true;
            }

            if (t.State == TweenState.RewindRunning)
            {
                t.CurrentTime -= dt;

                if (t.CurrentTime <= 0)
                {
                    t.CurrentLoopCount++;
                    t.State = TweenState.RewindCompleted;
                    t.CurrentTime = 0;
                    t.IsRewindCompletedInLastFrame = true;
                }

                return true;
            }

            return false;
        }

        public static bool UpdateSequenceTime(ref SequenceInternal t, ref SequenceOptions options, float dt)
        {
            if (Hint.Likely(t.State == TweenState.Running))
            {
                t.CurrentTime += dt;

                if (t.CurrentTime >= options.Duration)
                {
                    t.CurrentLoopCount++;
                    t.CurrentTime = options.Duration;
                    t.State = TweenState.Completed;
                    t.IsCompletedInLastFrame = true;
                }

                return true;
            }

            if (t.State == TweenState.RewindRunning)
            {
                t.CurrentTime -= dt;

                if (t.CurrentTime <= 0)
                {
                    t.CurrentLoopCount++;
                    t.State = TweenState.RewindCompleted;
                    t.CurrentTime = 0;
                    t.IsRewindCompletedInLastFrame = true;
                }

                return true;
            }

            return false;
        }
    }
}