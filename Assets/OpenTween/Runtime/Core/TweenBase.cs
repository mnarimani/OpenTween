// using System;
// using UnityEngine;
// #if UNITASK
// using Task = Cysharp.Threading.Tasks.UniTask;
// using TaskCompletionSource = Cysharp.Threading.Tasks.AutoResetUniTaskCompletionSource;
//
// #else
// using Task = System.Threading.Tasks.Task;
// using TaskCompletionSource = System.Threading.Tasks.TaskCompletionSource<bool>;
//
// #endif
//
// namespace OpenTween
// {
//     internal abstract class TweenBase : IDisposable
//     {
//         public float CurrentTime { get; set; }
//         public TweenState State { get; set; } = TweenState.NotPlayed;
//         public int Version { get; private set; }
//         public bool IsPartOfSequence { get; set; }
//         public string CreationStacktrace { get; set; }
//         protected bool HasBoundComponent { get; private set; }
//         protected Component BoundComponent { get; private set; }
//
//         public event Action PlayStarted;
//         public event Action Paused;
//         public event Action Completed;
//         public event Action RewindStarted;
//         public event Action RewindPaused;
//         public event Action RewindCompleted;
//         public event Action Disposing;
//
//         public abstract float Duration { get; }
//
//         public virtual bool Play(bool restart = false)
//         {
//             if (HasBoundComponent && BoundComponent == null)
//             {
//                 Dispose();
//                 return false;
//             }
//             
//             if (restart)
//             {
//                 CurrentTime = 0;
//             }
//             else
//             {
//                 if (State != TweenState.NotPlayed && State != TweenState.Paused && State != TweenState.RewindPaused && State != TweenState.RewindCompleted)
//                 {
//                     return false;
//                 }
//             }
//
//             State = TweenState.Running;
//             PlayStarted?.Invoke();
//             return true;
//         }
//
//         public void Rewind(bool restart = false)
//         {
//             if (HasBoundComponent && BoundComponent == null)
//             {
//                 Dispose();
//                 return;
//             }
//             
//             if (restart)
//             {
//                 CurrentTime = Duration;
//             }
//             else
//             {
//                 if (State != TweenState.NotPlayed && State != TweenState.Paused && State != TweenState.RewindPaused && State != TweenState.Completed)
//                 {
//                     return;
//                 }
//             }
//
//             State = TweenState.RewindRunning;
//             RewindStarted?.Invoke();
//         }
//
//         public void Pause()
//         {
//             if (HasBoundComponent && BoundComponent == null)
//             {
//                 Dispose();
//                 return;
//             }
//             
//             if (State == TweenState.Running)
//             {
//                 State = TweenState.Paused;
//                 Paused?.Invoke();
//             }
//             else if (State == TweenState.RewindRunning)
//             {
//                 State = TweenState.RewindPaused;
//                 RewindPaused?.Invoke();
//             }
//         }
//
//         public Task AwaitPlayStart()
//         {
//             if (HasBoundComponent && BoundComponent == null)
//             {
//                 Dispose();
//                 return Task.CompletedTask;
//             }
//             
//             TaskCompletionSource source = CreateTaskCompletionSource();
//
//             PlayStarted += SetResult;
//
//             return source.Task;
//
//             void SetResult()
//             {
//                 SetTaskCompletionResult(source);
//                 PlayStarted -= SetResult;
//             }
//         }
//
//         public Task AwaitPause()
//         {
//             if (HasBoundComponent && BoundComponent == null)
//             {
//                 Dispose();
//                 return Task.CompletedTask;
//             }
//             
//             TaskCompletionSource source = CreateTaskCompletionSource();
//
//             Paused += SetResult;
//
//             return source.Task;
//
//             void SetResult()
//             {
//                 SetTaskCompletionResult(source);
//                 Paused -= SetResult;
//             }
//         }
//
//         public Task AwaitCompletion()
//         {
//             if (HasBoundComponent && BoundComponent == null)
//             {
//                 Dispose();
//                 return Task.CompletedTask;
//             }
//             
//             TaskCompletionSource source = CreateTaskCompletionSource();
//
//             Completed += SetResult;
//
//             return source.Task;
//
//             void SetResult()
//             {
//                 SetTaskCompletionResult(source);
//                 Completed -= SetResult;
//             }
//         }
//
//         public Task AwaitRewindStart()
//         {
//             if (HasBoundComponent && BoundComponent == null)
//             {
//                 Dispose();
//                 return Task.CompletedTask;
//             }
//             
//             TaskCompletionSource source = CreateTaskCompletionSource();
//
//             RewindStarted += SetResult;
//
//             return source.Task;
//
//             void SetResult()
//             {
//                 SetTaskCompletionResult(source);
//                 RewindStarted -= SetResult;
//             }
//         }
//
//         public Task AwaitRewindCompletion()
//         {
//             if (HasBoundComponent && BoundComponent == null)
//             {
//                 Dispose();
//                 return Task.CompletedTask;
//             }
//             
//             TaskCompletionSource source = CreateTaskCompletionSource();
//
//             RewindCompleted += SetResult;
//
//             return source.Task;
//
//             void SetResult()
//             {
//                 SetTaskCompletionResult(source);
//                 RewindCompleted -= SetResult;
//             }
//         }
//
//         public Task AwaitDispose()
//         {
//             if (HasBoundComponent && BoundComponent == null)
//             {
//                 Dispose();
//                 return Task.CompletedTask;
//             }
//             
//             TaskCompletionSource source = CreateTaskCompletionSource();
//
//             Disposing += SetResult;
//
//             return source.Task;
//
//             void SetResult()
//             {
//                 SetTaskCompletionResult(source);
//                 Disposing -= SetResult;
//             }
//         }
//
//         public virtual void ForceComplete()
//         {
//             if (HasBoundComponent && BoundComponent == null)
//             {
//                 Dispose();
//                 return;
//             }
//             
//             if (State == TweenState.Running)
//             {
//                 CurrentTime = Duration;
//                 State = TweenState.Completed;
//             }
//             else if (State == TweenState.RewindRunning)
//             {
//                 CurrentTime = 0;
//                 State = TweenState.RewindCompleted;
//             }
//         }
//
//         public virtual bool Update(float dt, bool isFromSequence)
//         {
//             if (IsPartOfSequence != isFromSequence)
//                 return false;
//
//             if (HasBoundComponent && BoundComponent == null)
//             {
//                 Dispose();
//                 return false;
//             }
//
//             // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
//             switch (State)
//             {
//                 case TweenState.Running:
//                 {
//                     CurrentTime += dt;
//
//                     if (CurrentTime >= Duration)
//                     {
//                         CurrentTime = Duration;
//                         State = TweenState.Completed;
//                     }
//
//                     if (State == TweenState.Completed)
//                     {
//                         try
//                         {
//                             Completed?.Invoke();
//                         }
//                         catch (Exception e)
//                         {
//                             ReportWithStacktrace();
//                             Debug.LogException(e);
//                         }
//
//                         if (!IsPartOfSequence && GetOptions().DisposeOnComplete)
//                             Dispose();
//                     }
//
//                     return true;
//                 }
//                 case TweenState.RewindRunning:
//                 {
//                     CurrentTime -= dt;
//
//                     if (CurrentTime <= 0)
//                     {
//                         State = TweenState.RewindCompleted;
//                         CurrentTime = 0;
//                     }
//
//                     if (State == TweenState.RewindCompleted)
//                     {
//                         try
//                         {
//                             RewindCompleted?.Invoke();
//                         }
//                         catch (Exception e)
//                         {
//                             ReportWithStacktrace();
//                             Debug.LogException(e);
//                         }
//                     }
//
//                     return true;
//                 }
//                 default:
//                     return false;
//             }
//         }
//
//         public virtual void Dispose()
//         {
//             try
//             {
//                 Disposing?.Invoke();
//             }
//             catch (Exception e)
//             {
//                 ReportWithStacktrace();
//                 Debug.LogException(e);
//             }
//
//             Version++;
//             PlayStarted = null;
//             Paused = null;
//             Completed = null;
//             RewindStarted = null;
//             RewindPaused = null;
//             RewindCompleted = null;
//             Disposing = null;
//             State = TweenState.NotPlayed;
//             CurrentTime = 0;
//             IsPartOfSequence = false;
//             BoundComponent = null;
//             HasBoundComponent = false;
//         }
//
//         public void BindToComponent(Component c)
//         {
//             BoundComponent = c;
//             HasBoundComponent = true;
//         }
//
//         protected void ReportWithStacktrace()
//         {
//             if (OpenTweenSettings.CaptureCreationStacktrace)
//                 Debug.LogError($"Tween/Sequence has hit errors. Tween Creation StackTrace:\n{CreationStacktrace}");
//             else
//                 Debug.LogError("Tween/Sequence has hit errors. CaptureCreationStacktrace is off in options. Creation Stacktrace is not available.");
//         }
//
//         protected abstract IOptionsBase GetOptions();
//
//         protected static TaskCompletionSource CreateTaskCompletionSource()
//         {
// #if UNITASK
//             return TaskCompletionSource.Create();
// #else
//             return new TaskCompletionSource<bool>();
// #endif
//         }
//
//         protected static void SetTaskCompletionResult(TaskCompletionSource source)
//         {
//             try
//             {
// #if UNITASK
//                 source.TrySetResult();
// #else
//                 source.TrySetResult(false);
// #endif
//             }
//             catch (Exception e)
//             {
//                 Debug.LogException(e);
//             }
//         }
//
//         public void OnPoolSpawned()
//         {
//             
//         }
//     }
//
//     internal abstract class TweenBase<TClass> : TweenBase where TClass : TweenBase<TClass>, new()
//     {
//         public override void Dispose()
//         {
//             base.Dispose();
//             TweenPool<TClass>.Return((TClass) this);
//         }
//     }
// }