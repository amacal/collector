using NUnit.Framework;

namespace Collector.Tests
{
    public class StoragePositionTests
    {
        [Test]
        public void ShouldHaveIntialLowPosition()
        {
            StoragePosition position = new StoragePosition();

            Assert.That(position.Low, Is.Zero);
        }

        [Test]
        public void ShouldHaveIntialHighPosition()
        {
            StoragePosition position = new StoragePosition();

            Assert.That(position.High, Is.Zero);
        }

        [Test]
        public void ShouldIncreaseHighPosition()
        {
            StoragePosition position = new StoragePosition();
            position.Increase(100);

            Assert.That(position.High, Is.EqualTo(100));
        }

        [Test]
        public void ShouldIncreaseLowPosition()
        {
            StoragePosition position = new StoragePosition();

            position.Increase(100);
            position.Decrease(50);

            Assert.That(position.Low, Is.EqualTo(50));
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