namespace Collector
{
    public class StorageAllocation : Addressable
    {
        private readonly Memory memory;
        private readonly StoragePosition position;

        public StorageAllocation(Memory memory, StoragePosition position)
        {
            this.memory = memory;
            this.position = position;
        }

        public long Position
        {
            get { return position.Value; }
        }

        public byte Get(long index)
        {
            return memory.Get(position.Value + index);
        }

        public void GetBytes(long index, byte[] data)
        {
            memory.GetBytes(position.Value + index, data);
        }

        public void Set(long index, byte value)
        {
            memory.Set(position.Value + index, value);
        }

        public void SetBytes(long index, byte[] data)
        {
            memory.SetBytes(position.Value + index, data);
        }
    }
}