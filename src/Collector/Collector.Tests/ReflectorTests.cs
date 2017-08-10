using System;
using NUnit.Framework;

namespace Collector.Tests
{
    public class ReflectorTests
    {
        private class Empty
        {
        }

        private class Item
        {
            public Int64 Int64Value { get; set; }
            public String StringValue { get; set; }
            public Int64? NullableValue { get; set; }
            public Empty[] ArrayValue { get; set; }
        }

        [Test]
        public void ShouldHandleEmptyType()
        {
            Reflector reflector = new Reflector();
            Serializer<Empty> serializer = reflector.GetSerializer<Empty>();

            Assert.That(serializer.Count, Is.Zero);
        }

        [Test]
        public void ShouldGetAllProperties()
        {
            Reflector reflector = new Reflector();
            Serializer<Item> serializer = reflector.GetSerializer<Item>();

            Assert.That(serializer.Count, Is.EqualTo(4));
        }

        [Test]
        public void ShouldHandleNullableProperty()
        {
            Reflector reflector = new Reflector();
            Member<Item> member = reflector.GetMember<Item>("NullableValue");

            Assert.That(member.GetType().GetGenericTypeDefinition(), Is.EqualTo(typeof(MemberNullable<,>)));
        }
    }
}