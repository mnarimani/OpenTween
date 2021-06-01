using System;
using System.Collections.Generic;
using OpenTween.Jobs;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_COLLECTIONS
using NativeListInt = Unity.Collections.NativeList<int>;

#else
using NativeListInt = OpenTween.Jobs.NativeList<int>;
#endif

namespace OpenTween
{
    [ExecuteAlways]
    internal class TweenScheduleMaster : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
        private static void Initialize()
        {
            var obj = new GameObject(nameof(TweenScheduleMaster), typeof(TweenScheduleMaster)) {hideFlags = HideFlags.HideAndDontSave};

            if (Application.isPlaying)
                DontDestroyOnLoad(obj);
        }

        private static List<Action<float>> _schedules = new List<Action<float>>();
        private static List<Action> _completes = new List<Action>();

        public static void RegisterSchedule(Action<float> schedule)
        {
            _schedules.Add(schedule);
        }
        
        public static void RegisterComplete(Action complete)
        {
            _completes.Add(complete);
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            foreach (Action<float> s in _schedules)
            {
                s(dt);
            }
            
            foreach (Action c in _completes)
            {
                c();
            }
        }
    }
}