namespace Collector
{
    public class Storage
    {
        private readonly Memory memory;
        private readonly StoragePosition position;

        public Storage(int blockSize)
        {
            this.memory = new Memory(blockSize);
            this.position = new StoragePosition();
        }

        public long UsedSize
        {
            get { return position.Value; }
        }

        public long TotalSize
        {
            get { return memory.Size; }
        }

        public StorageAllocation Allocate(int size)
        {
            StoragePosition allocated = position.Clone();
            StorageAllocation allocation = new StorageAllocation(memory, allocated);

            position.Increase(size);
            allocation.Set(size - 1, 0);

            return allocation;
        }

        public StorageAllocation At(long index)
        {
            return new StorageAllocation(memory, new StoragePosition(index));
        }
    }
}