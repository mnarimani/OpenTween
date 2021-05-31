using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace OpenTween.Jobs
{
    internal static class NativeArrayRefExtensions
    {
        public static ref T GetRef<T>(this NativeArray<T> array, int index)
            where T : struct
        {
#if UNITY_EDITOR
            // You might want to validate the index first, as the unsafe method won't do that.
            if (index < 0 || index >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(index), index, "Index out of range. This check will be removed in build. Make sure to check boundaries of the array before calling GetRef<T>");
#endif
            unsafe
            {
                return ref UnsafeUtility.ArrayElementAsRef<T>(array.GetUnsafePtr(), index);
            }
        }
    }
}