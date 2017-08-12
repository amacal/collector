using System;
using System.Dynamic;

namespace Collector
{
    public class SubstituteText : DynamicObject, IComparable
    {
        private readonly int length;
        private readonly Lazy<string> text;

        public SubstituteText(int length, Func<string> callback)
        {
            this.length = length;
            this.text = new Lazy<string>(callback);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder.Name == "Size")
            {
                result = length;
                return true;
            }

            if (binder.Name == "Length")
            {
                result = text.Value.Length;
                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.Type == typeof(string))
            {
                result = ToString();
                return true;
            }

            if (binder.Type == typeof(IComparable))
            {
                result = this;
                return true;
            }

            return base.TryConvert(binder, out result);
        }

        public override string ToString()
        {
            return text.Value;
        }

        public int CompareTo(object other)
        {
            SubstituteText substitute = other as SubstituteText;

            if (substitute == null)
                return 1;

            return String.Compare(text.Value, substitute.text.Value, StringComparison.Ordinal);
        }
    }
}