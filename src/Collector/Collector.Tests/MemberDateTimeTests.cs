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
        public void ShouldSerializeValue()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, DateTime> property = new ReflectorProperty<Item, DateTime>(info);

            Member<Item> member = new MemberDateTime<Item>(property);
            Item item = new Item { Value = DateTime.Parse("2013-07-04 12:31:10") };

            MemoryMock memory = new MemoryMock(20);
            Assert.That(member.Transfer(item, memory, 0), Is.EqualTo(8));

            Assert.That(memory.GetData(8), Is.EqualTo(new[]
            {
                0x08, 0xd0, 0x46, 0xc9, 0x7f, 0x75, 0x3b, 0x00
            }));
        }

        [Test]
        public void ShouldDeserializeValueToInstance()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, DateTime> property = new ReflectorProperty<Item, DateTime>(info);

            Item item = new Item();
            Member<Item> member = new MemberDateTime<Item>(property);

            MemoryMock memory = new MemoryMock(new byte[]
            {
                0x08, 0xd0, 0x46, 0xc9, 0x7f, 0x75, 0x3b, 0x00
            });

            Assert.That(member.Transfer(memory, 0, item), Is.EqualTo(8));
            Assert.That(item.Value, Is.EqualTo(DateTime.Parse("2013-07-04 12:31:10")));
        }

        [Test]
        public void ShouldDeserializeValueToSubstitute()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, DateTime> property = new ReflectorProperty<Item, DateTime>(info);

            Member<Item> member = new MemberDateTime<Item>(property);
            Serializer<Item> serializer = new Serializer<Item>(member);

            Addressable source = new MemoryMock();
            Substitute<Item> item = new Substitute<Item>(serializer, source);

            MemoryMock memory = new MemoryMock(new byte[]
            {
                0x08, 0xd0, 0x46, 0xc9, 0x7f, 0x75, 0x3b, 0x00
            });

            Assert.That(member.Transfer(memory, 0, item), Is.EqualTo(8));
            Assert.That(item.AsDynamic().Value, Is.EqualTo(DateTime.Parse("2013-07-04 12:31:10")));
        }

        [Test]
        public void ShouldNotDeserializeData()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, DateTime> property = new ReflectorProperty<Item, DateTime>(info);

            Member<Item> member = new MemberDateTime<Item>(property);
            Serializer<Item> serializer = new Serializer<Item>(member);

            Addressable source = new MemoryMock();
            Substitute<Item> item = new Substitute<Item>(serializer, source);

            MemoryMock memory = new MemoryMock(new byte[]
            {
                0x08, 0xd0, 0x46, 0xc9, 0x7f, 0x75, 0x3b, 0x00
            });

            Assert.That(member.Transfer(memory, 0, item), Is.EqualTo(8));
            Assert.That(memory.Accessed, Is.Empty);
        }
    }
}