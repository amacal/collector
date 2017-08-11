namespace Collector
{
    public interface Addressable
    {
        byte Get(long index);

        void GetBytes(long index, byte[] data);

        void Set(long index, byte value);

        void SetBytes(long index, byte[] data);
    }
}