namespace Collector
{
    public interface Member<T>
    {
        string Name { get; }

        int Transfer(T source, Addressable destination, long index);

        int Transfer(Addressable source, long index, Substitute<T> destination);

        int Transfer(Addressable source, long index, T destination);
    }
}