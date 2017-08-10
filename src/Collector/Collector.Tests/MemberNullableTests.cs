using System;
using System.Reflection;
using NUnit.Framework;

namespace Collector.Tests
{
    public class MemberNullableTests
    {
        private class Item
        {
            public Int64? Value { get; set; }
        }

        [Test]
        public void ShouldMeasureValue()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, Int64?> property = new ReflectorProperty<Item, Int64?>(info);

            Member<Item> member = new MemberInt64<Item>(property);
            Member<Item> nullable = new MemberNullable<Item, Int64?>(member, property);

            Item item = new Item { Value = 13 };
            Assert.That(nullable.Measure(item), Is.EqualTo(9));
        }

        [Test]
        public void ShouldMeasureNull()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, Int64?> property = new ReflectorProperty<Item, Int64?>(info);

            Member<Item> member = new MemberInt64<Item>(property);
            Member<Item> nullable = new MemberNullable<Item, Int64?>(member, property);

            Item item = new Item { Value = null };
            Assert.That(nullable.Measure(item), Is.EqualTo(1));
        }

        [Test]
        public void ShouldSerializeValue()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, Int64?> property = new ReflectorProperty<Item, Int64?>(info);

            Member<Item> member = new MemberInt64<Item>(property);
            Member<Item> nullable = new MemberNullable<Item, Int64?>(member, property);

            Item item = new Item { Value = 0x0102030405060708 };
            MemoryMock memory = new MemoryMock(20);

            nullable.Transfer(item, memory, 0);
            Assert.That(memory.GetData(9), Is.EqualTo(new[]
            {
                0x01, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08
            }));
        }

        [Test]
        public void ShouldSerializeNull()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, Int64?> property = new ReflectorProperty<Item, Int64?>(info);

            Member<Item> member = new MemberInt64<Item>(property);
            Member<Item> nullable = new MemberNullable<Item, Int64?>(member, property);

            Item item = new Item { Value = null };
            MemoryMock memory = new MemoryMock(20);

            nullable.Transfer(item, memory, 0);
            Assert.That(memory.GetData(1), Is.EqualTo(new[] { 0x00 }));
        }

        [Test]
        public void ShouldDeserializeValue()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, Int64?> property = new ReflectorProperty<Item, Int64?>(info);

            Member<Item> member = new MemberInt64<Item>(property);
            Member<Item> nullable = new MemberNullable<Item, Int64?>(member, property);

            Item item = new Item { Value = null };
            MemoryMock memory = new MemoryMock(new byte[]
            {
                0x01, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08
            });

            nullable.Transfer(memory, 0, item);
            Assert.That(item.Value, Is.EqualTo(0x0102030405060708));
        }

        [Test]
        public void ShouldDeserializeNull()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, Int64?> property = new ReflectorProperty<Item, Int64?>(info);

            Member<Item> member = new MemberInt64<Item>(property);
            Member<Item> nullable = new MemberNullable<Item, Int64?>(member, property);

            Item item = new Item { Value = 12 };
            MemoryMock memory = new MemoryMock(new byte[] { 0x00 });

            nullable.Transfer(memory, 0, item);
            Assert.That(item.Value, Is.Null);
        }
    }
}