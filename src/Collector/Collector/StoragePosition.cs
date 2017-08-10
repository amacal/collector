namespace Collector
{
    public class StoragePosition
    {
        private long value;

        public StoragePosition()
        {
        }

        public StoragePosition(long value)
        {
            this.value = value;
        }

        public long Value
        {
            get { return value; }
        }

        public void Increase(int size)
        {
            value += size;
        }

        public StoragePosition Clone()
        {
            return new StoragePosition(value);
        }
    }
}