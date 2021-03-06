﻿using NUnit.Framework;

namespace Collector.Tests
{
    public class IndexTests
    {
        [Test]
        public void ShouldRegisterAndAccess()
        {
            Index index = new Index(1024);
            index.Add(1234);

            Assert.That(index.At(0), Is.EqualTo(1234));
        }

        [Test]
        public void ShouldHaveTotalSize()
        {
            Index index = new Index(1024);
            index.Add(1256);

            Assert.That(index.TotalSize, Is.EqualTo(1024));
        }

        [Test]
        public void ShouldHaveUsedSize()
        {
            Index index = new Index(1024);
            index.Add(1256);

            Assert.That(index.UsedSize, Is.EqualTo(8));
        }

        [Test]
        public void ShouldHaveNumberOfItemsWhenAdded()
        {
            Index index = new Index(1024);

            index.Add(1245);
            index.Add(2345);

            Assert.That(index.Count, Is.EqualTo(2));
        }

        [Test]
        public void ShouldHaveNumberOfItemsWhenRemoved()
        {
            Index index = new Index(1024);

            index.Add(1245);
            index.Add(2345);
            index.Remove();

            Assert.That(index.Count, Is.EqualTo(1));
        }

        [Test]
        public void ShouldSwapItems()
        {
            Index index = new Index(1024);

            index.Add(1245);
            index.Add(2345);
            index.Swap(0, 1);

            Assert.That(index.At(0), Is.EqualTo(2345));
            Assert.That(index.At(1), Is.EqualTo(1245));
        }
    }
}