using System;
using NUnit.Framework;

namespace Collector.Tests
{
    public class SortTests
    {
        private class Item
        {
            public long Value { get; set; }
        }

        [Test]
        public void ShouldSortEmpty()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;

            Collectible collectible = new Collectible(1024);
            SortPredicate<Item> predicate = new SortPredicate<Item>(serializer, x => x.Value);

            Sort.Table(collectible, predicate);
            Assert.That(collectible.Count, Is.Zero);
        }

        [Test]
        public void ShouldSortOneItem()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;

            Collectible collectible = new Collectible(1024);
            SortPredicate<Item> predicate = new SortPredicate<Item>(serializer, x => x.Value);

            collectible.Enqueue(serializer, new Item { Value = 1 });

            Sort.Table(collectible, predicate);
            Assert.That(collectible.At(serializer, 0).AsDynamic().Value, Is.EqualTo(1));
        }

        [Test]
        public void ShouldSortTwoItems()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;

            Collectible collectible = new Collectible(1024);
            SortPredicate<Item> predicate = new SortPredicate<Item>(serializer, x => x.Value);

            collectible.Enqueue(serializer, new Item { Value = 1 });
            collectible.Enqueue(serializer, new Item { Value = 1 });
            collectible.Enqueue(serializer, new Item { Value = 6 });

            Sort.Table(collectible, predicate.Inverse());
            Assert.That(collectible.At(serializer, 0).AsDynamic().Value, Is.EqualTo(6));
            Assert.That(collectible.At(serializer, 1).AsDynamic().Value, Is.EqualTo(1));
            Assert.That(collectible.At(serializer, 2).AsDynamic().Value, Is.EqualTo(1));
        }

        [Test]
        public void ShouldSortManyItems()
        {
            const int size = 100;
            Random random = new Random();

            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;

            Collectible collectible = new Collectible(1024);
            SortPredicate<Item> predicate = new SortPredicate<Item>(serializer, x => x.Value);

            for (int i = 0; i < size; i++)
            {
                collectible.Enqueue(serializer, new Item { Value = random.Next(5000) });
            }

            Sort.Table(collectible, predicate);
            Assert.That(collectible.Count, Is.EqualTo(size));

            for (int i = 0; i < size - 1; i++)
            {
                Assert.That(collectible.At(serializer, i).AsDynamic().Value,
                    Is.LessThanOrEqualTo(collectible.At(serializer, i + 1).AsDynamic().Value));
            }
        }
    }
}