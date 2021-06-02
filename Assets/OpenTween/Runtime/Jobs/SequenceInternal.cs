namespace OpenTween.Jobs
{
    internal struct SequenceInternal : ITweenBaseInternal
    {
        public int Index { get; set; }
        public int Version { get; set; }
        public float CurrentTime { get; set; }
        public TweenState State { get; set; }
        public float LerpParameter { get; set; }
        public bool IsCompletedInLastFrame { get; set; }
        public bool IsRewindCompletedInLastFrame { get; set; }
        public bool IsUpdatedInLastFrame { get; set; }

        public float Duration => SequenceRegistry.Instance.GetManagedReferences(Index).LastInsertPosition;

        public void ResetToDefaults()
        {
            CurrentTime = default;
            State = TweenState.NotPlayed;
            LerpParameter = default;
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

        public bool ReadonlyPlay(bool restart = false)
        {
            return SequenceRegistry.Instance.Play(Index, restart);
        }

        public void ReadonlyRewind(bool restart = false)
        {
            SequenceRegistry.Instance.Rewind(Index, restart);
        }

        public void ReadonlySetTime(float time)
        {
            ref SequenceInternal seq = ref SequenceRegistry.Instance.GetByRef(Index);
            seq.CurrentTime = time;
        }
    }
}