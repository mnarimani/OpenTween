using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace OpenTween.Jobs
{
    internal class ManagedReferences
    {
        public int Version;
        public string CreationStacktrace;
        public bool HasBoundComponent;
        public Component BoundComponent;

        private List<Action> _playStarted;
        private List<Action> _paused;
        private List<Action> _completed;
        private List<Action> _rewindStarted;
        private List<Action> _rewindPaused;
        private List<Action> _rewindCompleted;
        private List<Action> _disposing;

        public List<Action> PlayStarted => (_playStarted ??= new List<Action>(2));
        
        public List<Action> Paused => (_paused ??= new List<Action>(2));

        public List<Action> Completed => (_completed ??= new List<Action>(2));

        public List<Action> RewindStarted => (_rewindStarted ??= new List<Action>(2));

        public List<Action> RewindPaused => (_rewindPaused ??= new List<Action>(2));

        public List<Action> RewindCompleted => (_rewindCompleted ??= new List<Action>(2));

        public List<Action> Disposing => (_disposing ??= new List<Action>(2));

        internal void OnPlayStarted()
        {
            if (_playStarted == null) return;

            for (int index = _playStarted.Count - 1; index >= 0; index--)
            {
                _playStarted[index]();
            }
        }

        internal void OnPaused()
        {
            if (_paused == null) return;

            for (int index = _paused.Count - 1; index >= 0; index--)
            {
                _paused[index]();
            }
        }

        internal void OnCompleted()
        {
            if (_completed == null) return;
            
            for (int index = Completed.Count - 1; index >= 0; index--)
            {
                _completed[index]();
            }
        }

        internal void OnRewindStarted()
        {
            if (_rewindStarted == null) return;

            for (int index = _rewindStarted.Count - 1; index >= 0; index--)
            {
                _rewindStarted[index]();
            }
        }

        internal void OnRewindPaused()
        {
            if (_rewindPaused == null) return;
            
            for (int index = _rewindPaused.Count - 1; index >= 0; index--)
            {
                _rewindPaused[index]();
            }
        }

        internal void OnRewindCompleted()
        {
            if (_rewindCompleted == null) return;
            
            for (int index = _rewindCompleted.Count - 1; index >= 0; index--)
            {
                _rewindCompleted[index]();
            }
        }

        internal void OnDisposing()
        {
            if (_disposing == null) return;
            
            for (int index = _disposing.Count - 1; index >= 0; index--)
            {
                _disposing[index]();
            }
        }

        public virtual void ResetToDefaults()
        {
            HasBoundComponent = default;
            BoundComponent = default;
            CreationStacktrace = default;

            _playStarted?.Clear();
            _paused?.Clear();
            _completed?.Clear();
            _rewindStarted?.Clear();
            _rewindPaused?.Clear();
            _rewindCompleted?.Clear();
            _disposing?.Clear();
        }
    }
}