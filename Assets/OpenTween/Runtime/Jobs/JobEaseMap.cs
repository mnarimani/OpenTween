using Unity.Burst;
using Unity.Mathematics;

namespace OpenTween.Jobs
{
    [BurstCompile]
    public static class JobEaseMap
    {
        private const float Pi = math.PI;

        private const float HalfPi = Pi / 2f;
        private const float TwoPi = Pi * 2f;

        [BurstCompile]
        public static float Evaluate(Ease ease, float time, float duration, float amplitude, float period)
        {
            return ease switch
            {
                Ease.Linear => LinearImpl(time / duration),
                Ease.InSine => SineEaseInImpl(time / duration),
                Ease.OutSine => SineEaseOutImpl(time / duration),
                Ease.InOutSine => SineEaseInOutImpl(time / duration),
                Ease.InQuad => QuadraticEaseInImpl(time / duration),
                Ease.OutQuad => QuadraticEaseOutImpl(time / duration),
                Ease.InOutQuad => QuadraticEaseInOutImpl(time / duration),
                Ease.InCubic => CubicEaseInImpl(time / duration),
                Ease.OutCubic => CubicEaseOutImpl(time / duration),
                Ease.InOutCubic => CubicEaseInOutImpl(time / duration),
                Ease.InQuart => QuarticEaseInImpl(time / duration),
                Ease.OutQuart => QuarticEaseOutImpl(time / duration),
                Ease.InOutQuart => QuarticEaseInOutImpl(time / duration),
                Ease.InQuint => QuinticEaseInImpl(time / duration),
                Ease.OutQuint => QuinticEaseOutImpl(time / duration),
                Ease.InOutQuint => QuinticEaseInOutImpl(time / duration),
                Ease.InExpo => InExpo(time, duration),
                Ease.OutExpo => OutExpo(time, duration),
                Ease.InOutExpo => InOutExpo(time, duration),
                Ease.InElastic => InElastic(time, duration, amplitude, period),
                Ease.OutElastic => OutElastic(time, duration, amplitude, period),
                Ease.InOutElastic => InOutElastic(time, duration, amplitude, period),
                Ease.InBack => InBack(time, duration, amplitude),
                Ease.OutBack => OutBack(time, duration, amplitude),
                Ease.InOutBack => InOutBack(time, duration, amplitude),
                Ease.InBounce => InBounce(time, duration),
                Ease.OutBounce => OutBounce(time, duration),
                Ease.InOutBounce => InOutBounce(time, duration),
                _ => QuadraticEaseOutImpl(time / duration)
            };
        }

        [BurstCompile]
        private static float LinearImpl(float progress)
        {
            return progress;
        }

        [BurstCompile]
        private static float QuadraticEaseInImpl(float progress)
        {
            return EaseInPower(progress, 2);
        }

        [BurstCompile]
        private static float QuadraticEaseOutImpl(float progress)
        {
            return EaseOutPower(progress, 2);
        }

        [BurstCompile]
        private static float QuadraticEaseInOutImpl(float progress)
        {
            return EaseInOutPower(progress, 2);
        }

        [BurstCompile]
        private static float CubicEaseInImpl(float progress)
        {
            return EaseInPower(progress, 3);
        }

        [BurstCompile]
        private static float CubicEaseOutImpl(float progress)
        {
            return EaseOutPower(progress, 3);
        }

        [BurstCompile]
        private static float CubicEaseInOutImpl(float progress)
        {
            return EaseInOutPower(progress, 3);
        }

        [BurstCompile]
        private static float QuarticEaseInImpl(float progress)
        {
            return EaseInPower(progress, 4);
        }

        [BurstCompile]
        private static float QuarticEaseOutImpl(float progress)
        {
            return EaseOutPower(progress, 4);
        }

        [BurstCompile]
        private static float QuarticEaseInOutImpl(float progress)
        {
            return EaseInOutPower(progress, 4);
        }

        [BurstCompile]
        private static float QuinticEaseInImpl(float progress)
        {
            return EaseInPower(progress, 5);
        }

        [BurstCompile]
        private static float QuinticEaseOutImpl(float progress)
        {
            return EaseOutPower(progress, 5);
        }

        [BurstCompile]
        private static float QuinticEaseInOutImpl(float progress)
        {
            return EaseInOutPower(progress, 5);
        }

        [BurstCompile]
        private static float SineEaseInImpl(float progress)
        {
            return math.sin(progress * HalfPi - HalfPi) + 1;
        }

        [BurstCompile]
        private static float SineEaseOutImpl(float progress)
        {
            return math.sin(progress * HalfPi);
        }

        [BurstCompile]
        private static float SineEaseInOutImpl(float progress)
        {
            return (math.sin(progress * Pi - HalfPi) + 1) / 2;
        }

        [BurstCompile]
        private static float EaseInPower(float progress, int power)
        {
            return TweenLogic.Pow(progress, power);
        }

