namespace Collector
{
    public class IndexPosition
    {
        private long value;

        public long Value
        {
            get { return value; }
        }

        public long Count
        {
            get { return value / 8; }
        }

        public void Increase()
        {
            value += 8;
        }

        public long At(long index)
        {
            return index * 8;
        }
    }
}