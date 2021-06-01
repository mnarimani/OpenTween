// using System;
// using System.Runtime.CompilerServices;
// using OpenTween.Jobs;
// using UnityEngine;
// #if UNITASK
// using Task = Cysharp.Threading.Tasks.UniTask;
// using TaskCompletionSource = Cysharp.Threading.Tasks.AutoResetUniTaskCompletionSource;
//
// #else
//  using Task = System.Threading.Tasks.Task;
//  using TaskCompletionSource = System.Threading.Tasks.TaskCompletionSource<bool>;
// #endif
//
//
// namespace OpenTween
// {
//     public readonly struct Sequence : IDisposable
//     {
//         private readonly int _version;
//         private readonly int _index;
//
//         public Sequence(int version, int index)
//         {
//             _version = version;
//             _index = index;
//         }
//
//         private ref SequenceOptions Options
//         {
//             get
//             {
//                 ref SequenceOptions opt = ref SequenceRegistry.Instance.GetOptionsByRef(_index);
//                 AssertActive(opt.Version);
//                 return ref opt;
//             }
//         }
//
//         private SequenceReferences Refs
//         {
//             get
//             {
//                 SequenceReferences refs = SequenceRegistry.Instance.GetManagedReferences(_index);
//                 AssertActive(refs.Version);
//                 return refs;
//             }
//         }
//
//         private ref SequenceInternal InternalSequence
//         {
//             get
//             {
//                 ref SequenceInternal t = ref SequenceRegistry.Instance.GetByRef(_index);
//                 AssertActive(t.Version);
//                 return ref t;
//             }
//         }
//
//
//         public static Sequence Create()
//         {
//             int id = SequenceRegistry.Instance.New();
//
//             ref SequenceInternal t = ref SequenceRegistry.Instance.GetByRef(id);
//             t.CurrentTime = 0;
//
//             return new Sequence(id, t.Version);
//         }
//
//         public bool DisposeOnComplete { get => Options.DisposeOnComplete; set => Options.DisposeOnComplete = value; }
//
//         public float CurrentTime => InternalSequence.CurrentTime;
//
//         public TweenState State => InternalSequence.State;
//
//         public Sequence Append(float time)
//         {
//             _core.Append(time);
//             return this;
//         }
//
//         public Sequence Append(Action callback)
//         {
//             _core.Append(callback);
//             return this;
//         }
//
//         public Sequence Append<T>(Tween<T> tween)
//         {
//             AssertActive(InternalSequence.Version);
//
//             ref TweenInternal<T> internalTween = ref tween.InternalTween;
//             SequenceRegistry.Instance.Append(_index,
//                 internalTween.Index,
//                 internalTween.Version,
//                 TweenRegistry<T>.Instance.GetByInterface
//             );
//             return this;
//         }
//
//         public Sequence Insert(float position, Action callback)
//         {
//             AssertActive();
//             _core.Insert(position, callback);
//             return this;
//         }
//
//         public Sequence Insert<T>(float position, Tween<T> tween)
//         {
//             AssertActive(InternalSequence.Version);
//
//             ref TweenInternal<T> internalTween = ref tween.InternalTween;
//             SequenceRegistry.Instance.Insert(_index,
//                 position,
//                 internalTween.Index,
//                 internalTween.Version,
//                 TweenRegistry<T>.Instance.GetByInterface
//             );
//             return this;
//         }
//
//         public bool Play(bool restart = false)
//         {
//             AssertActive();
//             return _core.Play(restart);
//         }
//
//         public void Rewind(bool restart = false)
//         {
//             AssertActive();
//             _core.Rewind(restart);
//         }
//
//         public void ForceComplete()
//         {
//             AssertActive();
//             _core.ForceComplete();
//         }
//
//         public void Pause()
//         {
//             AssertActive();
//             _core.Pause();
//         }
//
//         public Task AwaitPlayStart()
//         {
//             AssertActive();
//             return _core.AwaitPlayStart();
//         }
//
//         public Task AwaitPause()
//         {
//             AssertActive();
//             return _core.AwaitPause();
//         }
//
//         public Task AwaitCompletion()
//         {
//             AssertActive();
//             return _core.AwaitCompletion();
//         }
//
//         public Task AwaitRewindStart()
//         {
//             AssertActive();
//             return _core.AwaitRewindStart();
//         }
//
//         public Task AwaitRewindCompletion()
//         {
//             AssertActive();
//             return _core.AwaitRewindCompletion();
//         }
//
//         public Task AwaitDispose()
//         {
//             AssertActive();
//             return _core.AwaitDispose();
//         }
//
//         public void BindToComponent(Component c)
//         {
//             AssertActive();
//             _core.BindToComponent(c);
//         }
//
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         private void AssertActive(int coreVersion)
//         {
//             if (coreVersion != _version) throw new InvalidOperationException("Sequence has been recycled.");
//         }
//
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         public bool IsActive()
//         {
//             ref SequenceInternal t = ref SequenceRegistry.Instance.GetByRef(_index);
//             return t.Version == _version;
//         }
//
//         public void Dispose()
//         {
//             if (!IsActive()) return;
//             SequenceRegistry.Instance.Return(_index);
//         }
//     }
// }