        [BurstCompile]
        private static float EaseOutPower(float progress, int power)
        {
            int sign = power % 2 == 0 ? -1 : 1;
            return sign * (TweenLogic.Pow(progress - 1, power) + sign);
        }

        [BurstCompile]
        private static float EaseInOutPower(float progress, int power)
        {
            progress *= 2;
            if (progress < 1)
            {
                return TweenLogic.Pow(progress, power) / 2f;
            }

            int sign = power % 2 == 0 ? -1 : 1;
            return (float) (sign / 2.0 * (TweenLogic.Pow(progress - 2, power) + sign * 2));
        }

        [BurstCompile]
        private static float InElastic(float time, float duration, float amp, float period)
        {
            float s0;
            if (time == 0) return 0;

            time /= duration;

            if (math.abs(time - 1) < 0.0001f) return 1;

            if (period == 0) period = duration * 0.3f;

            if (amp < 1)
            {
                amp = 1;
                s0 = period / 4;
            }
            else
            {
                s0 = period / TwoPi * math.asin(1 / amp);
            }

            return -(amp * math.pow(2, 10 * (time -= 1)) * math.sin((time * duration - s0) * TwoPi / period));
        }

        [BurstCompile]
        private static float OutElastic(float time, float duration, float amp, float period)
        {
            float s1;
            if (time == 0) return 0;
            if (math.abs((time /= duration) - 1) < 0.0001f) return 1;
            if (period == 0) period = duration * 0.3f;
            if (amp < 1)
            {
                amp = 1;
                s1 = period / 4;
            }
            else s1 = period / TwoPi * math.asin(1 / amp);

            return (amp * math.pow(2, -10 * time) * math.sin((time * duration - s1) * TwoPi / period) + 1);
        }

        [BurstCompile]
        private static float InOutElastic(float time, float duration, float amp, float period)
        {
            float s;
            if (time == 0) return 0;
            if (math.abs((time /= duration * 0.5f) - 2) < 0.0001f) return 1;
            if (period == 0) period = duration * (0.3f * 1.5f);
            if (amp < 1)
            {
                amp = 1;
                s = period / 4;
            }
            else s = period / TwoPi * math.asin(1 / amp);

            if (time < 1) return -0.5f * (amp * math.pow(2, 10 * (time -= 1)) * math.sin((time * duration - s) * TwoPi / period));
            return amp * math.pow(2, -10 * (time -= 1)) * math.sin((time * duration - s) * TwoPi / period) * 0.5f + 1;
        }

        [BurstCompile]
        private static float InBack(float time, float duration, float amp)
        {
            return (time /= duration) * time * ((amp + 1) * time - amp);
        }

        [BurstCompile]
        private static float OutBack(float time, float duration, float amp)
        {
            return ((time = time / duration - 1) * time * ((amp + 1) * time + amp) + 1);
        }

        [BurstCompile]
        private static float InOutBack(float time, float duration, float amp)
        {
            if ((time /= duration * 0.5f) < 1) return 0.5f * (time * time * (((amp *= (1.525f)) + 1) * time - amp));
            return 0.5f * ((time -= 2) * time * (((amp *= (1.525f)) + 1) * time + amp) + 2);
        }

        [BurstCompile]
        private static float InExpo(float time, float duration)
        {
            return (time == 0) ? 0 : math.pow(2, 10 * (time / duration - 1));
        }

        [BurstCompile]
        private static float OutExpo(float time, float duration)
        {
            if (math.abs(time - duration) < 0.0001f) return 1;
            return (-math.pow(2, -10 * time / duration) + 1);
        }

        [BurstCompile]
        private static float InOutExpo(float time, float duration)
        {
            if (time == 0) return 0;
            if (math.abs(time - duration) < 0.0001f) return 1;
            if ((time /= duration * 0.5f) < 1) return 0.5f * math.pow(2, 10 * (time - 1));
            return 0.5f * (-math.pow(2, -10 * --time) + 2);
        }


        [BurstCompile]
        public static float InBounce(in float time, in float duration)
        {
            return 1 - OutBounce(duration - time, duration);
        }


        [BurstCompile]
        public static float OutBounce(in float time, in float duration)
        {
            float t2 = time / duration;

            if (t2 < (1 / 2.75f))
            {
                return (7.5625f * time * time);
            }

            if (t2 < (2 / 2.75f))
            {
                t2 -= (1.5f / 2.75f);
                return (7.5625f * t2 * t2 + 0.75f);
            }

            if (t2 < (2.5f / 2.75f))
            {
                t2 -= (2.25f / 2.75f);
                return (7.5625f * t2 * t2 + 0.9375f);
            }

            t2 -= (2.625f / 2.75f);
            return (7.5625f * t2 * t2 + 0.984375f);
        }

        [BurstCompile]
        public static float InOutBounce(in float time, in float duration)
        {
            if (time < duration * 0.5f)
            {
                return InBounce(time * 2, duration) * 0.5f;
            }

            return OutBounce(time * 2 - duration, duration) * 0.5f + 0.5f;
        }
    }
}