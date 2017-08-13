using System;
using System.Text;

namespace Collector
{
    public class MemberString<T> : Member<T>
    {
        private readonly ReflectorProperty<T, String> property;

        public MemberString(ReflectorProperty<T, String> property)
        {
            this.property = property;
        }

        public string Name
        {
            get { return property.Name; }
        }

        public int Transfer(T source, Addressable destination, long index)
        {
            string value = property.GetValue(source);
            int length = value != null ? Encoding.UTF8.GetByteCount(value) : -1;

            destination.WriteInt32(index + 0, length);
            destination.WriteString(index + 4, value);

            return NormalizeLength(length);
        }

        public int Transfer(Addressable source, long index, Substitute<T> destination)
        {
            int length = source.ReadInt32(index + 0);

            destination.Add(property.Name, () =>
            {
                if (length < 0)
                    return null;

                return new SubstituteText(length, () => source.ReadString(index + 4, length));
            });

            return NormalizeLength(length);
        }

        public int Transfer(Addressable source, long index, T destination)
        {
            int length = source.ReadInt32(index + 0);
            property.SetValue(destination, source.ReadString(index + 4, length));
            return NormalizeLength(length);
        }

        private int NormalizeLength(int value)
        {
            return value >= 0 ? value + 4 : 4;
        }
    }
}