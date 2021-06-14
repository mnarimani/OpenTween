using System;
using Mono.Cecil;

namespace OpenTween
{
    public partial struct Tween<T>
    {
        public event Action PlayStarted { add => Refs.PlayStarted.Add(value); remove => Refs.PlayStarted.Remove(value); }
        public event Action Paused { add => Refs.Paused.Add(value); remove => Refs.Paused.Remove(value); }
        public event Action Completed { add => Refs.Completed.Add(value); remove => Refs.Completed.Remove(value); }
        public event Action RewindStarted { add => Refs.RewindStarted.Add(value); remove => Refs.RewindStarted.Remove(value); }
        public event Action RewindPaused { add => Refs.RewindPaused.Add(value); remove => Refs.RewindPaused.Remove(value); }
        public event Action RewindCompleted { add => Refs.RewindCompleted.Add(value); remove => Refs.RewindCompleted.Remove(value); }
        public event Action Disposing { add => Refs.Disposing.Add(value); remove => Refs.Disposing.Remove(value); }
        public event Action<T> ValueUpdated { add => Refs.ValueUpdated.Add(value); remove => Refs.ValueUpdated.Remove(value); } 
        
        public Tween<T> SetOnPlayStarted(Action callback)
        {
            Refs.PlayStarted.Add(callback);
            return this;
        }

        public Tween<T> SetOnPaused(Action callback)
        {
            Refs.Paused.Add(callback);
            return this;
        }

        public Tween<T> SetOnCompleted(Action callback)
        {
            Refs.Completed.Add(callback);
            return this;
        }

        public Tween<T> SetOnRewindStarted(Action callback)
        {
            Refs.RewindStarted.Add(callback);
            return this;
        }

        public Tween<T> SetOnRewindPaused(Action callback)
        {
            Refs.RewindPaused.Add(callback);
            return this;
        }

        public Tween<T> SetOnRewindCompleted(Action callback)
        {
            Refs.RewindCompleted.Add(callback);
            return this;
        }

        public Tween<T> SetOnDisposing(Action callback)
        {
            Refs.Disposing.Add(callback);
            return this;
        }

        public Tween<T> SetOnValueUpdated(Action<T> callback)
        {
            Refs.ValueUpdated.Add(callback);
            return this;
        }
    }
}