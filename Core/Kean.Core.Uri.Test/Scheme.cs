// 
//  Scheme.cs
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
    public class Scheme :
        Kean.Test.Fixture<Scheme>
    {
        string prefix = "Kean.Core.Uri.Test.Scheme.";
        protected override void Run()
        {
            this.Run(this.EqualityNull, this.Equality);
        }
        [Test]
        public void EqualityNull()
        {
            Target.Scheme scheme = null;
            Verify(scheme, Is.EqualTo(null), this.prefix + "EqualityNull.0");
            Verify(scheme == null, Is.True, this.prefix + "EqualityNull.1");
        }
        [Test]
        public void Equality()
        {
            Target.Scheme scheme = "svn+ssh";
            Verify(scheme, Is.Not.EqualTo(null), this.prefix + "Equality.0");
			Verify(scheme != null, Is.True , this.prefix + "Equality.1");
            Verify((string)scheme, Is.EqualTo("svn+ssh"), this.prefix + "Equality.2");
			Verify(scheme == "svn+ssh", Is.True, this.prefix + "Equality.3");
            Verify(scheme.Head, Is.EqualTo("svn"), this.prefix + "Equality.4");
            Verify((string)scheme.Tail, Is.EqualTo("ssh"), this.prefix + "Equality.5");
			Verify(scheme.Tail == "ssh", Is.True, this.prefix + "Equality.6");
            Verify(scheme.Tail.Head, Is.EqualTo("ssh"), this.prefix + "Equality.7");
            Verify(scheme.Tail.Tail, Is.EqualTo(null), this.prefix + "Equality.8");
        }
    }
}
