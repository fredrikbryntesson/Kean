using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
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
                this.Equality
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
            Expect((string)path, Is.EqualTo("folderA/folderB/file.extension"), this.prefix + "Equality.2");
            Expect(path == "folderA/folderB/file.extension", "path == \"folderA/folderB/file.extension\"", this.prefix + "Equality.3");
            Expect(path.Head, Is.EqualTo("folderA"), this.prefix + "Equality.4");
            Expect((string)path.Tail, Is.EqualTo("folderB/file.extension"), this.prefix + "Equality.5");
            Expect(path.Tail == "folderB/file.extension", "path.Tail == \"folderB/file.extension\"", this.prefix + "Equality.6");
            Expect(path.Tail.Head, Is.EqualTo("folderB"), this.prefix + "Equality.7");
            Expect((string)path.Tail.Tail, Is.EqualTo("file.extension"), this.prefix + "Equality.8");
            Expect(path.Tail.Tail.Tail, Is.EqualTo(null), this.prefix + "Equality.9");
        }
	}
}
