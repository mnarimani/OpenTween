using System;

namespace OpenTween
{
    public partial struct Tween<T>
    {
        ITween ITween.OnPlayStarted(Action callback)
        {
            return OnPlayStarted(callback);
        }

        ITween ITween.OnPaused(Action callback)
        {
            return OnPaused(callback);
        }

        ITween ITween.OnCompleted(Action callback)
        {
            return OnCompleted(callback);
        }

        ITween ITween.OnRewindStarted(Action callback)
        {
            return OnRewindStarted(callback);
        }

        ITween ITween.OnRewindPaused(Action callback)
        {
            return OnRewindPaused(callback);
        }

        ITween ITween.OnRewindCompleted(Action callback)
        {
            return OnRewindCompleted(callback);
        }

        ITween ITween.OnDisposing(Action callback)
        {
            return OnDisposing(callback);
        }

        ITween ITween.Play(bool restart)
        {
            return Play(restart);
        }

        ITween ITween.Rewind(bool restart)
        {
            return Rewind(restart);
        }

        ITween ITween.ForceComplete()
        {
            return ForceComplete();
        }

        ITween ITween.Pause()
        {
            return Pause();
        }
    }
}