using System;
using OpenTween.Jobs;

namespace OpenTween
{
    public partial struct Tween<T>
    {
        public T Start
        {
            get => Options.Start;
            set => Options.Start = value;
        }

        public T End
        {
            get => Options.End;
            set => Options.End = value;
        }

        public Ease Ease
        {
            get => Options.Ease;
            set => Options.Ease = value;
        }

        public float OvershootOrAmplitude
        {
            get => Options.OvershootOrAmplitude;
            set => Options.OvershootOrAmplitude = value;
        }

        public float Period
        {
            get => Options.Period;
            set => Options.Period = value;
        }

        public Func<T> StartEvalFunc { get => Refs.StartEvalFunc; set => Refs.StartEvalFunc = value; }

        public bool DynamicStartEval
        {
            get => Options.DynamicStartEvaluation;
            set => Options.DynamicStartEvaluation = value;
        }

        public bool IsLocal
        {
            get => Options.IsLocal;
            set => Options.IsLocal = value;
        }

        public bool IsRelative
        {
            get => Options.IsRelative;
            set => Options.IsRelative = value;
        }

        public float Duration
        {
            get => Options.Duration;
            set => Options.Duration = value;
        }

        public bool DisposeOnComplete
        {
            get => Options.DisposeOnComplete;
            set => Options.DisposeOnComplete = value;
        }

        public bool AutoPlay
        {
            get => Options.AutoPlay;
            set => Options.AutoPlay = value;
        }
        
        public bool IsFrom
        {
            get => Options.IsFrom;
            set => Options.IsFrom = value;
        }
        
        public T CurrentValue => InternalTween.CurrentValue;

        public float CurrentTime => InternalTween.CurrentTime;

        public TweenState State => InternalTween.State;

        public int LoopCount
        {
            get => Options.LoopCount;
            set => Options.LoopCount = value;
        }

        public LoopType LoopType
        {
            get => Options.LoopType;
            set => Options.LoopType = value;
        }

        public float PrePlayDelay
        {
            get => Options.PrePlayDelay;
            set => Options.PrePlayDelay = value;
        }

        public float PostPlayDelay
        {
            get => Options.PostPlayDelay;
            set => Options.PostPlayDelay = value;
        }
    }
}