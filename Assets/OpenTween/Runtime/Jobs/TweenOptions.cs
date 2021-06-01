using System;

namespace OpenTween.Jobs
{
    public interface IOptions
    {
        bool DisposeOnComplete { get; set; }
        bool AutoPlay { get; set; }
        float PrePlayDelay { get; set; }
        float PosPlayDelay { get; set; }
        void ResetToDefaults();
    }

    internal interface IOptionsBaseInternal : IOptions
    {
        int Version { get; set; }
        float Duration { get; set; }
    }

    [Serializable]
    public struct TweenOptions<T> : IOptionsBaseInternal
    {
        public T Start;
        public T End;
        public Ease Ease { get; set; }
        public int LoopCount;
        public LoopType LoopType;
        public float OvershootOrAmplitude;
        public float Period;
        public bool DynamicStartEvaluation;
        public bool IsLocal;
        public bool IsRelative;
        public bool IsFrom;
        public bool DisposeOnComplete;
        public float PrePlayDelay;
        public float PostPlayDelay;
        public bool AutoPlay;
        private bool _disposeOnComplete;
        private bool _autoPlay;
        private float _prePlayDelay;
        private float _posPlayDelay;

        public static TweenOptions<T> Default
        {
            get
            {
                var instance = new TweenOptions<T>();
                instance.ResetToDefaults();
                return instance;
            }
        }

        public void ResetToDefaults()
        {
            Start = default;
            End = default;
            Duration = default;
            IsFrom = default;
            OvershootOrAmplitude = OpenTweenSettings.DefaultOvershootOrAmplitude;
            Period = OpenTweenSettings.DefaultPeriod;
            DynamicStartEvaluation = default;
            IsLocal = default;
            IsRelative = default;
            Ease = Ease.OutQuad;
            DisposeOnComplete = true;
            AutoPlay = true;

            LoopCount = 1;
            LoopType = LoopType.Restart;
            PrePlayDelay = 0;
            PostPlayDelay = 1;
        }

        public void CopyFrom(TweenOptions<T> options)
        {
            CopyFrom(ref options);
        }

        public void CopyFrom(ref TweenOptions<T> options)
        {
            Start = options.Start;
            End = options.End;
            Duration = options.Duration;
            Ease = options.Ease;
            LoopCount = options.LoopCount;
            LoopType = options.LoopType;
            OvershootOrAmplitude = options.OvershootOrAmplitude;
            Period = options.Period;
            DynamicStartEvaluation = options.DynamicStartEvaluation;
            IsLocal = options.IsLocal;
            IsRelative = options.IsRelative;
            DisposeOnComplete = options.DisposeOnComplete;
            PrePlayDelay = options.PrePlayDelay;
            PostPlayDelay = options.PostPlayDelay;
        }

        public int Version { get; set; }
        public float Duration { get; set; }

        bool IOptions.DisposeOnComplete { get => _disposeOnComplete; set => _disposeOnComplete = value; }

        bool IOptions.AutoPlay { get => _autoPlay; set => _autoPlay = value; }

        float IOptions.PrePlayDelay { get => _prePlayDelay; set => _prePlayDelay = value; }

        float IOptions.PosPlayDelay { get => _posPlayDelay; set => _posPlayDelay = value; }
    }
}