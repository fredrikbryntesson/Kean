using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.IO.Uri;

namespace Kean.IO.Uri.Test
{
    [TestFixture]
    public class User :
        Kean.Test.Fixture<User>
    {
        string prefix = "Kean.IO.Uri.Test.";
        protected override void Run()
        {
            this.Run(this.EqualityNull, this.Equality, this.EqualityNameOnly, this.EqualityPasswordOnly);
        }
        [Test]
        public void EqualityNull()
        {
            Target.User user = null;
            Expect(user, Is.EqualTo(null), this.prefix + "EqualityNull.0");
            Expect(user == null, "user == null", this.prefix + "EqualityNull.1");
        }
        [Test]
        public void Equality()
        {
            Target.User user = "name:password";
            Expect(user, Is.Not.EqualTo(null), this.prefix + "Equality.0");
            Expect(user != null, "user != null", this.prefix + "Equality.1");
            Expect(user.Name, Is.EqualTo("name"), this.prefix + "Equality.2");
            Expect(user.Password, Is.EqualTo("password"), this.prefix + "Equality.3");
            Expect((string)user, Is.EqualTo("name:password"), this.prefix + "Equality.4");
            Expect(user == "name:password", "user == \"name:password\"", this.prefix + "Equality.5");
            Expect(user != "name", "user != \"name\"", this.prefix + "Equality.6");
        }
        [Test]
        public void EqualityNameOnly()
        {
            Target.User user = "name";
            Expect(user, Is.Not.EqualTo(null), this.prefix + "EqualityNameOnly.0");
            Expect(user != null, "user != null", this.prefix + "EqualityNameOnly.1");
            Expect(user.Name, Is.EqualTo("name"), this.prefix + "EqualityNameOnly.2");
            Expect(user.Password, Is.EqualTo(null), this.prefix + "EqualityNameOnly.3");
            Expect((string)user, Is.EqualTo("name"), this.prefix + "EqualityNameOnly.4");
            Expect(user == "name", "user == \"name\"", this.prefix + "EqualityNameOnly.5");
            Expect(user != "name:", "user == \"name:\"", this.prefix + "EqualityNameOnly.6");
            Expect(user != "name:password", "user != \"name:password\"", this.prefix + "EqualityNameOnly.7");
        }
        [Test]
        public void EqualityPasswordOnly()
        {
            Target.User user = ":password";
            Expect(user, Is.Not.EqualTo(null), this.prefix + "EqualityPasswordOnly.0");
            Expect(user != null, "user != null", this.prefix + "EqualityPasswordOnly.1");
            Expect(user.Name, Is.EqualTo(""), this.prefix + "EqualityPasswordOnly.2");
            Expect(user.Password, Is.EqualTo("password"), this.prefix + "EqualityPasswordOnly.3");
            Expect((string)user, Is.EqualTo(":password"), this.prefix + "EqualityPasswordOnly.4");
            Expect(user == ":password", "user == \":password\"", this.prefix + "EqualityPasswordOnly.5");
            Expect(user != "name:password", "user != \"name:password\"", this.prefix + "EqualityPasswordOnly.6");
        }
    }
}
