using System;

namespace Collector
{
    public class Memory : Addressable
    {
        private readonly int blockSize;
        private MemoryBlock[] items;

        public Memory(int blockSize)
        {
            this.blockSize = blockSize;
            this.items = new MemoryBlock[16];
        }

        public long Size
        {
            get
            {
                for (int i = items.Length - 1; i >= 0; i--)
                    if (items[i] != null)
                        return i * blockSize + blockSize;

                return 0;
            }
        }

        public byte Get(long index)
        {
            return GetOrCreate(index).Get(ToMinor(index));
        }

        public void Set(long index, byte value)
        {
            GetOrCreate(index).Set(ToMinor(index), value);
        }

        public void Set(long index, byte[] data)
        {
            int offset = 0;

            while (offset < data.Length)
            {
                MemoryBlock block = GetOrCreate(index + offset);
                int length = Math.Min(data.Length - offset, blockSize - ToMinor(index));

                block.Set(ToMinor(index + offset), data, offset, length);
                offset = offset + length;
            }
        }

        private MemoryBlock GetOrCreate(long index)
        {
            int major = ToMajor(index);

            if (items.Length <= major || items[major] == null)
            {
                if (items.Length <= major)
                {
                    Array.Resize(ref items, major + 16);
                }

                for (int i = 0; i <= major; i++)
                {
                    if (items[i] == null)
                    {
                        items[i] = CreateBlock();
                    }
                }
            }

            return items[major];
        }

        private int ToMinor(long index)
        {
            return (int)(index % blockSize);
        }

        private int ToMajor(long index)
        {
            return (int)(index / blockSize);
        }

        private MemoryBlock CreateBlock()
        {
            return new MemoryBlock(blockSize);
        }
    }
}