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
            get { return position.High - position.Low; }
        }

        public long TotalSize
        {
            get { return memory.Size; }
        }

        public StorageAllocation Allocate()
        {
            StoragePosition allocated = position.Clone();
            StorageAllocation allocation = new StorageAllocation(memory, allocated);

            return allocation;
        }

        public void Commit(int size)
        {
            position.Increase(size);
        }

        public void Release(int size)
        {
            position.Decrease(size);
        }

        public void Release()
        {
            memory.Release(position.Low);
        }

        public StorageAllocation At(long index)
        {
            return new StorageAllocation(memory, new StoragePosition(index, index));
        }
    }
}