using System;
using System.Text;

namespace Collector
{
    public static class AddressableExtensions
    {
        public static Addressable Scope(this Addressable addressable, long offset)
        {
            return new AddressableScope(addressable, offset);
        }

        public static void WriteInt32(this Addressable addressable, long index, Int32 value)
        {
            for (int i = 3; i >= 0; i--)
            {
                addressable.Set(index + i, (byte)(value & 0xff));
                value = value >> 8;
            }
        }

        public static void WriteInt64(this Addressable addressable, long index, Int64 value)
        {
            for (int i = 7; i >= 0; i--)
            {
                addressable.Set(index + i, (byte)(value & 0xff));
                value = value >> 8;
            }
        }

        public static void WriteString(this Addressable addressable, long index, String value)
        {
            if (value != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                addressable.SetBytes(index, bytes);
            }
        }

        public static Int32 ReadInt32(this Addressable addressable, long index)
        {
            Int32 value = 0;

            for (int i = 0; i < 4; i++)
            {
                value = value << 8;
                value = value + addressable.Get(index + i);
            }

            return value;
        }

        public static Int64 ReadInt64(this Addressable addressable, long index)
        {
            Int64 value = 0;

            for (int i = 0; i < 8; i++)
            {
                value = value << 8;
                value = value + addressable.Get(index + i);
            }

            return value;
        }

        public static String ReadString(this Addressable addressable, long index, int length)
        {
            if (length < 0)
                return null;

            if (length == 0)
                return String.Empty;

            byte[] bytes = new byte[length];
            addressable.GetBytes(index, bytes);

            return Encoding.UTF8.GetString(bytes);
        }
    }
}