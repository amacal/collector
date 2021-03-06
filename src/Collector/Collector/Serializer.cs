﻿namespace Collector
{
    public class Serializer<T>
    {
        private readonly Member<T>[] items;

        public Serializer(params Member<T>[] items)
        {
            this.items = items;
        }

        public int Count
        {
            get { return items.Length; }
        }

        public int Transfer(T source, Addressable destination)
        {
            int size = 0;

            for (int i = 0; i < items.Length; i++)
            {
                size = size + items[i].Transfer(source, destination, size);
            }

            return size;
        }

        public int Transfer(Addressable source, Substitute<T> destination)
        {
            int size = 0;

            for (int i = 0; i < items.Length; i++)
            {
                size = size + items[i].Transfer(source, size, destination);
            }

            return size;
        }

        public int Transfer(Addressable source, T destination)
        {
            int size = 0;

            for (int i = 0; i < items.Length; i++)
            {
                size = size + items[i].Transfer(source, size, destination);
            }

            return size;
        }
    }
}