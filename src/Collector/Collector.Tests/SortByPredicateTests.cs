using NUnit.Framework;

namespace Collector.Tests
{
    public class SortByPredicateTests
    {
        private class Item
        {
            public long Value { get; set; }
        }

        [Test]
        public void ShouldExtractItem()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;

            Collectible collectible = new Collectible(1024);
            SortBySelector<Item> selector = new SortBySelector<Item>(serializer, x => x.Value);

            collectible.Add(serializer, new Item { Value = 1 });
            Assert.That(selector.Extract(collectible, 0).Value, Is.EqualTo(1));
        }

        [Test]
        public void ShouldSwapItems()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;

            Collectible collectible = new Collectible(1024);
            SortBySelector<Item> selector = new SortBySelector<Item>(serializer, x => x.Value);

            collectible.Add(serializer, new Item { Value = 1 });
            collectible.Add(serializer, new Item { Value = 2 });
            selector.Swap(collectible, 0, 1);

            Assert.That(selector.Extract(collectible, 0).Value, Is.EqualTo(2));
            Assert.That(selector.Extract(collectible, 1).Value, Is.EqualTo(1));
        }

        [Test]
        public void ShouldCompareItemsPositive()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;

            Collectible collectible = new Collectible(1024);
            SortBySelector<Item> selector = new SortBySelector<Item>(serializer, x => x.Value);

            collectible.Add(serializer, new Item { Value = 1 });
            collectible.Add(serializer, new Item { Value = 2 });

            Assert.That(selector.IsLessThan(selector.Extract(collectible, 0), selector.Extract(collectible, 1)), Is.True);
        }

        [Test]
        public void ShouldCompareItemsNegative()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;

            Collectible collectible = new Collectible(1024);
            SortBySelector<Item> selector = new SortBySelector<Item>(serializer, x => x.Value);

            collectible.Add(serializer, new Item { Value = 1 });
            collectible.Add(serializer, new Item { Value = 2 });

            Assert.That(selector.IsLessThan(selector.Extract(collectible, 1), selector.Extract(collectible, 0)), Is.False);
        }
    }
}