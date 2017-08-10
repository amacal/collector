using System;
using System.Linq;

namespace Collector
{
    public class MemberArray<T, V> : Member<T>
    {
        private readonly ReflectorProperty<T, V[]> property;
        private readonly Serializer<V> nested;

        public MemberArray(ReflectorProperty<T, V[]> property, Serializer<V> nested)
        {
            this.property = property;
            this.nested = nested;
        }

        public int Measure(T source)
        {
            V[] value = property.GetValue(source);
            int length = value?.Sum(nested.Measure) ?? -1;

            return NormalizeLength(length);
        }

        public int Transfer(T source, Addressable destination, long index)
        {
            V[] value = property.GetValue(source);
            int length = value?.Length ?? -1;

            destination.WriteInt32(index + 0, length);
            index = index + 4;

            if (value != null)
            {
                foreach (V item in value)
                {
                    index = index + nested.Transfer(item, destination.Scope(index));
                }
            }

            return (int)index;
        }

        public int Transfer(Addressable source, long index, T destination)
        {
            int length = source.ReadInt32(index + 0);
            V[] value = length >= 0 ? new V[length] : null;

            index = index + 4;

            for (int i = 0; i < length; i++)
            {
                value[i] = Activator.CreateInstance<V>();
                index = index + nested.Transfer(source.Scope(index), value[i]);
            }

            property.SetValue(destination, value);
            return (int)index;
        }

        private int NormalizeLength(int value)
        {
            return value >= 0 ? value + 4 : 4;
        }
    }
}