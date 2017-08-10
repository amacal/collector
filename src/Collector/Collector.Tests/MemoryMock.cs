using System;
using System.Linq;

namespace Collector.Tests
{
    public class MemoryMock : Addressable
    {
        private readonly byte[] data;

        public MemoryMock(int size)
        {
            this.data = new byte[size];
        }

        public MemoryMock(byte[] data)
        {
            this.data = data;
        }

        public byte[] GetData(int size)
        {
            return data.Take(size).ToArray();
        }

        public byte Get(long index)
        {
            return data[index];
        }

        public void Set(long index, byte value)
        {
            data[index] = value;
        }

        public void Set(long index, byte[] value)
        {
            Array.Copy(value, 0, data, index, value.Length);
        }
    }
}