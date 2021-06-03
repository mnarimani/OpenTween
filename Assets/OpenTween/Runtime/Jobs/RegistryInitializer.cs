using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace OpenTween.Jobs
{
    public class RegistryInitializer
    {
        [RuntimeInitializeOnLoadMethod]
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
        private static void Initialize()
        {
            SetTweenLerp<float>((array, options, indices, handle) => new FloatLerp
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle));

            SetTweenLerp<float2>((array, options, indices, handle) => new Float2Lerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle));

            SetTweenLerp<float3>((array, options, indices, handle) => new Float3Lerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle));

            SetTweenLerp<float4>((array, options, indices, handle) => new Float4Lerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle));

            SetTweenLerp<Vector2>((array, options, indices, handle) => new Vec2Lerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle));

            SetTweenLerp<Vector3>((array, options, indices, handle) => new Vec3Lerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle));

            SetTweenLerp<Vector4>((array, options, indices, handle) => new Vec4Lerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle));

            SetTweenLerp<Quaternion>((array, options, indices, handle) => new NormalQuaternionLerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle));

            SetTweenLerp<quaternion>((array, options, indices, handle) => new QuaternionLerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle));

            SetTweenLerp<Color>((array, options, indices, handle) => new ColorLerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle));
        }

        static void SetTweenLerp<T>(LerpScheduleFunc<T> func)
        {
            TweenRegistry<T>.LerpScheduler = func;
        }
    }
}