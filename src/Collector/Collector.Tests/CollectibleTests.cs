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

            dynamic found = collectible.At(serializer, 0);
            Assert.That(found.Value, Is.EqualTo(0x1234));
            Assert.That(found.Text.ToString(), Is.EqualTo("abc"));
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

        [Test]
        public void ShouldSwapItems()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;
            Collectible collectible = new Collectible(1024);

            Item first = new Item { Value = 1 };
            Item second = new Item { Value = 2 };

            collectible.Add(serializer, first);
            collectible.Add(serializer, second);
            collectible.Swap(0, 1);

            Assert.That(collectible.At(serializer, 0).Value, Is.EqualTo(2));
            Assert.That(collectible.At(serializer, 1).Value, Is.EqualTo(1));
        }
    }
}