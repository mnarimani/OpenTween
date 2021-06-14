using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace OpenTween.Tweens
{
    public class Events
    {


        private static Tween<float> CreateTween(float duration)
        {
            return Tween.Create<float>().SetStart(0).SetEnd(10).SetDuration(duration);
        }

        [UnityTest]
        public IEnumerator Completed()
        {
            var tracker = new CallbackTracker(2);
            Tween<float> tween = CreateTween(0.2f).SetDisposeOnComplete(false);
            
            tween.SetOnCompleted(tracker[0]);
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
            
            tween.SetOnDisposing(tracker[0]);
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
            
            tween.SetOnDisposing(tracker[0]);
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
            
            tween.SetOnDisposing(tracker[0]);
            tween.Disposing += tracker[1];
            
            yield return new WaitForSeconds(0.1f);
            
            tween.Dispose();
            tracker.AssertTrue();
        }
    }
}
