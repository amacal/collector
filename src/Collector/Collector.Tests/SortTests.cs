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
            SortByPredicate<Item> predicate = new SortByPredicate<Item>(serializer, x => x.Value);

            Sort.Table(collectible, predicate);
            Assert.That(collectible.Count, Is.Zero);
        }

        [Test]
        public void ShouldSortOneItem()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;

            Collectible collectible = new Collectible(1024);
            SortByPredicate<Item> predicate = new SortByPredicate<Item>(serializer, x => x.Value);

            collectible.Add(serializer, new Item { Value = 1 });

            Sort.Table(collectible, predicate);
            Assert.That(collectible.At(serializer, 0).Value, Is.EqualTo(1));
        }

        [Test]
        public void ShouldSortTwoItems()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;

            Collectible collectible = new Collectible(1024);
            SortByPredicate<Item> predicate = new SortByPredicate<Item>(serializer, x => x.Value);

            collectible.Add(serializer, new Item { Value = 2 });
            collectible.Add(serializer, new Item { Value = 1 });

            Sort.Table(collectible, predicate);
            Assert.That(collectible.At(serializer, 0).Value, Is.EqualTo(1));
            Assert.That(collectible.At(serializer, 1).Value, Is.EqualTo(2));
        }

        [Test]
        public void ShouldSortManyItems()
        {
            const int size = 2000;
            Random random = new Random();

            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>(); ;

            Collectible collectible = new Collectible(1024);
            SortByPredicate<Item> predicate = new SortByPredicate<Item>(serializer, x => x.Value);

            for (int i = 0; i < size; i++)
            {
                collectible.Add(serializer, new Item { Value = random.Next(5000) });
            }

            Sort.Table(collectible, predicate);
            Assert.That(collectible.Count, Is.EqualTo(size));

            for (int i = 0; i < size - 1; i++)
            {
                Assert.That(collectible.At(serializer, i).Value,
                    Is.LessThanOrEqualTo(collectible.At(serializer, i + 1).Value));
            }
        }
    }
}