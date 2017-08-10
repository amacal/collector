using System;
using System.Xml.Linq;
using NUnit.Framework;

namespace Xenon.Tests
{
    public class DateTimeTests
    {
        [Test]
        public void ShouldFindAttributeAsInt32()
        {
            XElement xml = XElement.Parse(@"<root id=""2013-10-12""/>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.id.ToDateTime(), Is.EqualTo(DateTime.Parse("2013-10-12")));
        }

        [Test]
        public void ShouldFindNodeAsInt32()
        {
            XElement xml = XElement.Parse(@"<root>2013-10-12</root>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.ToDateTime(), Is.EqualTo(DateTime.Parse("2013-10-12")));
        }
    }
}