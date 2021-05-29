using System;
using System.Collections.Generic;
using UnityEngine;

namespace OpenTween
{
    internal class TweenUpdater : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            var obj = new GameObject(nameof(TweenUpdater), typeof(TweenUpdater)) {hideFlags = HideFlags.HideAndDontSave};
            DontDestroyOnLoad(obj);
        }

        private static readonly List<Action<float>> Updates = new List<Action<float>>();

        public static void RegisterUpdate(Action<float> updateFunc)
        {
            Updates.Add(updateFunc);
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            foreach (Action<float> u in Updates)
            {
                u(deltaTime);
            }
        }
    }
}