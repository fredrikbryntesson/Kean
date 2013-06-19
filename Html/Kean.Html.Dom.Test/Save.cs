using System;
using NUnit.Framework;

namespace Kean.Html.Dom.Test
{
	[TestFixture]
	public class Save :
		Kean.Test.Fixture<Save>
	{
		protected override void Run()
		{
			this.Run(
				this.Paragraph,
				this.Heading,
 				this.Anchor,
				this.Abbreviation,
 				this.Address,
 				this.Aside,
 				this.Audio,
 				this.Emphasized,
				this.Strong,
				this.Definition,
				this.Code,
 				this.KeyBoardInput,
				this.Sample,
				this.Variable,
				this.DescriptionList,
				this.Division
				);
		}
		protected void Verify(Document document)
		{
			document.Save(this.CurrentTestStep + ".html");
			VerifyAsResource(this.CurrentTestStep + ".html", ".Correct." + this.CurrentTestStep + ".html", "Failed saving: " + this.CurrentTestStep + ".html");
		}
		[Test]
		public void Paragraph()
		{
			Document document = new Document();
			document.Body.Add(new Paragraph("this is paragraph"));
			this.Verify(document);
		}
		[Test]
		public void Heading()
		{
			Document document = new Document();
			document.Body.Add(new Heading1("this is heading 1"));
			document.Body.Add(new Heading2("this is heading 2"));
			document.Body.Add(new Heading3("this is heading 3"));
			document.Body.Add(new Heading4("this is heading 4"));
			document.Body.Add(new Heading5("this is heading 5"));
			document.Body.Add(new Heading6("this is heading 6"));
			this.Verify(document);
		}
		[Test]
		public void Anchor()
		{
			Document document = new Document();
			document.Body.Add(new Paragraph("here is a ",new Anchor("link"){Destination = "http://imint.se", Style = "color : red"}));
			this.Verify(document);
		}
		[Test]
		public void Abbreviation()
		{
			Document document = new Document();
			document.Body.Add(new Paragraph(new Abbreviation("WHO") { Title = "World Health Organization" }, "was found in 1998"));
			this.Verify(document);
		}
		[Test]
		public void Address()
		{
			Document document = new Document();
			document.Body.Add(new Address("IMINT image Intelligence AB ", new LineBreak(), "Dag Hammarskjolds vag 10C ", new LineBreak(), "751 83 Uppsala, Sweden "));
			this.Verify(document);
		}
		[Test]
		public void Aside()
		{
			Document document = new Document();
			document.Body.Add(new Aside(new Heading4("Epcot Center"), new Paragraph("the Epcot Center is a theme park in Disney WORLD, Florida.")));
			this.Verify(document);
		}
		[Test]
		public void Audio()
		{
			Document document = new Document();
			document.Body.Add(new Audio(new MediaSource() { Source = "sample.ogg", Type = "audio/ogg" }, new MediaSource() { Source = "sample.mp3", Type = "audio/mpeg" }, "No supported format available.") { Controls = true });
			this.Verify(document);
		}
		[Test]
		public void Emphasized()
		{
			Document document = new Document();
			document.Body.Add(new Emphasized("Emphasized text"));
			this.Verify(document);
		}
		[Test]
		public void Strong()
		{
			Document document = new Document();
			document.Body.Add(new Strong("Strong text"));
			this.Verify(document);
		}
		[Test]
		public void Definition()
		{
			Document document = new Document();
			document.Body.Add(new Definition("Definition term"));
			this.Verify(document);
		}
		[Test]
		public void Code()
		{
			Document document = new Document();
			document.Body.Add(new Code("a piece of computer code"));
			this.Verify(document);
		}
		[Test]
		public void KeyBoardInput()
		{
			Document document = new Document();
			document.Body.Add(new KeyBoardInput("keyboardinput sample"));
			this.Verify(document);
		}
		[Test]
		public void Sample()
		{
			Document document = new Document();
			document.Body.Add(new Sample("sample output from computer code"));
			this.Verify(document);
		}
		[Test]
		public void Variable()
		{
			Document document = new Document();
			document.Body.Add(new Variable("variable"));
			this.Verify(document);
		}
		[Test]
		public void DescriptionList()
		{
			Document document = new Document();
			document.Body.Add(new DescriptionList(new DefineTerm("Coffee"), new DescriptionData("black hot drink"), new DefineTerm("Milk"), new DescriptionData("white cold drink")));
			this.Verify(document);
		}
		[Test]
		public void Division()
		{
			Document document = new Document();
			document.Body.Add(new Division(new Paragraph("this is paragraph"), new Heading3("this is heading")) { Style = "color : purple" });
			this.Verify(document);
		}
	}
}

