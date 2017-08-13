using System;

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

        public int Transfer(Addressable source, long index, Substitute<T> destination)
        {
            int total = source.ReadInt32(index + 0);

            destination.Add(property.Name, () =>
            {
                int length = source.ReadInt32(index + 4);
                SubstituteArray value = null;

                object[] callback()
                {
                    Substitute<V>[] items = new Substitute<V>[length];
                    index = index + 8;

                    for (int i = 0; i < length; i++)
                    {
                        Addressable scope = source.Scope(index);

                        items[i] = new Substitute<V>(nested, scope);
                        index = index + nested.Transfer(scope, items[i]);
                    }

                    return items;
                }

                if (length >= 0)
                    value = new SubstituteArray(length, callback);

                return value;
            });

            return total;
        }

        public int Transfer(Addressable source, long index, T destination)
        {
            int total = source.ReadInt32(index + 0);
            int length = source.ReadInt32(index + 4);

            if (length >= 0)
            {
                V[] items = new V[length];
                index = index + 8;

                for (int i = 0; i < length; i++)
                {
                    items[i] = (V)Activator.CreateInstance(typeof(V));
                    index = index + nested.Transfer(source.Scope(index), items[i]);
                }

                property.SetValue(destination, items);
            }
            else
            {
                property.SetNull(destination);
            }

            return total;
        }

        private int NormalizeLength(int value)
        {
            return value >= 0 ? value + 4 : 4;
        }
    }
}