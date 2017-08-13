using NUnit.Framework;

namespace Collector.Tests
{
    public class SubstituteTests
    {
        public class Item
        {
            public long Value { get; set; }
        }

        [Test]
        public void ShouldAccessRegisteredProperty()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>();

            Addressable source = new MemoryMock();
            Substitute<Item> data = new Substitute<Item>(serializer, source);

            data.Add("Value", () => 10);
            Assert.That(data.AsDynamic().Value, Is.EqualTo(10));
        }

        [Test]
        public void ShouldCastToAnotherObject()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>();

            Addressable source = new MemoryMock(new byte[]
            {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0a
            });

            Substitute<Item> data = new Substitute<Item>(serializer, source);

            Item item = data.AsDynamic();
            Assert.That(item.Value, Is.EqualTo(10));
        }
    }
}