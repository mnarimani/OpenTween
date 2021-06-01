using System;
using System.Runtime.CompilerServices;
using OpenTween.Jobs;
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
    public readonly partial struct Tween<T> : IDisposable, ITweenBase
    {
        private readonly int _index;
        private readonly int _version;

        internal Tween(int index, int version)
        {
            _version = version;
            _index = index;
        }

        private ref TweenOptions<T> Options
        {
            get
            {
                ref TweenOptions<T> opt = ref TweenRegistry<T>.Instance.GetOptionsByRef(_index);
                AssertActive(opt.Version);
                return ref opt;
            }
        }

        private TweenManagedReferences<T> Refs
        {
            get
            {
                TweenManagedReferences<T> refs = TweenRegistry<T>.Instance.GetManagedReferences(_index);
                AssertActive(refs.Version);
                return refs;
            }
        }

        internal ref TweenInternal<T> InternalTween
        {
            get
            {
                ref TweenInternal<T> t = ref TweenRegistry<T>.Instance.GetByRef(_index);
                AssertActive(t.Version);
                return ref t;
            }
        }

        public Tween<T> CopyOptionsFrom(TweenOptions<T> other)
        {
            Options.CopyFrom(other);
            return this;
        }
        
        public Tween<T> CopyOptionsFrom(ref TweenOptions<T> other)
        {
            Options.CopyFrom(ref other);
            return this;
        }

        public Tween<T> Play(bool restart = false)
        {
            TweenRegistry<T>.Instance.Play(_index, restart);
            return this;
        }

        public Tween<T> Rewind(bool restart = false)
        {
            TweenRegistry<T>.Instance.Rewind(_index, restart);
            return this;
        }

        public Tween<T> ForceComplete()
        {
            TweenRegistry<T>.Instance.ForceComplete(_index);
            return this;
        }

        public Tween<T> Pause()
        {
            TweenRegistry<T>.Instance.Pause(_index);
            return this;
        }

        public Task AwaitPlayStart()
        {
            return TweenRegistry<T>.Instance.AwaitPlayStart(_index);
        }

        public Task AwaitPause()
        {
            return TweenRegistry<T>.Instance.AwaitPause(_index);
        }

        public Task AwaitCompletion()
        {
            return TweenRegistry<T>.Instance.AwaitCompletion(_index);
        }

        public Task AwaitRewindStart()
        {
            return TweenRegistry<T>.Instance.AwaitRewindStart(_index);
        }

        public Task AwaitRewindCompletion()
        {
            return TweenRegistry<T>.Instance.AwaitRewindCompletion(_index);
        }

        public Task AwaitDispose()
        {
            return TweenRegistry<T>.Instance.AwaitDispose(_index);
        }

        public Tween<T> BindToComponent(Component c)
        {
            Refs.BoundComponent = (c);
            Refs.HasBoundComponent = true;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AssertActive(int coreVersion)
        {
            if (coreVersion != _version) throw new InvalidOperationException("Tween has been recycled.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsActive()
        {
            ref TweenInternal<T> t = ref TweenRegistry<T>.Instance.GetByRef(_index);
            return t.Version == _version;
        }

        public void Dispose()
        {
            if (!IsActive()) return;
            TweenRegistry<T>.Instance.Return(_index);
        }
    }
}