using OpenTween.Jobs;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_COLLECTIONS
using NativeListInt = Unity.Collections.NativeList<int>;

#else
using NativeListInt = OpenTween.Jobs.NativeList<int>;
#endif

namespace OpenTween
{
    [ExecuteAlways]
    internal class TweenScheduleMaster : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
        private static void Initialize()
        {
            var obj = new GameObject(nameof(TweenScheduleMaster), typeof(TweenScheduleMaster)) {hideFlags = HideFlags.HideAndDontSave};
            
            if (Application.isPlaying)
                DontDestroyOnLoad(obj);

            TweenRegistry<float, TweenManagedReferences<float>>.LerpScheduler = (array, options, indices, handle) => new FloatLerp
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle);

            TweenRegistry<float2, TweenManagedReferences<float2>>.LerpScheduler = (array, options, indices, handle) => new Float2Lerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle);

            TweenRegistry<float3, TweenManagedReferences<float3>>.LerpScheduler = (array, options, indices, handle) => new Float3Lerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle);

            TweenRegistry<float4, TweenManagedReferences<float4>>.LerpScheduler = (array, options, indices, handle) => new Float4Lerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle);

            TweenRegistry<Vector2, TweenManagedReferences<Vector2>>.LerpScheduler = (array, options, indices, handle) => new Vec2Lerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle);

            TweenRegistry<Vector3, TweenManagedReferences<Vector3>>.LerpScheduler = (array, options, indices, handle) => new Vec3Lerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle);

            TweenRegistry<Vector4, TweenManagedReferences<Vector4>>.LerpScheduler = (array, options, indices, handle) => new Vec4Lerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle);

            TweenRegistry<Quaternion, TweenManagedReferences<Quaternion>>.LerpScheduler = (array, options, indices, handle) => new NormalQuaternionLerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle);

            TweenRegistry<quaternion, TweenManagedReferences<quaternion>>.LerpScheduler = (array, options, indices, handle) => new QuaternionLerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle);

            TweenRegistry<Color, TweenManagedReferences<Color>>.LerpScheduler = (array, options, indices, handle) => new ColorLerp()
            {
                Array = array,
                Options = options,
                Indices = indices
            }.Schedule(indices.Length, 32, handle);
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            TweenRegistry<float, TweenManagedReferences<float>>.Schedule(dt);
            TweenRegistry<float2, TweenManagedReferences<float2>>.Schedule(dt);
            TweenRegistry<float3, TweenManagedReferences<float3>>.Schedule(dt);
            TweenRegistry<float4, TweenManagedReferences<float4>>.Schedule(dt);
            TweenRegistry<Vector2, TweenManagedReferences<Vector2>>.Schedule(dt);
            TweenRegistry<Vector3, TweenManagedReferences<Vector3>>.Schedule(dt);
            TweenRegistry<Vector4, TweenManagedReferences<Vector4>>.Schedule(dt);
            TweenRegistry<Color, TweenManagedReferences<Color>>.Schedule(dt);
            TweenRegistry<quaternion, TweenManagedReferences<quaternion>>.Schedule(dt);
            TweenRegistry<quaternion, TweenManagedReferences<quaternion>>.Schedule(dt);

            TweenRegistry<float, TweenManagedReferences<float>>.Complete();
            TweenRegistry<float2, TweenManagedReferences<float2>>.Complete();
            TweenRegistry<float3, TweenManagedReferences<float3>>.Complete();
            TweenRegistry<float4, TweenManagedReferences<float4>>.Complete();
            TweenRegistry<Vector2, TweenManagedReferences<Vector2>>.Complete();
            TweenRegistry<Vector3, TweenManagedReferences<Vector3>>.Complete();
            TweenRegistry<Vector4, TweenManagedReferences<Vector4>>.Complete();
            TweenRegistry<Color, TweenManagedReferences<Color>>.Complete();
            TweenRegistry<quaternion, TweenManagedReferences<quaternion>>.Complete();
            TweenRegistry<quaternion, TweenManagedReferences<quaternion>>.Complete();
        }
    }
}