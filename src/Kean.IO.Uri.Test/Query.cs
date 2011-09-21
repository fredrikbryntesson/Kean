using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.IO.Uri;
using Kean.Core.Collection.Linked.Extension;
using Kean.Core;

namespace Kean.IO.Uri.Test
{
    [TestFixture]
    public class Query :
        Kean.Test.Fixture<Query>
    {
        string prefix = "Kean.IO.Uri.Test.Query.";
        protected override void Run()
        {
            this.Run(this.EqualityNull, this.Equality, this.Find);
        }
        [Test]
        public void EqualityNull()
        {
            Target.Query query = null;
            Expect(query, Is.EqualTo(null), this.prefix + "EqualityNull.0");
            Expect(query == null, Is.True, this.prefix + "EqualityNull.1");
        }
        [Test]
        public void Equality()
        {
            Target.Query query = "keyA=valueA&keyB=valueB&keyC=valueC";
            Expect(query, Is.Not.EqualTo(null), this.prefix + "Equality.0");
            Expect(query != null, "query != null", this.prefix + "Equality.1");
            Expect((string)query, Is.EqualTo("keyA=valueA&keyB=valueB&keyC=valueC"), this.prefix + "Equality.2");
            Expect(query == "keyA=valueA&keyB=valueB&keyC=valueC", "query == \"keyA=valueA&keyB=valueB&keyC=valueC\"", this.prefix + "Equality.3");
            Expect(query.Head.Key, Is.EqualTo("keyA"), this.prefix + "Equality.4");
            Expect(query.Head.Value, Is.EqualTo("valueA"), this.prefix + "Equality.5");
            Expect((string)query.Tail, Is.EqualTo("keyB=valueB&keyC=valueC"), this.prefix + "Equality.6");
            Expect(query.Tail == "keyB=valueB&keyC=valueC", "query.Tail == \"keyB=valueB&keyC=valueC\"", this.prefix + "Equality.7");
            Expect(query.Tail.Head.Key, Is.EqualTo("keyB"), this.prefix + "Equality.8");
            Expect(query.Tail.Head.Value, Is.EqualTo("valueB"), this.prefix + "Equality.9");
            Expect((string)query.Tail.Tail, Is.EqualTo("keyC=valueC"), this.prefix + "Equality.10");
            Expect(query.Tail.Tail.Tail, Is.EqualTo(null), this.prefix + "Equality.11");
        }
        [Test]
        public void Find()
        {
            Target.Query query = "keyA=valueA&keyB=valueB&keyC=valueC";
            Expect(query.Find((KeyValue<string, string> q) => q.Key == "keyA").Value, Is.EqualTo("valueA"), this.prefix + "Find.0");
            Expect(query.Find((KeyValue<string, string> q) => q.Key == "keyB").Value, Is.EqualTo("valueB"), this.prefix + "Find.1");
            Expect(query.Find((KeyValue<string, string> q) => q.Key == "keyC").Value, Is.EqualTo("valueC"), this.prefix + "Find.2");
        }
    }
}
