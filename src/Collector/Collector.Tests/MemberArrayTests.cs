using System;
using System.Reflection;
using NUnit.Framework;

namespace Collector.Tests
{
    public class MemberArrayTests
    {
        private class Entry
        {
            public String Value { get; set; }
        }

        private class Item
        {
            public Entry[] Entries { get; set; }
        }

        [Test]
        public void ShouldMeasureValue()
        {
            PropertyInfo valueInfo = typeof(Item).GetProperty("Entries");
            ReflectorProperty<Item, Entry[]> valueProperty = new ReflectorProperty<Item, Entry[]>(valueInfo);

            PropertyInfo nestedInfo = typeof(Entry).GetProperty("Value");
            ReflectorProperty<Entry, String> nestedProperty = new ReflectorProperty<Entry, string>(nestedInfo);

            Member<Entry> nestedMember = new MemberString<Entry>(nestedProperty);
            Serializer<Entry> nested = new Serializer<Entry>(nestedMember);

            Member<Item> member = new MemberArray<Item, Entry>(valueProperty, nested);
            Item item = new Item { Entries = new[] { new Entry { Value = "abc" }, new Entry { Value = "cde" } } };

            Assert.That(member.Measure(item), Is.EqualTo(18));
        }

        [Test]
        public void ShouldMeasureNull()
        {
            PropertyInfo valueInfo = typeof(Item).GetProperty("Entries");
            ReflectorProperty<Item, Entry[]> valueProperty = new ReflectorProperty<Item, Entry[]>(valueInfo);

            PropertyInfo nestedInfo = typeof(Entry).GetProperty("Value");
            ReflectorProperty<Entry, String> nestedProperty = new ReflectorProperty<Entry, string>(nestedInfo);

            Member<Entry> nestedMember = new MemberString<Entry>(nestedProperty);
            Serializer<Entry> nested = new Serializer<Entry>(nestedMember);

            Member<Item> member = new MemberArray<Item, Entry>(valueProperty, nested);
            Item item = new Item { Entries = null };

            Assert.That(member.Measure(item), Is.EqualTo(4));
        }

        [Test]
        public void ShouldSerializeValue()
        {
            PropertyInfo valueInfo = typeof(Item).GetProperty("Entries");
            ReflectorProperty<Item, Entry[]> valueProperty = new ReflectorProperty<Item, Entry[]>(valueInfo);

            PropertyInfo nestedInfo = typeof(Entry).GetProperty("Value");
            ReflectorProperty<Entry, String> nestedProperty = new ReflectorProperty<Entry, string>(nestedInfo);

            Member<Entry> nestedMember = new MemberString<Entry>(nestedProperty);
            Serializer<Entry> nested = new Serializer<Entry>(nestedMember);

            Member<Item> member = new MemberArray<Item, Entry>(valueProperty, nested);
            Item item = new Item { Entries = new[] { new Entry { Value = "abc" }, new Entry { Value = "cde" } } };

            MemoryMock memory = new MemoryMock(20);

            Assert.That(member.Transfer(item, memory, 0), Is.EqualTo(18));
            Assert.That(memory.GetData(18), Is.EqualTo(new[]
            {
                0x00, 0x00, 0x00, 0x02,
                0x00, 0x00, 0x00, 0x03, 0x61, 0x62, 0x63,
                0x00, 0x00, 0x00, 0x03, 0x63, 0x64, 0x65
            }));
        }

        [Test]
        public void ShouldSerializeNull()
        {
            PropertyInfo valueInfo = typeof(Item).GetProperty("Entries");
            ReflectorProperty<Item, Entry[]> valueProperty = new ReflectorProperty<Item, Entry[]>(valueInfo);

            PropertyInfo nestedInfo = typeof(Entry).GetProperty("Value");
            ReflectorProperty<Entry, String> nestedProperty = new ReflectorProperty<Entry, string>(nestedInfo);

            Member<Entry> nestedMember = new MemberString<Entry>(nestedProperty);
            Serializer<Entry> nested = new Serializer<Entry>(nestedMember);

            Member<Item> member = new MemberArray<Item, Entry>(valueProperty, nested);
            Item item = new Item { Entries = null };

            MemoryMock memory = new MemoryMock(20);

            Assert.That(member.Transfer(item, memory, 0), Is.EqualTo(4));
            Assert.That(memory.GetData(4), Is.EqualTo(new[] { 0xff, 0xff, 0xff, 0xff }));
        }

        [Test]
        public void ShouldDeserializeValue()
        {
            PropertyInfo valueInfo = typeof(Item).GetProperty("Entries");
            ReflectorProperty<Item, Entry[]> valueProperty = new ReflectorProperty<Item, Entry[]>(valueInfo);

            PropertyInfo nestedInfo = typeof(Entry).GetProperty("Value");
            ReflectorProperty<Entry, String> nestedProperty = new ReflectorProperty<Entry, string>(nestedInfo);

            Member<Entry> nestedMember = new MemberString<Entry>(nestedProperty);
            Serializer<Entry> nested = new Serializer<Entry>(nestedMember);

            Member<Item> member = new MemberArray<Item, Entry>(valueProperty, nested);
            Item item = new Item { Entries = null };

            MemoryMock memory = new MemoryMock(new byte[]
            {
                0x00, 0x00, 0x00, 0x02,
                0x00, 0x00, 0x00, 0x03, 0x61, 0x62, 0x63,
                0x00, 0x00, 0x00, 0x03, 0x63, 0x64, 0x65
            });

            Assert.That(member.Transfer(memory, 0, item), Is.EqualTo(18));
            Assert.That(item.Entries[0].Value, Is.EqualTo("abc"));
            Assert.That(item.Entries[1].Value, Is.EqualTo("cde"));
        }

        [Test]
        public void ShouldDeserializeNull()
        {
            PropertyInfo valueInfo = typeof(Item).GetProperty("Entries");
            ReflectorProperty<Item, Entry[]> valueProperty = new ReflectorProperty<Item, Entry[]>(valueInfo);

            PropertyInfo nestedInfo = typeof(Entry).GetProperty("Value");
            ReflectorProperty<Entry, String> nestedProperty = new ReflectorProperty<Entry, string>(nestedInfo);

            Member<Entry> nestedMember = new MemberString<Entry>(nestedProperty);
            Serializer<Entry> nested = new Serializer<Entry>(nestedMember);

            Member<Item> member = new MemberArray<Item, Entry>(valueProperty, nested);
            Item item = new Item { Entries = new Entry[0] };

            MemoryMock memory = new MemoryMock(new byte[] { 0xff, 0xff, 0xff, 0xff });

            Assert.That(member.Transfer(memory, 0, item), Is.EqualTo(4));
            Assert.That(item.Entries, Is.Null);
        }
    }
}