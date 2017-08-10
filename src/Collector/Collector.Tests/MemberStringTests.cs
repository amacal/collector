using System;
using System.Reflection;
using NUnit.Framework;

namespace Collector.Tests
{
    public class MemberStringTests
    {
        private class Item
        {
            public String Value { get; set; }
        }

        [Test]
        public void ShouldMeasureValue()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, String> property = new ReflectorProperty<Item, String>(info);

            Member<Item> member = new MemberString<Item>(property);
            Item item = new Item { Value = "abc" };

            Assert.That(member.Measure(item), Is.EqualTo(7));
        }

        [Test]
        public void ShouldMeasureNull()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, String> property = new ReflectorProperty<Item, String>(info);

            Member<Item> member = new MemberString<Item>(property);
            Item item = new Item { Value = null };

            Assert.That(member.Measure(item), Is.EqualTo(4));
        }

        [Test]
        public void ShouldSerializeValue()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, String> property = new ReflectorProperty<Item, String>(info);

            Member<Item> member = new MemberString<Item>(property);
            Item item = new Item { Value = "abc" };
            MemoryMock memory = new MemoryMock(20);

            Assert.That(member.Transfer(item, memory, 0), Is.EqualTo(7));
            Assert.That(memory.GetData(7), Is.EqualTo(new[]
            {
                0x00, 0x00, 0x00, 0x03, 0x61, 0x62, 0x63
            }));
        }

        [Test]
        public void ShouldSerializeNull()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, String> property = new ReflectorProperty<Item, String>(info);

            Member<Item> member = new MemberString<Item>(property);
            Item item = new Item { Value = null };
            MemoryMock memory = new MemoryMock(20);

            Assert.That(member.Transfer(item, memory, 0), Is.EqualTo(4));
            Assert.That(memory.GetData(4), Is.EqualTo(new[] { 0xff, 0xff, 0xff, 0xff }));
        }

        [Test]
        public void ShouldDeserializeValue()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, String> property = new ReflectorProperty<Item, String>(info);

            Item item = new Item();
            Member<Item> member = new MemberString<Item>(property);

            MemoryMock memory = new MemoryMock(new byte[]
            {
                0x00, 0x00, 0x00, 0x03, 0x61, 0x62, 0x63
            });

            Assert.That(member.Transfer(memory, 0, item), Is.EqualTo(7));
            Assert.That(item.Value, Is.EqualTo("abc"));
        }

        [Test]
        public void ShouldDeserializeNull()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, String> property = new ReflectorProperty<Item, String>(info);

            Item item = new Item { Value = "x" };
            Member<Item> member = new MemberString<Item>(property);
            MemoryMock memory = new MemoryMock(new byte[] { 0xff, 0xff, 0xff, 0xff });

            Assert.That(member.Transfer(memory, 0, item), Is.EqualTo(4));
            Assert.That(item.Value, Is.Null);
        }
    }
}