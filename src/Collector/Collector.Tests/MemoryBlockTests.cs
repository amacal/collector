﻿using NUnit.Framework;

namespace Collector.Tests
{
    public class MemoryBlockTests
    {
        [Test]
        public void ShouldHaveRequestedSize()
        {
            MemoryBlock block = new MemoryBlock(1024);
            Assert.That(block.Size, Is.EqualTo(1024));
        }

        [Test]
        public void ShouldModifyRequestedByte()
        {
            MemoryBlock block = new MemoryBlock(1024);
            block.Set(10, 15);

            Assert.That(block.Get(10), Is.EqualTo(15));
        }

        [Test]
        public void ShouldReadRequestedRange()
        {
            byte[] store = new byte[2];
            MemoryBlock block = new MemoryBlock(1024);

            block.Set(10, 15);
            block.Set(11, 16);

            block.GetBytes(10, store, 0, 2);
            Assert.That(store, Is.EqualTo(new byte[] { 15, 16 }));
        }

        [Test]
        public void ShouldModifyRequestedRange()
        {
            MemoryBlock block = new MemoryBlock(1024);
            block.SetBytes(10, new byte[] { 15, 16 }, 0, 2);

            Assert.That(block.Get(10), Is.EqualTo(15));
            Assert.That(block.Get(11), Is.EqualTo(16));
        }
    }
}