using OpenTween.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace OpenTween
{
    public static partial class UnityHelpers
    {
        public static Tween<float3> DOMove(this Transform t)
        {
            Tween<float3> tween = Tween.Create(TweenOptions<float3>.Default);

            tween.ValueUpdated += ValueChanged;
            tween.StartEvalFunc = StartEval;
            tween.BindToComponent(t);
            return tween;

            void ValueChanged(float3 pos)
            {
                t.position = pos;
            }

            float3 StartEval()
            {
                return t.position;
            }
        }

        public static Tween<float3> DOMove(this Transform t, Vector3 end, float duration, Ease ease = Ease.OutQuad)
        {
            Tween<float3> tween = t.DOMove();

            tween.End = end;
            tween.Duration = duration;
            tween.Ease = ease;
            tween.DynamicStartEval = true;

            return tween;
        }

        public static Tween<float3> DOLocalMove(this Transform t)
        {
            var tween = Tween.Create<float3>(default);
            tween.ValueUpdated += ValueChanged;
            tween.StartEvalFunc = StartEval;
            tween.BindToComponent(t);
            return tween;

            void ValueChanged(float3 pos)
            {
                t.localPosition = pos;
            }

            float3 StartEval()
            {
                return t.localPosition;
            }
        }

        public static Tween<float3> DOLocalMove(this Transform t, Vector3 end, float duration, Ease ease = Ease.OutQuad)
        {
            Tween<float3> tween = t.DOLocalMove();

            tween.End = end;
            tween.Duration = duration;
            tween.Ease = ease;
            tween.DynamicStartEval = true;

            return tween;
        }
    }
}