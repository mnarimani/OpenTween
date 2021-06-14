using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace OpenTween.Tweens
{
    [TestFixture]
    public class DelayTests
    {
        [UnityTest]
        public IEnumerator SimplePrePlayDelay()
        {
            float value = 0;

            int start = 2;
            int end = 4;
            int dur = 1;
            float preDelay = 2;

            Tween.Create<float>()
                .SetStart(start)
                .SetEnd(end)
                .SetDuration(dur)
                .SetPrePlayDelay(preDelay)
                .SetOnValueUpdated(f => { value = f; });

            yield return new WaitForSeconds(preDelay / 2);

            Assert.AreEqual(0, value);

            yield return new WaitForSeconds(preDelay / 2 + 0.1f);

            Assert.GreaterOrEqual(value, start);

            yield return new WaitForSeconds(dur);

            Assert.GreaterOrEqual(value, end);
        }
        
        [UnityTest]
        public IEnumerator SimplePostPlayDelay()
        {
            float value = 0;

            int start = 2;
            int end = 4;
            int dur = 1;
            float post = 2;

            Tween.Create<float>()
                .SetStart(start)
                .SetEnd(end)
                .SetDuration(dur)
                .SetPostPlayDelay(post)
                .SetOnValueUpdated(f => { value = f; });

            yield return new WaitForSeconds(post / 2);

            Assert.AreEqual(start, value);

            yield return new WaitForSeconds(post / 2 + 0.1f);

            Assert.GreaterOrEqual(value, start);

            yield return new WaitForSeconds(dur);

            Assert.GreaterOrEqual(value, end);
        }
    }
}