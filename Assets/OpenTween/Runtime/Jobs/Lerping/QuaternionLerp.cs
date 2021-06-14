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
    internal struct QuaternionLerp : IJobParallelFor
    {
        [NativeDisableParallelForRestriction] [ReadOnly]
        public NativeArray<TweenOptions<quaternion>> Options;

        [ReadOnly] [NativeDisableParallelForRestriction]
        public NativeListInt Indices;

        [NativeDisableParallelForRestriction] public NativeArray<TweenInternal<quaternion>> Array;

        public unsafe void Execute(int i)
        {
            int index = Indices[i];
            
            ref TweenInternal<quaternion> t = ref UnsafeUtility.ArrayElementAsRef<TweenInternal<quaternion>>(Array.GetUnsafePtr(), index);
            
            if (Hint.Unlikely(!t.IsUpdatedInLastFrame))
                return;
            
            ref TweenOptions<quaternion> options = ref UnsafeUtility.ArrayElementAsRef<TweenOptions<quaternion>>(Options.GetUnsafePtr(), index);
            
            var end = options.IsRelative ? math.mul(options.Start, options.End) : options.End;
            t.CurrentValue = options.IsFrom
                ? math.slerp(end, options.Start, t.LerpParameter)
                : math.slerp(options.Start, end, t.LerpParameter);
        }
    }
}