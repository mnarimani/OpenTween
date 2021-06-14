using System.Collections;
using NUnit.Framework;
using OpenTween.Jobs;
using UnityEngine;
using UnityEngine.TestTools;

namespace OpenTween.Tweens
{
    [TestFixture]
    public class Loops
    {
        [UnityTest]
        public IEnumerator FloatIncremental()
        {
            float value = 0;
            Tween<float> t = Tween.Create<float>()
                .SetStart(1)
                .SetEnd(2)
                .SetDuration(0.1f)
                .SetOnValueUpdated(f => value = f)
                .SetLoops(9, LoopType.Incremental)
                .SetDisposeOnComplete(false);

            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(TweenState.Running, t.State);
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(TweenState.Completed, t.State);
            Assert.AreEqual(10, value);
        }
        
        [UnityTest]
        public IEnumerator FloatRestart()
        {
            float value = 0;
            Tween<float> t = Tween.Create<float>()
                .SetStart(1)
                .SetEnd(2)
                .SetDuration(0.1f)
                .SetOnValueUpdated(f => value = f)
                .SetLoops(2)
                .SetDisposeOnComplete(false);

            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(TweenState.Running, t.State);
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(TweenState.Completed, t.State);
            Assert.AreEqual(2, value);
        }
        
        [UnityTest]
        public IEnumerator FloatEvenYoYo()
        {
            float value = 0;
            Tween<float> t = Tween.Create<float>()
                .SetStart(1)
                .SetEnd(2)
                .SetDuration(0.1f)
                .SetOnValueUpdated(f => value = f)
                .SetLoops(2, LoopType.YoYo)
                .SetDisposeOnComplete(false);
            
            yield return new WaitForSeconds(0.02f);
            Assert.AreEqual(TweenState.Running, t.State);
            yield return new WaitForSeconds(0.09f);
            Assert.AreEqual(TweenState.RewindRunning, t.State);
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(TweenState.RewindCompleted, t.State);
            Assert.AreEqual(1, value);
        }
        
        [UnityTest]
        public IEnumerator FloatOddYoYo()
        {
            float value = 0;
            Tween<float> t = Tween.Create<float>()
                .SetStart(1)
                .SetEnd(2)
                .SetDuration(0.1f)
                .SetOnValueUpdated(f => value = f)
                .SetLoops(3, LoopType.YoYo)
                .SetDisposeOnComplete(false);
            
            yield return new WaitForSeconds(0.02f);
            Assert.AreEqual(TweenState.Running, t.State);
            yield return new WaitForSeconds(0.09f);
            Assert.AreEqual(TweenState.RewindRunning, t.State);
            yield return new WaitForSeconds(0.2f);
            Assert.AreEqual(TweenState.Completed, t.State);
            Assert.AreEqual(2, value);
        }
    }
}