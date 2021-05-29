using System.Diagnostics;
using NUnit.Framework;
using Debug = UnityEngine.Debug;

namespace OpenTween.Tests
{
    [TestFixture]
    public class SequenceTests
    {
        [Test]
        public void ForceComplete_InNormalMode_CallsAllCallbacks()
        {
            var seq = new SequenceInternal();
            seq.Append(10);
        }

        [Test]
        public void Benchmark()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                var tween = Tween.Create<float>();
                tween.Start = 0;
                tween.End = 10;
                tween.Duration = 1;
            }
            stopwatch.Stop();
            Debug.Log("stopwatch.ElapsedMilliseconds = " + stopwatch.ElapsedMilliseconds);
        }
    }
}