using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace OpenTween.Sequences
{
    [TestFixture]
    public class SequenceTests
    {
        [UnityTest]
        public IEnumerator AppendTime()
        {
            var sequence = Sequence.Create();
            sequence.DisposeOnComplete = false;
            sequence.Append(1);
            Assert.AreEqual(TweenState.NotPlayed, sequence.State);
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(TweenState.Running, sequence.State);
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(TweenState.Completed, sequence.State);
        }

        [UnityTest]
        public IEnumerator AppendCallback()
        {
            var tracker = new CallbackTracker(1);
            var sequence = Sequence.Create();
            sequence.DisposeOnComplete = false;
            sequence.Append(tracker[0]);
            yield return null;
            tracker.AssertTrue();
            tracker.Clear();
            for (int i = 0; i < 10; i++)
            {
                yield return null;
                tracker.AssertFalse();
            }
        }

        [UnityTest]
        public IEnumerator AppendTimeCallback()
        {
            var tracker = new CallbackTracker(3);
            var sequence = Sequence.Create();
            sequence.Append(tracker[0]);
            sequence.Append(0.3f);
            sequence.Append(tracker[1]);
            sequence.Append(0.3f);
            sequence.Append(tracker[2]);

            yield return null;
            tracker.AssertTrue(0);
            tracker.AssertFalse(1);
            tracker.AssertFalse(2);

            yield return new WaitForSeconds(0.1f);
            tracker.AssertFalse(1);
            tracker.AssertFalse(2);
            yield return new WaitForSeconds(0.2f);
            tracker.AssertTrue(1);
            tracker.AssertFalse(2);

            yield return new WaitForSeconds(0.1f);
            tracker.AssertFalse(2);
            yield return new WaitForSeconds(0.2f);
            tracker.AssertTrue(2);
        }

        [UnityTest]
        public IEnumerator AppendTween()
        {
            float tween1 = 0, tween2 = 0;

            var sequence = Sequence.Create();
            sequence.Append(Tween.Create<float>()
                .SetStart(1)
                .SetEnd(2)
                .SetDuration(1)
                .SetOnValueUpdated(f => tween1 = f)
            );
            sequence.Append(Tween.Create<float>()
                .SetStart(3)
                .SetEnd(4)
                .SetDuration(1)
                .SetOnValueUpdated(f => tween2 = f)
            );

            yield return new WaitForSeconds(0.9f);
            Assert.AreEqual(0, tween2);
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(2, tween1);
            yield return new WaitForSeconds(0.1f);
            Assert.LessOrEqual(3, tween2);
            yield return new WaitForSeconds(1);
            Assert.AreEqual(4, tween2);
        }

        [UnityTest]
        public IEnumerator AppendTweenTime()
        {
            float tween1 = 0, tween2 = 0;

            var sequence = Sequence.Create();
            sequence.DisposeOnComplete = false;
            sequence.Append(1);
            sequence.Append(Tween.Create<float>()
                .SetStart(1)
                .SetEnd(2)
                .SetDuration(1)
                .SetOnValueUpdated(f => tween1 = f)
            );
            sequence.Append(1);
            sequence.Append(Tween.Create<float>()
                .SetStart(3)
                .SetEnd(4)
                .SetDuration(1)
                .SetOnValueUpdated(f => tween2 = f)
            );
            sequence.Append(1);

            // First Delay
            yield return new WaitForSeconds(0.9f);
            Assert.AreEqual(0, tween1);
            Assert.AreEqual(0, tween2);
            yield return new WaitForSeconds(0.1f);

            // First Tween tween
            yield return new WaitForSeconds(0.9f);
            Assert.GreaterOrEqual(tween1, 0.5f);
            Assert.AreEqual(0, tween2);
            yield return new WaitForSeconds(0.1f);

            // Second Delay
            yield return new WaitForSeconds(0.9f);
            Assert.AreEqual(2, tween1);
            Assert.AreEqual(0, tween2);
            yield return new WaitForSeconds(0.1f);

            // Second Tween
            yield return new WaitForSeconds(0.9f);
            Assert.GreaterOrEqual(tween2, 0.5f);
            Assert.GreaterOrEqual(tween2, 0.5f);
            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(TweenState.Running, sequence.State);

            // Last delay
            yield return new WaitForSeconds(1);
            
            Assert.AreEqual(TweenState.Completed, sequence.State);
        }
    }
}