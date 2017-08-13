using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Collector
{
    public class Substitute<T> : DynamicObject
    {
        private readonly Serializer<T> serializer;
        private readonly Addressable source;
        private readonly Dictionary<string, Lazy<object>> items;

        public Substitute(Serializer<T> serializer, Addressable source)
        {
            this.serializer = serializer;
            this.source = source;

            this.items = new Dictionary<string, Lazy<object>>();
        }

        public void Add(string name, Func<object> callback)
        {
            items[name] = new Lazy<object>(callback);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            Lazy<object> item;

            if (items.TryGetValue(binder.Name, out item))
            {
                result = item.Value;
                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.Type == typeof(T))
            {
                T destination = Activator.CreateInstance<T>();
                serializer.Transfer(source, destination);

                result = destination;
                return true;
            }

            return base.TryConvert(binder, out result);
        }
    }
}