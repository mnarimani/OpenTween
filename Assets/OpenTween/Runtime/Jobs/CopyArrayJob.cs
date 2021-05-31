using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace OpenTween.Jobs
{
    [BurstCompile]
    public struct CopyArrayJob<T> : IJobParallelFor where T : struct
    {
        [ReadOnly] private NativeArray<T> _source;
        private NativeArray<T> _dest;

        public CopyArrayJob(NativeArray<T> source, NativeArray<T> dest)
        {
            _dest = dest;
            _source = source;
        }

        public void Execute(int index)
        {
            _dest[index] = _source[index];
        }
    }
}