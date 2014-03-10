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

using Target = Kean.Uri;

namespace Kean.Uri.Test
{
    [TestFixture]
    public class User :
        Kean.Test.Fixture<User>
    {
        string prefix = "Kean.Uri.Test.";
        protected override void Run()
        {
            this.Run(this.EqualityNull, this.Equality, this.EqualityNameOnly, this.EqualityPasswordOnly);
        }
        [Test]
        public void EqualityNull()
        {
            Target.User user = null;
            Verify(user, Is.EqualTo(null), this.prefix + "EqualityNull.0");
			Verify(user == null, Is.True, this.prefix + "EqualityNull.1");
        }
        [Test]
        public void Equality()
        {
            Target.User user = "name:password";
            Verify(user, Is.Not.EqualTo(null), this.prefix + "Equality.0");
			Verify(user != null, Is.True, this.prefix + "Equality.1");
            Verify(user.Name, Is.EqualTo("name"), this.prefix + "Equality.2");
            Verify(user.Password, Is.EqualTo("password"), this.prefix + "Equality.3");
            Verify((string)user, Is.EqualTo("name:password"), this.prefix + "Equality.4");
			Verify(user == "name:password", Is.True, this.prefix + "Equality.5");
			Verify(user != "name", Is.True, this.prefix + "Equality.6");
        }
        [Test]
        public void EqualityNameOnly()
        {
            Target.User user = "name";
            Verify(user, Is.Not.EqualTo(null), this.prefix + "EqualityNameOnly.0");
			Verify(user != null, Is.True, this.prefix + "EqualityNameOnly.1");
            Verify(user.Name, Is.EqualTo("name"), this.prefix + "EqualityNameOnly.2");
            Verify(user.Password, Is.EqualTo(null), this.prefix + "EqualityNameOnly.3");
            Verify((string)user, Is.EqualTo("name"), this.prefix + "EqualityNameOnly.4");
			Verify(user == "name", Is.True, this.prefix + "EqualityNameOnly.5");
			Verify(user != "name:", Is.True, this.prefix + "EqualityNameOnly.6");
			Verify(user != "name:password", Is.True, this.prefix + "EqualityNameOnly.7");
        }
        [Test]
        public void EqualityPasswordOnly()
        {
            Target.User user = ":password";
            Verify(user, Is.Not.EqualTo(null), this.prefix + "EqualityPasswordOnly.0");
			Verify(user != null, Is.True, this.prefix + "EqualityPasswordOnly.1");
            Verify(user.Name, Is.EqualTo(""), this.prefix + "EqualityPasswordOnly.2");
            Verify(user.Password, Is.EqualTo("password"), this.prefix + "EqualityPasswordOnly.3");
            Verify((string)user, Is.EqualTo(":password"), this.prefix + "EqualityPasswordOnly.4");
            Verify(user == ":password", Is.True, this.prefix + "EqualityPasswordOnly.5");
			Verify(user != "name:password", Is.True, this.prefix + "EqualityPasswordOnly.6");
        }
    }
}
