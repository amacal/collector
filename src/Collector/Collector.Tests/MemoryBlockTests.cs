using NUnit.Framework;

namespace Collector.Tests
{
    public class MemoryBlockTests
    {
        [Test]
        public void ShouldHaveRequestedSize()
        {
            MemoryBlock block = new MemoryBlock(1024);
            Assert.That(block.Size, Is.EqualTo(1024));
        }

        [Test]
        public void ShouldModifyRequestedByte()
        {
            MemoryBlock block = new MemoryBlock(1024);
            block.Set(10, 15);

            Assert.That(block.Get(10), Is.EqualTo(15));
        }
    }
}