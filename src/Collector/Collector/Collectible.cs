namespace Collector
{
    public class Collectible
    {
        private readonly Index index;
        private readonly Storage storage;
        private readonly CollectibleFlags flags;

        public Collectible(int blockSize)
        {
            this.index = new Index(blockSize);
            this.storage = new Storage(blockSize);

            this.flags = new CollectibleFlags
            {
                IsIndexed = true,
                IsAligned = true,
                IsSorted = true
            };
        }

        public long Count
        {
            get { return index.Count; }
        }

        public long UsedSize
        {
            get { return index.UsedSize + storage.UsedSize; }
        }

        public long TotalSize
        {
            get { return index.TotalSize + storage.TotalSize; }
        }

        public CollectibleFlags Flags
        {
            get { return flags; }
        }

        public void Enqueue<T>(Serializer<T> serializer, T data)
        {
            StorageAllocation allocation = storage.Allocate();
            index.Add(allocation.Position);

            int size = serializer.Transfer(data, allocation);
            storage.Commit(size);

            flags.IsSorted = false;
        }

        public Substitute<T> Dequeue<T>(Serializer<T> serializer)
        {
            StorageAllocation allocation = storage.At(index.At(index.LowerBound));
            Substitute<T> substitute = new Substitute<T>(serializer, allocation);

            int size = serializer.Transfer(allocation, substitute);

            storage.Release(size);
            index.Remove();

            flags.IsIndexed = false;
            return substitute;
        }

        public void Release()
        {
            storage.Release();
        }

        public Substitute<T> At<T>(Serializer<T> serializer, long position)
        {
            StorageAllocation source = storage.At(index.At(position));
            Substitute<T> destination = new Substitute<T>(serializer, source);

            serializer.Transfer(source, destination);
            return destination;
        }

        public void Swap(long left, long right)
        {
            index.Swap(left, right);
            flags.IsAligned = false;
        }
    }
}