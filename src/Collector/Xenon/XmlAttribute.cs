using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Xml.Linq;

namespace Xenon
{
    public class XmlAttribute : DynamicObject
    {
        private readonly string[] inner;

        public XmlAttribute(IEnumerable<XAttribute> inner)
        {
            this.inner = inner.Select(x => x.Value).ToArray();
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (binder.Name == "ToArray" && args.Length == 0)
            {
                result = inner;
                return true;
            }

            if (binder.Name == "ToInt32" && args.Length == 0)
            {
                result = Int32.Parse(inner[0]);
                return true;
            }

            if (binder.Name == "ToInt64" && args.Length == 0)
            {
                result = Int64.Parse(inner[0]);
                return true;
            }

            if (binder.Name == "ToDateTime" && args.Length == 0)
            {
                result = DateTime.Parse(inner[0]);
                return true;
            }

            return base.TryInvokeMember(binder, args, out result);
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length == 1 && indexes[0] is int)
            {
                result = inner[(int)indexes[0]];
                return true;
            }

            return base.TryGetIndex(binder, indexes, out result);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            return true;
        }

        public override string ToString()
        {
            return inner[0];
        }
    }
}