using System.Collections;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.TestTools;

namespace OpenTween.Tweens
{
    [TestFixture]
    public class RelativeTests
    {
        private static IEnumerator Relative<T>(T start, T end, T expected)
        {
            Tween<T> t = Tween.Create<T>()
                .SetStart(start)
                .SetEnd(end)
                .SetIsRelative(true)
                .SetDuration(1)
                .SetDisposeOnComplete(false);

            yield return new WaitForSeconds(t.Duration);

            Assertions<T>.AreEqual(expected, t.CurrentValue, "");
        }

        [OneTimeSetUp]
        public void SetAssertions()
        {
            Assertions.Init();
        }

        [UnityTest]
        public IEnumerator FloatTest()
        {
            return Relative(2f, 2f, 4);
        }

        [UnityTest]
        public IEnumerator ColorTest()
        {
            var start = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            return Relative(start, start, UnityEngine.Color.white);
        }

        [UnityTest]
        public IEnumerator Float2Test()
        {
            var f = new float2(0.5f, 0.5f);
            return Relative(f, f, new float2(1, 1));
        }

        [UnityTest]
        public IEnumerator Float3Test()
        {
            var f = new float3(0.5f, 0.5f, 0.5f);
            return Relative(f, f, new float3(1, 1, 1));
        }

        [UnityTest]
        public IEnumerator Float4Test()
        {
            var f = new float4(0.5f, 0.5f, 0.5f, 0.5f);
            return Relative(f, f, new float4(1, 1, 1, 1));
        }

        [UnityTest]
        public IEnumerator NormalQuaternionTest()
        {
            var q = Quaternion.Euler(0, 90, 0);
            return Relative(q, q, Quaternion.Euler(0, 180, 0));
        }

        [UnityTest]
        public IEnumerator QuaternionTest()
        {
            var q = quaternion.Euler(0, 90, 0);
            return Relative(q, q, quaternion.Euler(0, 180, 0));
        }


        [UnityTest]
        public IEnumerator Vec2Test()
        {
            var f = new Vector2(0.5f, 0.5f);
            return Relative(f, f, new Vector2(1, 1));
        }

        [UnityTest]
        public IEnumerator Vec3Test()
        {
            var f = new Vector3(0.5f, 0.5f, 0.5f);
            return Relative(f, f, new Vector3(1, 1, 1));
        }

        [UnityTest]
        public IEnumerator Vec4Test()
        {
            var f = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
            return Relative(f, f, new Vector4(1, 1, 1, 1));
        }
    }
}