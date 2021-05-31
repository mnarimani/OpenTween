using OpenTween.Jobs;
using UnityEngine;

namespace OpenTween
{
    public static partial class UnityHelpers
    {
        public static Tween<Vector3> DOMove(this Transform t)
        {
            Tween<Vector3> tween = Tween.Create(TweenOptions<Vector3>.Default);

            tween.ValueUpdated += ValueChanged;
            tween.StartEvalFunc = StartEval;
            tween.BindToComponent(t);
            return tween;

            void ValueChanged(Vector3 pos)
            {
                t.position = pos;
            }

            Vector3 StartEval()
            {
                return t.position;
            }
        }

        public static Tween<Vector3> DOMove(this Transform t, Vector3 end, float duration, Ease ease = Ease.OutQuad)
        {
            Tween<Vector3> tween = t.DOMove();

            tween.End = end;
            tween.Duration = duration;
            tween.Ease = ease;
            tween.DynamicStartEval = true;

            return tween;
        }

        public static Tween<Vector3> DOLocalMove(this Transform t)
        {
            var tween = Tween.Create<Vector3>(default);
            tween.ValueUpdated += ValueChanged;
            tween.StartEvalFunc = StartEval;
            tween.BindToComponent(t);
            return tween;

            void ValueChanged(Vector3 pos)
            {
                t.localPosition = pos;
            }

            Vector3 StartEval()
            {
                return t.localPosition;
            }
        }

        public static Tween<Vector3> DOLocalMove(this Transform t, Vector3 end, float duration, Ease ease = Ease.OutQuad)
        {
            Tween<Vector3> tween = t.DOLocalMove();

            tween.End = end;
            tween.Duration = duration;
            tween.Ease = ease;
            tween.DynamicStartEval = true;

            return tween;
        }
    }
}