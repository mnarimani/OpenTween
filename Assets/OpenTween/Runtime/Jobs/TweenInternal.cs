using System;

namespace OpenTween.Jobs
{
    internal struct TweenInternal<T>
    {
        public int Index;
        public int Version;

        public float CurrentTime;
        public T CurrentValue;
        public TweenState State;
        public float LerpParameter;
        
        public bool IsCompletedInLastFrame;
        public bool ValueChangedInLastFrame;
        public bool IsRewindCompletedInLastFrame;

        public TweenInternal(int index, int version) : this()
        {
            Index = index;
            Version = version;
        }

        public void ResetToDefaults()
        {
            CurrentTime = default;
            CurrentValue = default;
            State = TweenState.NotPlayed;
            IsCompletedInLastFrame = default;
            ValueChangedInLastFrame = default;
            IsRewindCompletedInLastFrame = default;
            LerpParameter = default;
        }
    }
}