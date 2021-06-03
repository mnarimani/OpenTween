using System.Collections;
using NUnit.Framework;
using OpenTween.Helpers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.TestTools;

namespace OpenTween.Tweens
{
    [TestFixture]
    public class FromTests
    {
        private static IEnumerator From<T>()
        {
            Tween<T> t = Tween.Create<T>()
                .SetStart(TweenValues<T>.Start)
                .SetEnd(TweenValues<T>.End)
                .SetIsFrom(true)
                .SetDuration(1)
                .SetDisposeOnComplete(false);

            yield return new WaitForSeconds(t.Duration);

            Assertions<T>.AreEqual(TweenValues<T>.Start, t.CurrentValue, "");
        }

        [OneTimeSetUp]
        public void SetAssertions()
        {
            Assertions.Init();
            TweenValues.Init();
        }

        [UnityTest]
        public IEnumerator FloatTest()
        {
            return From<float>();
        }

        [UnityTest]
        public IEnumerator ColorTest()
        {
            return From<Color>();
        }

        [UnityTest]
        public IEnumerator Float2Test()
        {
            return From<float2>();
        }

        [UnityTest]
        public IEnumerator Float3Test()
        {
            return From<float3>();
        }

        [UnityTest]
        public IEnumerator Float4Test()
        {
            return From<float4>();
        }

        [UnityTest]
        public IEnumerator NormalQuaternionTest()
        {
            return From<Quaternion>();
        }

        [UnityTest]
        public IEnumerator QuaternionTest()
        {
            return From<quaternion>();
        }

        [UnityTest]
        public IEnumerator Vec2Test()
        {
            return From<Vector2>();
        }

        [UnityTest]
        public IEnumerator Vec3Test()
        {
            return From<Vector3>();
        }

        [UnityTest]
        public IEnumerator Vec4Test()
        {
            return From<Vector4>();
        }
    }
}