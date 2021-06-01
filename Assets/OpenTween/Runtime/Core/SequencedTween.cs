namespace OpenTween
{
    internal delegate ITweenBaseInternal TweenGetter(int index);
    
    internal readonly struct SequencedTween
    {
        public readonly float Position;
        public readonly int TweenIndex;
        public readonly int Version;
        public readonly TweenGetter TweenGetter;

        public SequencedTween(float position, int tweenIndex, int version, TweenGetter getter)
        {
            Position = position;
            TweenIndex = tweenIndex;
            Version = version;
            TweenGetter = getter;
        }
    }
}