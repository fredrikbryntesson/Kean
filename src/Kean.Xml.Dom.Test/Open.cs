// 
//  Open.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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
using Kean.Core;
using Kean.Core.Extension;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
namespace Kean.Xml.Dom.Test
{
	[TestFixture]
	public class Open :
		AssertionHelper
	{
		[Test]
		public void Valid001()
		{
			Document opened = Document.OpenResource("Data/valid001.xml");
			Document created = new Document() { Root = new Element("empty") };
			Expect(opened, Is.EqualTo(created));
		}
		[Test]
		public void Valid002()
		{
			Document opened = Document.OpenResource("Data/valid002.xml");
			Document created = new Document() { Root = new Element("empty") };
			Expect(opened, Is.EqualTo(created));
		}
		[Test]
		public void Valid003()
		{
			Document opened = Document.OpenResource("Data/valid003.xml");
			Document created = new Document() { Root = new Element("text") };
			created.Root.Add(new Text("Text"));
			Expect(opened, Is.EqualTo(created));
		}

		public void Run()
		{
			this.Run(
				this.Valid001,
				this.Valid002
			);
		}
		void Run(params System.Action[] tests)
		{
			foreach (System.Action test in tests)
				if (test.NotNull())
					test();
		}
		public static void Test()
		{
			Open fixture = new Open();
			fixture.Run();
		}
	}
}

