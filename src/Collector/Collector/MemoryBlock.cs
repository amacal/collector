using System;

namespace Collector
{
    public class MemoryBlock
    {
        private readonly byte[] data;

        public MemoryBlock(int size)
        {
            this.data = new byte[size];
        }

        public int Size
        {
            get { return data.Length; }
        }

        public byte Get(int index)
        {
            return data[index];
        }

        public void Set(int index, byte value)
        {
            data[index] = value;
        }

        public void Set(long index, byte[] source, int offset, int length)
        {
            Array.Copy(source, offset, data, index, length);
        }
    }
}