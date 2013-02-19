// 
//  Authority.cs
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
    public class Authority : 
        Kean.Test.Fixture<Authority>
    {
        string prefix = "Kean.Core.Uri.Test.Authority.0";
        protected override void Run()
        {
            this.Run(
            this.EqualityNull,
            this.Equality,
            this.EqualityUserOnly,
            this.EqualityEndpointOnly);
        }
        [Test]
        public void EqualityNull()
        {
            Target.Authority authority = null;
            Expect(authority, Is.EqualTo(null), this.prefix + "EqualityNull.0");
            Expect(authority == null, "authority == null", this.prefix + "EqualityNull.1");
        }
        [Test]
        public void Equality()
        {
            Target.Authority authority = "name:password@example.com:80";
            Expect(authority, Is.Not.EqualTo(null), this.prefix + "Equality.0");
            Expect(authority != null, "authority != null", this.prefix + "Equality.1");
            Expect((string)authority.User, Is.EqualTo("name:password"), this.prefix + "Equality.2");
            Expect((string)authority.Endpoint, Is.EqualTo("example.com:80"), this.prefix + "Equality.3");
            Expect((string)authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "Equality.4");
            Expect(authority == "name:password@example.com:80", "authority == \"name:password@example.com:80\"", this.prefix + "Equality.5");
            Expect(authority != "name:password", "authority != \"name:password\"", this.prefix + "Equality.6");
            Expect(authority != "example.com:80", "authority != \"example.com:80\"", this.prefix + "Equality.7");
        }
        [Test]
        public void EqualityUserOnly()
        {
            Target.Authority authority = "name:password@";
            Expect(authority, Is.Not.EqualTo(null), this.prefix + "EqualityUserOnly.0");
            Expect(authority != null, "authority != null", this.prefix + "EqualityUserOnly.1");
            Expect((string)authority.User, Is.EqualTo("name:password"), this.prefix + "EqualityUserOnly.2");
            Expect(authority.Endpoint, Is.EqualTo(null), this.prefix + "EqualityUserOnly.3");
            Expect((string)authority, Is.EqualTo("name:password@"), this.prefix + "EqualityUserOnly.4");
            Expect(authority == "name:password@", "authority == \"name:password@\"", this.prefix + "EqualityUserOnly.5");
            Expect(authority != "name:password", "authority == \"name:password\"", this.prefix + "EqualityUserOnly.6");
            Expect(authority != "", "authority != \"\"", this.prefix + "EqualityUserOnly.7");
        }
        [Test]
        public void EqualityEndpointOnly()
        {
            Target.Authority authority = "example.com:80";
            Expect(authority, Is.Not.EqualTo(null), this.prefix + "EqualityEndpointOnly.0");
            Expect(authority != null, "authority != null", this.prefix + "EqualityEndpointOnly.1");
            Expect(authority.User, Is.EqualTo(null), this.prefix + "EqualityEndpointOnly.2");
            Expect((string)authority.Endpoint, Is.EqualTo("example.com:80"), this.prefix + "EqualityEndpointOnly.3");
            Expect((string)authority, Is.EqualTo("example.com:80"), this.prefix + "EqualityEndpointOnly.4");
            Expect(authority == "@example.com:80", "authority == \"@example.com:80\"", this.prefix + "EqualityEndpointOnly.5");
            Expect(authority != "name:password@example.com:80", "authority != \"name:password@example.com:80\"", this.prefix + "EqualityEndpointOnly.6");
        }
    }
}
