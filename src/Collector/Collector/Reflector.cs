using System;
using System.Collections.Generic;
using System.Reflection;

namespace Collector
{
    public class Reflector
    {
        public Serializer<T> GetSerializer<T>()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<Member<T>> members = new List<Member<T>>();

            foreach (PropertyInfo property in properties)
            {
                Member<T> member = GetMember<T>(property);

                if (member != null)
                    members.Add(member);
            }

            return new Serializer<T>(members.ToArray());
        }

        public Member<T> GetMember<T>(string name)
        {
            return GetMember<T>(typeof(T).GetProperty(name));
        }

        private Member<T> GetMember<T>(PropertyInfo property)
        {
            Type type = property.PropertyType;

            if (IsNullable(property))
            {
                type = Nullable.GetUnderlyingType(type);
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Int64:
                    if (type == property.PropertyType)
                        return new MemberInt64<T>(new ReflectorProperty<T, long>(property));

                    return new MemberNullable<T, long?>(
                        new MemberInt64<T>(new ReflectorProperty<T, long?>(property)),
                        new ReflectorProperty<T, long?>(property));

                case TypeCode.String:
                    return new MemberString<T>(new ReflectorProperty<T, string>(property));
            }

            if (type?.IsArray == true && type.GetArrayRank() == 1)
            {
                Type item = type.GetElementType();
                Type generic = typeof(MemberArray<,>).MakeGenericType(typeof(T), item);

                Type accessorType = typeof(ReflectorProperty<,>).MakeGenericType(typeof(T), type);
                object accessorInstance = Activator.CreateInstance(accessorType, property);

                object serializer = typeof(Reflector).GetMethod("GetSerializer").MakeGenericMethod(item).Invoke(this, null);
                object instance = Activator.CreateInstance(generic, accessorInstance, serializer);

                return (Member<T>)instance;
            }

            return null;
        }

        private static bool IsNullable(PropertyInfo property)
        {
            return property.PropertyType.IsGenericType &&
                   property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}