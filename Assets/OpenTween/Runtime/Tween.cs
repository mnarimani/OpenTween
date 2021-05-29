using System;
using System.Runtime.CompilerServices;
using UnityEngine;
#if UNITASK
using Task = Cysharp.Threading.Tasks.UniTask;
using TaskCompletionSource = Cysharp.Threading.Tasks.AutoResetUniTaskCompletionSource;

#else
using Task = System.Threading.Tasks.Task;
using TaskCompletionSource = System.Threading.Tasks.TaskCompletionSource<bool>;

#endif


namespace OpenTween
{
    public interface ITween
    {
        event Action PlayStarted;
        event Action Paused;
        event Action Completed;
        event Action RewindStarted;
        event Action RewindPaused;
        event Action RewindCompleted;
        event Action Disposing;
        bool Play(bool restart = false);
        void Rewind(bool restart = false);
        void ForceComplete();
        void Pause();
        Task AwaitPlayStart();
        Task AwaitPause();
        Task AwaitCompletion();
        Task AwaitRewindStart();
        Task AwaitRewindCompletion();
        Task AwaitDispose();
        bool IsActive();
    }

    public struct Tween
    {
        public static Tween<T> Create<T>()
        {
            return new Tween<T>(TweenPool<TweenInternal<T>>.GetNew());
        }
    }

    public readonly struct Tween<T> : IDisposable, ITween
    {
        private readonly int _version;

        internal Tween(TweenInternal<T> core)
        {
            Core = core;
            _version = Core.Version;
        }

        internal TweenInternal<T> Core { get; }

        public T Start
        {
            get
            {
                AssertActive();
                return Core.Options.Start;
            }
            set
            {
                AssertActive();
                Core.Options.Start = value;
            }
        }

        public T End
        {
            get
            {
                AssertActive();
                return Core.Options.End;
            }
            set
            {
                AssertActive();
                Core.Options.End = value;
            }
        }

        public Ease Ease
        {
            get
            {
                AssertActive();
                return Core.Options.Ease;
            }
            set
            {
                AssertActive();
                Core.Options.Ease = value;
            }
        }

        public EaseFunc CustomEase
        {
            get
            {
                AssertActive();
                return Core.Options.CustomEase;
            }
            set
            {
                AssertActive();
                Core.Options.CustomEase = value;
            }
        }

        public float OvershootOrAmplitude
        {
            get
            {
                AssertActive();
                return Core.Options.OvershootOrAmplitude;
            }
            set
            {
                AssertActive();
                Core.Options.OvershootOrAmplitude = value;
            }
        }

        public float Period
        {
            get
            {
                AssertActive();
                return Core.Options.Period;
            }
            set
            {
                AssertActive();
                Core.Options.Period = value;
            }
        }

        public Func<T> StartEvalFunc
        {
            get
            {
                AssertActive();
                return Core.Options.StartEvalFunc;
            }
            set
            {
                AssertActive();
                Core.Options.StartEvalFunc = value;
            }
        }

        public bool DynamicStartEval
        {
            get
            {
                AssertActive();
                return Core.Options.DynamicStartEval;
            }
            set
            {
                AssertActive();
                Core.Options.DynamicStartEval = value;
            }
        }

        public bool IsLocal
        {
            get
            {
                AssertActive();
                return Core.Options.IsLocal;
            }
            set
            {
                AssertActive();
                Core.Options.IsLocal = value;
            }
        }

        public bool IsRelative
        {
            get
            {
                AssertActive();
                return Core.Options.IsRelative;
            }
            set
            {
                AssertActive();
                Core.Options.IsRelative = value;
            }
        }

        public float Duration
        {
            get
            {
                AssertActive();
                return Core.Options.Duration;
            }
            set
            {
                AssertActive();
                Core.Options.Duration = value;
            }
        }

        public bool DisposeOnComplete
        {
            get
            {
                AssertActive();
                return Core.Options.DisposeOnComplete;
            }
            set
            {
                AssertActive();
                Core.Options.DisposeOnComplete = value;
            }
        }

        public T CurrentValue
        {
            get
            {
                AssertActive();
                return Core.CurrentValue;
            }
        }

        public float CurrentTime
        {
            get
            {
                AssertActive();
                return Core.CurrentTime;
            }
        }

        public TweenState State
        {
            get
            {
                AssertActive();
                return Core.State;
            }
        }

        public event Action<T> ValueUpdated
        {
            add
            {
                AssertActive();
                Core.ValueUpdated += value;
            }
            remove
            {
                AssertActive();
                Core.ValueUpdated -= value;
            }
        }

        public event Action PlayStarted
        {
            add
            {
                AssertActive();
                Core.PlayStarted += value;
            }
            remove
            {
                AssertActive();
                Core.PlayStarted -= value;
            }
        }

        public event Action Paused
        {
            add
            {
                AssertActive();
                Core.Paused += value;
            }
            remove
            {
                AssertActive();
                Core.Paused -= value;
            }
        }

        public event Action Completed
        {
            add
            {
                AssertActive();
                Core.Completed += value;
            }
            remove
            {
                AssertActive();
                Core.Completed -= value;
            }
        }

        public event Action RewindStarted
        {
            add
            {
                AssertActive();
                Core.RewindStarted += value;
            }
            remove
            {
                AssertActive();
                Core.RewindStarted -= value;
            }
        }

        public event Action RewindPaused
        {
            add
            {
                AssertActive();
                Core.RewindPaused += value;
            }
            remove
            {
                AssertActive();
                Core.RewindPaused -= value;
            }
        }

        public event Action RewindCompleted
        {
            add
            {
                AssertActive();
                Core.RewindCompleted += value;
            }
            remove
            {
                AssertActive();
                Core.RewindCompleted -= value;
            }
        }

        public event Action Disposing
        {
            add
            {
                AssertActive();
                Core.Disposing += value;
            }
            remove
            {
                AssertActive();
                Core.Disposing -= value;
            }
        }

        public void CopyOptionsFrom(TweenOptions<T> other)
        {
            AssertActive();
            Core.CopyOptionsFrom(other);
        }

        public bool Play(bool restart = false)
        {
            AssertActive();
            return Core.Play(restart);
        }

        public void Rewind(bool restart = false)
        {
            AssertActive();
            Core.Rewind(restart);
        }

        public void ForceComplete()
        {
            AssertActive();
            Core.ForceComplete();
        }

        public void Pause()
        {
            AssertActive();
            Core.Pause();
        }

        public Task AwaitPlayStart()
        {
            AssertActive();
            return Core.AwaitPlayStart();
        }

        public Task AwaitPause()
        {
            AssertActive();
            return Core.AwaitPause();
        }

        public Task AwaitCompletion()
        {
            AssertActive();
            return Core.AwaitCompletion();
        }

        public Task AwaitRewindStart()
        {
            AssertActive();
            return Core.AwaitRewindStart();
        }

        public Task AwaitRewindCompletion()
        {
            AssertActive();
            return Core.AwaitRewindCompletion();
        }

        public Task AwaitDispose()
        {
            AssertActive();
            return Core.AwaitDispose();
        }

        public void BindToComponent(Component c)
        {
            AssertActive();
            Core.BindToComponent(c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void AssertActive()
        {
            if (Core.Version != _version) throw new InvalidOperationException("Tween has been recycled.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsActive()
        {
            return Core.Version == _version;
        }

        public void Dispose()
        {
            if (!IsActive()) return;
            Core.Dispose();
        }
    }
}