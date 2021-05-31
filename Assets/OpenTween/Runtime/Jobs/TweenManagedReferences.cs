using System;
using System.Collections.Generic;
using UnityEngine;

namespace OpenTween.Jobs
{
    internal class TweenManagedReferences<T> : ManagedReferences
    {
        public Func<T> StartEvalFunc;
        public readonly List<Action<T>> ValueUpdated = new List<Action<T>>(2);

        internal void OnValueUpdated(T obj)
        {
            for (int index = ValueUpdated.Count - 1; index >= 0; index--)
            {
                ValueUpdated[index](obj);
            }
        }

        public override void ResetToDefaults()
        {
            base.ResetToDefaults();

            ValueUpdated.Clear();
            StartEvalFunc = null;
        }
    }
}