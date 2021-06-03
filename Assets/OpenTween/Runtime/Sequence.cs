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
    public readonly struct Sequence : IDisposable
    {
        private readonly int _version;
        private readonly int _index;

        public Sequence(int index, int version)
        {
            _version = version;
            _index = index;
        }

        public static Sequence Create()
        {
            int id = SequenceRegistry.Instance.New();

            ref SequenceInternal t = ref SequenceRegistry.Instance.GetByRef(id);
            t.CurrentTime = 0;

            return new Sequence(id, t.Version);
        }

        private ref SequenceOptions Options
        {
            get
            {
                ref SequenceOptions opt = ref SequenceRegistry.Instance.GetOptionsByRef(_index);
                AssertActive(opt.Version);
                return ref opt;
            }
        }

        private SequenceReferences Refs
        {
            get
            {
                SequenceReferences refs = SequenceRegistry.Instance.GetManagedReferences(_index);
                AssertActive(refs.Version);
                return refs;
            }
        }

        private ref SequenceInternal InternalSequence
        {
            get
            {
                ref SequenceInternal t = ref SequenceRegistry.Instance.GetByRef(_index);
                AssertActive(t.Version);
                return ref t;
            }
        }


        public bool DisposeOnComplete { get => Options.DisposeOnComplete; set => Options.DisposeOnComplete = value; }

        public float CurrentTime => InternalSequence.CurrentTime;

        public TweenState State => InternalSequence.State;

        public Sequence Append(float time)
        {
            AssertActive(SequenceRegistry.Instance.GetByRef(_index).Version);
            SequenceRegistry.Instance.Append(_index, time);
            return this;
        }

        public Sequence Append(Action callback)
        {
            AssertActive(SequenceRegistry.Instance.GetByRef(_index).Version);
            SequenceRegistry.Instance.Append(_index, callback);
            return this;
        }

        public Sequence Append<T>(Tween<T> tween)
        {
            AssertActive(SequenceRegistry.Instance.GetByRef(_index).Version);
            SequenceRegistry.Instance.Append(_index, tween);
            return this;
        }

        public Sequence Insert(float position, Action callback)
        {
            AssertActive(SequenceRegistry.Instance.GetByRef(_index).Version);
            SequenceRegistry.Instance.Insert(_index, position, callback);
            return this;
        }

        public Sequence Insert<T>(float position, Tween<T> tween)
        {
            AssertActive(SequenceRegistry.Instance.GetByRef(_index).Version);
            SequenceRegistry.Instance.Insert(_index, position, tween);
            return this;
        }

        public bool Play(bool restart = false)
        {
            return InternalSequence.RegistryPlay(restart);
        }

        public void Rewind(bool restart = false)
        {
            InternalSequence.RegistryRewind(restart);
        }

        public void ForceComplete()
        {
            AssertActive(SequenceRegistry.Instance.GetByRef(_index).Version);
            SequenceRegistry.Instance.ForceComplete(_index);
        }

        public void Pause()
        {
            AssertActive(SequenceRegistry.Instance.GetByRef(_index).Version);
            SequenceRegistry.Instance.Pause(_index);
        }

        public Task AwaitPlayStart()
        {
            AssertActive(SequenceRegistry.Instance.GetByRef(_index).Version);
            return SequenceRegistry.Instance.AwaitPlayStart(_index);
        }

        public Task AwaitPause()
        {
            AssertActive(SequenceRegistry.Instance.GetByRef(_index).Version);
            return SequenceRegistry.Instance.AwaitPause(_index);
        }

        public Task AwaitCompletion()
        {
            AssertActive(SequenceRegistry.Instance.GetByRef(_index).Version);
            return SequenceRegistry.Instance.AwaitCompletion(_index);
        }

        public Task AwaitRewindStart()
        {
            AssertActive(SequenceRegistry.Instance.GetByRef(_index).Version);
            return SequenceRegistry.Instance.AwaitRewindStart(_index);
        }

        public Task AwaitRewindCompletion()
        {
            AssertActive(SequenceRegistry.Instance.GetByRef(_index).Version);
            return SequenceRegistry.Instance.AwaitRewindCompletion(_index);
        }

        public Task AwaitDispose()
        {
            AssertActive(SequenceRegistry.Instance.GetByRef(_index).Version);
            return SequenceRegistry.Instance.AwaitDispose(_index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AssertActive(int coreVersion)
        {
            if (coreVersion != _version) throw new InvalidOperationException("Sequence has been recycled.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsActive()
        {
            ref SequenceInternal t = ref SequenceRegistry.Instance.GetByRef(_index);
            return t.Version == _version;
        }

        public void Dispose()
        {
            if (!IsActive()) return;
            SequenceRegistry.Instance.Return(_index);
        }
    }
}