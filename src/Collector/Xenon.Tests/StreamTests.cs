using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Xenon.Tests
{
    public class StreamTests
    {
        [Test]
        public void ShouldStreamTwoObjects()
        {
            const string xml = @"<root><item id=""1""/><item id=""2""/></root>";

            using (TextReader source = new StringReader(xml))
            using (XmlStream stream = new XmlStream(source))
            {
                Assert.That(stream.Open("item").Count(), Is.EqualTo(2));
            }
        }

        [Test]
        public void ShouldStreamAllProperties()
        {
            const string xml = @"<root><item id=""1""/><item id=""2""/></root>";

            using (TextReader source = new StringReader(xml))
            using (XmlStream stream = new XmlStream(source))
            {
                foreach (dynamic element in stream.Open("item"))
                {
                    Assert.That(element.id.ToString(), Is.EqualTo("1").Or.EqualTo("2"));
                }
            }
        }
    }
}