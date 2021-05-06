using System.Collections.Generic;
using UnityEngine;
namespace OpenTween
{
    public class Tween : MonoBehaviour
    {
        private static Tween instance;
        private static Tween Instance
        {
            get
            {
                return Initialize();
            }
        }

        private static Tween Initialize()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Tween>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(Tween).Name;
                    instance = obj.AddComponent<Tween>();
                }
            }
            initialized = true;
            return instance;
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as Tween;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private static List<ITween> allTweens = new List<ITween>();
        private static bool initialized;

        public static ITween To(Vector3 start, Vector3 end, float duration, ScaleFunc scaleFunc)
        {
            Init();
            var vector3Tween = new Vector3Tween();
            vector3Tween.Start(start, end, duration, scaleFunc);
            allTweens.Add(vector3Tween);
            return vector3Tween;
        }

        private static void Init()
        {
            if (initialized) return;
            else Initialize();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            foreach (var tween in allTweens)
            {
                tween.Update(deltaTime);
            }
        }
    }
}