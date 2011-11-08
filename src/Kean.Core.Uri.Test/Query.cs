// 
//  Query.cs
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
using Kean.Core.Collection.Linked.Extension;
using Kean.Core;

namespace Kean.Core.Uri.Test
{
    [TestFixture]
    public class Query :
        Kean.Test.Fixture<Query>
    {
        string prefix = "Kean.Core.Uri.Test.Query.";
        protected override void Run()
        {
            this.Run(this.EqualityNull, this.Equality, this.Find, this.RemoveList, this.RemoveQuery);
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
		[Test]
		public void RemoveList()
		{
			Target.Query query = "keyA=valueA&keyB=valueB&keyC=valueC&keyD=valueD&keyE=valueE&keyA=valueAA";
			Expect(query = query.Remove((KeyValue<string, string> q) => q.Key == "keyA"), Is.EqualTo((Target.Query)"keyB=valueB&keyC=valueC&keyD=valueD&keyE=valueE"), this.prefix + "RemoveList.0");
			Expect(query = query.Remove((KeyValue<string, string> q) => q.Key == "keyE"), Is.EqualTo((Target.Query)"keyB=valueB&keyC=valueC&keyD=valueD"), this.prefix + "RemoveList.1");
			Expect(query = query.Remove((KeyValue<string, string> q) => q.Key == "keyC"), Is.EqualTo((Target.Query)"keyB=valueB&keyD=valueD"), this.prefix + "RemoveList.2");
			Expect(query = query.Remove((KeyValue<string, string> q) => q.Key == "keyF"), Is.EqualTo((Target.Query)"keyB=valueB&keyD=valueD"), this.prefix + "RemoveList.3");
			Expect(query = query.Remove((KeyValue<string, string> q) => q.Key == "keyB"), Is.EqualTo((Target.Query)"keyD=valueD"), this.prefix + "RemoveList.4");
			Expect(query = query.Remove((KeyValue<string, string> q) => q.Key == "keyD"), Is.EqualTo(null), this.prefix + "RemoveList.5");
		}
		[Test]
		public void RemoveQuery()
		{
			Target.Query query = "keyA=valueA&keyB=valueB&keyC=valueC&keyD=valueD&keyE=valueE&keyA=valueAA";
			Expect(query = query.Remove("keyA", "keyE"), Is.EqualTo((Target.Query)"keyB=valueB&keyC=valueC&keyD=valueD"), this.prefix + "RemoveQuery.0");
			Expect(query = query.Keep("keyC"), Is.EqualTo((Target.Query)"keyC=valueC"), this.prefix + "RemoveQuery.1");
		}
    }
}
