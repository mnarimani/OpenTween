using System;
using OpenTween.Jobs;

namespace OpenTween
{
    public partial struct Tween<T>
    {
        public Tween<T> SetStart(T value)
        {
            Start = value;
            return this;
        }

        public Tween<T> SetEnd(T value)
        {
            End = value;
            return this;
        }

        public Tween<T> SetDuration(float value)
        {
            Duration = value;
            return this;
        }

        public Tween<T> SetEase(Ease value)
        {
            Ease = value;
            return this;
        }

        public Tween<T> SetLoopCount(int value)
        {
            LoopCount = value;
            return this;
        }

        public Tween<T> SetLoopType(LoopType value)
        {
            LoopType = value;
            return this;
        }

        public Tween<T> SetOvershootOrAmplitude(float value)
        {
            OvershootOrAmplitude = value;
            return this;
        }

        public Tween<T> SetPeriod(float value)
        {
            Period = value;
            return this;
        }

        public Tween<T> SetDynamicStartEvaluation(Func<T> evalFunc)
        {
            DynamicStartEval = evalFunc != null;
            StartEvalFunc = evalFunc;
            return this;
        }

        public Tween<T> SetIsLocal(bool value)
        {
            IsLocal = value;
            return this;
        }

        public Tween<T> SetIsRelative(bool value)
        {
            IsRelative = value;
            return this;
        }

        public Tween<T> SetDisposeOnComplete(bool value)
        {
            DisposeOnComplete = value;
            return this;
        }

        public Tween<T> SetPrePlayDelay(float value)
        {
            PrePlayDelay = value;
            return this;
        }

        public Tween<T> SetPostPlayDelay(float value)
        {
            PostPlayDelay = value;
            return this;
        }
        
        public Tween<T> SetAutoPlay(bool value)
        {
            AutoPlay = value;
            return this;
        }
    }
}