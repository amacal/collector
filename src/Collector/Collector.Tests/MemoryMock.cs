using System;
using System.Collections.Generic;
using System.Linq;

namespace Collector.Tests
{
    public class MemoryMock : Addressable
    {
        private readonly byte[] data;
        private readonly HashSet<long> accessed;

        public MemoryMock()
        {
            this.data = new byte[0];
            this.accessed = new HashSet<long>();
        }

        public MemoryMock(int size)
        {
            this.data = new byte[size];
            this.accessed = new HashSet<long>();
        }

        public MemoryMock(byte[] data)
        {
            this.data = data;
            this.accessed = new HashSet<long>();
        }

        public long[] Accessed
        {
            get { return accessed.OrderBy(x => x).ToArray(); }
        }

        public byte[] GetData(int size)
        {
            return data.Take(size).ToArray();
        }

        public byte Get(long index)
        {
            accessed.Add(index);
            return data[index];
        }

        public void GetBytes(long index, byte[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                accessed.Add(index + i);
            }

            Array.Copy(data, index, value, 0, value.Length);
        }

        public void Set(long index, byte value)
        {
            data[index] = value;
        }

        public void SetBytes(long index, byte[] value)
        {
            Array.Copy(value, 0, data, index, value.Length);
        }
    }
}