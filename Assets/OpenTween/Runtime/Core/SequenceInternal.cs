// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace OpenTween
// {
//     internal class SequenceInternal : TweenBase<SequenceInternal>
//     {
//         public List<SequencedCallback> Callbacks { get; } = new List<SequencedCallback>();
//         public List<SequencedTween> Tweens { get; } = new List<SequencedTween>();
//         public float LastInsertPosition { get; set; }
//         public SequenceOptions Options { get; }
//         public override float Duration => LastInsertPosition;
//
//         public SequenceInternal()
//         {
//             Options = new SequenceOptions();
//         }
//
//         public override void ForceComplete()
//         {
//             if (HasBoundComponent && BoundComponent == null)
//             {
//                 Dispose();
//                 return;
//             }
//
//             float delta;
//
//             if (State == TweenState.Running)
//             {
//                 delta = LastInsertPosition - CurrentTime;
//                 State = TweenState.Completed;
//             }
//             else if (State == TweenState.RewindRunning)
//             {
//                 delta = -CurrentTime;
//                 State = TweenState.RewindCompleted;
//             }
//             else
//             {
//                 throw new InvalidOperationException($"Sequence is not running. State: `{State}`");
//             }
//
//             Update(delta, true);
//         }
//
//         public void Append(Action callback)
//         {
//             Callbacks.Add(new SequencedCallback(LastInsertPosition, callback));
//         }
//
//         public void Append(float time)
//         {
//             LastInsertPosition += time;
//         }
//
//         public void Append(TweenBase tween)
//         {
//             tween.IsPartOfSequence = true;
//             tween.State = TweenState.NotPlayed;
//             Tweens.Add(new SequencedTween(LastInsertPosition, tween.Version, tween.Version));
//
//             LastInsertPosition += tween.Duration;
//         }
//
//         public void Insert(float position, Action callback)
//         {
//             Callbacks.Add(new SequencedCallback(position, callback));
//         }
//
//         public void Insert<T>(float position, Tween<T> tween)
//         {
//             tween.Core.IsPartOfSequence = true;
//             Tweens.Add(new SequencedTween(position, tween.Core));
//         }
//
//         public override bool Update(float dt, bool isFromSequence)
//         {
//             float last = CurrentTime;
//
//             if (!base.Update(dt, isFromSequence))
//                 return false;
//
//             foreach (SequencedCallback callback in Callbacks)
//             {
//                 float p = callback.Position;
//                 bool isRewind = State == TweenState.RewindRunning;
//
//
//                 if ((!isRewind && p > last && p <= CurrentTime) ||
//                     (isRewind && p > CurrentTime && p <= last))
//                 {
//                     try
//                     {
//                         callback.Callback.Invoke();
//                     }
//                     catch (Exception e)
//                     {
//                         Debug.LogException(e);
//                     }
//                 }
//             }
//
//             foreach (SequencedTween tween in Tweens)
//             {
//                 if (tween.Version != tween.Tween.Version)
//                     continue;
//
//                 float delta = CurrentTime - tween.Position;
//                 if (delta > 0 & delta <= tween.Tween.Duration)
//                 {
//                     if (State == TweenState.Running)
//                         tween.Tween.Play();
//                     else
//                         tween.Tween.Rewind();
//
//                     tween.Tween.CurrentTime = delta - dt;
//                     tween.Tween.Update(dt, true);
//                 }
//             }
//
//             return true;
//         }
//
//         public override void Dispose()
//         {
//             base.Dispose();
//
//             foreach (SequencedTween t in Tweens)
//             {
//                 if (t.Version == t.Tween.Version)
//                     t.Tween.Dispose();
//             }
//
//             Callbacks.Clear();
//             Tweens.Clear();
//             LastInsertPosition = 0;
//
//             Options.Reset();
//         }
//
//         protected override IOptionsBase GetOptions()
//         {
//             return Options;
//         }
//     }
// }