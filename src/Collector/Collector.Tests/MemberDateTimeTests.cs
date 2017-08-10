using System;
using System.Reflection;
using NUnit.Framework;

namespace Collector.Tests
{
    public class MemberDateTimeTests
    {
        private class Item
        {
            public DateTime Value { get; set; }
        }

        [Test]
        public void ShouldMeasureValue()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, DateTime> property = new ReflectorProperty<Item, DateTime>(info);

            Member<Item> member = new MemberDateTime<Item>(property);
            Item item = new Item { Value = DateTime.Today };

            Assert.That(member.Measure(item), Is.EqualTo(8));
        }

        [Test]
        public void ShouldSerializeValue()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, DateTime> property = new ReflectorProperty<Item, DateTime>(info);

            Member<Item> member = new MemberDateTime<Item>(property);
            Item item = new Item { Value = DateTime.Parse("2013-07-04 12:31:10") };

            MemoryMock memory = new MemoryMock(20);
            member.Transfer(item, memory, 0);

            Assert.That(memory.GetData(8), Is.EqualTo(new[]
            {
                0x08, 0xd0, 0x46, 0xc9, 0x7f, 0x75, 0x3b, 0x00
            }));
        }

        [Test]
        public void ShouldDeserializeValue()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, DateTime> property = new ReflectorProperty<Item, DateTime>(info);

            Item item = new Item();
            Member<Item> member = new MemberDateTime<Item>(property);

            MemoryMock memory = new MemoryMock(new byte[]
            {
                0x08, 0xd0, 0x46, 0xc9, 0x7f, 0x75, 0x3b, 0x00
            });

            member.Transfer(memory, 0, item);
            Assert.That(item.Value, Is.EqualTo(DateTime.Parse("2013-07-04 12:31:10")));
        }
    }
}