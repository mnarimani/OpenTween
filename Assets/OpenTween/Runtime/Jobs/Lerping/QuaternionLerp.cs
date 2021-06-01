﻿using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
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

        public void Execute(int i)
        {
            int index = Indices[i];
            TweenInternal<quaternion> t = Array[index];
            if (Hint.Unlikely(!t.IsUpdatedInLastFrame))
                return;
            TweenOptions<quaternion> options = Options[index];
            var end = options.IsRelative ? math.mul(options.Start,  options.End) : options.End;
            t.CurrentValue = math.slerp(options.Start, end, t.LerpParameter);
            Array[index] = t;
        }
    }
}