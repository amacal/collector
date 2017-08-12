using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Collector.Tests
{
    public class SubstituteTextTests
    {
        [Test]
        public void ShouldBeComparable()
        {
            IComparable first = new SubstituteText(3, () => "abc");
            IComparable second = new SubstituteText(3, () => "cde");

            IComparer<IComparable> comparer = Comparer<IComparable>.Default;
            int result = comparer.Compare(first, second);

            Assert.That(result, Is.Negative);
        }
    }
}