using System;

namespace OpenTween
{
    public partial struct Tween<T>
    {
        ITweenBase ITweenBase.OnPlayStarted(Action callback)
        {
            return OnPlayStarted(callback);
        }

        ITweenBase ITweenBase.OnPaused(Action callback)
        {
            return OnPaused(callback);
        }

        ITweenBase ITweenBase.OnCompleted(Action callback)
        {
            return OnCompleted(callback);
        }

        ITweenBase ITweenBase.OnRewindStarted(Action callback)
        {
            return OnRewindStarted(callback);
        }

        ITweenBase ITweenBase.OnRewindPaused(Action callback)
        {
            return OnRewindPaused(callback);
        }

        ITweenBase ITweenBase.OnRewindCompleted(Action callback)
        {
            return OnRewindCompleted(callback);
        }

        ITweenBase ITweenBase.OnDisposing(Action callback)
        {
            return OnDisposing(callback);
        }

        ITweenBase ITweenBase.Play(bool restart)
        {
            return Play(restart);
        }

        ITweenBase ITweenBase.Rewind(bool restart)
        {
            return Rewind(restart);
        }

        ITweenBase ITweenBase.ForceComplete()
        {
            return ForceComplete();
        }

        ITweenBase ITweenBase.Pause()
        {
            return Pause();
        }
    }
}