using NUnit.Framework;

namespace Collector.Tests
{
    public class MemoryTests
    {
        [Test]
        public void ShouldHaveInitialSize()
        {
            Memory memory = new Memory(1024);
            Assert.That(memory.Size, Is.EqualTo(0));
        }

        [Test]
        public void ShouldHaveIncreasedSize()
        {
            Memory memory = new Memory(1024);
            memory.Get(1732);

            Assert.That(memory.Size, Is.EqualTo(2048));
        }

        [Test]
        public void ShouldHaveDecreasedSize()
        {
            Memory memory = new Memory(1024);

            memory.Get(2732);
            memory.Release(2048);

            Assert.That(memory.Size, Is.EqualTo(1024));
        }

        [Test]
        public void ShouldModifyRequestedNearByte()
        {
            Memory memory = new Memory(1024);
            memory.Set(732, 17);

            Assert.That(memory.Get(732), Is.EqualTo(17));
        }

        [Test]
        public void ShouldModifyRequestedFarByte()
        {
            Memory memory = new Memory(1024);
            memory.Set(10732, 17);

            Assert.That(memory.Get(10732), Is.EqualTo(17));
        }

        [Test]
        public void ShouldReadRequestedRange()
        {
            byte[] store = new byte[2];
            Memory memory = new Memory(1024);

            memory.Set(10, 15);
            memory.Set(11, 16);

            memory.GetBytes(10, store);
            Assert.That(store, Is.EqualTo(new byte[] { 15, 16 }));
        }

        [Test]
        public void ShouldModifyRequestedRange()
        {
            Memory memory = new Memory(1024);
            memory.SetBytes(10, new byte[] { 15, 16 });

            Assert.That(memory.Get(10), Is.EqualTo(15));
            Assert.That(memory.Get(11), Is.EqualTo(16));
        }

        [Test]
        public void ShouldReadRequestedRangeBetweenBlocks()
        {
            byte[] store = new byte[4];
            Memory memory = new Memory(1024);

            memory.Set(1022, 15);
            memory.Set(1023, 16);
            memory.Set(1024, 17);
            memory.Set(1025, 18);

            memory.GetBytes(1022, store);
            Assert.That(store, Is.EqualTo(new byte[] { 15, 16, 17, 18 }));
        }

        [Test]
        public void ShouldModifyRequestedRangeBetweenBlocks()
        {
            Memory memory = new Memory(1024);
            memory.SetBytes(1022, new byte[] { 15, 16, 17, 18 });

            Assert.That(memory.Get(1022), Is.EqualTo(15));
            Assert.That(memory.Get(1023), Is.EqualTo(16));
            Assert.That(memory.Get(1024), Is.EqualTo(17));
            Assert.That(memory.Get(1025), Is.EqualTo(18));
        }
    }
}