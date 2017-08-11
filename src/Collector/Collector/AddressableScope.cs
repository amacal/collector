namespace Collector
{
    public class AddressableScope : Addressable
    {
        private readonly Addressable target;
        private readonly long position;

        public AddressableScope(Addressable target, long position)
        {
            this.target = target;
            this.position = position;
        }

        public byte Get(long index)
        {
            return target.Get(index + position);
        }

        public void GetBytes(long index, byte[] data)
        {
            target.GetBytes(index + position, data);
        }

        public void Set(long index, byte value)
        {
            target.Set(index + position, value);
        }

        public void SetBytes(long index, byte[] data)
        {
            target.SetBytes(index + position, data);
        }

        public Addressable Scope(int offset)
        {
            return new AddressableScope(target, offset);
        }
    }
}