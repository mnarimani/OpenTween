using System;

namespace OpenTween.Helpers
{
    public static class Assertions<T>
    {
        public static Action<T, T, string> AreEqual;
    }
}