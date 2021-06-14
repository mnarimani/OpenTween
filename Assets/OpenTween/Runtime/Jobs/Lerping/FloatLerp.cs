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
    internal struct FloatLerp : IJobParallelFor
    {
        [NativeDisableParallelForRestriction] [ReadOnly]
        public NativeArray<TweenOptions<float>> Options;

        [ReadOnly] [NativeDisableParallelForRestriction]
        public NativeListInt Indices;

        [NativeDisableParallelForRestriction] public NativeArray<TweenInternal<float>> Array;

        public unsafe void Execute(int i)
        {
            int index = Indices[i];
            ref TweenInternal<float> t = ref UnsafeUtility.ArrayElementAsRef<TweenInternal<float>>(Array.GetUnsafePtr(), index);

            if (Hint.Unlikely(!t.IsUpdatedInLastFrame))
                return;

            ref TweenOptions<float> options = ref UnsafeUtility.ArrayElementAsRef<TweenOptions<float>>(Options.GetUnsafePtr(), index);

            float end = options.IsRelative ? options.Start + options.End : options.End;
            t.CurrentValue = options.IsFrom
                ? math.lerp(end, options.Start, t.LerpParameter)
                : math.lerp(options.Start, end, t.LerpParameter);
        }
    }
}