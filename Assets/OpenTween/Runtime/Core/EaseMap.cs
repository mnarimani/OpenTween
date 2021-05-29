using UnityEngine;

namespace OpenTween
{
    public delegate float EaseFunc(float time, float duration, float overshootOrAmplitude, float period);

    internal static class EaseMap
    {
        public const Ease Default = Ease.OutQuad;
        
        private const float Pi = Mathf.PI;
        private const float HalfPi = Pi / 2f;
        
        /// <summary>
        /// A linear progress scale function.
        /// </summary>
        private static readonly EaseFunc Linear = (time, duration, amplitude, period) => time / duration;

        /// <summary>
        /// A quadratic (x^2) progress scale function that eases in.
        /// </summary>
        private static readonly EaseFunc QuadraticEaseIn = (time, duration, amplitude, period) => EaseInPower(time / duration, 2);

        /// <summary>
        /// A quadratic (x^2) progress scale function that eases out.
        /// </summary>
        private static readonly EaseFunc QuadraticEaseOut = (time, duration, amplitude, period) => EaseOutPower(time / duration, 2);

        /// <summary>
        /// A quadratic (x^2) progress scale function that eases in and out.
        /// </summary>
        private static readonly EaseFunc QuadraticEaseInOut = (time, duration, amplitude, period) => EaseInOutPower(time / duration, 2);

        /// <summary>
        /// A cubic (x^3) progress scale function that eases in.
        /// </summary>
        private static readonly EaseFunc CubicEaseIn = (time, duration, amplitude, period) => EaseInPower(time / duration, 3);

        /// <summary>
        /// A cubic (x^3) progress scale function that eases out.
        /// </summary>
        private static readonly EaseFunc CubicEaseOut = (time, duration, amplitude, period) => EaseOutPower(time / duration, 3);

        /// <summary>
        /// A cubic (x^3) progress scale function that eases in and out.
        /// </summary>
        private static readonly EaseFunc CubicEaseInOut = (time, duration, amplitude, period) => EaseInOutPower(time / duration, 3);

        /// <summary>
        /// A quartic (x^4) progress scale function that eases in.
        /// </summary>
        private static readonly EaseFunc QuarticEaseIn = (time, duration, amplitude, period) => EaseInPower(time / duration, 4);

        /// <summary>
        /// A quartic (x^4) progress scale function that eases out.
        /// </summary>
        private static readonly EaseFunc QuarticEaseOut = (time, duration, amplitude, period) => EaseOutPower(time / duration, 4);

        /// <summary>
        /// A quartic (x^4) progress scale function that eases in and out.
        /// </summary>
        private static readonly EaseFunc QuarticEaseInOut = (time, duration, amplitude, period) => EaseInOutPower(time / duration, 4);

        /// <summary>
        /// A quintic (x^5) progress scale function that eases in.
        /// </summary>
        private static readonly EaseFunc QuinticEaseIn = (time, duration, amplitude, period) => EaseInPower(time / duration, 5);

        /// <summary>
        /// A quintic (x^5) progress scale function that eases out.
        /// </summary>
        private static readonly EaseFunc QuinticEaseOut = (time, duration, amplitude, period) => EaseOutPower(time / duration, 5);

        /// <summary>
        /// A quintic (x^5) progress scale function that eases in and out.
        /// </summary>
        private static readonly EaseFunc QuinticEaseInOut = (time, duration, amplitude, period) => EaseInOutPower(time / duration, 5);

        /// <summary>
        /// A sinusoidal progress scale function that eases in.
        /// </summary>
        private static readonly EaseFunc SineEaseIn = (time, duration, amplitude, period) => Mathf.Sin(time / duration * HalfPi - HalfPi) + 1;

        /// <summary>
        /// A sinusoidal progress scale function that eases out.
        /// </summary>
        private static readonly EaseFunc SineEaseOut = (time, duration, amplitude, period) => Mathf.Sin(time / duration * HalfPi);

        /// <summary>
        /// A sinusoidal progress scale function that eases in and out.
        /// </summary>
        private static readonly EaseFunc SineEaseInOut = (time, duration, amplitude, period) => (Mathf.Sin(time / duration * Pi - HalfPi) + 1) / 2;

        private static readonly EaseFunc InBounce =  Bounce.EaseIn;
        private static readonly EaseFunc OutBounce =  Bounce.EaseOut;
        private static readonly EaseFunc InOutBounce =  Bounce.EaseInOut;

        public static float Evaluate(Ease id, float time, float duration, float overshootOrAmplitude, float period)
        {
            return id switch
            {
                Ease.Linear => Linear(time, duration, overshootOrAmplitude, period),
                Ease.InQuad => QuadraticEaseIn(time, duration, overshootOrAmplitude, period),
                Ease.OutQuad => QuadraticEaseOut(time, duration, overshootOrAmplitude, period),
                Ease.InOutQuad => QuadraticEaseInOut(time, duration, overshootOrAmplitude, period),
                Ease.InCubic => CubicEaseIn(time, duration, overshootOrAmplitude, period),
                Ease.OutCubic => CubicEaseOut(time, duration, overshootOrAmplitude, period),
                Ease.InOutCubic => CubicEaseInOut(time, duration, overshootOrAmplitude, period),
                Ease.InQuart => QuarticEaseIn(time, duration, overshootOrAmplitude, period),
                Ease.OutQuart => QuarticEaseOut(time, duration, overshootOrAmplitude, period),
                Ease.InOutQuart => QuarticEaseInOut(time, duration, overshootOrAmplitude, period),
                Ease.InQuint => QuinticEaseIn(time, duration, overshootOrAmplitude, period),
                Ease.OutQuint => QuinticEaseOut(time, duration, overshootOrAmplitude, period),
                Ease.InOutQuint => QuinticEaseInOut(time, duration, overshootOrAmplitude, period),
                Ease.InSine => SineEaseIn(time, duration, overshootOrAmplitude, period),
                Ease.OutSine => SineEaseOut(time, duration, overshootOrAmplitude, period),
                Ease.InOutSine => SineEaseInOut(time, duration, overshootOrAmplitude, period),
                Ease.InBounce => InBounce(time, duration, overshootOrAmplitude, period),
                Ease.OutBounce => OutBounce(time, duration, overshootOrAmplitude, period),
                Ease.InOutBounce => InOutBounce(time, duration, overshootOrAmplitude, period),
                _ => QuadraticEaseOut(time, duration, overshootOrAmplitude, period)
            };
        }

        private static float EaseInPower(float progress, int power)
        {
            return Pow(progress, power);
        }

        private static float EaseOutPower(float progress, int power)
        {
            int sign = power % 2 == 0 ? -1 : 1;
            return sign * (Pow(progress - 1, power) + sign);
        }

        private static float EaseInOutPower(float progress, int power)
        {
            progress *= 2;
            if (progress < 1)
            {
                return Pow(progress, power) / 2f;
            }

            int sign = power % 2 == 0 ? -1 : 1;
            return (float) (sign / 2.0 * (Pow(progress - 2, power) + sign * 2));
        }

        private static float Pow(float v, int p)
        {
            if (p == 0)
                return 1;

            float result = 1;
            for (int i = 0; i < p; i++)
            {
                result *= v;
            }

            return result;
        }
    }
}