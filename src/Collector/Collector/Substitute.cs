using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Collector
{
    public class Substitute : DynamicObject
    {
        private readonly Dictionary<string, Lazy<object>> items;

        public Substitute()
        {
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
    }
}