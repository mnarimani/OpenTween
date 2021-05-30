using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace OpenTween.Tests.Helpers
{
    [TestFixture]
    public class ImageHelpers : HelperTestBase<Image>
    {
        [UnityTest] public IEnumerator DOAlpha() => Verify(UnityHelpers.DOAlpha, t => t.color.a);
    }
}