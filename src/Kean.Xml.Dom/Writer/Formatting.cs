// 
//  Formatting.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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
using Text = System.Text;
using Collection = Kean.Core.Collection;
using Kean.Core;
using Kean.Core.Extension;

namespace Kean.Xml.Dom.Writer
{
	public class Formatting : 
		IWriter
	{
		bool lineIndented;
		string indention = "";
		public bool Indent { get; set; }
		IO.ICharacterWriter writer;
		public Formatting(IO.ICharacterWriter writer)
		{
			this.Indent = true;
			this.writer = writer;
		}
		protected bool AddIndent()
		{
			return (this.indention += "\t").NotEmpty();
		}
		protected bool RemoveIndent()
		{
			return (this.indention = this.indention.Substring(1)).NotNull();
		}
		protected virtual bool Write(string value)
		{
			return (this.lineIndented || (this.lineIndented = this.writer.Write(this.indention))) && this.writer.Write(value);
		}
		protected virtual bool WriteLine()
		{
			return !(this.lineIndented = !(!this.Indent || this.Write("\n")));
		}
		protected virtual bool WriteLine(string value)
		{
			return this.Write(value) && this.WriteLine();
		}
		protected virtual bool Write(string format, object argument)
		{
			return this.Write(string.Format(format, argument));
		}
		protected virtual bool Write(string format, object argument0, object argument1)
		{
			return this.Write(string.Format(format, argument0, argument1));
		}
		protected virtual bool Write(string format, params object[] arguments)
		{
			return this.Write(string.Format(format, arguments));
		}
		protected virtual bool WriteLine(string format, object argument)
		{
			return this.WriteLine(string.Format(format, argument));
		}
		protected virtual bool WriteLine(string format, object argument0, object argument1)
		{
			return this.WriteLine(string.Format(format, argument0, argument1));
		}
		protected virtual bool WriteLine(string format, params object[] arguments)
		{
			return this.WriteLine(string.Format(format, arguments));
		}
		#region IWriter Members
		public bool Write(Document document)
		{
			return this.WriteLine("<?xml version=\"{0}\" encoding=\"{1}\" ?>", document.Version.ToString("#.0"), document.Encoding) && this.Write(document.Root);
		}
		public bool Write(Node node)
		{
			return node is Element ? this.Write(node as Element) :
				node is Text ? this.Write(node as Text) :
				node is Data ? this.Write(node as Data) :
				node is Comment ? this.Write(node as Comment) :
				node is ProcessingInstruction ? this.Write(node as ProcessingInstruction) :
				false;
		}
		public bool Write(Attribute attribute)
		{
			return this.Write(" {0}=\"{1}\"", attribute.Name, attribute.Value);
		}
		public bool Write(Comment comment)
		{
			return this.WriteLine("<!--") && this.Write(comment.Value) && this.Write("-->");
		}
		public bool Write(Data data)
		{
			return this.WriteLine("<![CDATA[") && this.Write(data.Value) && this.Write("]]>");
		}
		public bool Write(Element element)
		{
			return element.IsNull() || (this.Write("<") && this.Write(element.Name) && 
				element.Attributes.Fold((attribute, result) => result && this.Write(attribute), true) &&
				element.Count > 0 ? 
				this.WriteLine(">") && this.AddIndent() && element.Fold((child, result) => result && this.Write(child), true) && this.RemoveIndent() && this.WriteLine("</{0}>", element.Name) : 
				this.WriteLine(" />"));
		}
		public bool Write(ProcessingInstruction processingInstruction)
		{
			return this.Write("<?") && this.Write(processingInstruction.Target) && this.Write(" ") && this.AddIndent() &&
				this.Write(processingInstruction.Value) && this.RemoveIndent() && this.WriteLine(" ?>");
		}
		public bool Write(Text text)
		{
			return this.WriteLine(text.Value);
		}
		#endregion
	}
}
