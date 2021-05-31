using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace OpenTween
{
    [TestFixture]
    public class SimpleTweenTests
    {
        [UnityTest]
        public IEnumerator SimpleFloat()
        {
            float value = 0;
            
            Tween.Create<float>()
                .SetStart(0)
                .SetEnd(1)
                .SetDuration(1)
                .OnValueUpdated(f => value = f);

            yield return new WaitForSeconds(2);

            Assert.AreEqual(1, value);
        }
    }
}