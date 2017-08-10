using NUnit.Framework;

namespace Collector.Tests
{
    public class AddressableTests
    {
        [Test]
        public void ShouldScope()
        {
            MemoryMock memory = new MemoryMock(20);
            Addressable scope = memory.Scope(10);

            memory.Set(13, 10);
            Assert.That(scope.Get(3), Is.EqualTo(10));
        }
    }
}