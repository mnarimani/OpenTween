using System.Collections.Generic;

namespace OpenTween
{
    internal class Pool<T> where T : new()
    {
        private readonly Stack<T> _pool;

        public Pool(int capacity)
        {
            _pool = new Stack<T>(capacity);
        }
        public T GetNew()
        {
            return _pool.Count > 0 ? _pool.Pop() : new T();
        }

        public void Return(T instance)
        {
            _pool.Push(instance);
        }
    }
}