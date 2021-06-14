using System;

namespace OpenTween
{
    public partial struct Tween<T>
    {
        ITweenBase ITweenBase.SetOnPlayStarted(Action callback)
        {
            return SetOnPlayStarted(callback);
        }

        ITweenBase ITweenBase.SetOnPaused(Action callback)
        {
            return SetOnPaused(callback);
        }

        ITweenBase ITweenBase.SetOnCompleted(Action callback)
        {
            return SetOnCompleted(callback);
        }

        ITweenBase ITweenBase.SetOnRewindStarted(Action callback)
        {
            return SetOnRewindStarted(callback);
        }

        ITweenBase ITweenBase.SetOnRewindPaused(Action callback)
        {
            return SetOnRewindPaused(callback);
        }

        ITweenBase ITweenBase.SetOnRewindCompleted(Action callback)
        {
            return SetOnRewindCompleted(callback);
        }

        ITweenBase ITweenBase.SetOnDisposing(Action callback)
        {
            return SetOnDisposing(callback);
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