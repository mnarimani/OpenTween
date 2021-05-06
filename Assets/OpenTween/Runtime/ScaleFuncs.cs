// TinyTween.cs
//
// Copyright (c) 2013 Nick Gravelyn
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using UnityEngine;

namespace OpenTween
{
    /// <summary>
    /// Defines a set of premade scale functions for use with tweens.
    /// </summary>
    /// <remarks>
    /// To avoid excess allocations of delegates, the public members of ScaleFuncs are already
    /// delegates that reference private methods.
    ///
    /// Implementations based on http://theinstructionlimit.com/flash-style-tweeneasing-functions-in-c
    /// which are based on http://www.robertpenner.com/easing/
    /// </remarks>
    public static class ScaleFuncs
    {
        /// <summary>
        /// A linear progress scale function.
        /// </summary>
        public static readonly ScaleFunc Linear = LinearImpl;

        /// <summary>
        /// A quadratic (x^2) progress scale function that eases in.
        /// </summary>
        public static readonly ScaleFunc QuadraticEaseIn = QuadraticEaseInImpl;

        /// <summary>
        /// A quadratic (x^2) progress scale function that eases out.
        /// </summary>
        public static readonly ScaleFunc QuadraticEaseOut = QuadraticEaseOutImpl;

        /// <summary>
        /// A quadratic (x^2) progress scale function that eases in and out.
        /// </summary>
        public static readonly ScaleFunc QuadraticEaseInOut = QuadraticEaseInOutImpl;

        /// <summary>
        /// A cubic (x^3) progress scale function that eases in.
        /// </summary>
        public static readonly ScaleFunc CubicEaseIn = CubicEaseInImpl;

        /// <summary>
        /// A cubic (x^3) progress scale function that eases out.
        /// </summary>
        public static readonly ScaleFunc CubicEaseOut = CubicEaseOutImpl;

        /// <summary>
        /// A cubic (x^3) progress scale function that eases in and out.
        /// </summary>
        public static readonly ScaleFunc CubicEaseInOut = CubicEaseInOutImpl;

        /// <summary>
        /// A quartic (x^4) progress scale function that eases in.
        /// </summary>
        public static readonly ScaleFunc QuarticEaseIn = QuarticEaseInImpl;

        /// <summary>
        /// A quartic (x^4) progress scale function that eases out.
        /// </summary>
        public static readonly ScaleFunc QuarticEaseOut = QuarticEaseOutImpl;

        /// <summary>
        /// A quartic (x^4) progress scale function that eases in and out.
        /// </summary>
        public static readonly ScaleFunc QuarticEaseInOut = QuarticEaseInOutImpl;

        /// <summary>
        /// A quintic (x^5) progress scale function that eases in.
        /// </summary>
        public static readonly ScaleFunc QuinticEaseIn = QuinticEaseInImpl;

        /// <summary>
        /// A quintic (x^5) progress scale function that eases out.
        /// </summary>
        public static readonly ScaleFunc QuinticEaseOut = QuinticEaseOutImpl;

        /// <summary>
        /// A quintic (x^5) progress scale function that eases in and out.
        /// </summary>
        public static readonly ScaleFunc QuinticEaseInOut = QuinticEaseInOutImpl;

        /// <summary>
        /// A sinusoidal progress scale function that eases in.
        /// </summary>
        public static readonly ScaleFunc SineEaseIn = SineEaseInImpl;

        /// <summary>
        /// A sinusoidal progress scale function that eases out.
        /// </summary>
        public static readonly ScaleFunc SineEaseOut = SineEaseOutImpl;

        /// <summary>
        /// A sinusoidal progress scale function that eases in and out.
        /// </summary>
        public static readonly ScaleFunc SineEaseInOut = SineEaseInOutImpl;

        private const float Pi = (float)Mathf.PI;
        private const float HalfPi = Pi / 2f;

        private static float LinearImpl(float progress) { return progress; }
        private static float QuadraticEaseInImpl(float progress) { return EaseInPower(progress, 2); }
        private static float QuadraticEaseOutImpl(float progress) { return EaseOutPower(progress, 2); }
        private static float QuadraticEaseInOutImpl(float progress) { return EaseInOutPower(progress, 2); }
        private static float CubicEaseInImpl(float progress) { return EaseInPower(progress, 3); }
        private static float CubicEaseOutImpl(float progress) { return EaseOutPower(progress, 3); }
        private static float CubicEaseInOutImpl(float progress) { return EaseInOutPower(progress, 3); }
        private static float QuarticEaseInImpl(float progress) { return EaseInPower(progress, 4); }
        private static float QuarticEaseOutImpl(float progress) { return EaseOutPower(progress, 4); }
        private static float QuarticEaseInOutImpl(float progress) { return EaseInOutPower(progress, 4); }
        private static float QuinticEaseInImpl(float progress) { return EaseInPower(progress, 5); }
        private static float QuinticEaseOutImpl(float progress) { return EaseOutPower(progress, 5); }
        private static float QuinticEaseInOutImpl(float progress) { return EaseInOutPower(progress, 5); }

        private static float EaseInPower(float progress, int power)
        {
            return (float)Mathf.Pow(progress, power);
        }

        private static float EaseOutPower(float progress, int power)
        {
            int sign = power % 2 == 0 ? -1 : 1;
            return (float)(sign * (Mathf.Pow(progress - 1, power) + sign));
        }

        private static float EaseInOutPower(float progress, int power)
        {
            progress *= 2;
            if (progress < 1)
            {
                return (float)Mathf.Pow(progress, power) / 2f;
            }
            else
            {
                int sign = power % 2 == 0 ? -1 : 1;
                return (float)(sign / 2.0 * (Mathf.Pow(progress - 2, power) + sign * 2));
            }
        }

        private static float SineEaseInImpl(float progress)
        {
            return (float)Mathf.Sin(progress * HalfPi - HalfPi) + 1;
        }

        private static float SineEaseOutImpl(float progress)
        {
            return (float)Mathf.Sin(progress * HalfPi);
        }

        private static float SineEaseInOutImpl(float progress)
        {
            return (float)(Mathf.Sin(progress * Pi - HalfPi) + 1) / 2;
        }
    }
}
