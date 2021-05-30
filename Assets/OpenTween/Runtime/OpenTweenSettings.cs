using System;
using System.IO;
using UnityEngine;

namespace OpenTween
{
    [Serializable]
    public class OpenTweenSettings
    {
        [SerializeField] private int _initialCapacity = 500;
        [SerializeField] private bool _captureCreationStacktrace;
        [SerializeField] private float _defaultOvershootOrAmplitude = 1.70158f;
        [SerializeField] private float _defaultPeriod;

        private static OpenTweenSettingsFile _file;

        private static OpenTweenSettings Instance
        {
            get
            {
                try
                {
                    if (_file != null)
                        return _file.Settings;

                    _file = Resources.Load<OpenTweenSettingsFile>("OpenTweenSettings.asset");
                    if (_file != null)
                        return _file.Settings;

                    _file = ScriptableObject.CreateInstance<OpenTweenSettingsFile>();
#if UNITY_EDITOR
                    if (!Directory.Exists(Application.dataPath + "/Resources"))
                        UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
                    UnityEditor.AssetDatabase.CreateAsset(_file, "Assets/Resources/OpenTweenSettings.asset");
                    UnityEditor.AssetDatabase.SaveAssets();
#endif

                    return _file.Settings;
                }
                catch
                {
                    return new OpenTweenSettings();
                }
            }
        }

        public static int InitialCapacity { get => Instance._initialCapacity; set => Instance._initialCapacity = value; }

        public static bool CaptureCreationStacktrace { get => Instance._captureCreationStacktrace; set => Instance._captureCreationStacktrace = value; }

        public static float DefaultPeriod { get => Instance._defaultPeriod; set => Instance._defaultPeriod = value; }

        public static float DefaultOvershootOrAmplitude { get => Instance._defaultOvershootOrAmplitude; set => Instance._defaultOvershootOrAmplitude = value; }
    }
}