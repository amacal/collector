namespace Collector
{
    public interface SortBy
    {
        dynamic Extract(Collectible source, long index);

        void Swap(Collectible source, long left, long right);

        bool IsLessThan(dynamic left, dynamic right);
    }
}