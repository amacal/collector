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
        public void ShouldDeserializeValueToInstance()
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
        public void ShouldDeserializeValueToSubstitute()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, String> property = new ReflectorProperty<Item, String>(info);

            Member<Item> member = new MemberString<Item>(property);
            Serializer<Item> serializer = new Serializer<Item>(member);

            MemoryMock memory = new MemoryMock(new byte[]
            {
                0x00, 0x00, 0x00, 0x03, 0x61, 0x62, 0x63
            });

            Substitute<Item> item = new Substitute<Item>(serializer, memory);

            Assert.That(member.Transfer(memory, 0, item), Is.EqualTo(7));
            Assert.That(item.AsDynamic().Value.ToString(), Is.EqualTo("abc"));
        }

        [Test]
        public void ShouldDeserializeNullToInstance()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, String> property = new ReflectorProperty<Item, String>(info);

            Member<Item> member = new MemberString<Item>(property);
            MemoryMock memory = new MemoryMock(new byte[] { 0xff, 0xff, 0xff, 0xff });
            Item item = new Item { Value = "abc" };

            Assert.That(member.Transfer(memory, 0, item), Is.EqualTo(4));
            Assert.That(item.Value, Is.Null);
        }

        [Test]
        public void ShouldDeserializeNullToSubstitute()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, String> property = new ReflectorProperty<Item, String>(info);

            Member<Item> member = new MemberString<Item>(property);
            MemoryMock memory = new MemoryMock(new byte[] { 0xff, 0xff, 0xff, 0xff });

            Serializer<Item> serializer = new Serializer<Item>(member);
            Substitute<Item> item = new Substitute<Item>(serializer, memory);

            Assert.That(member.Transfer(memory, 0, item), Is.EqualTo(4));
            Assert.That(item.AsDynamic().Value, Is.Null);
        }

        [Test]
        public void ShouldDeserializeOnlySize()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, String> property = new ReflectorProperty<Item, String>(info);

            Member<Item> member = new MemberString<Item>(property);
            Serializer<Item> serializer = new Serializer<Item>(member);

            MemoryMock memory = new MemoryMock(new byte[]
            {
                0x00, 0x00, 0x00, 0x03, 0x61, 0x62, 0x63
            });

            Substitute<Item> item = new Substitute<Item>(serializer, memory);

            Assert.That(member.Transfer(memory, 0, item), Is.EqualTo(7));
            Assert.That(memory.Accessed, Is.EqualTo(new[] { 0, 1, 2, 3 }));
        }

        [Test]
        public void ShouldDeserializeOnlySizeEvenIfSizeAccessed()
        {
            PropertyInfo info = typeof(Item).GetProperty("Value");
            ReflectorProperty<Item, String> property = new ReflectorProperty<Item, String>(info);

            Member<Item> member = new MemberString<Item>(property);
            Serializer<Item> serializer = new Serializer<Item>(member);

            MemoryMock memory = new MemoryMock(new byte[]
            {
                0x00, 0x00, 0x00, 0x03, 0x61, 0x62, 0x63
            });

            Substitute<Item> item = new Substitute<Item>(serializer, memory);
            Assert.That(member.Transfer(memory, 0, item), Is.EqualTo(7));

            Assert.That(item.AsDynamic().Value.Size, Is.EqualTo(3));
            Assert.That(memory.Accessed, Is.EqualTo(new[] { 0, 1, 2, 3 }));
        }
    }
}