using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OpenTween
{
    [Serializable]
    public class OpenTweenSettings
    {
        public const int InnerLoopBatchCount = 32;
        private const string settingFileName = "OpenTweenSettings.asset";
        [SerializeField] private int _initialCapacity = 64000;
        [SerializeField] private bool _captureCreationStacktrace;
        [SerializeField] private float _defaultOvershootOrAmplitude = 1.70158f;
        [SerializeField] private float _defaultPeriod;

        private static OpenTweenSettings _instance;

        [RuntimeInitializeOnLoadMethod]
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
        private static void Load()
        {
            /*var file = Resources.Load<OpenTweenSettingsFile>(settingFileName);
            if (file != null)
            {
                _instance = file.Settings;
                return;
            }

            file = ScriptableObject.CreateInstance<OpenTweenSettingsFile>();
#if UNITY_EDITOR
            if (!Directory.Exists(Application.dataPath + "/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");
            AssetDatabase.CreateAsset(file, "Assets/Resources/" + settingFileName);
            AssetDatabase.SaveAssets();
#endif

            _instance = file.Settings;*/
            _instance = new OpenTweenSettings();
        }

        public static int InitialCapacity { get => _instance._initialCapacity; set => _instance._initialCapacity = value; }

        public static bool CaptureCreationStacktrace { get => _instance._captureCreationStacktrace; set => _instance._captureCreationStacktrace = value; }

        public static float DefaultPeriod { get => _instance._defaultPeriod; set => _instance._defaultPeriod = value; }

        public static float DefaultOvershootOrAmplitude { get => _instance._defaultOvershootOrAmplitude; set => _instance._defaultOvershootOrAmplitude = value; }
    }
}