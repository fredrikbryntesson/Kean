// 
//  Path.cs
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
using NUnit.Framework;
using Target = Kean.Core.Uri;

namespace Kean.Core.Uri.Test
{
    [TestFixture]
    public class Path :
        Kean.Test.Fixture<Path>
    {
        string prefix = "Kean.Core.Uri.Test.Path.";
        protected override void Run()
        {
            this.Run(
                this.EqualityNull,
                this.Equality,
				this.Space,
				this.Plus,
				this.Hash
                );
        }
        [Test]
        public void EqualityNull()
        {
            Target.Path path = null;
            Expect(path, Is.EqualTo(null), this.prefix + "EqualityNull.0");
            Expect(path == null, "path == null", this.prefix + "EqualityNull.1");
        }
        [Test]
        public void Equality()
        {
            Target.Path path = "folderA/folderB/file.extension";
            Expect(path, Is.Not.EqualTo(null), this.prefix + "Equality.0");
            Expect(path != null, "path != null", this.prefix + "Equality.1");
            Expect((string)path, Is.EqualTo("/folderA/folderB/file.extension"), this.prefix + "Equality.2");
            Expect(path == "folderA/folderB/file.extension", "path == \"folderA/folderB/file.extension\"", this.prefix + "Equality.3");
        }
		[Test]
		public void Space()
		{
			Target.Path path = "folder A/folder B/file C.extension";
			Expect(path, Is.Not.EqualTo(null), this.prefix + "Space.0");
			Expect(path != null, "path != null", this.prefix + "Space.1");
			Expect((string)path, Is.EqualTo("/folder A/folder B/file C.extension"), this.prefix + "Space.2");
			Expect(path == "folder A/folder B/file C.extension", "path == \"folder A/folder B/file C.extension\"", this.prefix + "Space.3");
		}
		[Test]
		public void Plus()
		{
			Target.Path path = "folder+A/folder+B/file+C.extension";
			Expect(path, Is.Not.EqualTo(null), this.prefix + "Plus.0");
			Expect(path != null, "path != null", this.prefix + "Plus.1");
			Expect((string)path, Is.EqualTo("/folder+A/folder+B/file+C.extension"), this.prefix + "Plus.2");
			Expect(path == "folder+A/folder+B/file+C.extension", "path == \"folder+A/folder+B/file+C.extension\"", this.prefix + "Plus.3");
		}
		[Test]
		public void Hash()
		{
			Target.Path path = "folderA/folderB/file.extension";
			Target.Path path2 = "folderA/folderB/file.extension2";
			Expect(path, Is.Not.EqualTo(path2), this.prefix + "Hash.0");
			Expect(path.GetHashCode(), Is.Not.EqualTo(path2.GetHashCode()), this.prefix + "Hash.1");
 
		}
	}
}
