using System;
using System.Collections.Generic;

namespace Collector
{
    public class SortBySelector<T, K> : SortBy<T>
        where T : new()
    {
        private readonly Serializer<T> serializer;
        private readonly Func<T, K> selector;

        public SortBySelector(Serializer<T> serializer, Func<T, K> selector)
        {
            this.serializer = serializer;
            this.selector = selector;
        }

        public T Extract(Collectible source, long index)
        {
            return source.At(serializer, index);
        }

        public void Swap(Collectible source, long left, long right)
        {
            source.Swap(left, right);
        }

        public bool IsLessThan(T left, T right)
        {
            return Comparer<K>.Default.Compare(selector(left), selector(right)) < 0;
        }
    }
}