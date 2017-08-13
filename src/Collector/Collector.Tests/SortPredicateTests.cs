using NUnit.Framework;

namespace Collector.Tests
{
    public class SortPredicateTests
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
            SortPredicate<Item> predicate = new SortPredicate<Item>(serializer, x => x.Value);

            collectible.Enqueue(serializer, new Item { Value = 1 });
            Assert.That(predicate.Extract(collectible, 0).Value, Is.EqualTo(1));
        }

        [Test]
        public void ShouldSwapItems()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;

            Collectible collectible = new Collectible(1024);
            SortPredicate<Item> predicate = new SortPredicate<Item>(serializer, x => x.Value);

            collectible.Enqueue(serializer, new Item { Value = 1 });
            collectible.Enqueue(serializer, new Item { Value = 2 });
            predicate.Swap(collectible, 0, 1);

            Assert.That(predicate.Extract(collectible, 0).Value, Is.EqualTo(2));
            Assert.That(predicate.Extract(collectible, 1).Value, Is.EqualTo(1));
        }

        [Test]
        public void ShouldCompareItemsPositive()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;

            Collectible collectible = new Collectible(1024);
            SortPredicate<Item> predicate = new SortPredicate<Item>(serializer, x => x.Value);

            collectible.Enqueue(serializer, new Item { Value = 1 });
            collectible.Enqueue(serializer, new Item { Value = 2 });

            Assert.That(predicate.IsLessThan(predicate.Extract(collectible, 0), predicate.Extract(collectible, 1)), Is.True);
        }

        [Test]
        public void ShouldCompareItemsNegative()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;

            Collectible collectible = new Collectible(1024);
            SortPredicate<Item> predicate = new SortPredicate<Item>(serializer, x => x.Value);

            collectible.Enqueue(serializer, new Item { Value = 1 });
            collectible.Enqueue(serializer, new Item { Value = 2 });

            Assert.That(predicate.IsLessThan(predicate.Extract(collectible, 1), predicate.Extract(collectible, 0)), Is.False);
        }
    }
}