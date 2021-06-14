using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
#if UNITY_COLLECTIONS
using NativeListInt = Unity.Collections.NativeList<int>;

#else
using NativeListInt = OpenTween.Jobs.NativeList<int>;
#endif

namespace OpenTween.Jobs
{
    [BurstCompile]
    internal struct NormalQuaternionLerp : IJobParallelFor
    {
        [NativeDisableParallelForRestriction] [ReadOnly]
        public NativeArray<TweenOptions<Quaternion>> Options;

        [ReadOnly] [NativeDisableParallelForRestriction]
        public NativeListInt Indices;

        [NativeDisableParallelForRestriction] public NativeArray<TweenInternal<Quaternion>> Array;

        public unsafe void Execute(int i)
        {
            int index = Indices[i];
            
            ref TweenInternal<Quaternion> t = ref UnsafeUtility.ArrayElementAsRef<TweenInternal<Quaternion>>(Array.GetUnsafePtr(), index);
            
            if (Hint.Unlikely(!t.IsUpdatedInLastFrame))
                return;
            
            ref TweenOptions<Quaternion> options = ref UnsafeUtility.ArrayElementAsRef<TweenOptions<Quaternion>>(Options.GetUnsafePtr(), index);
            
            var end = options.IsRelative ? options.Start * options.End : options.End;
            t.CurrentValue = options.IsFrom
                ? math.slerp(end, options.Start, t.LerpParameter)
                : math.slerp(options.Start, end, t.LerpParameter);
        }
    }
}