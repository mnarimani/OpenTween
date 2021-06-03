using System;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

namespace OpenTween
{
    public static class Assertions
    {
        public static void Init()
        {
            Assertions<Vector2>.AreEqual = AssertEqual;
            Assertions<Vector3>.AreEqual = AssertEqual;
            Assertions<Vector4>.AreEqual = AssertEqual;
            Assertions<Quaternion>.AreEqual = AssertEqual;
            Assertions<quaternion>.AreEqual = AssertEqual;
            Assertions<float>.AreEqual = AssertEqual;
            Assertions<Color>.AreEqual = AssertEqual;
            Assertions<float2>.AreEqual = AssertEqual;
            Assertions<float3>.AreEqual = AssertEqual;
            Assertions<float4>.AreEqual = AssertEqual;
            
            Assertions<Vector2>.AreEqual = AssertEqual;
            Assertions<Vector3>.AreEqual = AssertEqual;
            Assertions<Vector4>.AreEqual = AssertEqual;
            Assertions<Quaternion>.AreEqual = AssertEqual;
            Assertions<quaternion>.AreEqual = AssertEqual;
            Assertions<float>.AreEqual = AssertEqual;
            Assertions<Color>.AreEqual = AssertEqual;
            Assertions<float2>.AreEqual = AssertEqual;
            Assertions<float3>.AreEqual = AssertEqual;
            Assertions<float4>.AreEqual = AssertEqual;
        }
        
        private static void AssertEqual(Color a, Color b, string message)
        {
            var af = new float4(a.r, a.g, a.b, a.a);
            var bf = new float4(b.r, b.g, b.b, b.a);
            AssertEqual(af, bf, message);
        }
        
        private static void AssertEqual(float2 a, float2 b, string message)
        {
            Assert.LessOrEqual(math.length((a - b)), 0.001f, message);
        }

        private static void AssertEqual(float3 a, float3 b, string message)
        {
            Assert.LessOrEqual(math.length((a - b)), 0.001f, message);
        }

        private static void AssertEqual(float4 a, float4 b, string message)
        {
            Assert.LessOrEqual(math.length((a - b)), 0.001f, message);
        }
        
        private static void AssertEqual(Vector4 a, Vector4 b, string message)
        {
            Assert.LessOrEqual((a - b).magnitude, 0.001f, message);
        }

        private static void AssertEqual(Vector3 a, Vector3 b, string message)
        {
            Assert.LessOrEqual((a - b).magnitude, 0.001f, message);
        }

        private static void AssertEqual(Vector2 a, Vector2 b, string message)
        {
            Assert.LessOrEqual((a - b).magnitude, 0.001f, message);
        }

        private static void AssertEqual(Quaternion a, Quaternion b, string message)
        {
            Assert.True(a == b, message);
        }
        
        private static void AssertEqual(quaternion a, quaternion b, string message)
        {
            Assert.True(math.dot(a, b) > 0.999998986721039, message);
        }

        private static void AssertEqual(float a, float b, string message)
        {
            Assert.LessOrEqual((a - b), 0.001f, message);
        }
    }
    
    public static class Assertions<T>
    {
        public static Action<T, T, string> AreEqual;
    }
}