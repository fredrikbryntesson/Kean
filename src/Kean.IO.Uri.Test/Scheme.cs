using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.IO.Uri;

namespace Kean.IO.Uri.Test
{
    [TestFixture]
    public class Scheme :
        Kean.Test.Fixture<Scheme>
    {
        string prefix = "Kean.IO.Uri.Test.Scheme.";
        protected override void Run()
        {
            this.Run(this.EqualityNull, this.Equality);
        }
        [Test]
        public void EqualityNull()
        {
            Target.Scheme scheme = null;
            Expect(scheme, Is.EqualTo(null), this.prefix + "EqualityNull.0");
            Expect(scheme == null, "scheme == null", this.prefix + "EqualityNull.1");
        }
        [Test]
        public void Equality()
        {
            Target.Scheme scheme = "svn+ssh";
            Expect(scheme, Is.Not.EqualTo(null), this.prefix + "Equality.0");
            Expect(scheme != null, "scheme != null", this.prefix + "Equality.1");
            Expect((string)scheme, Is.EqualTo("svn+ssh"), this.prefix + "Equality.2");
            Expect(scheme == "svn+ssh", "scheme == \"svn+ssh\"", this.prefix + "Equality.3");
            Expect(scheme.Head, Is.EqualTo("svn"), this.prefix + "Equality.4");
            Expect((string)scheme.Tail, Is.EqualTo("ssh"), this.prefix + "Equality.5");
            Expect(scheme.Tail == "ssh", "scheme.Tail == \"ssh\"", this.prefix + "Equality.6");
            Expect(scheme.Tail.Head, Is.EqualTo("ssh"), this.prefix + "Equality.7");
            Expect(scheme.Tail.Tail, Is.EqualTo(null), this.prefix + "Equality.8");
        }
    }
}
