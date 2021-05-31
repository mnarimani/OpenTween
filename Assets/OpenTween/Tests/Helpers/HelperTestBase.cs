using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OpenTween.Helpers
{
    public abstract class HelperTestBase<TComponent> where TComponent : Component
    {
        protected delegate Tween<T> TweenCreator<T>(TComponent component);

        protected readonly List<GameObject> Created = new List<GameObject>();
        private const float Duration = 0.4f;

        [OneTimeSetUp]
        public void SetTweenValues()
        {
            TweenValues<Vector2>.Start = Vector2.left;
            TweenValues<Vector2>.End = Vector2.right;

            TweenValues<Vector3>.Start = Vector3.left;
            TweenValues<Vector3>.End = Vector3.right;

            TweenValues<Vector4>.Start = Vector4.zero;
            ;
            TweenValues<Vector4>.End = Vector4.one;

            TweenValues<Quaternion>.Start = Quaternion.Euler(0, 90, 0);
            TweenValues<Quaternion>.End = Quaternion.Euler(0, 180, 0);

            TweenValues<Color>.Start = Color.white;
            TweenValues<Color>.End = Color.black;

            TweenValues<float>.Start = 0;
            TweenValues<float>.End = 1;
        }

        [OneTimeSetUp]
        public void SetAssertions()
        {
            Assertions<Vector2>.AreEqual = AssertEqual;
            Assertions<Vector3>.AreEqual = AssertEqual;
            Assertions<Vector4>.AreEqual = AssertEqual;
            Assertions<Quaternion>.AreEqual = AssertEqual;
            Assertions<float>.AreEqual = AssertEqual;
        }

        [TearDown]
        public void DestroyCreated()
        {
            foreach (GameObject obj in Created)
            {
                if (obj == null)
                    continue;

                if (Application.isPlaying)
                    Object.Destroy(obj);
                else
                    Object.DestroyImmediate(obj);
            }

            Created.Clear();
        }

        protected IEnumerator Verify<T>(TweenCreator<T> creator, Expression<Func<TComponent, T>> property)
        {
            Func<TComponent, T> getter = property.Compile();
            Action<TComponent, T> setter = ExpressionHelper.CreateSetter(property);
            
            yield return RunTween(creator, getter);
            yield return RunTweenDestroyInMiddle(creator);
            yield return RunTweenDynStart(creator, getter, setter);
        } 
        
        protected virtual TComponent Create()
        {
            var obj = new GameObject("Test " + typeof(TComponent).Name, typeof(TComponent));
            Created.Add(obj);
            return obj.GetComponent<TComponent>();
        }
        
        private IEnumerator RunTween<T>(TweenCreator<T> creator, Func<TComponent, T> getter)
        {
            TComponent comp = Create();
            Tween<T> tween = creator(comp);
            tween.Start = TweenValues<T>.Start;
            tween.End = TweenValues<T>.End;
            tween.Duration = Duration;
            tween.Play();
            yield return new WaitForSeconds(1.1f);
            Assertions<T>.AreEqual(TweenValues<T>.End, getter(comp), "RunTween: Invalid end value");
        }

        private IEnumerator RunTweenDestroyInMiddle<T>(TweenCreator<T> creator)
        {
            TComponent comp = Create();
            Tween<T> tween = creator(comp);
            tween.Start = TweenValues<T>.Start;
            tween.End = TweenValues<T>.End;
            tween.Duration = Duration;
            tween.Play();
            float halfDuration = tween.Duration / 2;
            yield return new WaitForSeconds(halfDuration);
            Object.Destroy(comp.gameObject);
            yield return new WaitForSeconds(halfDuration);
            Assert.IsFalse(tween.IsActive(), "RunTweenDestroyInMiddle: Tween is active");
        }

        private IEnumerator RunTweenDynStart<T>(TweenCreator<T> creator, Func<TComponent, T> getter, Action<TComponent, T> setter)
        {
            TComponent comp = Create();
            setter(comp, TweenValues<T>.Start);
            Tween<T> tween = creator(comp);
            tween.DynamicStartEval = true;
            tween.Start = default;
            tween.End = TweenValues<T>.End;
            tween.Duration = Duration;
            tween.Play();

            Assertions<T>.AreEqual(TweenValues<T>.Start, tween.Start, "RunTweenDynStart: Start value is not evaluated.");
            yield return new WaitForSeconds(Duration + 0.1f);
            Assertions<T>.AreEqual(TweenValues<T>.End, getter(comp), "RunTweenDynStart: Invalid end value.");
        }



        private static void AssertEqual(Vector4 a, Vector4 b, string message)
        {
            Assert.LessOrEqual((a - b).magnitude, 0.001f, message);
        }

        private static void AssertEqual(Vector3 a, Vector3 b, string message)
        {
            Assert.LessOrEqual((a - b).magnitude, 0.001f, message);
        }

        private static void AssertEqual(Vector2 a, Vector2 b, string message)
        {
            Assert.LessOrEqual((a - b).magnitude, 0.001f, message);
        }

        private static void AssertEqual(Quaternion a, Quaternion b, string message)
        {
            Assert.True(a == b, message);
        }

        private static void AssertEqual(float a, float b, string message)
        {
            Assert.LessOrEqual((a - b), 0.001f, message);
        }
    }
}