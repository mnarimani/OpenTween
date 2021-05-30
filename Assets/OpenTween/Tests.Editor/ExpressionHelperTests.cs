using System;
using System.Linq.Expressions;
using NUnit.Framework;
using UnityEngine.UI;

namespace OpenTween.Tests
{
    [TestFixture]
    public class ExpressionHelperTests
    {
        [Test]
        public void MultiNested()
        {
            Expression<Func<A, float>> getter = a => a.b.c.d.value;
            Action<A, float> setter = ExpressionHelper.CreateSetter(getter);

            var instance = new A {b = new B {c = new C {d = new D()}}};
            setter(instance, 10);
            Assert.AreEqual(10, instance.b.c.d.value);
        }

        public class A
        {
            public B b { get; set; }
        }

        public struct B
        {
            public C c { get; set; }
        }

        public struct C
        {
            public D d { get; set; }
        }

        public struct D
        {
            public float value { get; set; }
        }
    }
}