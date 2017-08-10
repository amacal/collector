using NUnit.Framework;

namespace Collector.Tests
{
    public class StorageTests
    {
        [Test]
        public void ShouldAllocate()
        {
            Storage storage = new Storage(1024);
            StorageAllocation allocation = storage.Allocate(256);

            Assert.That(allocation, Is.Not.Null);
            Assert.That(allocation.Position, Is.EqualTo(0));
        }

        [Test]
        public void ShouldAllocateNextAvailable()
        {
            Storage storage = new Storage(1024);
            storage.Allocate(256);

            StorageAllocation allocation = storage.Allocate(256);
            Assert.That(allocation.Position, Is.EqualTo(256));
        }

        [Test]
        public void ShouldAccessByPosition()
        {
            Storage storage = new Storage(1024);
            StorageAllocation allocation = storage.At(13);

            Assert.That(allocation.Position, Is.EqualTo(13));
        }

        [Test]
        public void ShouldHaveTotalSize()
        {
            Storage storage = new Storage(1024);
            storage.Allocate(256);

            Assert.That(storage.TotalSize, Is.EqualTo(1024));
        }

        [Test]
        public void ShouldHaveUsedSize()
        {
            Storage storage = new Storage(1024);
            storage.Allocate(256);

            Assert.That(storage.UsedSize, Is.EqualTo(256));
        }
    }
}