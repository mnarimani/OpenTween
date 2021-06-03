namespace OpenTween.Jobs
{
    internal struct SequenceInternal : ITweenBaseInternal
    {
        public int Index { get; set; }
        public int Version { get; set; }
        public float CurrentTime { get; set; }
        public TweenState State { get; set; }
        public bool IsCompletedInLastFrame { get; set; }
        public bool IsRewindCompletedInLastFrame { get; set; }
        public bool IsUpdatedInLastFrame { get; set; }
        public int CurrentLoopCount { get; set; }
        // public float PreUpdateTime;

        public void ResetToDefaults()
        {
            CurrentTime = default;
            State = TweenState.NotPlayed;
            IsCompletedInLastFrame = default;
            IsRewindCompletedInLastFrame = default;
            IsUpdatedInLastFrame = default;
        }

        public void ReadonlySave()
        {
            // Does this work?
            ref SequenceInternal seq = ref SequenceRegistry.Instance.GetByRef(Index);
            seq = this;
        }

        public bool RegistryPlay(bool restart = false)
        {
            return SequenceRegistry.Instance.Play(Index, restart);
        }

        public void RegistryRewind(bool restart = false)
        {
            SequenceRegistry.Instance.Rewind(Index, restart);
        }

        public void RegistrySetTime(float time)
        {
            SequenceRegistry.Instance.GetByRef(Index).CurrentTime = time;
        }

        public float GetDurationFromRegistry()
        {
            return SequenceRegistry.Instance.GetOptionsByRef(Index).Duration;
        }
    }
}