using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace Collector
{
    public class SubstituteArray<T> : DynamicObject, IEnumerable
    {
        private readonly int length;
        private readonly Lazy<Substitute<T>[]> items;

        public SubstituteArray(int length, Func<Substitute<T>[]> callback)
        {
            this.length = length;
            this.items = new Lazy<Substitute<T>[]>(callback);
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.Type == typeof(IEnumerable<T>))
            {
                IEnumerable<T> enumerate()
                {
                    foreach (Substitute<T> substitute in items.Value)
                    {
                        yield return substitute.AsDynamic();
                    }
                }

                result = enumerate();
                return true;
            }

            return base.TryConvert(binder, out result);
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.Value.GetEnumerator();
        }
    }
}