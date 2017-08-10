using System;

namespace Collector
{
    public class MemberDateTime<T> : Member<T>
    {
        private readonly ReflectorProperty<T, DateTime> property;

        public MemberDateTime(ReflectorProperty<T, DateTime> property)
        {
            this.property = property;
        }

        public MemberDateTime(ReflectorProperty<T, DateTime?> property)
        {
            this.property = property.Cast(x => x.Value, x => x);
        }

        public int Measure(T source)
        {
            return 8;
        }

        public int Transfer(T source, Addressable destination, long index)
        {
            destination.WriteInt64(index, property.GetValue(source).Ticks);
            return 8;
        }

        public int Transfer(Addressable source, long index, T destination)
        {
            property.SetValue(destination, new DateTime(source.ReadInt64(index)));
            return 8;
        }
    }
}