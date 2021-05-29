using System;
using UnityEngine;

namespace OpenTween
{
    internal class TweenInternal<T> : TweenBase<TweenInternal<T>>
    {
        private readonly LerpFunc<T> _lerpFunc;

        public TweenInternal()
        {
            Options = new TweenOptions<T>();
            _lerpFunc = Lerp<T>.Func;

            if (_lerpFunc == null)
                throw new NotSupportedException($"Lerp function is not defined for data type `{typeof(T)}`");
        }

        public event Action<T> ValueUpdated;

        public TweenOptions<T> Options { get; }

        public T CurrentValue { get; private set; }

        public override float Duration => Options.Duration;

        public override bool Play(bool restart = false)
        {
            if (HasBoundComponent && BoundComponent == null)
            {
                Dispose();
                return false;
            }

            if ((State == TweenState.NotPlayed || restart) && Options.DynamicStartEval)
                Options.Start = Options.StartEvalFunc();

            return base.Play(restart);
        }

        public override bool Update(float elapsedTime, bool isFromSequence)
        {
            if (base.Update(elapsedTime, isFromSequence))
            {
                UpdateValue();
                return true;
            }

            return false;
        }

        private void UpdateValue()
        {
            if (Options.Duration <= 0)
            {
                CurrentValue = Options.Start;
            }
            else if (CurrentTime >= Options.Duration)
            {
                CurrentValue = Options.End;
            }
            else
            {
                float t;
                
                if (Options.CustomEase != null)
                    t = Options.CustomEase(CurrentTime, Options.Duration, Options.OvershootOrAmplitude, Options.Period);
                else
                    t = EaseMap.Evaluate(Options.Ease, CurrentTime, Options.Duration, Options.OvershootOrAmplitude, Options.Period);
                
                CurrentValue = _lerpFunc(Options.Start, Options.End, t);
            }

            try
            {
                ValueUpdated?.Invoke(CurrentValue);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void CopyOptionsFrom(TweenOptions<T> other)
        {
            Options.CopyFrom(other);
        }

        public override void Dispose()
        {
            base.Dispose();

            IsPartOfSequence = false;
            CurrentValue = default;
            ValueUpdated = null;
            Options.ResetToDefault();
        }

        protected override IOptionsBase GetOptions()
        {
            return Options;
        }
    }
}