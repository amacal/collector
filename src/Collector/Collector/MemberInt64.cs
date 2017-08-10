using System;

namespace Collector
{
    public class MemberInt64<T> : Member<T>
    {
        private readonly ReflectorProperty<T, Int64> property;

        public MemberInt64(ReflectorProperty<T, Int64> property)
        {
            this.property = property;
        }

        public MemberInt64(ReflectorProperty<T, Int64?> property)
        {
            this.property = property.Cast(x => x.Value, x => x);
        }

        public int Measure(T source)
        {
            return 8;
        }

        public int Transfer(T source, Addressable destination, long index)
        {
            destination.WriteInt64(index, property.GetValue(source));
            return 8;
        }

        public int Transfer(Addressable source, long index, T destination)
        {
            property.SetValue(destination, source.ReadInt64(index));
            return 8;
        }
    }
}