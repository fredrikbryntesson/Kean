using System;
using Collection = Kean.Core.Collection;
using Kean.Core;
using Kean.Core.Extension;
using Text = System.Text;

namespace Kean.Xml.Sax.Parser
{
	public abstract class Abstract<T> :
		IParser<T>
	{
		public event Action<string, Collection.IDictionary<string, string>> OnElementStart;
		public event Action<string> OnElementEnd;
		public event Action<string> OnText;
		public event Action<string> OnData;
		public event Action<string, string> OnProccessingInstruction;
		public event Action<string> OnComment;

		protected abstract bool EndOfFile { get; }
		protected abstract char Current { get; }

		public abstract bool Parse(T input);
		protected abstract bool Next();
		protected bool Parse()
		{
			while (!this.EndOfFile)
			{
				switch (this.Current)
				{
					case '<':
						if (!this.Next())
							return false;
						switch (this.Current)
						{
							case '/': // end element
								this.OnElementEnd(this.AccumulateUntil('>').Trim());
								break;
							case '?': // proccessing instruction
								this.OnProccessingInstruction(this.AccumulateUntilWhiteSpace().Trim(), this.AccumulateUntil("?>").Trim());
								break;
							case '!': // CDATA section or comment
								if (!this.Next())
									return false;
								switch (this.Current)
								{
									case '[': // CDATA section
										if (!this.Verify("CDATA["))
											return false;
										this.OnData.Call(this.AccumulateUntil("]]>"));
										break;
									case '-': // comment
										if (!this.Verify("-"))
											return false;
										this.OnComment.Call(this.AccumulateUntil("-->"));
										break;
								}
								break;
							default: // element name
								string name = this.AccumulateUntilWhiteSpaceOr('/', '>').Trim();
								Collection.IDictionary<string, string> attributes = new Collection.Dictionary<string, string>();
								if (this.Current != '/' && this.Current != '>')
									while (this.Next() && this.Current != '/' && this.Current != '>')
									{
										string key = this.AccumulateUntil('=').Trim();
										this.AccumulateUntil('"');
										attributes[key] = this.AccumulateUntil('"');
									}
								this.OnElementStart(name, attributes);
								if (this.Current == '/' && this.Verify(">"))
								{
									this.OnElementEnd(name);
									this.Next(); // Might have been the last element in fragment, eol acceptable.
								}
								else if (this.Current != '>' || !this.Next())
									return false;
								break;
						}
						break;
					default:
						this.OnText.Call(this.AccumulateUntil('<'));
						break;
				}
			}
			return true; // returns false on errors within the function body
		}
		bool Verify(string value)
		{
			bool result = true;
			for (int i = 0; i < value.Length; i++)
				if (!(result = this.Next() && this.Current == value[i]))
					break;
			return result && this.Next();
		}
		string AccumulateUntil(params char[] needles)
		{
			Text.StringBuilder result = new Text.StringBuilder();
			while (this.Next() && !needles.Contains(this.Current))
				result.Append(this.Current);
			return result.ToString();
		}
		string AccumulateUntilWhiteSpace()
		{
			Text.StringBuilder result = new Text.StringBuilder();
			while (this.Next() && !char.IsWhiteSpace(this.Current))
				result.Append(this.Current);
			return result.ToString();
		}
		string AccumulateUntilWhiteSpaceOr(params char[] needles)
		{
			Text.StringBuilder result = new Text.StringBuilder();
			while (this.Next() && !char.IsWhiteSpace(this.Current) && !needles.Contains(this.Current))
				result.Append(this.Current);
			return result.ToString();
		}
		string AccumulateUntil(string needle)
		{
			Text.StringBuilder result = new Text.StringBuilder();
			int position = 0;
			while (this.Next())
				if (this.Current == needle[position] && ++position == needle.Length && this.Next())
					break;
				else if (position > 0)
				{
					result.Append(needle.Substring(0, position));
					position = 0;
				}
				else
					result.Append(this.Current);
			return result.ToString();
		}
	}
}
