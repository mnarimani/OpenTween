using System;
using UnityEngine;

namespace OpenTween.Jobs
{
    internal class SequenceRegistry : RegistryBase<SequenceInternal, SequenceOptions, SequenceReferences, SequenceRegistry>
    {
        private float _lastDeltaTime;

        public void Append(int index, Action callback)
        {
            ref var seq = ref GetOptionsByRef(index);
            SequenceReferences refs = GetManagedReferences(index);
            refs.Callbacks.Add(new SequencedCallback(seq.Duration, callback));
        }

        public void Append(int index, float time)
        {
            ref var seq = ref GetOptionsByRef(index);
            seq.Duration += time;
        }

        public void Append<T>(int index, Tween<T> tween)
        {
            ref var seq = ref GetOptionsByRef(index);
            Insert(index, seq.Duration, tween);
        }

        public void Insert(int index, float position, Action callback)
        {
            SequenceReferences refs = GetManagedReferences(index);
            refs.Callbacks.Add(new SequencedCallback(position, callback));
        }

        public void Insert<T>(int index, float position, Tween<T> tween)
        {
            SequenceReferences refs = GetManagedReferences(index);
            ref var seq = ref GetOptionsByRef(index);

            ref TweenInternal<T> tweenInternalTween = ref tween.InternalTween;
            tween.Options.DisposeOnComplete = false;
            tween.Options.AutoPlay = false;
            refs.Tweens.Add(new SequencedTween(
                position,
                tweenInternalTween.Index,
                tweenInternalTween.Version,
                tweenIndex => TweenRegistry<T>.Instance.GetByInterface(tweenIndex)
            ));

            seq.Duration = Mathf.Max(position + tween.Duration, seq.Duration);
        }

        public void Join<T>(int index, Tween<T> tween)
        {
            SequenceReferences refs = GetManagedReferences(index);
            float position = refs.Tweens[refs.Tweens.Count - 1].Position;
            Insert(index, position, tween);
        }

        protected override void Schedule(float dt)
        {
            _lastDeltaTime = dt;
            if (!IsInitialized)
                return;

            base.Schedule(dt);

            foreach (int index in ActiveIndices)
            {
                var refs = GetManagedReferences(index);
                ref SequenceInternal seq = ref GetByRef(index);
                ref SequenceOptions option = ref GetOptionsByRef(index);
                var preUpdateTime = seq.CurrentTime;
                
                if (!TweenLogic.UpdateSequenceTime(ref seq, ref option, dt)) 
                    continue;
                
                foreach (SequencedCallback callback in refs.Callbacks)
                {
                    bool isRewind = seq.State == TweenState.RewindRunning;
                    float p = callback.Position;

                    if ((!isRewind && p >= preUpdateTime && p <= seq.CurrentTime) ||
                        (isRewind && p >= seq.CurrentTime && p <= preUpdateTime))
                    {
                        try
                        {
                            callback.Callback.Invoke();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                }

                foreach (SequencedTween nestedTween in refs.Tweens)
                {
                    ITweenBaseInternal tween = nestedTween.TweenGetter(nestedTween.TweenIndex);
                    if (nestedTween.Version != tween.Version)
                        continue;

                    float delta = seq.CurrentTime - nestedTween.Position;
                    if (delta < 0)
                    {
                        tween.State = TweenState.NotPlayed;
                    }
                    else if (delta > tween.GetDurationFromRegistry())
                    {
                        tween.State = TweenState.Completed;
                    }
                    else
                    {
                        if (seq.State == TweenState.Running && seq.State != tween.State)
                            tween.RegistryPlay();
                        else if (seq.State == TweenState.RewindRunning && seq.State != tween.State)
                            tween.RegistryRewind();

                        tween.RegistrySetTime(delta - _lastDeltaTime);
                    }
                }
            }
        }

        protected override void ProcessPostComplete(int index, ref SequenceInternal seq, ref SequenceOptions options, SequenceReferences refs)
        {
            if (seq.IsCompletedInLastFrame && (options.LoopCount == -1 || seq.CurrentLoopCount < options.LoopCount))
            {
                switch (options.LoopType)
                {
                    case LoopType.Restart when seq.State == TweenState.Completed:
                    {
                        seq.State = TweenState.Running;
                        seq.CurrentTime = 0;
                        break;
                    }
                    case LoopType.Restart when seq.State == TweenState.RewindCompleted:
                    {
                        seq.State = TweenState.RewindRunning;
                        seq.CurrentTime = options.Duration;
                        break;
                    }
                    case LoopType.YoYo when seq.State == TweenState.Completed:
                    {
                        seq.State = TweenState.RewindRunning;
                        break;
                    }
                    case LoopType.YoYo when seq.State == TweenState.RewindCompleted:
                    {
                        seq.State = TweenState.Running;
                        break;
                    }
                    case LoopType.Incremental:
                        throw new ArgumentException("Incremental Looping is not supported for sequences");
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                seq.IsCompletedInLastFrame = false;
            }
        }
    }
}