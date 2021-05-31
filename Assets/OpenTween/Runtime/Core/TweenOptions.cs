// using System;
// using UnityEngine;
//
// namespace OpenTween
// {
//     public interface IOptionsBase
//     {
//         bool DisposeOnComplete { get; }
//     }
//
//     [Serializable]
//     public class TweenOptions<T> : IOptionsBase
//     {
//         [SerializeField] private T _start;
//         [SerializeField] private T _end;
//         [SerializeField] private float _duration;
//         [SerializeField] private Ease _ease;
//         [SerializeField] private bool _useAnimationCurve;
//         [SerializeReference] private AnimationCurve _animationCurveEase;
//         [SerializeField] private float _overshootOrAmplitude;
//         [SerializeField] private float _period;
//         [SerializeField] private bool _dynamicStartEval;
//         [SerializeField] private bool _isLocal;
//         [SerializeField] private bool _isRelative;
//         [SerializeField] private bool _disposeOnComplete;
//
//         private Func<T> _startEvalFunc;
//
//         public TweenOptions()
//         {
//             ResetToDefault();
//         }
//
//         public TweenOptions(T start, T end, float duration, Ease ease = Ease.OutQuad) : this()
//         {
//             _start = start;
//             _end = end;
//             _ease = ease;
//             _duration = duration;
//         }
//
//         public T Start { get => _start; set => _start = value; }
//
//         public T End { get => _end; set => _end = value; }
//
//         public Ease Ease { get => _ease; set => _ease = value; }
//
//
//         public float OvershootOrAmplitude { get => _overshootOrAmplitude; set => _overshootOrAmplitude = value; }
//
//         public float Period { get => _period; set => _period = value; }
//
//         public Func<T> StartEvalFunc { get => _startEvalFunc; set => _startEvalFunc = value; }
//         public bool DynamicStartEval { get => _dynamicStartEval; set => _dynamicStartEval = value; }
//
//         public bool IsLocal { get => _isLocal; set => _isLocal = value; }
//
//         public bool IsRelative { get => _isRelative; set => _isRelative = value; }
//
//         public float Duration { get => _duration; set => _duration = value; }
//
//         public bool DisposeOnComplete { get => _disposeOnComplete; set => _disposeOnComplete = value; }
//
//         public AnimationCurve AnimationCurveEase { get => _animationCurveEase; set => _animationCurveEase = value; }
//
//         public bool UseAnimationCurve { get => _useAnimationCurve; set => _useAnimationCurve = value; }
//
//         public void CopyFrom(TweenOptions<T> other)
//         {
//             _duration = other._duration;
//             _disposeOnComplete = other._disposeOnComplete;
//             _start = other._start;
//             _end = other._end;
//             _ease = other._ease;
//             _overshootOrAmplitude = other._overshootOrAmplitude;
//             _period = other._period;
//             _dynamicStartEval = other._dynamicStartEval;
//             _startEvalFunc = other._startEvalFunc;
//         }
//
//         public void ResetToDefault()
//         {
//             _start = default;
//             _end = default;
//             _duration = default;
//             _overshootOrAmplitude = OpenTweenSettings.DefaultOvershootOrAmplitude;
//             _period = OpenTweenSettings.DefaultPeriod;
//             _dynamicStartEval = default;
//             _isLocal = default;
//             _isRelative = default;
//             _ease = Ease.OutQuad;
//             _disposeOnComplete = true;
//             _startEvalFunc = default;
//             _useAnimationCurve = default;
//             _animationCurveEase = default;
//         }
//     }
// }