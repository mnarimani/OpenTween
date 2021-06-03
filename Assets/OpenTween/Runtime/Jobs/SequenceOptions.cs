namespace OpenTween.Jobs
{
    internal struct SequenceOptions : IOptionsBaseInternal
    {
        public int Version { get; set; }
        public float Duration { get; set; }
        public bool DisposeOnComplete { get; set; }
        public bool AutoPlay { get; set; }
        public float PrePlayDelay { get; set; }
        public float PostPlayDelay { get; set; }
        public int LoopCount { get; set; }
        public LoopType LoopType { get; set; }

        public void ResetToDefaults()
        {
            DisposeOnComplete = true;
            AutoPlay = true;
            PrePlayDelay = default;
            PostPlayDelay = default;
            Duration = default;
        }
    }
}