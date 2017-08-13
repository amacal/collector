using NUnit.Framework;

namespace Collector.Tests
{
    public class SelectTests
    {
        private class ItemInput
        {
            public long Value { get; set; }
        }

        private class ItemOutput
        {
            public string Value { get; set; }
        }

        [Test]
        public void ShouldSelectOneEmpty()
        {
            Reflector reflector = new Reflector();
            Serializer<ItemInput> input = reflector.GetSerializer<ItemInput>(); ;
            Serializer<ItemOutput> output = reflector.GetSerializer<ItemOutput>(); ;

            Collectible collectible = new Collectible(1024);
            SelectBy by = Select.One<ItemInput, ItemOutput>(input, output, x => x.Value.ToString());

            Select.Table(collectible, by);
            Assert.That(collectible.Count, Is.Zero);
        }

        [Test]
        public void ShouldSelectOneWithItem()
        {
            Reflector reflector = new Reflector();
            Serializer<ItemInput> input = reflector.GetSerializer<ItemInput>(); ;
            Serializer<ItemOutput> output = reflector.GetSerializer<ItemOutput>(); ;

            Collectible collectible = new Collectible(1024);
            collectible.Enqueue(input, new ItemInput { Value = 1 });

            SelectBy by = Select.One(input, output, x => new ItemOutput
            {
                Value = x.Value.ToString()
            });

            collectible = Select.Table(collectible, by);
            Assert.That(collectible.Count, Is.EqualTo(1));
            Assert.That(collectible.At(output, 0).AsDynamic().Value.ToString(), Is.EqualTo("1"));
        }
    }
}