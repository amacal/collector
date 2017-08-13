using System;
using System.Collections.Generic;

namespace Collector
{
    public class SortPredicate<T> : SortBy
    {
        private readonly Serializer<T> serializer;
        private readonly Func<dynamic, dynamic> selector;

        public SortPredicate(Serializer<T> serializer, Func<dynamic, dynamic> selector)
        {
            this.serializer = serializer;
            this.selector = selector;
        }

        public dynamic Extract(Collectible source, long index)
        {
            return source.At(serializer, index);
        }

        public void Swap(Collectible source, long left, long right)
        {
            source.Swap(left, right);
        }

        public bool IsLessThan(dynamic left, dynamic right)
        {
            IComparer<IComparable> comparer = Comparer<IComparable>.Default;

            IComparable cLeft = selector(left);
            IComparable cRight = selector(right);

            return comparer.Compare(cLeft, cRight) < 0;
        }
    }
}