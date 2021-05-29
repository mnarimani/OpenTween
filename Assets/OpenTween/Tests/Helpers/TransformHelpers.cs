using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace OpenTween.Tests.Helpers
{
    [TestFixture]
    public class TransformHelpers : HelperTestBase<Transform>
    {
        protected override Transform Create()
        {
            Transform parent = new GameObject("Test Transform").transform;
            parent.position = Vector3.one;
            Created.Add(parent.gameObject);
            
            Transform child = new GameObject("child").transform;
            child.SetParent(parent, false);
            return child;
        }

        [UnityTest] public IEnumerator DOMove() => Verify(UnityHelpers.DOMove, t => t.position);
        [UnityTest] public IEnumerator DOLocalMove() => Verify(UnityHelpers.DOLocalMove, t => t.localPosition);
    }
}