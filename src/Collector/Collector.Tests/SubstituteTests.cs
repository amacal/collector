using NUnit.Framework;

namespace Collector.Tests
{
    public class SubstituteTests
    {
        [Test]
        public void ShouldAccessRegisteredProperty()
        {
            Substitute data = new Substitute();
            dynamic reference = data;

            data.Add("key", () => 10);
            Assert.That(reference.key, Is.EqualTo(10));
        }
    }
}