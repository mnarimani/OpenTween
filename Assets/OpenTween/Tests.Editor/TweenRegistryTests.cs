using NUnit.Framework;

namespace OpenTween.Tests
{
    [TestFixture]
    public class TweenRegistryTests
    {
        [Test]
        public void TweensAreAllocatedProperly()
        {
            for (int i = 0; i < 1000; i++)
            {
                TweenInternal<float> tween = TweenPool<TweenInternal<float>>.GetNew();
                // Assert.AreEqual(i, tween.Id);
            }
        }
    }
}