// 
//  User.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2011 Simon Mika
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using NUnit.Framework;

using Target = Kean.Core.Uri;

namespace Kean.Core.Uri.Test
{
    [TestFixture]
    public class User :
        Kean.Test.Fixture<User>
    {
        string prefix = "Kean.Core.Uri.Test.";
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
