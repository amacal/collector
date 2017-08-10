namespace Collector
{
    public interface Addressable
    {
        byte Get(long index);

        void Set(long index, byte value);

        void Set(long index, byte[] data);
    }
}