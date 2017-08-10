using NUnit.Framework;

namespace Collector.Tests
{
    public class SerializerTests
    {
        private class Item
        {
            public byte Value { get; set; }
        }

        [Test]
        public void ShouldMeasureAllMembers()
        {
            Serializer<Item> serializer = new Serializer<Item>(
                new MemberMock(7), new MemberMock(13));

            Item item = new Item { Value = 10 };
            Assert.That(serializer.Measure(item), Is.EqualTo(20));
        }

        [Test]
        public void ShouldTransferItemToMemory()
        {
            Serializer<Item> serializer = new Serializer<Item>(
                new MemberMock(7), new MemberMock(13));

            MemoryMock mock = new MemoryMock(100);
            serializer.Transfer(new Item { Value = 10 }, mock);

            Assert.That(mock.GetData(20), Is.EqualTo(new[]
            {
                70, 70, 70, 70, 70, 70, 70, 130, 130, 130,
                130, 130, 130, 130, 130, 130, 130, 130, 130, 130
            }));
        }

        [Test]
        public void ShouldTransferMemoryToItem()
        {
            Serializer<Item> serializer = new Serializer<Item>(
                new MemberMock(7), new MemberMock(13));

            MemoryMock mock = new MemoryMock(new byte[]
            {
                70, 70, 70, 70, 70, 70, 70, 130, 130, 130,
                130, 130, 130, 130, 130, 130, 130, 130, 130, 130
            });

            Item item = new Item();
            serializer.Transfer(mock, item);

            Assert.That(item.Value, Is.EqualTo(10));
        }

        private class MemberMock : Member<Item>
        {
            private readonly byte value;

            public MemberMock(byte value)
            {
                this.value = value;
            }

            public int Measure(Item source)
            {
                return value;
            }

            public int Transfer(Item source, Addressable destination, long index)
            {
                for (int i = 0; i < value; i++)
                {
                    destination.Set(index + i, (byte)(source.Value * value));
                }

                return value;
            }

            public int Transfer(Addressable source, long index, Item destination)
            {
                long sum = 0;

                for (int i = 0; i < value; i++)
                {
                    sum = sum + source.Get(index + i);
                }

                destination.Value = (byte)(sum / value / value);
                return value;
            }
        }
    }
}