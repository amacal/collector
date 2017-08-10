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
    }
}