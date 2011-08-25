using System;
using Text = System.Text;
using IO = System.IO;
using Collection = Kean.Core.Collection;
using Kean.Core;
using Kean.Core.Extension;

namespace Kean.Xml.Writer
{
	public class StringBuilder : 
		IWriter<Text.StringBuilder>
	{
		public StringBuilder()
		{
		}
		public Text.StringBuilder WriteElement(string name, Collection.IDictionary<string, string> attributes, Text.StringBuilder content)
		{
			Text.StringBuilder result = new Text.StringBuilder();
			result.Append("<").Append(name);
			if (attributes.NotNull())
				foreach (Tuple<string, string> attribute in attributes)
					result.AppendFormat(" {0}=\"{1}\"", attribute.Item1, attribute.Item2);
			if (content.NotNull())
				result.Append(">").Append(content).AppendFormat("</{0}>", name);
			else
				result.Append(" />");
			return result;
		}
		public Text.StringBuilder WriteProcessingInstruction(string target, string data)
		{
			return new Text.StringBuilder().Append("<?").Append(target).Append(" ").Append(data).Append(" ?>");
		}
		public Text.StringBuilder WriteText(string text)
		{
			return new Text.StringBuilder(text);
		}
		public Text.StringBuilder WriteData(string data)
		{
			return new Text.StringBuilder("<![CDATA[").Append(data).Append("]]>");
		}
		public Text.StringBuilder WriteComment(string comment)
		{
			return new Text.StringBuilder("<!--").Append(comment).Append("-->");
		}
		public Text.StringBuilder WriteDocument(float version, string encoding, Text.StringBuilder content)
		{
			return new Text.StringBuilder().AppendFormat("<?xml version=\"{0}\" encoding=\"{1}\" ?>", version.ToString("02d", System.Globalization.CultureInfo.InvariantCulture.NumberFormat), encoding).Append(content);
		}
		public Text.StringBuilder WriteDocument(Text.StringBuilder content)
		{
			return this.WriteDocument(1.0f, "UTF-8", content);
		}
	}
}
