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
        Kean.Test.Fixture<Open>
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
        [Test]
        public void Valid004()
        {
            Document opened = Document.OpenResource("Data/valid004.xml");
            Document created = new Document() { Root = new Element("library") };
            Element book = new Element("book");
            Element title = new Element("title");
            title.Add(new Text("Write with XML"));
            book.Add(title);
            created.Root.Add(book);
            Expect(opened, Is.EqualTo(created));
        }
        [Test]
        public void Valid005()
        {
            Document opened = Document.OpenResource("Data/valid005.xml");
            Document created = new Document() { Root = new Element("library") };
            Element book = new Element("book");
            book.Attributes.Add(new Attribute("language", "english"));
            Element title = new Element("title");
            title.Attributes.Add(new Attribute("location", "office"));
            title.Add(new Text("Write with XML"));
            book.Add(title);
            created.Root.Add(book);
            Expect(opened, Is.EqualTo(created));
        }
        [Test]
        public void Valid006()
        {
            Document opened = Document.OpenResource("Data/valid006.xml");
            Document created = new Document() { Root = new Element("library") };
            Comment comment = new Comment("New Book");
            created.Root.Add(comment);
            Element book = new Element("book");
            book.Add(new Text("Write with XML"));
            created.Root.Add(book);
            Expect(opened, Is.EqualTo(created));
        }
        [Test]
        public void Valid007()
        {
            Document opened = Document.OpenResource("Data/valid007.xml");
            Document created = new Document() { Root = new Element("data") };
            Data data = new Data("Example");
            created.Root.Add(data);
            Expect(opened, Is.EqualTo(created));
        }
        [Test]
        public void Valid008()
        {
            Document opened = Document.OpenResource("Data/valid008.xml");
            Document created = new Document() { Root = new Element("instruction") };
            ProcessingInstruction instruction = new ProcessingInstruction("target", "Try this!");
            created.Root.Add(instruction);
            Expect(opened, Is.EqualTo(created));
        }
        protected override void Run()
        {
            this.Run(
                this.Valid001,
                this.Valid002,
                this.Valid003,
                this.Valid004,
                this.Valid005,
                this.Valid006,
                this.Valid007,
                this.Valid008
                );
        }
    }
}

