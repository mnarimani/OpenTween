using Unity.Burst;

namespace OpenTween.Jobs
{
    internal struct TweenInternal<T> : ITweenBaseInternal
    {
        public int Version { get; set; }
        public float CurrentTime { get; set; }
        public T CurrentValue { get; set; }
        public int CurrentLoopCount { get; set; }
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

        public void ResetToDefaults()
        {
            CurrentTime = default;
            CurrentValue = default;
            State = TweenState.NotPlayed;
            IsCompletedInLastFrame = default;
            IsUpdatedInLastFrame = default;
            IsRewindCompletedInLastFrame = default;
            LerpParameter = default;
            CurrentLoopCount = default;
        }

        public void ReadonlySave()
        {
            ref TweenInternal<T> tweenInternal = ref TweenRegistry<T>.Instance.GetByRef(Index);
            tweenInternal = this;
        }

        public bool RegistryPlay(bool restart)
        {
            return TweenRegistry<T>.Instance.Play(Index, restart);
        }

        public void RegistryRewind(bool restart)
        {
            TweenRegistry<T>.Instance.Rewind(Index, restart);
        }

        public void RegistrySetTime(float time)
        {
            ref TweenInternal<T> tweenInternal = ref TweenRegistry<T>.Instance.GetByRef(Index);
            tweenInternal.CurrentTime = time;
        }

        public float GetDurationFromRegistry()
        {
            return TweenRegistry<T>.Instance.GetOptionsByRef(Index).Duration;
        }
    }
}