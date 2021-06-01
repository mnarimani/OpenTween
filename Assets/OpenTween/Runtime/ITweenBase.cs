using System;
using Cysharp.Threading.Tasks;

namespace OpenTween
{
    public interface ITweenBase
    {
        ITweenBase OnPlayStarted(Action callback);
        ITweenBase OnPaused(Action callback);
        ITweenBase OnCompleted(Action callback);
        ITweenBase OnRewindStarted(Action callback);
        ITweenBase OnRewindPaused(Action callback);
        ITweenBase OnRewindCompleted(Action callback);
        ITweenBase OnDisposing(Action callback);
        ITweenBase Play(bool restart = false);
        ITweenBase Rewind(bool restart = false);
        ITweenBase ForceComplete();
        ITweenBase Pause();
        UniTask AwaitPlayStart();
        UniTask AwaitPause();
        UniTask AwaitCompletion();
        UniTask AwaitRewindStart();
        UniTask AwaitRewindCompletion();
        UniTask AwaitDispose();
        bool IsActive();
    }
}