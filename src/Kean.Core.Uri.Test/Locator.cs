// 
//  Locator.cs
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
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Core.Uri;

namespace Kean.Core.Uri.Test
{
    [TestFixture]
    public class Locator :
        Kean.Test.Fixture<Locator>
    {
        string prefix = "Kean.Core.Uri.Test.Locator.";
        protected override void Run()
        {
            this.Run(
                this.EqualityNull,
                this.Equality,
                this.PathAbsolute,
				this.PathAbsoluteWithoutResource,
				this.PathRelative,
				this.NoPath,
                this.NoPathWithQuery,
                this.RootPathWithQuery,
                this.NoPathWithQueryAndFragment,
                this.NoPathAndQueryWithFragment,
			    this.FromPlattformPath,
				this.Space,
				this.Space2);
        }
        [Test]
        public void EqualityNull()
        {
            Target.Locator locator = null;
            Expect(locator, Is.EqualTo(null), this.prefix + "EqualityNull.0");
            Expect(locator == null, "locator == null", this.prefix + "EqualityNull.1");
        }
        [Test]
        public void Equality()
        {
            Target.Locator locator = "schemeA+schemeB://name:password@example.com:80/folderA/folderB/file.extension?keyA=valueA&keyB=valueB#fragment";
            Expect(locator, Is.Not.EqualTo(null), this.prefix + "Equality.0");
            Expect(locator != null, "locator != null", this.prefix + "Equality.1");
            Expect((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "Equality.2");
            Expect((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "Equality.3");
            Expect((string)locator.Path, Is.EqualTo("folderA/folderB/file.extension"), this.prefix + "Equality.4");
            Expect((string)locator.Query, Is.EqualTo("keyA=valueA&keyB=valueB"), this.prefix + "Equality.5");
            Expect((string)locator.Fragment, Is.EqualTo("fragment"), this.prefix + "Equality.6");
            Expect((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/folderA/folderB/file.extension?keyA=valueA&keyB=valueB#fragment"), this.prefix + "Equality.7");
            Expect(locator == "schemeA+schemeB://name:password@example.com:80/folderA/folderB/file.extension?keyA=valueA&keyB=valueB#fragment", "locator == \"schemeA+schemeB://name:password@example.com:80/folderA/folderB/file.extension?keyA=valueA&keyB=valueB#fragment\"", this.prefix + "Equality.8");
        }
		[Test]
		public void Space()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80/folder+a/folder+b/file+0.extension?key+a=value+a&keyB=valueB#fragment+0";
			Expect(locator, Is.Not.EqualTo(null), this.prefix + "Space.0");
			Expect(locator != null, "locator != null", this.prefix + "Space.1");
			Expect((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "Space.2");
			Expect((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "Space.3");
			Expect((string)locator.Path, Is.EqualTo("folder a/folder b/file 0.extension"), this.prefix + "Space.4");
			Expect((string)locator.Query, Is.EqualTo("key+a=value+a&keyB=valueB"), this.prefix + "Space.5");
			Expect((string)locator.Fragment, Is.EqualTo("fragment 0"), this.prefix + "Space.6");
			Expect((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/folder+a/folder+b/file+0.extension?key+a=value+a&keyB=valueB#fragment+0"), this.prefix + "Space.7");
			Expect(locator == "schemeA+schemeB://name:password@example.com:80/folder+a/folder+b/file+0.extension?key+a=value+a&keyB=valueB#fragment+0", "locator == \"schemeA+schemeB://name:password@example.com:80/folder+a/folder+b/file+0.extension?key+a=value+a&keyB=valueB#fragment+0\"", this.prefix + "Space.8");
		}
		[Test]
		public void Space2()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80/folder a/folder b/file 0.extension?key a=value a&keyB=valueB#fragment 0";
			Expect(locator, Is.Not.EqualTo(null), this.prefix + "Space2.0");
			Expect(locator != null, "locator != null", this.prefix + "Space2.1");
			Expect((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "Space2.2");
			Expect((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "Space2.3");
			Expect((string)locator.Path, Is.EqualTo("folder a/folder b/file 0.extension"), this.prefix + "Space2.4");
			Expect((string)locator.Query, Is.EqualTo("key+a=value+a&keyB=valueB"), this.prefix + "Space2.5");
			Expect((string)locator.Fragment, Is.EqualTo("fragment 0"), this.prefix + "Space2.6");
			Expect((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/folder+a/folder+b/file+0.extension?key+a=value+a&keyB=valueB#fragment+0"), this.prefix + "Space2.7");
			Expect(locator == "schemeA+schemeB://name:password@example.com:80/folder a/folder b/file+0.extension?key a=value a&keyB=valueB#fragment 0", "locator == \"schemeA+schemeB://name:password@example.com:80/folder a/folder b/file 0.extension?key a=value a&keyB=valueB#fragment 0\"", this.prefix + "Space2.8");
		}
		[Test]
        public void PathAbsolute()
        {
            Target.Locator locator = "/folderA/folderB/file.extension";
            Expect((string)locator.Scheme, Is.EqualTo(null), this.prefix + "PathAbsolute.0");
            Expect((string)locator.Authority, Is.EqualTo(null), this.prefix + "PathAbsolute.1");
            Expect((string)locator.Path, Is.EqualTo("folderA/folderB/file.extension"), this.prefix + "PathAbsolute.2");
            Expect((string)locator.Query, Is.EqualTo(null), this.prefix + "PathAbsolute.3");
            Expect((string)locator.Fragment, Is.EqualTo(null), this.prefix + "PathAbsolute.4");
            Expect((string)locator, Is.EqualTo("/folderA/folderB/file.extension"), this.prefix + "PathAbsolute.5");
            Expect(locator == "/folderA/folderB/file.extension", "locator == \"/folderA/folderB/file.extension\"", this.prefix + "PathAbsolute.6");
        }
		[Test]
		public void PathAbsoluteWithoutResource()
		{
			Target.Locator locator = "/folderA/folderB/";
			Expect((string)locator.Scheme, Is.EqualTo(null), this.prefix + "PathAbsoluteWithoutResource.0");
			Expect((string)locator.Authority, Is.EqualTo(null), this.prefix + "PathAbsoluteWithoutResource.1");
			Expect((string)locator.Path, Is.EqualTo("folderA/folderB/"), this.prefix + "PathAbsoluteWithoutResource.2");
			Expect((string)locator.Query, Is.EqualTo(null), this.prefix + "PathAbsoluteWithoutResource.3");
			Expect((string)locator.Fragment, Is.EqualTo(null), this.prefix + "PathAbsoluteWithoutResource.4");
			Expect((string)locator, Is.EqualTo("/folderA/folderB/"), this.prefix + "PathAbsoluteWithoutResource.5");
			Expect(locator == "/folderA/folderB/", "locator == \"/folderA/folderB/\"", this.prefix + "PathAbsoluteWithoutResource.6");
		}
		[Test]
		public void PathRelative()
		{
			Target.Locator locator = "./folderA/folderB/file.extension";
			Expect((string)locator.Scheme, Is.EqualTo(null), this.prefix + "PathRelative.0");
			Expect((string)locator.Authority, Is.EqualTo("."), this.prefix + "PathRelative.1");
			Expect((string)locator.Path, Is.EqualTo("folderA/folderB/file.extension"), this.prefix + "PathRelative.2");
			Expect((string)locator.Query, Is.EqualTo(null), this.prefix + "PathRelative.3");
			Expect((string)locator.Fragment, Is.EqualTo(null), this.prefix + "PathRelative.4");
			Expect((string)locator, Is.EqualTo("./folderA/folderB/file.extension"), this.prefix + "PathRelative.5");
			Expect(locator == "./folderA/folderB/file.extension", "locator == \"./folderA/folderB/file.extension\"", this.prefix + "PathRelative.6");
		}
		[Test]
        public void NoPath()
        {
            Target.Locator locator = "schemeA+schemeB://name:password@example.com:80";
            Expect((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "NoPath.0");
            Expect((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "NoPath.1");
            Expect((string)locator.Path, Is.EqualTo(null), this.prefix + "NoPath.2");
            Expect((string)locator.Query, Is.EqualTo(null), this.prefix + "NoPath.3");
            Expect((string)locator.Fragment, Is.EqualTo(null), this.prefix + "NoPath.4");
            Expect((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/"), this.prefix + "NoPath.5");
            Expect(locator == "schemeA+schemeB://name:password@example.com:80/", "locator == \"schemeA+schemeB://name:password@example.com:80/\"", this.prefix + "NoPath.6");
        }
        [Test]
        public void NoPathWithQuery()
        {
            Target.Locator locator = "schemeA+schemeB://name:password@example.com:80?keyA=valueA&keyB=valueB";
            Expect((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "NoPathWithQuery.0");
            Expect((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "NoPathWithQuery.1");
            Expect((string)locator.Path, Is.EqualTo(null), this.prefix + "NoPathWithQuery.2");
            Expect((string)locator.Query, Is.EqualTo("keyA=valueA&keyB=valueB"), this.prefix + "NoPathWithQuery.3");
            Expect((string)locator.Fragment, Is.EqualTo(null), this.prefix + "NoPathWithQuery.4");
            Expect((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/?keyA=valueA&keyB=valueB"), this.prefix + "NoPathWithQuery.5");
            Expect(locator == "schemeA+schemeB://name:password@example.com:80/?keyA=valueA&keyB=valueB", "locator == \"schemeA+schemeB://name:password@example.com:80/?keyA=valueA&keyB=valueB\"", this.prefix + "NoPathWithQuery.6");
        }
        [Test]
        public void RootPathWithQuery()
        {
            Target.Locator locator = "schemeA+schemeB://name:password@example.com:80/?keyA=valueA&keyB=valueB";
            Expect((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "RootPathWithQuery.0");
            Expect((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "RootPathWithQuery.1");
            Expect((string)locator.Path, Is.EqualTo(null), this.prefix + "RootPathWithQuery.2");
            Expect((string)locator.Query, Is.EqualTo("keyA=valueA&keyB=valueB"), this.prefix + "RootPathWithQuery.3");
            Expect((string)locator.Fragment, Is.EqualTo(null), this.prefix + "RootPathWithQuery.4");
            Expect((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/?keyA=valueA&keyB=valueB"), this.prefix + "RootPathWithQuery.5");
            Expect(locator == "schemeA+schemeB://name:password@example.com:80/?keyA=valueA&keyB=valueB", "locator == \"schemeA+schemeB://name:password@example.com:80/?keyA=valueA&keyB=valueB\"", this.prefix + "RootPathWithQuery.6");
        }
        [Test]
        public void NoPathWithQueryAndFragment()
        {
            Target.Locator locator = "schemeA+schemeB://name:password@example.com:80?keyA=valueA&keyB=valueB#fragment";
            Expect((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "NoPathWithQueryAndFragment.0");
            Expect((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "NoPathWithQueryAndFragment.1");
            Expect((string)locator.Path, Is.EqualTo(null), this.prefix + "NoPathWithQueryAndFragment.2");
            Expect((string)locator.Query, Is.EqualTo("keyA=valueA&keyB=valueB"), this.prefix + "NoPathWithQueryAndFragment3");
            Expect((string)locator.Fragment, Is.EqualTo("fragment"), this.prefix + "NoPathWithQueryAndFragment.4");
            Expect((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/?keyA=valueA&keyB=valueB#fragment"), this.prefix + "NoPathWithQueryAndFragment.5");
            Expect(locator == "schemeA+schemeB://name:password@example.com:80/?keyA=valueA&keyB=valueB#fragment", "locator == \"schemeA+schemeB://name:password@example.com:80/?keyA=valueA&keyB=valueB#fragment\"", this.prefix + "NoPathWithQueryAndFragment.6");
        }
        [Test]
        public void NoPathAndQueryWithFragment()
        {
            Target.Locator locator = "schemeA+schemeB://name:password@example.com:80#fragment";
            Expect((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "NoPathAndQueryWithFragment.0");
            Expect((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "NoPathAndQueryWithFragment.1");
            Expect((string)locator.Path, Is.EqualTo(null), this.prefix + "NoPathAndQueryWithFragment.2");
            Expect((string)locator.Query, Is.EqualTo(null), this.prefix + "NoPathAndQueryWithFragment.3");
            Expect((string)locator.Fragment, Is.EqualTo("fragment"), this.prefix + "NoPathAndQueryWithFragment.4");
            Expect((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/#fragment"), this.prefix + "NoPathAndQueryWithFragment.5");
            Expect(locator == "schemeA+schemeB://name:password@example.com:80/#fragment", "locator == \"schemeA+schemeB://name:password@example.com:80/#fragment\"", this.prefix + "NoPathAndQueryWithFragment.6");
        }
        [Test]
        public void FromPlattformPath()
        {
            Target.Locator locator = Target.Locator.FromPlattformPath("C:\\Windows\\System32\\etc\\hosts");
            Expect((string)locator.Scheme, Is.EqualTo("file"), this.prefix + "FromPlattformPath.0");
            Expect((string)locator.Authority, Is.EqualTo(null), this.prefix + "FromPlattformPath.1");
            Expect((string)locator.Path, Is.EqualTo("C:/Windows/System32/etc/hosts"), this.prefix + "FromPlattformPath.2");
            Expect((string)locator.Query, Is.EqualTo(null), this.prefix + "FromPlattformPath.3");
            Expect((string)locator.Fragment, Is.EqualTo(null), this.prefix + "FromPlattformPath.4");
            Expect((string)locator, Is.EqualTo("file:///C:/Windows/System32/etc/hosts"), this.prefix + "FromPlattformPath.5");
            Expect(locator == "file:///C:/Windows/System32/etc/hosts", "locator == \"file:///C:/Windows/System32/etc/hosts\"", this.prefix + "FromPlattformPath.6");
        }
		[Test]
		public void Relative()
		{
			Target.Locator absolute = "";
			//Target.Locator relative = 
		}
    }
}
