namespace Collector
{
    public class Index
    {
        private readonly Memory memory;
        private readonly IndexPosition position;

        public Index(int blockSize)
        {
            this.memory = new Memory(blockSize);
            this.position = new IndexPosition();
        }

        public long Count
        {
            get { return position.Count; }
        }

        public long UsedSize
        {
            get { return position.Value; }
        }

        public long TotalSize
        {
            get { return memory.Size; }
        }

        public void Add(long index)
        {
            memory.WriteInt64(position.Value, index);
            position.Increase();
        }

        public long At(long index)
        {
            return memory.ReadInt64(position.At(index));
        }

        public void Swap(long left, long right)
        {
            left = position.At(left);
            right = position.At(right);

            long iLeft = memory.ReadInt64(left);
            long iRight = memory.ReadInt64(right);

            memory.WriteInt64(left, iRight);
            memory.WriteInt64(right, iLeft);
        }
    }
}