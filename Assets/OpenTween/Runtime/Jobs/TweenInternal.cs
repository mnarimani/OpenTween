namespace OpenTween.Jobs
{
    internal struct TweenInternal<T> : ITweenBaseInternal
    {
        public int Version { get; set; }
        public float CurrentTime { get; set; }
        public T CurrentValue { get; set; }
        public TweenState State { get; set; }
        public float LerpParameter { get; set; }
        
        public bool IsCompletedInLastFrame { get; set; }
        public bool IsRewindCompletedInLastFrame { get; set; }
        public bool IsUpdatedInLastFrame { get; set; }

        public TweenInternal(int index, int version) : this()
        {
            Index = index;
            Version = version;
        }

        public int Index { get; set; }

        public float Duration => TweenRegistry<T>.Instance.GetOptionsByRef(Index).Duration;

        public void ResetToDefaults()
        {
            CurrentTime = default;
            CurrentValue = default;
            State = TweenState.NotPlayed;
            IsCompletedInLastFrame = default;
            IsUpdatedInLastFrame = default;
            IsRewindCompletedInLastFrame = default;
            LerpParameter = default;
        }

        public void Save()
        {
            // TODO: Does this work?
            ref TweenInternal<T> tweenInternal = ref TweenRegistry<T>.Instance.GetByRef(Index);
            tweenInternal = this;
        }

        public void Play()
        {
            TweenRegistry<T>.Instance.Play(Index, false);
        }

        public void Rewind()
        {
            TweenRegistry<T>.Instance.Rewind(Index, false);
        }
    }
}