using System;
using System.Collections.Generic;

namespace Collector
{
    public static class Select
    {
        public static Collectible Table(Collectible source, SelectBy by)
        {
            Collectible destination = new Collectible(1024 * 1024);

            foreach (dynamic extracted in by.Extract(source))
            {
                foreach (dynamic converted in by.Convert(extracted))
                {
                    by.Enqueue(destination, converted);
                }

                source.Release();
            }

            return destination;
        }

        public static SelectBy One<T, U>(Serializer<T> input, Serializer<U> output, Func<dynamic, U> selector)
        {
            return new SelectOne<T, U>(input, output, selector);
        }

        public static SelectBy Many<T, U>(Serializer<T> input, Serializer<U> output, Func<dynamic, IEnumerable<U>> selector)
        {
            return new SelectMany<T, U>(input, output, selector);
        }
    }
}