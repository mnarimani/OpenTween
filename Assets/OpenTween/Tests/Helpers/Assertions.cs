using System;

namespace OpenTween.Tests.Helpers
{
    public static class Assertions<T>
    {
        public static Action<T, T, string> Method;
    }
}