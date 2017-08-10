using System.Xml.Linq;
using NUnit.Framework;

namespace Xenon.Tests
{
    public class AttributeTests
    {
        [Test]
        public void ShouldFindAttributeValue()
        {
            XElement xml = XElement.Parse(@"<root id=""10""/>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.id.ToString(), Is.EqualTo("10"));
        }

        [Test]
        public void ShouldFindAttributePresence()
        {
            XElement xml = XElement.Parse(@"<root id=""10""/>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.Has("id"), Is.True);
        }

        [Test]
        public void ShouldFindAttributePresenceNegative()
        {
            XElement xml = XElement.Parse(@"<root id=""10""/>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.Has("value"), Is.False);
        }

        [Test]
        public void ShouldFindElementValue()
        {
            XElement xml = XElement.Parse(@"<root><id>10</id></root>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.Has("id"), Is.True);
        }

        [Test]
        public void ShouldGetContentEmpty()
        {
            XElement xml = XElement.Parse(@"<root id=""10""/>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.ToString(), Is.Empty);
        }

        [Test]
        public void ShouldGetContentText()
        {
            XElement xml = XElement.Parse(@"<root id=""10"">abc</root>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.ToString(), Is.EqualTo("abc"));
        }

        [Test]
        public void ShouldGetNestedKey()
        {
            XElement xml = XElement.Parse(@"<root id=""10""><node key=""abc""/></root>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.node.key.ToString(), Is.EqualTo("abc"));
        }

        [Test]
        public void ShouldGetNestedKeyByIndex()
        {
            XElement xml = XElement.Parse(@"<root id=""10""><node key=""abc""/><node key=""cde""/></root>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.node.key[1].ToString(), Is.EqualTo("cde"));
        }

        [Test]
        public void ShouldGetNestedKeys()
        {
            XElement xml = XElement.Parse(@"<root id=""10""><node key=""abc""/><node key=""cde""/></root>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.node.key.ToArray(), Is.EqualTo(new[] { "abc", "cde" }));
        }

        [Test]
        public void ShouldGetNestedNode()
        {
            XElement xml = XElement.Parse(@"<root id=""10""><node>abc</node></root>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.node.ToString(), Is.EqualTo("abc"));
        }

        [Test]
        public void ShouldGetNestedNodeByIndex()
        {
            XElement xml = XElement.Parse(@"<root id=""10""><node>abc</node><node>cde</node></root>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.node[1].ToString(), Is.EqualTo("cde"));
        }

        [Test]
        public void ShouldGetNestedNodes()
        {
            XElement xml = XElement.Parse(@"<root id=""10""><node>abc</node><node>cde</node></root>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.node.ToArray(), Is.EqualTo(new[] { "abc", "cde" }));
        }

        [Test]
        public void ShouldGetNestedNodesUsingEnumerator()
        {
            XElement xml = XElement.Parse(@"<root id=""10""><node>abc</node><node>cde</node></root>");
            dynamic element = new XmlElement(true, xml);

            foreach (dynamic node in element.node)
            {
                Assert.That(node.ToString(), Is.EqualTo("abc").Or.EqualTo("cde"));
            }
        }

        [Test]
        public void ShouldGetDoubleNestedNodeByIndex()
        {
            XElement xml = XElement.Parse(@"<root id=""10""><node>abc</node><node><value>data</value></node></root>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.node[1].value.ToString(), Is.EqualTo("data"));
        }

        [Test]
        public void ShouldGetDoubleNestedTextByIndex()
        {
            XElement xml = XElement.Parse(@"<root id=""10""><node>abc</node><node><value>data</value></node></root>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.node.ToArray()[1], Is.EqualTo("data"));
        }

        [Test]
        public void ShouldHandleNullNodePropagation()
        {
            XElement xml = XElement.Parse(@"<root id=""10""><node>abc</node><node>cde</node></root>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.node?.data?.test?.ToString(), Is.Null);
        }

        [Test]
        public void ShouldHandleNullAttributePropagation()
        {
            XElement xml = XElement.Parse(@"<root id=""10""><node>abc</node><node>cde</node></root>");
            dynamic element = new XmlElement(true, xml);

            Assert.That(element.id?.data?.test?.ToString(), Is.Null);
        }
    }
}