using Unity.Mathematics;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace OpenTween.Jobs
{
    internal static class TweenValueOpInitializer
    {
        [RuntimeInitializeOnLoadMethod]
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
        public static void Initialize()
        {
            TweenValueOp<float>.Sub = (f1, f2) => f1 - f2;
            TweenValueOp<float>.Add = (f1, f2) => f1 + f2;
            
            TweenValueOp<float2>.Sub = (f1, f2) => f1 - f2;
            TweenValueOp<float2>.Add = (f1, f2) => f1 + f2;
            
            TweenValueOp<float3>.Sub = (f1, f2) => f1 - f2;
            TweenValueOp<float3>.Add = (f1, f2) => f1 + f2;
            
            TweenValueOp<float4>.Sub = (f1, f2) => f1 - f2;
            TweenValueOp<float4>.Add = (f1, f2) => f1 + f2;
            
            
            TweenValueOp<Vector2>.Sub = (f1, f2) => f1 - f2;
            TweenValueOp<Vector2>.Add = (f1, f2) => f1 + f2;
            
            TweenValueOp<Vector3>.Sub = (f1, f2) => f1 - f2;
            TweenValueOp<Vector3>.Add = (f1, f2) => f1 + f2;
            
            TweenValueOp<Vector4>.Sub = (f1, f2) => f1 - f2;
            TweenValueOp<Vector4>.Add = (f1, f2) => f1 + f2;
            
            TweenValueOp<quaternion>.Sub = (f1, f2) =>  math.mul(f1, math.inverse(f2));
            TweenValueOp<quaternion>.Add = math.mul;
            
            TweenValueOp<Quaternion>.Sub = (f1, f2) => f1 * Quaternion.Inverse(f2);
            TweenValueOp<Quaternion>.Add = (f1, f2) => f1 * f2;
            
            TweenValueOp<Color>.Sub = (f1, f2) => f1 - f2;
            TweenValueOp<Color>.Add = (f1, f2) => f1 + f2;
        }
    }
}