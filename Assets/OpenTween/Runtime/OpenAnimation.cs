using System;
using System.Collections.Generic;
using UnityEngine;

namespace OpenTween
{
    public partial class OpenAnimation : MonoBehaviour
    {
        [SerializeField] private List<Anim> _animations;
        [SerializeField] private bool _playOnEnable = true;

        public bool PlayOnEnable { get => _playOnEnable; set => _playOnEnable = value; }

        private void OnEnable()
        {
            Play(null);
        }

        public void Play(List<ITween> tweens)
        {
            tweens?.Clear();
            foreach (Anim a in _animations)
            {
                ITween t = a.Animation.Play();
                tweens?.Add(t);
            }
        }

        [Serializable]
        public class Anim
        {
            [SerializeField] private string _method;

            // ReSharper disable once UnassignedField.Local
            [SerializeReference] private IAnimation _parameters;

            // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
            public IAnimation Animation => _parameters;
        }
    }
}