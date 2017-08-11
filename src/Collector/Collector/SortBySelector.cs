using System;
using System.Collections.Generic;

namespace Collector
{
    public class SortBySelector<T> : SortBy
    {
        private readonly Serializer<T> serializer;
        private readonly Func<dynamic, dynamic> selector;

        public SortBySelector(Serializer<T> serializer, Func<dynamic, dynamic> selector)
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
            return Comparer<dynamic>.Default.Compare(selector(left), selector(right)) < 0;
        }
    }
}