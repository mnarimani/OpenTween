using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
#if UNITY_COLLECTIONS
using NativeListInt = Unity.Collections.NativeList<int>;

#else
using NativeListInt = OpenTween.Jobs.NativeList<int>;
#endif

namespace OpenTween.Jobs
{
    [BurstCompile]
    internal struct Float2Lerp : IJobParallelFor
    {
        [NativeDisableParallelForRestriction] [ReadOnly]
        public NativeArray<TweenOptions<float2>> Options;

        [ReadOnly] [NativeDisableParallelForRestriction]
        public NativeListInt Indices;

        [NativeDisableParallelForRestriction] public NativeArray<TweenInternal<float2>> Array;

        public unsafe void Execute(int i)
        {
            int index = Indices[i];
           
            ref TweenInternal<float2> t = ref UnsafeUtility.ArrayElementAsRef<TweenInternal<float2>>(Array.GetUnsafePtr(), index);

            if (Hint.Unlikely(!t.IsUpdatedInLastFrame))
                return;

            ref TweenOptions<float2> options = ref UnsafeUtility.ArrayElementAsRef<TweenOptions<float2>>(Options.GetUnsafePtr(), index);

            var end = options.IsRelative ? options.Start + options.End : options.End;
            t.CurrentValue = options.IsFrom
                ? math.lerp(end, options.Start, t.LerpParameter)
                : math.lerp(options.Start, end, t.LerpParameter);
        }
    }
}