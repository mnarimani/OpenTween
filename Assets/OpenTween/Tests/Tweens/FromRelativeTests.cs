using System.Collections;
using NUnit.Framework;
using OpenTween.Helpers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.TestTools;

namespace OpenTween.Tweens
{
    [TestFixture]
    public class FromRelativeTests
    {
        private static IEnumerator FromRelative<T>(T start, T end, T sum)
        {
            Tween<T> t = Tween.Create<T>()
                .SetStart(start)
                .SetEnd(end)
                .SetIsRelative(true)
                .SetIsFrom(true)
                .SetDuration(1)
                .SetDisposeOnComplete(false);

            //yield return null;

            //Assertions<T>.AreEqual(sum, t.CurrentValue, "");
            
            yield return new WaitForSeconds(t.Duration);

            Assertions<T>.AreEqual(start, t.CurrentValue, "");
        }

        [OneTimeSetUp]
        public void SetAssertions()
        {
            Assertions.Init();
        }

        [UnityTest]
        public IEnumerator FloatTest()
        {
            return FromRelative(2f, 4f, 6);
        }

        [UnityTest]
        public IEnumerator ColorTest()
        {
            var start = new Color(0.1f, 0.1f, 0.1f, 0.1f);
            var end = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            return FromRelative(start, end, new Color(0.6f, 0.6f, 0.6f, 0.6f));
        }

        [UnityTest]
        public IEnumerator Float2Test()
        {
            var f1 = new float2(0.2f, 0.2f);
            var f2 = new float2(0.5f, 0.5f);
            return FromRelative(f1, f2, new float2(0.7f, 0.7f));
        }

        [UnityTest]
        public IEnumerator Float3Test()
        {
            var f1 = new float3(0.2f, 0.2f, 0.2f);
            var f2 = new float3(0.5f, 0.5f, 0.5f);
            return FromRelative(f1, f2, new float3(0.7f,0.7f,0.7f));
        }

        [UnityTest]
        public IEnumerator Float4Test()
        {
            var f1 = new float4(0.2f, 0.2f, 0.2f, 0.2f);
            var f2 = new float4(0.5f, 0.5f, 0.5f, 0.5f);
            return FromRelative(f1, f2, new float4(0.7f,0.7f,0.7f,0.7f));
        }

        [UnityTest]
        public IEnumerator NormalQuaternionTest()
        {
            var q1 = Quaternion.Euler(0, 20, 0);
            var q2 = Quaternion.Euler(0, 60, 0);
            return FromRelative(q1, q2, Quaternion.Euler(0,80,0));
        }

        [UnityTest]
        public IEnumerator QuaternionTest()
        {
            var q1 = quaternion.Euler(0, 20, 0);
            var q2 = quaternion.Euler(0, 60, 0);
            return FromRelative(q1, q2, quaternion.Euler(0,80,0));
        }

        [UnityTest]
        public IEnumerator Vec2Test()
        {
            var f1 = new Vector2(0.2f, 0.2f);
            var f2 = new Vector2(0.5f, 0.5f);
            return FromRelative(f1, f2, new Vector2(0.7f, 0.7f));
        }

        [UnityTest]
        public IEnumerator Vec3Test()
        {
            var f1 = new Vector3(0.2f, 0.2f, 0.2f);
            var f2 = new Vector3(0.5f, 0.5f, 0.5f);
            return FromRelative(f1, f2, new Vector3(0.7f,0.7f,0.7f));
        }

        [UnityTest]
        public IEnumerator Vec4Test()
        {
            var f1 = new Vector4(0.2f, 0.2f, 0.2f, 0.2f);
            var f2 = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
            return FromRelative(f1, f2, new Vector4(0.7f,0.7f,0.7f,0.7f));
        }
    }
}