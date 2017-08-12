using NUnit.Framework;
using System;
using System.Reflection;

namespace Collector.Tests
{
    public class MemberInt64Tests
    {
        private class Item
        {
            public Int64 Value { get; set; }
        }

        [Test]
        public void ShouldMeasureValue()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, Int64> property = new ReflectorProperty<Item, Int64>(info);

            Member<Item> member = new MemberInt64<Item>(property);
            Item item = new Item { Value = 13 };

            Assert.That(member.Measure(item), Is.EqualTo(8));
        }

        [Test]
        public void ShouldSerializeValue()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, Int64> property = new ReflectorProperty<Item, long>(info);

            Member<Item> member = new MemberInt64<Item>(property);
            Item item = new Item { Value = 0x0102030405060708 };

            MemoryMock memory = new MemoryMock(20);
            Assert.That(member.Transfer(item, memory, 0), Is.EqualTo(8));

            Assert.That(memory.GetData(8), Is.EqualTo(new[]
            {
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08
            }));
        }

        [Test]
        public void ShouldDeserializeValue()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, Int64> property = new ReflectorProperty<Item, long>(info);

            dynamic item = new Substitute();
            Member<Item> member = new MemberInt64<Item>(property);

            MemoryMock memory = new MemoryMock(new byte[]
            {
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08
            });

            Assert.That(member.Transfer(memory, 0, item), Is.EqualTo(8));
            Assert.That(item.Value, Is.EqualTo(0x0102030405060708));
        }

        [Test]
        public void ShouldNotDeserializeData()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, Int64> property = new ReflectorProperty<Item, long>(info);

            dynamic item = new Substitute();
            Member<Item> member = new MemberInt64<Item>(property);

            MemoryMock memory = new MemoryMock(new byte[]
            {
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08
            });

            Assert.That(member.Transfer(memory, 0, item), Is.EqualTo(8));
            Assert.That(memory.Accessed, Is.Empty);
        }
    }
}