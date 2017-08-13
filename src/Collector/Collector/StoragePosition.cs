namespace Collector
{
    public class StoragePosition
    {
        private long low;
        private long high;

        public StoragePosition()
        {
        }

        public StoragePosition(long low, long high)
        {
            this.low = low;
            this.high = high;
        }

        public long Low
        {
            get { return low; }
        }

        public long High
        {
            get { return high; }
        }

        public void Decrease(int size)
        {
            low += size;
        }

        public void Increase(int size)
        {
            high += size;
        }

        public StoragePosition Clone()
        {
            return new StoragePosition(low, high);
        }
    }
}