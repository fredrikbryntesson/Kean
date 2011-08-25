using System;
using Kean.Core.Extension;

namespace Kean.Xml.Dom
{
	public class Document
	{
		Element root;
		public Element Root
		{
			get { return this.root; }
			set
			{
				if (this.root.NotNull())
					this.root.Document = null;
				this.root = value;
				if (this.root.NotNull())
				{
					this.root.Parent = null;
					this.root.Document = this;
				}
			}
		}
	}
}
