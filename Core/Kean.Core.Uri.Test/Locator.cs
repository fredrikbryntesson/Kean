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
				this.FromPlatformPath,
				this.FromNetworkPlatformPath,
				this.SpaceEscaped,
				this.SpaceSpace,
				this.Plus,
				this.RelativeResolve);
		}
		[Test]
		public void EqualityNull()
		{
			Target.Locator locator = null;
			Verify(locator, Is.EqualTo(null), this.prefix + "EqualityNull.0");
			Verify(locator == null, Is.True, this.prefix + "EqualityNull.1");
		}
		[Test]
		public void Equality()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80/folderA/folderB/file.extension?keyA=valueA&keyB=valueB#fragment";
			Verify(locator, Is.Not.EqualTo(null), this.prefix + "Equality.0");
			Verify(locator != null, Is.True, this.prefix + "Equality.1");
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "Equality.2");
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "Equality.3");
			Verify((string)locator.Path, Is.EqualTo("/folderA/folderB/file.extension"), this.prefix + "Equality.4");
			Verify((string)locator.Query, Is.EqualTo("keyA=valueA&keyB=valueB"), this.prefix + "Equality.5");
			Verify((string)locator.Fragment, Is.EqualTo("fragment"), this.prefix + "Equality.6");
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/folderA/folderB/file.extension?keyA=valueA&keyB=valueB#fragment"), this.prefix + "Equality.7");
			Verify(locator == "schemeA+schemeB://name:password@example.com:80/folderA/folderB/file.extension?keyA=valueA&keyB=valueB#fragment", Is.True, this.prefix + "Equality.8");
		}
		[Test]
		public void SpaceEscaped()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80/folder%20a/folder%20b/file%200.extension?key+a=value+a&keyB=valueB#fragment%200";
			Verify(locator, Is.Not.EqualTo(null), this.prefix + "SpacePlus.0");
			Verify(locator != null, Is.True, this.prefix + "SpacePlus.1");
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "SpacePlus.2");
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "SpacePlus.3");
			Verify((string)locator.Path, Is.EqualTo("/folder a/folder b/file 0.extension"), this.prefix + "SpacePlus.4");
			Verify((string)locator.Query, Is.EqualTo("key+a=value+a&keyB=valueB"), this.prefix + "SpacePlus.5");
			Verify((string)locator.Fragment, Is.EqualTo("fragment 0"), this.prefix + "SpacePlus.6");
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/folder%20a/folder%20b/file%200.extension?key+a=value+a&keyB=valueB#fragment%200"), this.prefix + "SpacePlus.7");
			Verify(locator == "schemeA+schemeB://name:password@example.com:80/folder%20a/folder%20b/file%200.extension?key+a=value+a&keyB=valueB#fragment%200", Is.True, this.prefix + "SpacePlus.8");
		}
		[Test]
		public void SpaceSpace()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80/folder a/folder b/file 0.extension?key a=value a&keyB=valueB#fragment 0";
			Verify(locator, Is.Not.EqualTo(null), this.prefix + "SpaceSpace.0");
			Verify(locator != null, Is.True, this.prefix + "SpaceSpace.1");
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "SpaceSpace.2");
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "SpaceSpace.3");
			Verify((string)locator.Path, Is.EqualTo("/folder a/folder b/file 0.extension"), this.prefix + "SpaceSpace.4");
			Verify((string)locator.Query, Is.EqualTo("key+a=value+a&keyB=valueB"), this.prefix + "SpaceSpace.5");
			Verify((string)locator.Fragment, Is.EqualTo("fragment 0"), this.prefix + "SpaceSpace.6");
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/folder%20a/folder%20b/file%200.extension?key+a=value+a&keyB=valueB#fragment%200"), this.prefix + "SpaceSpace.7");
			Verify(locator == "schemeA+schemeB://name:password@example.com:80/folder a/folder b/file 0.extension?key a=value a&keyB=valueB#fragment 0", Is.True, this.prefix + "SpaceSpace.8");
		}
		[Test]
		public void Plus()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80/folder+a/folder+b/file+0.extension?key+a=value+a&keyB=valueB#fragment+0";
			Verify(locator, Is.Not.EqualTo(null), this.prefix + "Plus.0");
			Verify(locator != null, Is.True, this.prefix + "Plus.1");
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "Plus.2");
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "Plus.3");
			Verify((string)locator.Path, Is.EqualTo("/folder+a/folder+b/file+0.extension"), this.prefix + "Plus.4");
			Verify((string)locator.Query, Is.EqualTo("key+a=value+a&keyB=valueB"), this.prefix + "Plus.5");
			Verify((string)locator.Fragment, Is.EqualTo("fragment+0"), this.prefix + "Plus.6");
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/folder+a/folder+b/file+0.extension?key+a=value+a&keyB=valueB#fragment+0"), this.prefix + "Plus.7");
			Verify(locator == "schemeA+schemeB://name:password@example.com:80/folder+a/folder+b/file+0.extension?key a=value a&keyB=valueB#fragment+0", Is.True, this.prefix + "Plus.8");
		}
		[Test]
		public void PathAbsolute()
		{
			Target.Locator locator = "/folderA/folderB/file.extension";
			Verify((string)locator.Scheme, Is.EqualTo(null), this.prefix + "PathAbsolute.0");
			Verify((string)locator.Authority, Is.EqualTo(null), this.prefix + "PathAbsolute.1");
			Verify((string)locator.Path, Is.EqualTo("/folderA/folderB/file.extension"), this.prefix + "PathAbsolute.2");
			Verify((string)locator.Query, Is.EqualTo(""), this.prefix + "PathAbsolute.3");
			Verify((string)locator.Fragment, Is.EqualTo(null), this.prefix + "PathAbsolute.4");
			Verify((string)locator, Is.EqualTo("/folderA/folderB/file.extension"), this.prefix + "PathAbsolute.5");
			Verify(locator == "/folderA/folderB/file.extension",Is.True, this.prefix + "PathAbsolute.6");
		}
		[Test]
		public void PathAbsoluteWithoutResource()
		{
			Target.Locator locator = "/folderA/folderB/";
			Verify((string)locator.Scheme, Is.EqualTo(null), this.prefix + "PathAbsoluteWithoutResource.0");
			Verify((string)locator.Authority, Is.EqualTo(null), this.prefix + "PathAbsoluteWithoutResource.1");
			Verify((string)locator.Path, Is.EqualTo("/folderA/folderB/"), this.prefix + "PathAbsoluteWithoutResource.2");
			Verify((string)locator.Query, Is.EqualTo(""), this.prefix + "PathAbsoluteWithoutResource.3");
			Verify((string)locator.Fragment, Is.EqualTo(null), this.prefix + "PathAbsoluteWithoutResource.4");
			Verify((string)locator, Is.EqualTo("/folderA/folderB/"), this.prefix + "PathAbsoluteWithoutResource.5");
			Verify(locator == "/folderA/folderB/", Is.True, this.prefix + "PathAbsoluteWithoutResource.6");
		}
		[Test]
		public void PathRelative()
		{
			Target.Locator locator = "./folderA/folderB/file.extension";
			Verify((string)locator.Scheme, Is.EqualTo(null), this.prefix + "PathRelative.0");
			Verify((string)locator.Authority, Is.EqualTo(null), this.prefix + "PathRelative.1");
			Verify((string)locator.Path, Is.EqualTo("./folderA/folderB/file.extension"), this.prefix + "PathRelative.2");
			Verify((string)locator.Query, Is.EqualTo(""), this.prefix + "PathRelative.3");
			Verify((string)locator.Fragment, Is.EqualTo(null), this.prefix + "PathRelative.4");
			Verify((string)locator, Is.EqualTo("./folderA/folderB/file.extension"), this.prefix + "PathRelative.5");
			Verify(locator == "./folderA/folderB/file.extension", Is.True, this.prefix + "PathRelative.6");
		}
		[Test]
		public void NoPath()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80";
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "NoPath.0");
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "NoPath.1");
			Verify((string)locator.Path, Is.EqualTo(""), this.prefix + "NoPath.2");
			Verify((string)locator.Query, Is.EqualTo(""), this.prefix + "NoPath.3");
			Verify((string)locator.Fragment, Is.EqualTo(null), this.prefix + "NoPath.4");
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80"), this.prefix + "NoPath.5");
			Verify(locator == "schemeA+schemeB://name:password@example.com:80", Is.True, this.prefix + "NoPath.6");
		}
		[Test]
		public void NoPathWithQuery()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80?keyA=valueA&keyB=valueB";
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "NoPathWithQuery.0");
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "NoPathWithQuery.1");
			Verify((string)locator.Path, Is.EqualTo(""), this.prefix + "NoPathWithQuery.2");
			Verify((string)locator.Query, Is.EqualTo("keyA=valueA&keyB=valueB"), this.prefix + "NoPathWithQuery.3");
			Verify((string)locator.Fragment, Is.EqualTo(null), this.prefix + "NoPathWithQuery.4");
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80?keyA=valueA&keyB=valueB"), this.prefix + "NoPathWithQuery.5");
			Verify(locator == "schemeA+schemeB://name:password@example.com:80?keyA=valueA&keyB=valueB", Is.True, this.prefix + "NoPathWithQuery.6");
		}
		[Test]
		public void RootPathWithQuery()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80/?keyA=valueA&keyB=valueB";
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "RootPathWithQuery.0");
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "RootPathWithQuery.1");
			Verify((string)locator.Path, Is.EqualTo("/"), this.prefix + "RootPathWithQuery.2");
			Verify((string)locator.Query, Is.EqualTo("keyA=valueA&keyB=valueB"), this.prefix + "RootPathWithQuery.3");
			Verify((string)locator.Fragment, Is.EqualTo(null), this.prefix + "RootPathWithQuery.4");
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/?keyA=valueA&keyB=valueB"), this.prefix + "RootPathWithQuery.5");
			Verify(locator == "schemeA+schemeB://name:password@example.com:80/?keyA=valueA&keyB=valueB", Is.True, this.prefix + "RootPathWithQuery.6");
		}
		[Test]
		public void NoPathWithQueryAndFragment()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80?keyA=valueA&keyB=valueB#fragment";
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "NoPathWithQueryAndFragment.0");
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "NoPathWithQueryAndFragment.1");
			Verify((string)locator.Path, Is.EqualTo(""), this.prefix + "NoPathWithQueryAndFragment.2");
			Verify((string)locator.Query, Is.EqualTo("keyA=valueA&keyB=valueB"), this.prefix + "NoPathWithQueryAndFragment3");
			Verify((string)locator.Fragment, Is.EqualTo("fragment"), this.prefix + "NoPathWithQueryAndFragment.4");
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80?keyA=valueA&keyB=valueB#fragment"), this.prefix + "NoPathWithQueryAndFragment.5");
			Verify(locator == "schemeA+schemeB://name:password@example.com:80?keyA=valueA&keyB=valueB#fragment", Is.True, this.prefix + "NoPathWithQueryAndFragment.6");
		}
		[Test]
		public void NoPathAndQueryWithFragment()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80#fragment";
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"), this.prefix + "NoPathAndQueryWithFragment.0");
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"), this.prefix + "NoPathAndQueryWithFragment.1");
			Verify((string)locator.Path, Is.EqualTo(""), this.prefix + "NoPathAndQueryWithFragment.2");
			Verify((string)locator.Query, Is.EqualTo(""), this.prefix + "NoPathAndQueryWithFragment.3");
			Verify((string)locator.Fragment, Is.EqualTo("fragment"), this.prefix + "NoPathAndQueryWithFragment.4");
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80#fragment"), this.prefix + "NoPathAndQueryWithFragment.5");
			Verify(locator == "schemeA+schemeB://name:password@example.com:80#fragment", Is.True, this.prefix + "NoPathAndQueryWithFragment.6");
		}
		[Test]
		public void FromPlatformPath()
		{
			string platformPath = this.OperatingSystem == Kean.Test.OperatingSystem.Windows ? "C:\\Windows\\System32\\etc\\hosts" : "/C:/Windows/System32/etc/hosts"; 
			Target.Locator locator = Target.Locator.FromPlatformPath(platformPath);
			Verify((string)locator.Scheme, Is.EqualTo("file"), this.prefix + "FromPlatformPath.0");
			Verify((string)locator.Authority, Is.EqualTo(null), this.prefix + "FromPlatformPath.1");
			Verify((string)locator.Path, Is.EqualTo("/C:/Windows/System32/etc/hosts"), this.prefix + "FromPlatformPath.2");
			Verify((string)locator.Query, Is.EqualTo(""), this.prefix + "FromPlatformPath.3");
			Verify((string)locator.Fragment, Is.EqualTo(null), this.prefix + "FromPlatformPath.4");
			Verify((string)locator, Is.EqualTo("file:///C:/Windows/System32/etc/hosts"), this.prefix + "FromPlatformPath.5");
			Verify(locator == "file:///C:/Windows/System32/etc/hosts", Is.True, this.prefix + "FromPlatformPath.6");
			Verify((string)locator.PlatformPath, Is.EqualTo(platformPath), this.prefix + "FromPlatformPath.7");
			
		}
		[Test]
		public void FromNetworkPlatformPath()
		{
			string platformPath = this.OperatingSystem == Kean.Test.OperatingSystem.Windows ? "\\\\SERVER\\Windows\\System32\\etc\\hosts" : "/C:/Windows/System32/etc/hosts";
			Target.Locator locator = Target.Locator.FromPlatformPath(platformPath);
			Verify((string)locator.Scheme, Is.EqualTo("file"));
			Verify((string)locator.Authority, Is.EqualTo("server"));
			Verify((string)locator.Path, Is.EqualTo("/Windows/System32/etc/hosts"));
			Verify((string)locator.Query, Is.EqualTo(""));
			Verify((string)locator.Fragment, Is.EqualTo(null));
			Verify((string)locator, Is.EqualTo("file://server/Windows/System32/etc/hosts"));
			Verify(locator == "file://server/Windows/System32/etc/hosts", Is.True);
			Verify((string)locator.PlatformPath, Is.EqualTo(platformPath));

		}
		[Test]
		public void RelativeResolve()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80/folder+a/folder+b/file+0.extension?key+a=value+a&keyB=valueB#fragment+0";
			Target.Locator file = "schemeA+schemeB://name:password@example.com:80/folder+a/folder+b/file+0.extension?key+a=value+a&keyB=valueB";
			Target.Locator relative = locator.Relative(file);
			Verify(relative == "#fragment+0",Is.True, this.prefix + "RelativeResolve.0");
			Target.Locator resolve = relative.Resolve(file);
			Verify(resolve == locator, Is.True, this.prefix + "RelativeResolve.1");
		}
	}
}
