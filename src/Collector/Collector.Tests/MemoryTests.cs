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
        public void ShouldModifyRequestedRangeBetweenBlocks()
        {
            Memory memory = new Memory(1024);
            memory.Set(1022, new byte[] { 15, 16, 17, 18 });

            Assert.That(memory.Get(1022), Is.EqualTo(15));
            Assert.That(memory.Get(1023), Is.EqualTo(16));
            Assert.That(memory.Get(1024), Is.EqualTo(17));
            Assert.That(memory.Get(1025), Is.EqualTo(18));
        }
    }
}