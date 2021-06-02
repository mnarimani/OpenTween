using Unity.Mathematics;
using UnityEngine;

namespace OpenTween.Helpers
{
    public static class TweenValues
    {
        public static void Init()
        {
            TweenValues<Vector2>.Start = Vector2.left;
            TweenValues<Vector2>.End = Vector2.right;

            TweenValues<Vector3>.Start = Vector3.left;
            TweenValues<Vector3>.End = Vector3.right;

            TweenValues<Vector4>.Start = Vector4.zero;
            TweenValues<Vector4>.End = Vector4.one;

            TweenValues<Quaternion>.Start = Quaternion.Euler(0, 90, 0);
            TweenValues<Quaternion>.End = Quaternion.Euler(0, 180, 0);
            
            TweenValues<float2>.Start = new float2(-1,0);
            TweenValues<float2>.End = new float2(1,0);

            TweenValues<float3>.Start = new float3(0,0,-1);
            TweenValues<float3>.End = new float3(0,0,1);

            TweenValues<float4>.Start = new float4(0, 0, 0, -1);
            TweenValues<float4>.End = new float4(0, 0, 0, 1);

            TweenValues<quaternion>.Start = Quaternion.Euler(0, 90, 0);
            TweenValues<quaternion>.End = Quaternion.Euler(0, 180, 0);

            TweenValues<Color>.Start = Color.white;
            TweenValues<Color>.End = Color.black;

            TweenValues<float>.Start = 0;
            TweenValues<float>.End = 1;
        }
    }

    public static class TweenValues<T>
    {
        public static T Start;
        public static T End;
    }
}