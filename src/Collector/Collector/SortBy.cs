namespace Collector
{
    public interface SortBy<T>
    {
        T Extract(Collectible source, long index);

        void Swap(Collectible source, long left, long right);

        bool IsLessThan(T left, T right);
    }
}