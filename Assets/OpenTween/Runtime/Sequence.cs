// using System;
// using System.Runtime.CompilerServices;
// using UnityEngine;
// #if UNITASK
// using Task = Cysharp.Threading.Tasks.UniTask;
// using TaskCompletionSource = Cysharp.Threading.Tasks.AutoResetUniTaskCompletionSource;
//
// #else
// using Task = System.Threading.Tasks.Task;
// using TaskCompletionSource = System.Threading.Tasks.TaskCompletionSource<bool>;
// #endif
//
//
// namespace OpenTween
// {
//     public readonly struct Sequence : IDisposable
//     {
//         private readonly int _version;
//         private readonly SequenceInternal _core;
//
//         internal Sequence(SequenceInternal core)
//         {
//             _core = core;
//             _version = _core.Version;
//         }
//
//         /*
//         public static Sequence Create()
//         {
//             return new Sequence(TweenPool<SequenceInternal>.GetNew());
//         }*/
//
//         public bool DisposeOnComplete
//         {
//             get
//             {
//                 AssertActive();
//                 return _core.Options.DisposeOnComplete;
//             }
//             set
//             {
//                 AssertActive();
//                 _core.Options.DisposeOnComplete = value;
//             }
//         }
//
//         public float CurrentTime
//         {
//             get
//             {
//                 AssertActive();
//                 return _core.CurrentTime;
//             }
//         }
//
//         public TweenState State
//         {
//             get
//             {
//                 AssertActive();
//                 return _core.State;
//             }
//         }
//
//         public event Action PlayStarted
//         {
//             add
//             {
//                 AssertActive();
//                 _core.PlayStarted += value;
//             }
//             remove
//             {
//                 AssertActive();
//                 _core.PlayStarted -= value;
//             }
//         }
//
//         public event Action Paused
//         {
//             add
//             {
//                 AssertActive();
//                 _core.Paused += value;
//             }
//             remove
//             {
//                 AssertActive();
//                 _core.Paused -= value;
//             }
//         }
//
//         public event Action Completed
//         {
//             add
//             {
//                 AssertActive();
//                 _core.Completed += value;
//             }
//             remove
//             {
//                 AssertActive();
//                 _core.Completed -= value;
//             }
//         }
//
//         public event Action RewindStarted
//         {
//             add
//             {
//                 AssertActive();
//                 _core.RewindStarted += value;
//             }
//             remove
//             {
//                 AssertActive();
//                 _core.RewindStarted -= value;
//             }
//         }
//
//         public event Action RewindPaused
//         {
//             add
//             {
//                 AssertActive();
//                 _core.RewindPaused += value;
//             }
//             remove
//             {
//                 AssertActive();
//                 _core.RewindPaused -= value;
//             }
//         }
//
//         public event Action RewindCompleted
//         {
//             add
//             {
//                 AssertActive();
//                 _core.RewindCompleted += value;
//             }
//             remove
//             {
//                 AssertActive();
//                 _core.RewindCompleted -= value;
//             }
//         }
//
//         public event Action Disposing
//         {
//             add
//             {
//                 AssertActive();
//                 _core.Disposing += value;
//             }
//             remove
//             {
//                 AssertActive();
//                 _core.Disposing -= value;
//             }
//         }
//
//         public Sequence Append(float time)
//         {
//             AssertActive();
//             _core.Append(time);
//             return this;
//         }
//
//         public Sequence Append(Action callback)
//         {
//             AssertActive();
//             _core.Append(callback);
//             return this;
//         }
//
//         public Sequence Append<T>(Tween<T> tween)
//         {
//             AssertActive();
//             tween.AssertActive();
//             _core.Append(tween.Core);
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
//             AssertActive();
//             tween.AssertActive();
//             _core.Insert(position, tween);
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
//         private void AssertActive()
//         {
//             if (_core.Version != _version) throw new InvalidOperationException("Sequence has been recycled.");
//         }
//
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         public bool IsActive()
//         {
//             return _core.Version == _version;
//         }
//
//         public void Dispose()
//         {
//             if (!IsActive()) return;
//             _core.Dispose();
//         }
//     }
// }