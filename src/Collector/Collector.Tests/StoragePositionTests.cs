using NUnit.Framework;

namespace Collector.Tests
{
    public class StoragePositionTests
    {
        [Test]
        public void ShouldHaveStartPosition()
        {
            StoragePosition position = new StoragePosition();

            Assert.That(position.Value, Is.Zero);
        }

        [Test]
        public void ShouldIncreasePosition()
        {
            StoragePosition position = new StoragePosition();
            position.Increase(100);

            Assert.That(position.Value, Is.EqualTo(100));
        }

        [Test]
        public void ShouldClonePosition()
        {
            StoragePosition position = new StoragePosition();
            StoragePosition cloned = position.Clone();

            Assert.That(cloned, Is.Not.SameAs(position));
        }
    }
}