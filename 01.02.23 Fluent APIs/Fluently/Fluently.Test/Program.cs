using Fluently.Core;

namespace Fluently.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool assert = Assert.That(1, Is.EqualTo(1));
            assert = Assert.That(1, Is.EqualTo(2));
            assert = Assert.That(1, Is.Not.EqualTo(2));
            assert = Assert.That(1, Is.Not.EqualTo(1));

            assert = Assert.That("Hello, world", Is.Not.EqualTo("A different string"));
            assert = Assert.That("Hello, world", Is.EqualTo("Hello, world"));

        }
    }
}