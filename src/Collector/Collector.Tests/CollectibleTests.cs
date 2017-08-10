using NUnit.Framework;

namespace Collector.Tests
{
    public class CollectibleTests
    {
        private class Item
        {
            public long Value { get; set; }
            public string Text { get; set; }
        }

        [Test]
        public void ShouldCountItems()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;

            Collectible collectible = new Collectible(1024);
            Item item = new Item { Value = 0x1234 };

            collectible.Add(serializer, item);
            Assert.That(collectible.Count, Is.EqualTo(1));
        }

        [Test]
        public void ShouldAccessItem()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;

            Collectible collectible = new Collectible(1024);
            collectible.Add(serializer, new Item { Value = 0x1234, Text = "abc" });

            Item found = collectible.At(serializer, 0);
            Assert.That(found.Value, Is.EqualTo(0x1234));
            Assert.That(found.Text, Is.EqualTo("abc"));
        }

        [Test]
        public void ShouldHaveInitialSize()
        {
            Collectible collectible = new Collectible(1024);

            Assert.That(collectible.TotalSize, Is.EqualTo(0));
            Assert.That(collectible.UsedSize, Is.EqualTo(0));
        }

        [Test]
        public void ShouldHaveIncreasedSize()
        {
            Reflector reflector = new Reflector();
            Collectible collectible = new Collectible(1024);

            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;
            collectible.Add(serializer, new Item { Value = 0x1234, Text = "abc" });

            Assert.That(collectible.TotalSize, Is.EqualTo(2048));
            Assert.That(collectible.UsedSize, Is.EqualTo(23));
        }
    }
}