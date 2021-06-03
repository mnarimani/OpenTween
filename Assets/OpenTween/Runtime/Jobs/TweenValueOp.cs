namespace OpenTween.Jobs
{
    internal static class TweenValueOp<T>
    {
        internal delegate T Op(T a, T b);

        public static Op Sub;
        public static Op Add;
    }
}