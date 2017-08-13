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
            get { return position.High; }
        }

        public byte Get(long index)
        {
            return memory.Get(position.High + index);
        }

        public void GetBytes(long index, byte[] data)
        {
            memory.GetBytes(position.High + index, data);
        }

        public void Set(long index, byte value)
        {
            memory.Set(position.High + index, value);
        }

        public void SetBytes(long index, byte[] data)
        {
            memory.SetBytes(position.High + index, data);
        }
    }
}