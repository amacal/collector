using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Collector
{
    public static class Sort
    {
        public static Collectible Table(Collectible source, SortBy by)
        {
            const int threshold = 100000;
            ConcurrentQueue<Task> tasks = new ConcurrentQueue<Task>();

            void watch()
            {
                Task task;

                while (tasks.TryDequeue(out task))
                {
                    task.Wait();
                }
            }

            void recurse(long low, long high)
            {
                if (high - low > threshold)
                {
                    tasks.Enqueue(Task.Run(() => sort(low, high)));
                }
                else
                {
                    sort(low, high);
                }
            }

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
                    recurse(low, j);

                if (i < high)
                    recurse(i, high);
            }

            sort(0, source.Count - 1);
            watch();

            source.Flags.IsSorted = true;
            return source;
        }

        public static SortBy By<T>(Serializer<T> serializer, Func<dynamic, dynamic> selector)
        {
            return new SortPredicate<T>(serializer, selector);
        }
    }

    public static class SortExtensions
    {
        public static SortBy Inverse(this SortBy by)
        {
            return new SortByInverse(by);
        }

        private class SortByInverse : SortBy
        {
            private readonly SortBy by;

            public SortByInverse(SortBy by)
            {
                this.by = by;
            }

            public dynamic Extract(Collectible source, long index)
            {
                return by.Extract(source, index);
            }

            public void Swap(Collectible source, long left, long right)
            {
                by.Swap(source, left, right);
            }

            public bool IsLessThan(dynamic left, dynamic right)
            {
                return by.IsLessThan(right, left);
            }
        }
    }
}