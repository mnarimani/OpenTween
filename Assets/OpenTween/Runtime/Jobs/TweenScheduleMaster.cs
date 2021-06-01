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
        private static void Initialize()
        {
            _obj = new GameObject(nameof(TweenScheduleMaster), typeof(TweenScheduleMaster)) {hideFlags = HideFlags.HideAndDontSave}
                .GetComponent<TweenScheduleMaster>();

            if (Application.isPlaying)
                DontDestroyOnLoad(_obj);
        }

        private static TweenScheduleMaster _obj;

        private readonly List<Action<float>> _schedules = new List<Action<float>>();
        private readonly List<Action> _completes = new List<Action>();

        public static void RegisterSchedule(Action<float> schedule)
        {
            if (_obj == null)
                Initialize();
            _obj._schedules.Add(schedule);
        }

        public static void RegisterComplete(Action complete)
        {
            if (_obj == null)
                Initialize();
            _obj._completes.Add(complete);
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