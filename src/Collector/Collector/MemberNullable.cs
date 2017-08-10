using System;

namespace Collector
{
    public class MemberNullable<T, V> : Member<T>
    {
        private readonly Member<T> inner;
        private readonly ReflectorProperty<T, V> nullable;

        public MemberNullable(Member<T> inner, ReflectorProperty<T, V> nullable)
        {
            this.inner = inner;
            this.nullable = nullable;
        }

        public int Measure(T source)
        {
            if (nullable.IsNull(source))
                return 1;

            return inner.Measure(source) + 1;
        }

        public int Transfer(T source, Addressable destination, long index)
        {
            if (nullable.IsNull(source))
            {
                destination.Set(index, 0);
                return 1;
            }

            destination.Set(index, 1);
            return inner.Transfer(source, destination, index + 1) + 1;
        }

        public int Transfer(Addressable source, long index, T destination)
        {
            if (source.Get(index) == 0)
            {
                nullable.SetNull(destination);
                return 1;
            }

            return inner.Transfer(source, index + 1, destination) + 1;
        }
    }
}