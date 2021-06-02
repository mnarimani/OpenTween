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
        void ReadonlySave();
        bool ReadonlyPlay(bool restart = false);
        void ReadonlyRewind(bool restart = false);
        void ReadonlySetTime(float time);
    }
}