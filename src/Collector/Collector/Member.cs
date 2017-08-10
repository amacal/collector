namespace Collector
{
    public interface Member<T>
    {
        int Measure(T source);

        int Transfer(T source, Addressable destination, long index);

        int Transfer(Addressable source, long index, T destination);
    }
}