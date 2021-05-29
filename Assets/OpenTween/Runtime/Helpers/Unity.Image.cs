using System;
using UnityEngine;
using UnityEngine.UI;

namespace OpenTween
{
    public static partial class UnityHelpers
    {
        public static Tween<Color> DOColor(this Image t)
        {
            var tween = Tween.Create<Color>();
            tween.ValueUpdated += ValueChanged;
            tween.StartEvalFunc = StartEval;
            return tween;

            void ValueChanged(Color pos)
            {
                t.color = pos;
            }

            Color StartEval()
            {
                return t.color;
            }
        }
        
        public static Tween<Color> DOColor(this Image t, Color end, float duration, Ease ease = EaseMap.Default)
        {
            Tween<Color> tween = t.DOColor();
            
            tween.End = end;
            tween.Duration = duration;
            tween.Ease = ease;
            tween.DynamicStartEval = true;
            
            return tween;
        }
    }
}