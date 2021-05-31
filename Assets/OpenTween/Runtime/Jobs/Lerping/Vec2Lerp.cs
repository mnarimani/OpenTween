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
    internal struct Vec2Lerp : IJobParallelFor
    {
        [NativeDisableParallelForRestriction] [ReadOnly]
        public NativeArray<TweenOptions<Vector2>> Options;

        [ReadOnly] [NativeDisableParallelForRestriction]
        public NativeListInt Indices;

        [NativeDisableParallelForRestriction] public NativeArray<TweenInternal<Vector2>> Array;

        public void Execute(int i)
        {
            int index = Indices[i];
            TweenInternal<Vector2> t = Array[index];
            if (Hint.Unlikely(!t.ValueChangedInLastFrame))
                return;
            TweenOptions<Vector2> options = Options[index];
            t.CurrentValue = math.lerp(options.Start, options.End, t.LerpParameter);
            Array[index] = t;
        }
    }
}