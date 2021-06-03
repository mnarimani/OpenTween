namespace OpenTween
{
    internal interface ITweenBaseInternal
    {
        int Index { get; set; }
        int Version { get; set; }
        float CurrentTime { get; set; }
        TweenState State { get; set; }
        bool IsCompletedInLastFrame { get; set; }
        bool IsRewindCompletedInLastFrame { get; set; }
        bool IsUpdatedInLastFrame { get; set; }
        int CurrentLoopCount { get; set; }
        void ResetToDefaults();
        void ReadonlySave();
        bool RegistryPlay(bool restart = false);
        void RegistryRewind(bool restart = false);
        void RegistrySetTime(float time);
        float GetDurationFromRegistry();
    }
}
