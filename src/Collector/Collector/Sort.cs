using System;

namespace Collector
{
    public static class Sort
    {
        public static Collectible Table(Collectible source, SortBy by)
        {
            void sort(long low, long high)
            {
                long i = low, j = high;
                dynamic pivot = by.Extract(source, (low + high) / 2);

                while (i < j)
                {
                    while (by.IsLessThan(by.Extract(source, i), pivot))
                        i++;

                    while (by.IsLessThan(pivot, by.Extract(source, j)))
                        j--;

                    if (i <= j)
                        by.Swap(source, i++, j--);
                }

                if (low < j)
                    sort(low, j);

                if (i < high)
                    sort(i, high);
            }

            sort(0, source.Count - 1);
            return source;
        }

        public static SortBy By<T>(Serializer<T> serializer, Func<dynamic, dynamic> selector)
        {
            return new SortBySelector<T>(serializer, selector);
        }
    }
}