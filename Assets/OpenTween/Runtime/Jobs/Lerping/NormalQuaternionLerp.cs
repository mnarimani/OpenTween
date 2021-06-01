using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
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

        public void Execute(int i)
        {
            int index = Indices[i];
            TweenInternal<Quaternion> t = Array[index];
            if (Hint.Unlikely(!t.IsUpdatedInLastFrame))
                return;
            TweenOptions<Quaternion> options = Options[index];
            var end = options.IsRelative ? options.Start * options.End : options.End;
            t.CurrentValue = math.slerp(options.Start, end, t.LerpParameter);
            Array[index] = t;
        }
    }
}