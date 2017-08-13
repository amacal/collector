using System;
using System.Collections.Generic;

namespace Collector
{
    public class SelectOne<T, U> : SelectBy
    {
        private readonly Serializer<T> input;
        private readonly Serializer<U> output;
        private readonly Func<dynamic, U> selector;

        public SelectOne(Serializer<T> input, Serializer<U> output, Func<dynamic, U> selector)
        {
            this.input = input;
            this.output = output;
            this.selector = selector;
        }

        public IEnumerable<dynamic> Extract(Collectible source)
        {
            while (source.Count > 0)
            {
                yield return source.Dequeue(input).AsDynamic();
            }
        }

        public IEnumerable<dynamic> Convert(dynamic item)
        {
            yield return selector((object)item);
        }

        public void Enqueue(Collectible destination, dynamic value)
        {
            destination.Enqueue(output, (U)value);
        }
    }
}