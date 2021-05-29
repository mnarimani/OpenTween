using System;

namespace OpenTween
{
    internal readonly struct SequencedCallback
    {
        public readonly float Position;
        public readonly Action Callback;

        public SequencedCallback(float position, Action callback)
        {
            Position = position;
            Callback = callback;
        }
    }
}