using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Xenon
{
    public class XmlStream : IDisposable
    {
        private readonly XmlReader reader;

        public XmlStream(TextReader reader)
        {
            this.reader = new XmlTextReader(reader);
        }

        public IEnumerable<dynamic> Open(string node)
        {
            while (reader.Read())
            {
                if (reader.Name == node)
                {
                    using (XmlReader inner = reader.ReadSubtree())
                    {
                        yield return new XmlElement(true, XElement.Load(inner));
                    }
                }
            }
        }

        public void Dispose()
        {
            reader?.Dispose();
        }
    }
}