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

        public string Name
        {
            get { return property.Name; }
        }

        public int Measure(T source)
        {
            V[] value = property.GetValue(source);
            int length = value?.Sum(nested.Measure) ?? -1;

            return NormalizeLength(length);
        }

        public int Transfer(T source, Addressable destination, long index)
        {
            long initial = index;
            V[] value = property.GetValue(source);
            int length = value?.Length ?? -1;

            destination.WriteInt32(initial + 4, length);
            index = index + 8;

            if (value != null)
            {
                foreach (V item in value)
                {
                    index = index + nested.Transfer(item, destination.Scope(index));
                }
            }

            destination.WriteInt32(initial, (int)(index - initial));
            return (int)(index - initial);
        }

        public int Transfer(Addressable source, long index, Substitute destination)
        {
            int total = source.ReadInt32(index + 0);

            destination.Add(property.Name, () =>
            {
                int length = source.ReadInt32(index + 4);
                SubstituteArray value = null;

                object[] callback()
                {
                    Substitute[] items = new Substitute[length];
                    index = index + 8;

                    for (int i = 0; i < length; i++)
                    {
                        items[i] = new Substitute();
                        index = index + nested.Transfer(source.Scope(index), items[i]);
                    }

                    return items;
                }

                if (length >= 0)
                    value = new SubstituteArray(length, callback);

                return value;
            });

            return total;
        }

        private int NormalizeLength(int value)
        {
            return value >= 0 ? value + 4 : 4;
        }
    }
}