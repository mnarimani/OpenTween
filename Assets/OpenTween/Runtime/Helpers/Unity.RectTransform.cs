using OpenTween.Jobs;
using UnityEngine;

namespace OpenTween
{
    public static partial class UnityHelpers
    {
        public static Tween<Vector2> DOAnchorPos(this RectTransform t)
        {
            var tween = Tween.Create<Vector2>();
            tween.ValueUpdated += ValueChanged;
            tween.StartEvalFunc = StartEval;
            return tween;

            void ValueChanged(Vector2 pos)
            {
                t.anchoredPosition = pos;
            }

            Vector2 StartEval()
            {
                return t.anchoredPosition;
            }
        }
        
        public static Tween<Vector2> DOAnchorPos(this RectTransform t, Vector2 end, float duration, Ease ease = Ease.OutQuad)
        {
            Tween<Vector2> tween = t.DOAnchorPos();
            
            tween.End = end;
            tween.Duration = duration;
            tween.Ease = ease;
            tween.DynamicStartEval = true;
            
            return tween;
        }
    }
}