using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.IO.Uri;

namespace Kean.IO.Uri.Test
{
    [TestFixture]
    public class Endpoint :
        Kean.Test.Fixture<Endpoint>
    {
        string prefix = "Kean.IO.Uri.Test.Endpoint.";
        protected override void Run()
        {
            this.Run(
                this.EqualityNull, 
                this.Equality, 
                this.EqualityHostOnly,
                this.EqualityPortOnly);
        }
        [Test]
        public void EqualityNull()
        {
            Target.Endpoint endpoint = null;
            Expect(endpoint, Is.EqualTo(null), this.prefix + "EqualityNull.0");
            Expect(endpoint == null, "endpoint == null", this.prefix + "EqualityNull.1");
        }
        [Test]
        public void Equality()
        {
            Target.Endpoint endpoint = "www.example.com:80";
            Expect(endpoint, Is.Not.EqualTo(null), this.prefix + "Equality.0");
            Expect(endpoint != null, "endpoint != null", this.prefix + "Equality.1");
            Expect((string)endpoint.Host, Is.EqualTo("www.example.com"), this.prefix + "Equality.2");
            Expect(endpoint.Port, Is.EqualTo(80), this.prefix + "Equality.3");
            Expect((string)endpoint, Is.EqualTo("www.example.com:80"), this.prefix + "Equality.4");
            Expect(endpoint == "www.example.com:80", "endpoint == \"www.example.com:80\"", this.prefix + "Equality.5");
            Expect(endpoint != "www.example.com", "endpoint != \"www.example.com\"", this.prefix + "Equality.6");
        }
        [Test]
        public void EqualityHostOnly()
        {
            Target.Endpoint endpoint = "www.example.com";
            Expect(endpoint, Is.Not.EqualTo(null), this.prefix + "EqualityHostOnly.0");
            Expect(endpoint != null, "endpoint != null", this.prefix + "EqualityHostOnly.1");
            Expect((string)endpoint.Host, Is.EqualTo("www.example.com"), this.prefix + "EqualityHostOnly.2");
            Expect(endpoint.Port, Is.EqualTo(null), this.prefix + "EqualityHostOnly.3");
            Expect((string)endpoint, Is.EqualTo("www.example.com"), this.prefix + "EqualityHostOnly.4");
            Expect(endpoint == "www.example.com", "endpoint == \"www.example.com\"", this.prefix + "EqualityHostOnly.5");
            Expect(endpoint == "www.example.com:", "endpoint == \"www.example.com:\"", this.prefix + "EqualityHostOnly.6");
            Expect(endpoint != "www.example.com:80", "endpoint != \"www.example.com:80\"", this.prefix + "EqualityHostOnly.7");
        }
        [Test]
        public void EqualityPortOnly()
        {
            Target.Endpoint endpoint = ":80";
            Expect(endpoint, Is.Not.EqualTo(null), this.prefix + "EqualityPortOnly.0");
            Expect(endpoint != null, "endpoint != null", this.prefix + "EqualityPortOnly.1");
            Expect((string)endpoint.Host, Is.EqualTo(""), this.prefix + "EqualityPortOnly.2");
            Expect(endpoint.Port, Is.EqualTo(80), this.prefix + "EqualityPortOnly.3");
            Expect((string)endpoint, Is.EqualTo(":80"), this.prefix + "EqualityPortOnly.4");
            Expect(endpoint == ":80", "endpoint == \":80\"", this.prefix + "EqualityPortOnly.5");
            Expect(endpoint != "wwww.example.com:80", "endpoint != \"www.example.com:80\"", this.prefix + "EqualityPortOnly.6");
        }
    }
}
