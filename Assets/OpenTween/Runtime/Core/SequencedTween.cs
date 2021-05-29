namespace OpenTween
{
    internal readonly struct SequencedTween
    {
        public readonly float Position;
        public readonly TweenBase Tween;
        public readonly int Version;

        public SequencedTween(float position, TweenBase tween)
        {
            Position = position;
            Tween = tween;
            Version = tween.Version;
        }
    }
}