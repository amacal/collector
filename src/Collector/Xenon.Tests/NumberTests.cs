using System.Xml.Linq;
using NUnit.Framework;

namespace Xenon.Tests
{
    public class NumberTests
    {
        [Test]
        public void ShouldFindAttributeAsInt32()
        {
            XElement xml = XElement.Parse(@"<root id=""10""/>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.id.ToInt32(), Is.EqualTo(10));
        }

        [Test]
        public void ShouldFindNodeAsInt32()
        {
            XElement xml = XElement.Parse(@"<root>10</root>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.ToInt32(), Is.EqualTo(10));
        }

        [Test]
        public void ShouldFindAttributeAsInt64()
        {
            XElement xml = XElement.Parse(@"<root id=""10""/>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.id.ToInt64(), Is.EqualTo(10));
        }

        [Test]
        public void ShouldFindNodeAsInt64()
        {
            XElement xml = XElement.Parse(@"<root>10</root>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.ToInt64(), Is.EqualTo(10));
        }
    }
}