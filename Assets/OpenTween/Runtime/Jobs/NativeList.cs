#if !UNITY_COLLECTIONS
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;

namespace OpenTween.Jobs
{
    public struct NativeList<T> : IDisposable, IEnumerable<T> where T : struct
    {
        private NativeArray<T> _inner;
        private Allocator _allocator;

        public NativeList(Allocator allocator) : this(4, allocator)
        {
        }

        public NativeList(int capacity, Allocator allocator)
        {
            _allocator = allocator;
            _inner = new NativeArray<T>(capacity, allocator, NativeArrayOptions.UninitializedMemory);
            Length = 0;
        }

        public bool IsCreated => _inner.IsCreated;
        public int Length { get; private set; }
        public int Capacity => _inner.Length;

        public void Add(T item)
        {
            if (Length >= Capacity)
            {
                int newCap = Capacity * 2;
                var temp = new NativeArray<T>(newCap, _allocator, NativeArrayOptions.UninitializedMemory);
                new CopyArrayJob<T>(_inner, temp).Schedule(_inner.Length, 32).Complete();
                _inner.Dispose();
                _inner = temp;
            }

            _inner[Length] = item;
            Length++;
        }

        public void RemoveAtSwapBack(int index)
        {
            if(index < Length - 1)
                _inner[index] = _inner[Length - 1];
            Length--;
        }

        public NativeSlice<T> GetSlice()
        {
            return new NativeSlice<T>(_inner, 0, Length);
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public T this[int i] { get => _inner[i]; set => _inner[i] = value; }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _inner.GetEnumerator();
        }
    }
}
#endif