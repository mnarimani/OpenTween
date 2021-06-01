namespace OpenTween
{
    internal interface ITweenBaseInternal
    {
        int Index { get; set; }
        int Version { get; set; }
        float CurrentTime { get; set; }
        TweenState State { get; set; }
        float LerpParameter { get; set; }
        bool IsCompletedInLastFrame { get; set; }
        bool IsRewindCompletedInLastFrame { get; set; }
        bool IsUpdatedInLastFrame { get; set; }
        float Duration { get; }
        void ResetToDefaults();
        void Save();
        void Play();
        void Rewind();
    }
}