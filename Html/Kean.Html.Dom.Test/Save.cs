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
				this.Paragraph 
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
	}
}

