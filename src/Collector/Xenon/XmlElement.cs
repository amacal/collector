using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Xml.Linq;

namespace Xenon
{
    public class XmlElement : DynamicObject
    {
        private readonly XElement[] inner;
        private readonly Lazy<Dictionary<string, object>> nodes;

        public XmlElement(bool single, params XElement[] inner)
        {
            this.inner = inner;

            this.nodes = new Lazy<Dictionary<string, object>>(() =>
            {
                Dictionary<string, object> instance = new Dictionary<string, object>();

                foreach (var group in inner.Attributes().GroupBy(x => x.Name.LocalName))
                {
                    instance[group.Key] = new XmlAttribute(group);
                }

                foreach (var group in inner.Elements().GroupBy(x => x.Name.LocalName))
                {
                    instance[group.Key] = new XmlElement(false, group.ToArray());
                }

                return instance;
            });
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (binder.Name == "Has" && args.Length == 1 && args[0] is string)
            {
                result = nodes.Value.ContainsKey((string)args[0]);
                return true;
            }

            if (binder.Name == "ToArray" && args.Length == 0)
            {
                result = inner.Select(x => x.Value).ToArray();
                return true;
            }

            if (binder.Name == "ToInt32" && args.Length == 0)
            {
                result = inner.Select(x => (int?)Int32.Parse(x.Value)).FirstOrDefault();
                return true;
            }

            if (binder.Name == "ToInt64" && args.Length == 0)
            {
                result = inner.Select(x => (int?)Int64.Parse(x.Value)).FirstOrDefault();
                return true;
            }

            if (binder.Name == "ToDateTime" && args.Length == 0)
            {
                result = inner.Select(x => (DateTime?)DateTime.Parse(x.Value)).FirstOrDefault();
                return true;
            }

            return base.TryInvokeMember(binder, args, out result);
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length == 1 && indexes[0] is int)
            {
                result = new XmlElement(false, inner[(int)indexes[0]]);
                return true;
            }

            return base.TryGetIndex(binder, indexes, out result);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            nodes.Value.TryGetValue(binder.Name, out result);
            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.ReturnType == typeof(IEnumerable))
            {
                IEnumerable enumerate()
                {
                    foreach (XElement element in inner)
                    {
                        yield return new XmlElement(false, element);
                    }
                }

                result = enumerate();
                return true;
            }

            return base.TryConvert(binder, out result);
        }

        public override string ToString()
        {
            return inner[0].Value;
        }
    }
}