using System;
using NUnit.Framework;

namespace OpenTween
{
    public class CallbackTracker
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

        public void AssertFalse()
        {
            foreach (bool called in _called)
            {
                Assert.IsFalse(called);
            }
        }

        public void AssertTrue(int index)
        {
            Assert.IsTrue(_called[index]);
        }

        public void AssertFalse(int index)
        {
            Assert.IsFalse(_called[index]);
        }
    }
}