using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.IO.Uri;

namespace Kean.IO.Uri.Test
{
    [TestFixture]
    public class Locator :
        Kean.Test.Fixture<Locator>
    {
        string prefix = "Kean.IO.Uri.Test.Locator.0";
        protected override void Run()
        {
            this.Run(
                this.EqualityNull,
                this.Equality,
                this.PathAbsolute,
                this.NoPath,
                this.NoPathWithQuery,
                this.RootPathWithQuery,
                this.NoPathWithQueryAndFragment,
                this.NoPathAndQueryWithFragment,
			    this.FromPlattformPath);
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
    }
}
