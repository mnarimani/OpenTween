using OpenTween.Jobs;
using Unity.Mathematics;

namespace OpenTween
{
    [System.Serializable]
    public sealed class DOColorTarget : IAnimation
    {
        public UnityEngine.UI.Image Target;
        public TweenOptions<UnityEngine.Color> Options;

        public ITweenBase Play()
        {
            var anim = Target.DOColor();
            anim.CopyOptionsFrom(Options);
            anim.Play();
            return anim;
        }

        private void Reset()
        {
            Options.ResetToDefaults();
        }
    }

    [System.Serializable]
    public sealed class DOAnchorPosTarget : IAnimation
    {
        public UnityEngine.RectTransform Target;
        public TweenOptions<UnityEngine.Vector2> Options;

        public ITweenBase Play()
        {
            var anim = Target.DOAnchorPos();
            anim.CopyOptionsFrom(Options);
            anim.Play();
            return anim;
        }

        private void Reset()
        {
            Options.ResetToDefaults();
        }
    }

    [System.Serializable]
    public sealed class DOMoveTarget : IAnimation
    {
        public UnityEngine.Transform Target;
        public TweenOptions<float3> Options;

        public ITweenBase Play()
        {
            var anim = Target.DOMove();
            anim.CopyOptionsFrom(Options);
            anim.Play();
            return anim;
        }

        private void Reset()
        {
            Options.ResetToDefaults();
        }
    }

    [System.Serializable]
    public sealed class DOLocalMoveTarget : IAnimation
    {
        public UnityEngine.Transform Target;
        public TweenOptions<float3> Options;

        public ITweenBase Play()
        {
            var anim = Target.DOLocalMove();
            anim.CopyOptionsFrom(Options);
            anim.Play();
            return anim;
        }
    }

    public partial class OpenAnimation
    {
#if UNITY_EDITOR
        internal static readonly string[] MethodList = {"Image/DOColor", "RectTransform/DOAnchorPos", "Transform/DOMove", "Transform/DOLocalMove"};
        internal static System.Type GetTargetType(string methodName)
        {
            switch (methodName)
            {
                case "Image/DOColor": return typeof(DOColorTarget);
                case "RectTransform/DOAnchorPos": return typeof(DOAnchorPosTarget);
                case "Transform/DOMove": return typeof(DOMoveTarget);
                case "Transform/DOLocalMove": return typeof(DOLocalMoveTarget);
                default: return null;
            }
        }
#endif
    }
}