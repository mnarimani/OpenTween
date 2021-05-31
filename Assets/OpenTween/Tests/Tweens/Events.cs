using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace OpenTween.Tweens
{
    public class Events
    {
        private class CallbackTracker
        {
            private bool[] _called;

            public CallbackTracker(int length)
            {
                _called = new bool[length];
            }

            public Action this[int index] => () => _called[index] = true;

            public void Clear()
            {
                for (int index = 0; index < _called.Length; index++)
                {
                    _called[index] = false;
                }
            }

            public void AssertTrue()
            {
                foreach (bool called in _called)
                {
                    Assert.IsTrue(called);
                }
            }
        }

        private static Tween<float> CreateTween(float duration)
        {
            return Tween.Create<float>().SetStart(0).SetEnd(10).SetDuration(duration);
        }

        [UnityTest]
        public IEnumerator Completed()
        {
            var tracker = new CallbackTracker(2);
            Tween<float> tween = CreateTween(0.2f).SetDisposeOnComplete(false);
            
            tween.OnCompleted(tracker[0]);
            tween.Completed += tracker[1];
            
            yield return new WaitForSeconds(0.21f);

            tracker.AssertTrue();

            tracker.Clear();
            
            tween.Play(true);
            
            yield return new WaitForSeconds(0.21f);
            
            tracker.AssertTrue();
        }

        [UnityTest]
        public IEnumerator Disposing_OnAutoDispose()
        {
            var tracker = new CallbackTracker(2);
            Tween<float> tween = CreateTween(0.2f);
            
            tween.OnDisposing(tracker[0]);
            tween.Disposing += tracker[1];
            
            yield return new WaitForSeconds(0.21f);

            tracker.AssertTrue();
        }

        [UnityTest]
        public IEnumerator Disposing_BoundComponentMissing()
        {
            Transform comp = new GameObject().transform;
            var tracker = new CallbackTracker(2);
            Tween<float> tween = CreateTween(1000).BindToComponent(comp);
            
            tween.OnDisposing(tracker[0]);
            tween.Disposing += tracker[1];
            
            yield return new WaitForSeconds(0.1f);

            Object.Destroy(comp.gameObject);

            yield return new WaitForSeconds(0.1f);

            tracker.AssertTrue();
        }
        
        [UnityTest]
        public IEnumerator Disposing_DirectDisposeCall()
        {
            var tracker = new CallbackTracker(2);
            Tween<float> tween = CreateTween(1000);
            
            tween.OnDisposing(tracker[0]);
            tween.Disposing += tracker[1];
            
            yield return new WaitForSeconds(0.1f);
            
            tween.Dispose();
            tracker.AssertTrue();
        }
    }
}
