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

        public int Measure(T source)
        {
            string value = property.GetValue(source);
            int length = value != null ? Encoding.UTF8.GetByteCount(value) : -1;

            return NormalizeLength(length);
        }

        public int Transfer(T source, Addressable destination, long index)
        {
            string value = property.GetValue(source);
            int length = value != null ? Encoding.UTF8.GetByteCount(value) : -1;

            destination.WriteInt32(index + 0, length);
            destination.WriteString(index + 4, value);

            return NormalizeLength(length);
        }

        public int Transfer(Addressable source, long index, T destination)
        {
            int length = source.ReadInt32(index + 0);
            string value = source.ReadString(index + 4, length);

            property.SetValue(destination, value);
            return NormalizeLength(length);
        }

        private int NormalizeLength(int value)
        {
            return value >= 0 ? value + 4 : 4;
        }
    }
}