using NUnit.Framework;

namespace Collector.Tests
{
    public class StorageTests
    {
        [Test]
        public void ShouldAllocate()
        {
            Storage storage = new Storage(1024);
            StorageAllocation allocation = storage.Allocate();

            Assert.That(allocation, Is.Not.Null);
            Assert.That(allocation.Position, Is.EqualTo(0));
        }

        [Test]
        public void ShouldAllocateNextAvailable()
        {
            Storage storage = new Storage(1024);
            storage.Commit(256);

            StorageAllocation allocation = storage.Allocate();
            Assert.That(allocation.Position, Is.EqualTo(256));
        }

        [Test]
        public void ShouldReleaseUnusedBlock()
        {
            Storage storage = new Storage(1024);
            StorageAllocation allocation = storage.Allocate();

            storage.Commit(1500);
            allocation.Set(1499, 0);

            storage.Release(1024);
            storage.Release();

            Assert.That(storage.UsedSize, Is.EqualTo(476));
            Assert.That(storage.TotalSize, Is.EqualTo(1024));
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
            StorageAllocation allocation = storage.Allocate();

            storage.Commit(256);
            allocation.Set(255, 0);

            Assert.That(storage.TotalSize, Is.EqualTo(1024));
        }

        [Test]
        public void ShouldHaveUsedSize()
        {
            Storage storage = new Storage(1024);
            storage.Commit(256);

            Assert.That(storage.UsedSize, Is.EqualTo(256));
        }
    }
}