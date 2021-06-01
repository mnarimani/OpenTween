using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenTween.Jobs
{
    internal struct SequenceInternal : ITweenBaseInternal
    {
        public int Index { get; set; }
        public int Version { get; set; }
        public float CurrentTime { get; set; }
        public TweenState State { get; set; }
        public float LerpParameter { get; set; }
        public bool IsCompletedInLastFrame { get; set; }
        public bool IsRewindCompletedInLastFrame { get; set; }
        public bool IsUpdatedInLastFrame { get; set; }

        public float Duration => SequenceRegistry.Instance.GetManagedReferences(Index).LastInsertPosition;

        public void ResetToDefaults()
        {
            CurrentTime = default;
            State = TweenState.NotPlayed;
            LerpParameter = default;
            IsCompletedInLastFrame = default;
            IsRewindCompletedInLastFrame = default;
            IsUpdatedInLastFrame = default;
        }

        public void Save()
        {
            // Does this work?
            ref SequenceInternal seq = ref SequenceRegistry.Instance.GetByRef(Index);
            seq = this;
        }

        public void Play()
        {
            SequenceRegistry.Instance.Play(Index, false);
        }

        public void Rewind()
        {
            SequenceRegistry.Instance.Rewind(Index, false);
        }
    }

    internal struct SequenceOptions : IOptionsBaseInternal
    {
        public bool DisposeOnComplete { get; set; }
        public bool AutoPlay { get; set; }
        public float PrePlayDelay { get; set; }
        public float PosPlayDelay { get; set; }
        public Ease Ease { get; set; }

        public void ResetToDefaults()
        {
            throw new System.NotImplementedException();
        }

        public int Version { get; set; }
        public float Duration { get; set; }
    }

    internal class SequenceReferences : ManagedReferences
    {
        public List<SequencedCallback> Callbacks { get; } = new List<SequencedCallback>();
        public List<SequencedTween> Tweens { get; } = new List<SequencedTween>();
        public float LastInsertPosition { get; set; }
    }

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

        public void Append(int index, int tweenIndex, int tweenVersion, TweenGetter getter)
        {
            SequenceReferences refs = GetManagedReferences(index);
            refs.Tweens.Add(new SequencedTween(refs.LastInsertPosition, tweenIndex, tweenVersion, getter));
        }
        
        public void Insert(int index, float position, Action callback)
        {
            SequenceReferences refs = GetManagedReferences(index);
            refs.Callbacks.Add(new SequencedCallback(position, callback));
        }
        
        public void Insert(int index, float position, int tweenIndex, int tweenVersion, TweenGetter getter)
        {
            SequenceReferences refs = GetManagedReferences(index);
            refs.Tweens.Add(new SequencedTween(position, tweenIndex, tweenVersion, getter));
        }

        protected override void ProcessUpdate(float dt)
        {
            foreach (int index in ActiveIndices)
            {
                ref SequenceInternal seq = ref GetByRef(index);
                ref SequenceOptions option = ref GetOptionsByRef(index);

                float last = seq.CurrentTime;

                if (!TweenLogic.UpdateTime(ref seq, option.AutoPlay, option.Duration, dt))
                    continue;

                SequenceReferences refs = GetManagedReferences(index);

                foreach (SequencedCallback callback in refs.Callbacks)
                {
                    bool isRewind = seq.State == TweenState.RewindRunning;
                    float p = callback.Position;

                    if ((!isRewind && p > last && p <= seq.CurrentTime) ||
                        (isRewind && p > seq.CurrentTime && p <= last))
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

                foreach (SequencedTween seqT in refs.Tweens)
                {
                    ITweenBaseInternal tween = seqT.TweenGetter(seqT.TweenIndex);
                    if (seqT.Version != tween.Version)
                        continue;

                    float delta = tween.CurrentTime - seqT.Position;
                    if (delta > 0 & delta <= tween.Duration)
                    {
                        if (tween.State == TweenState.Running)
                            tween.Play();
                        else
                            tween.Rewind();

                        tween.CurrentTime = delta - dt;
                        TweenLogic.UpdateTime(ref tween, true, tween.Duration, delta);
                        tween.Save();
                    }
                }
            }
        }

        protected override void ProcessPostComplete(int index, ref SequenceInternal tween, ref SequenceOptions options, SequenceReferences refs)
        {
        }
    }
}