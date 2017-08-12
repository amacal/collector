using System.Reflection;
using NUnit.Framework;

namespace Collector.Tests
{
    public class ReflectorPropertyTests
    {
        private class Regular
        {
            public string Value { get; set; }
        }

        private class Nullable
        {
            public long? Value { get; set; }
        }

        [Test]
        public void ShouldNotFindNullInNullableString()
        {
            Regular regular = new Regular { Value = "abc" };
            PropertyInfo info = typeof(Regular).GetProperty("Value");
            ReflectorProperty<Regular, string> property = new ReflectorProperty<Regular, string>(info);

            Assert.That(property.IsNull(regular), Is.False);
        }

        [Test]
        public void ShouldFindNullInNullableString()
        {
            Regular regular = new Regular { Value = null };
            PropertyInfo info = typeof(Regular).GetProperty("Value");
            ReflectorProperty<Regular, string> property = new ReflectorProperty<Regular, string>(info);

            Assert.That(property.IsNull(regular), Is.True);
        }

        [Test]
        public void ShouldNotFindNullInNullablePrimitive()
        {
            Nullable nullable = new Nullable { Value = 123 };
            PropertyInfo info = typeof(Nullable).GetProperty("Value");
            ReflectorProperty<Nullable, long?> property = new ReflectorProperty<Nullable, long?>(info);

            Assert.That(property.IsNull(nullable), Is.False);
        }

        [Test]
        public void ShouldFindNullInNullablePrimitive()
        {
            Nullable nullable = new Nullable { Value = null };
            PropertyInfo info = typeof(Nullable).GetProperty("Value");
            ReflectorProperty<Nullable, long?> property = new ReflectorProperty<Nullable, long?>(info);

            Assert.That(property.IsNull(nullable), Is.True);
        }

        [Test]
        public void ShouldSetNullInNullableString()
        {
            dynamic regular = new Substitute();
            PropertyInfo info = typeof(Regular).GetProperty("Value");
            ReflectorProperty<Regular, string> property = new ReflectorProperty<Regular, string>(info);

            property.SetNull(regular);
            Assert.That(regular.Value, Is.Null);
        }

        [Test]
        public void ShouldSetNullInNullablePrimitive()
        {
            dynamic nullable = new Substitute();
            PropertyInfo info = typeof(Nullable).GetProperty("Value");
            ReflectorProperty<Nullable, long?> property = new ReflectorProperty<Nullable, long?>(info);

            property.SetNull(nullable);
            Assert.That(nullable.Value, Is.Null);
        }

        [Test]
        public void ShouldGetValue()
        {
            Regular regular = new Regular { Value = "abc" };
            PropertyInfo info = typeof(Regular).GetProperty("Value");
            ReflectorProperty<Regular, string> property = new ReflectorProperty<Regular, string>(info);

            Assert.That(property.GetValue(regular), Is.EqualTo("abc"));
        }

        [Test]
        public void ShouldGetNullableValue()
        {
            Nullable nullable = new Nullable { Value = 123 };
            PropertyInfo info = typeof(Nullable).GetProperty("Value");
            ReflectorProperty<Nullable, long?> property = new ReflectorProperty<Nullable, long?>(info);

            Assert.That(property.GetValue(nullable), Is.EqualTo(123));
        }

        [Test]
        public void ShouldSetValue()
        {
            dynamic regular = new Substitute();
            PropertyInfo info = typeof(Regular).GetProperty("Value");
            ReflectorProperty<Regular, string> property = new ReflectorProperty<Regular, string>(info);

            property.SetValue((Substitute)regular, () => "cde");
            Assert.That(regular.Value, Is.EqualTo("cde"));
        }

        [Test]
        public void ShouldSetNullableValue()
        {
            dynamic nullable = new Substitute();
            PropertyInfo info = typeof(Nullable).GetProperty("Value");
            ReflectorProperty<Nullable, long?> property = new ReflectorProperty<Nullable, long?>(info);

            property.SetValue((Substitute)nullable, () => 123);
            Assert.That(nullable.Value, Is.EqualTo(123));
        }

        [Test]
        public void ShouldSetNullableValueToNull()
        {
            dynamic nullable = new Substitute();
            PropertyInfo info = typeof(Nullable).GetProperty("Value");
            ReflectorProperty<Nullable, long?> property = new ReflectorProperty<Nullable, long?>(info);

            property.SetValue((Substitute)nullable, () => null);
            Assert.That(nullable.Value, Is.Null);
        }

        [Test]
        public void ShouldHandleCastedGetConversion()
        {
            Regular item = new Regular { Value = "aBc" };
            PropertyInfo info = typeof(Regular).GetProperty("Value");

            ReflectorProperty<Regular, string> property = new ReflectorProperty<Regular, string>(info);
            ReflectorProperty<Regular, string> casted = property.Cast(x => x.ToLower(), x => x.ToUpper());

            Assert.That(casted.GetValue(item), Is.EqualTo("abc"));
        }

        [Test]
        public void ShouldHandleCastedSetConversion()
        {
            dynamic item = new Substitute();
            PropertyInfo info = typeof(Regular).GetProperty("Value");

            ReflectorProperty<Regular, string> property = new ReflectorProperty<Regular, string>(info);
            ReflectorProperty<Regular, string> casted = property.Cast(x => x.ToLower(), x => x.ToUpper());

            casted.SetValue((Substitute)item, () => "cDe");
            Assert.That(item.Value, Is.EqualTo("CDE"));
        }

        [Test]
        public void ShouldHavePropertName()
        {
            PropertyInfo info = typeof(Regular).GetProperty("Value");
            ReflectorProperty<Regular, string> property = new ReflectorProperty<Regular, string>(info);

            Assert.That(property.Name, Is.EqualTo("Value"));
        }

        [Test]
        public void ShouldStillHavePropertNameAfterCasting()
        {
            PropertyInfo info = typeof(Regular).GetProperty("Value");

            ReflectorProperty<Regular, string> property = new ReflectorProperty<Regular, string>(info);
            ReflectorProperty<Regular, string> casted = property.Cast(x => x.ToLower(), x => x.ToUpper());

            Assert.That(casted.Name, Is.EqualTo("Value"));
        }
    }
}