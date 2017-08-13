using NUnit.Framework;

namespace Collector.Tests
{
    public class IndexPositionTests
    {
        [Test]
        public void ShouldHaveStartPosition()
        {
            IndexPosition position = new IndexPosition();

            Assert.That(position.Value, Is.Zero);
        }

        [Test]
        public void ShouldIncreasePosition()
        {
            IndexPosition position = new IndexPosition();
            position.Increase();

            Assert.That(position.Value, Is.EqualTo(8));
        }

        [Test]
        public void ShouldResolePosition()
        {
            IndexPosition position = new IndexPosition();
            long index = position.At(9);

            Assert.That(index, Is.EqualTo(72));
        }

        [Test]
        public void ShouldResolveCount()
        {
            IndexPosition position = new IndexPosition();

            position.Increase();
            position.Increase();

            Assert.That(position.Size, Is.EqualTo(2));
        }
    }
}