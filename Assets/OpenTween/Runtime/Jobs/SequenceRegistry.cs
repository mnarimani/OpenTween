using System;
using UnityEngine;

namespace OpenTween.Jobs
{
    internal class SequenceRegistry : RegistryBase<SequenceInternal, SequenceOptions, SequenceReferences, SequenceRegistry>
    {
        public void Append(int index, Action callback)
        {
            SequenceReferences refs = GetManagedReferences(index);
            refs.Callbacks.Add(new SequencedCallback(refs.LastInsertPosition, callback));
        }

        public void Append(int index, float time)
        {
            SequenceReferences refs = GetManagedReferences(index);
            refs.LastInsertPosition += time;
        }

        public void Append<T>(int index, Tween<T> tween)
        {
            SequenceReferences refs = GetManagedReferences(index);
            Insert(index, refs.LastInsertPosition, tween);
        }

        public void Insert(int index, float position, Action callback)
        {
            SequenceReferences refs = GetManagedReferences(index);
            refs.Callbacks.Add(new SequencedCallback(position, callback));
        }

        public void Insert<T>(int index, float position, Tween<T> tween)
        {
            SequenceReferences refs = GetManagedReferences(index);
            ref TweenInternal<T> tweenInternalTween = ref tween.InternalTween;
            tween.Options.DisposeOnComplete = false;
            tween.Options.AutoPlay = false;
            refs.Tweens.Add(new SequencedTween(
                position,
                tweenInternalTween.Index,
                tweenInternalTween.Version,
                tweenIndex => TweenRegistry<T>.Instance.GetByInterface(tweenIndex)
            ));

            refs.LastInsertPosition = Mathf.Max(position + tween.Duration, refs.LastInsertPosition);
        }

        public void Join<T>(int index, Tween<T> tween)
        {
            SequenceReferences refs = GetManagedReferences(index);
            float position = refs.Tweens[refs.Tweens.Count - 1].Position;
            Insert(index, position, tween);
        }

        protected override void Schedule(float dt)
        {
            if (!IsInitialized)
                return;

            base.Schedule(dt);

            foreach (int index in ActiveIndices)
            {
                ref SequenceInternal seq = ref GetByRef(index);
                ref SequenceOptions option = ref GetOptionsByRef(index);
                SequenceReferences refs = GetManagedReferences(index);

                option.Duration = refs.LastInsertPosition;

                float last = seq.CurrentTime;

                if (!TweenLogic.UpdateTime(ref seq, option.Duration, dt))
                    continue;

                foreach (SequencedCallback callback in refs.Callbacks)
                {
                    bool isRewind = seq.State == TweenState.RewindRunning;
                    float p = callback.Position;

                    if ((!isRewind && p >= last && p <= seq.CurrentTime) ||
                        (isRewind && p >= seq.CurrentTime && p <= last))
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
                    else if (delta > tween.Duration)
                    {
                        tween.State = TweenState.Completed;
                    }
                    else 
                    {
                        if (seq.State == TweenState.Running && seq.State != tween.State)
                            tween.ReadonlyPlay();
                        else if (seq.State == TweenState.RewindRunning && seq.State != tween.State)
                            tween.ReadonlyRewind();

                        tween.ReadonlySetTime(delta - dt);
                    }
                }
            }
        }

        protected override void ProcessPostComplete(int index, ref SequenceInternal tween, ref SequenceOptions options, SequenceReferences refs)
        {
        }
    }
}