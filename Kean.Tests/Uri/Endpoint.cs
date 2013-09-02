// 
//  Endpoint.cs
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
    public class Endpoint :
        Kean.Test.Fixture<Endpoint>
    {
        string prefix = "Kean.Uri.Test.Endpoint.";
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
            Verify(endpoint, Is.EqualTo(null), this.prefix + "EqualityNull.0");
            Verify(endpoint == null, Is.True , this.prefix + "EqualityNull.1");
        }
        [Test]
        public void Equality()
        {
            Target.Endpoint endpoint = "www.example.com:80";
            Verify(endpoint, Is.Not.EqualTo(null), this.prefix + "Equality.0");
            Verify(endpoint != null, Is.True, this.prefix + "Equality.1");
            Verify((string)endpoint.Host, Is.EqualTo("www.example.com"), this.prefix + "Equality.2");
            Verify(endpoint.Port, Is.EqualTo(80), this.prefix + "Equality.3");
            Verify((string)endpoint, Is.EqualTo("www.example.com:80"), this.prefix + "Equality.4");
            Verify(endpoint == "www.example.com:80", Is.True, this.prefix + "Equality.5");
			Verify(endpoint != "www.example.com", Is.True, this.prefix + "Equality.6");
        }
        [Test]
        public void EqualityHostOnly()
        {
            Target.Endpoint endpoint = "www.example.com";
            Verify(endpoint, Is.Not.EqualTo(null), this.prefix + "EqualityHostOnly.0");
			Verify(endpoint != null, Is.True, this.prefix + "EqualityHostOnly.1");
            Verify((string)endpoint.Host, Is.EqualTo("www.example.com"), this.prefix + "EqualityHostOnly.2");
            Verify(endpoint.Port, Is.EqualTo(null), this.prefix + "EqualityHostOnly.3");
            Verify((string)endpoint, Is.EqualTo("www.example.com"), this.prefix + "EqualityHostOnly.4");
			Verify(endpoint == "www.example.com", Is.True, this.prefix + "EqualityHostOnly.5");
			Verify(endpoint == "www.example.com:", Is.True, this.prefix + "EqualityHostOnly.6");
			Verify(endpoint != "www.example.com:80", Is.True, this.prefix + "EqualityHostOnly.7");
        }
        [Test]
        public void EqualityPortOnly()
        {
            Target.Endpoint endpoint = ":80";
            Verify(endpoint, Is.Not.EqualTo(null), this.prefix + "EqualityPortOnly.0");
			Verify(endpoint != null, Is.True, this.prefix + "EqualityPortOnly.1");
            Verify((string)endpoint.Host, Is.EqualTo(""), this.prefix + "EqualityPortOnly.2");
            Verify(endpoint.Port, Is.EqualTo(80), this.prefix + "EqualityPortOnly.3");
            Verify((string)endpoint, Is.EqualTo(":80"), this.prefix + "EqualityPortOnly.4");
			Verify(endpoint == ":80", Is.True, this.prefix + "EqualityPortOnly.5");
			Verify(endpoint != "wwww.example.com:80", Is.True, this.prefix + "EqualityPortOnly.6");
        }
    }
}
