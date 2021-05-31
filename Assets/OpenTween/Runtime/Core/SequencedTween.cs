namespace OpenTween
{
    internal readonly struct SequencedTween
    {
        public readonly float Position;
        public readonly int Tween;
        public readonly int Version;

        public SequencedTween(float position, int tween, int version)
        {
            Position = position;
            Tween = tween;
            Version = version;
        }
    }
}