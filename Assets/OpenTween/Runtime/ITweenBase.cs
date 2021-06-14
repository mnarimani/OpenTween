using System;
using Cysharp.Threading.Tasks;

namespace OpenTween
{
    public interface ITweenBase
    {
        ITweenBase SetOnPlayStarted(Action callback);
        ITweenBase SetOnPaused(Action callback);
        ITweenBase SetOnCompleted(Action callback);
        ITweenBase SetOnRewindStarted(Action callback);
        ITweenBase SetOnRewindPaused(Action callback);
        ITweenBase SetOnRewindCompleted(Action callback);
        ITweenBase SetOnDisposing(Action callback);
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