using System.Collections;
using NUnit.Framework;

namespace Collector.Tests
{
    public class SubstituteArrayTests
    {
        [Test]
        public void ShouldBeEnumerable()
        {
            Substitute<int>[] items = { };
            dynamic array = new SubstituteArray<int>(10, () => items);

            Assert.That(array, Is.InstanceOf<IEnumerable>());
        }
    }
}