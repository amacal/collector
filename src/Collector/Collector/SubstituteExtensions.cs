namespace Collector
{
    public static class SubstituteExtensions
    {
        public static dynamic AsDynamic<T>(this Substitute<T> target)
        {
            return target;
        }
    }
}