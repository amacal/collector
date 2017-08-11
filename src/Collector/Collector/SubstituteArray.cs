using System;
using System.Dynamic;

namespace Collector
{
    public class SubstituteArray : DynamicObject
    {
        private readonly int length;
        private readonly Lazy<object[]> items;

        public SubstituteArray(int length, Func<object[]> callback)
        {
            this.length = length;
            this.items = new Lazy<object[]>(callback);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder.Name == "Length")
            {
                result = length;
                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length == 1 && indexes[0] is int)
            {
                result = items.Value[(int)indexes[0]];
                return true;
            }

            return base.TryGetIndex(binder, indexes, out result);
        }
    }
}