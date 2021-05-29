using UnityEngine;

namespace OpenTween
{
    public delegate T LerpFunc<T>(T start, T end, float progress);

    public class Lerp
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#endif
        public static void Init()
        {
            Lerp<float>.Func = Mathf.LerpUnclamped;
            Lerp<Vector2>.Func = Vector2.LerpUnclamped;
            Lerp<Vector3>.Func = Vector3.LerpUnclamped;
            Lerp<Vector4>.Func = Vector4.LerpUnclamped;
            Lerp<Quaternion>.Func = Quaternion.LerpUnclamped;
        }
    }

    public class Lerp<T> : Lerp
    {
        public static LerpFunc<T> Func;
    }
}