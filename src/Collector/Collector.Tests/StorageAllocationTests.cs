using NUnit.Framework;

namespace Collector.Tests
{
    public class StorageAllocationTests
    {
        [Test]
        public void ShouldHaveRequestedPosition()
        {
            Memory memory = new Memory(1024);
            StoragePosition position = new StoragePosition();

            position.Increase(300);
            StorageAllocation allocation = new StorageAllocation(memory, position);

            Assert.That(allocation.Position, Is.EqualTo(300));
        }

        [Test]
        public void ShouldReadFromAllocatedPosition()
        {
            Memory memory = new Memory(1024);
            StoragePosition position = new StoragePosition();

            position.Increase(300);
            StorageAllocation allocation = new StorageAllocation(memory, position);

            memory.Set(312, 23);
            Assert.That(allocation.Get(12), Is.EqualTo(23));
        }

        [Test]
        public void ShouldWriteToAllocatedPosition()
        {
            Memory memory = new Memory(1024);
            StoragePosition position = new StoragePosition();

            position.Increase(300);
            StorageAllocation allocation = new StorageAllocation(memory, position);

            allocation.Set(12, 23);
            Assert.That(memory.Get(312), Is.EqualTo(23));
        }
    }
}