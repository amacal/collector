namespace Collector
{
    public class Collectible
    {
        private readonly Index index;
        private readonly Storage storage;

        public Collectible(int blockSize)
        {
            this.index = new Index(blockSize);
            this.storage = new Storage(blockSize);
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

        public void Add<T>(Serializer<T> serializer, T data)
        {
            int size = serializer.Measure(data);
            StorageAllocation allocation = storage.Allocate(size);

            index.Add(allocation.Position);
            serializer.Transfer(data, allocation);
        }

        public T At<T>(Serializer<T> serializer, long position)
            where T : new()
        {
            T destination = new T();
            StorageAllocation source = storage.At(index.At(position));

            serializer.Transfer(source, destination);
            return destination;
        }
    }
}