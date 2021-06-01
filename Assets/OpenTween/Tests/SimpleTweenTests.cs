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
                .SetEase(Ease.Linear)
                .OnValueUpdated(f => value = f);

            yield return new WaitForSeconds(0.25f);

            Assert.GreaterOrEqual(value, 0.2f);
            Assert.LessOrEqual(value, 0.3f);
            
            yield return new WaitForSeconds(0.25f);

            Assert.GreaterOrEqual(value, 0.45f);
            Assert.LessOrEqual(value, 0.55f);
            
            yield return new WaitForSeconds(0.25f);

            Assert.GreaterOrEqual(value, 0.7f);
            Assert.LessOrEqual(value, 0.8f);

            yield return new WaitForSeconds(0.25f);
            
            Assert.AreEqual(1, value);
        }
    }
}