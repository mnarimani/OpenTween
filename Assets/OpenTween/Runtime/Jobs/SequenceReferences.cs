using System.Collections.Generic;

namespace OpenTween.Jobs
{
    internal class SequenceReferences : ManagedReferences
    {
        public List<SequencedCallback> Callbacks { get; } = new List<SequencedCallback>();
        public List<SequencedTween> Tweens { get; } = new List<SequencedTween>();
    }
}