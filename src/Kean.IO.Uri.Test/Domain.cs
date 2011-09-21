using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.IO.Uri;

namespace Kean.IO.Uri.Test
{
    [TestFixture]
    public class Domain :
        Kean.Test.Fixture<Domain>
    {
        string prefix = "Kean.IO.Uri.Test.Domain.";
        protected override void Run()
        {
            this.Run(this.EqualityNull, this.Equality);
        }
        [Test]
        public void EqualityNull()
        {
            Target.Domain domain = null;
            Expect(domain, Is.EqualTo(null), this.prefix + "EqualityNull.0");
            Expect(domain == null, "domain == null", this.prefix + "EqualityNull.1");
        }
        [Test]
        public void Equality()
        {
            Target.Domain domain = "www.example.com";
            Expect(domain, Is.Not.EqualTo(null), this.prefix + "Equality.0");
            Expect(domain != null, "domain != null", this.prefix + "Equality.1");
            Expect((string)domain, Is.EqualTo("www.example.com"), this.prefix + "Equality.2");
            Expect(domain == "www.example.com", "domain == \"www.example.com\"", this.prefix + "Equality.3");
            Expect(domain.Head, Is.EqualTo("www"), this.prefix + "Equality.4");
            Expect((string)domain.Tail, Is.EqualTo("example.com"), this.prefix + "Equality.5");
            Expect(domain.Tail == "example.com", "domain.Tail == \"example.com\"", this.prefix + "Equality.6");
            Expect(domain.Tail.Head, Is.EqualTo("example"), this.prefix + "Equality.7");
            Expect((string)domain.Tail.Tail, Is.EqualTo("com"), this.prefix + "Equality.8");
            Expect(domain.Tail.Tail.Tail, Is.EqualTo(null), this.prefix + "Equality.9");
        }
    }
}
