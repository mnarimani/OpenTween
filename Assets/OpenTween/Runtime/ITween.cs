using System;
using Cysharp.Threading.Tasks;

namespace OpenTween
{
    public interface ITween
    {
        ITween OnPlayStarted(Action callback);
        ITween OnPaused(Action callback);
        ITween OnCompleted(Action callback);
        ITween OnRewindStarted(Action callback);
        ITween OnRewindPaused(Action callback);
        ITween OnRewindCompleted(Action callback);
        ITween OnDisposing(Action callback);
        ITween Play(bool restart = false);
        ITween Rewind(bool restart = false);
        ITween ForceComplete();
        ITween Pause();
        UniTask AwaitPlayStart();
        UniTask AwaitPause();
        UniTask AwaitCompletion();
        UniTask AwaitRewindStart();
        UniTask AwaitRewindCompletion();
        UniTask AwaitDispose();
        bool IsActive();
    }
}