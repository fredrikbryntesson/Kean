using System;
using Collection = Kean.Core.Collection;
using Kean.Core;
using Kean.Core.Extension;
using Text = System.Text;

namespace Kean.Xml.Sax
{
	public class Parser :
		IParser
	{
		public event Action<float, string, bool?, Region> OnDeclaration;
		public event Action<string, Collection.IDictionary<string, Tuple<string, Region>>, Region> OnElementStart;
		public event Action<string, Region> OnElementEnd;
		public event Action<string, Region> OnText;
		public event Action<string, Region> OnData;
		public event Action<string, string, Region> OnProccessingInstruction;
		public event Action<string, Region> OnComment;

		Reader.Abstract reader;

		public string Resource { get { return this.reader.Resource; } }
		public Position Position { get { return this.reader.Position; } }

		public Parser(System.Reflection.Assembly assembly, string filename) :
			this(new Reader.Resource(assembly, filename)) { }
		public Parser(System.IO.Stream stream) :
			this(new Reader.Stream(stream)) { }
		public Parser(System.IO.TextReader reader) :
			this(new Reader.Text(reader)) { }
		public Parser(string data) :
			this(new Reader.String(data)) { }
		internal Parser(Reader.Abstract reader)
		{
			this.reader = reader;
		}

		public bool Parse()
		{
			this.reader.Next();
			while (!this.reader.EndOfFile)
			{
				switch (this.reader.Current)
				{
					case '<':
						if (!this.reader.Next())
							return false;
						switch (this.reader.Current)
						{
							case '/': // end element
								this.OnElementEnd(this.AccumulateUntil('>').Trim(), null);
								break;
							case '?': // proccessing instruction
								string target = this.AccumulateUntilWhiteSpace().Trim();
								if (target == "xml")
								{
									string attribute = this.AccumulateUntil('=').Trim();
									if (attribute != "version")
										return false;
									this.AccumulateUntil('"');
                                    float version = float.Parse(this.AccumulateUntil('"'), System.Globalization.CultureInfo.InvariantCulture);
									this.AccumulateUntil("?>").Trim();
									this.OnDeclaration.Call(version, null, null, null);
								}
								else
									this.OnProccessingInstruction.Call(target, this.AccumulateUntil("?>").Trim(), null);
								break;
							case '!': // CDATA section or comment
								if (!this.reader.Next())
									return false;
								switch (this.reader.Current)
								{
									case '[': // CDATA section
										if (!this.Verify("CDATA["))
											return false;
										this.OnData.Call(this.AccumulateUntil("]]>"), null);
										break;
									case '-': // comment
										if (!this.Verify("-"))
											return false;
										this.OnComment.Call(this.AccumulateUntil("-->"), null);
										break;
								}
								break;
							default: // element name
								string name = this.reader.Current + this.AccumulateUntilWhiteSpaceOr('/', '>').Trim();
								Collection.IDictionary<string, Tuple<string, Region>> attributes = new Collection.Dictionary<string, Tuple<string, Region>>();
								if (this.reader.Current != '/' && this.reader.Current != '>')
									while (this.reader.Next() && this.reader.Current != '/' && this.reader.Current != '>')
									{
										string key = this.AccumulateUntil('=').Trim();
										this.AccumulateUntil('"');
										Position start = this.Position;
										attributes[key] = Tuple.Create(this.AccumulateUntil('"'), new Region(this.Resource, start, this.Position));
									}
								this.OnElementStart(name, attributes, null);
								if (this.reader.Current == '/' && this.Verify(">"))
								{
									this.OnElementEnd(name, null);
									this.reader.Next(); // Might have been the last element in fragment, eol acceptable.
								}
								else if (this.reader.Current != '>' || !this.reader.Next())
									return false;
								break;
						}
						break;
					default:
						string text = (this.reader.Current + this.AccumulateUntil('<')).Trim();
						if (text.NotEmpty())
							this.OnText.Call(text, null);
						break;
				}
			}
			return true; // returns false on errors within the function body
		}
		bool Verify(string value)
		{
			bool result = true;
			for (int i = 0; i < value.Length; i++)
				if (!(result = this.reader.Next() && this.reader.Current == value[i]))
					break;
			return result;
		}
		string AccumulateUntil(params char[] needles)
		{
			Text.StringBuilder result = new Text.StringBuilder();
			while (this.reader.Next() && !needles.Contains(this.reader.Current))
				result.Append(this.reader.Current);
			return result.ToString();
		}
		string AccumulateUntilWhiteSpace()
		{
			Text.StringBuilder result = new Text.StringBuilder();
			while (this.reader.Next() && !char.IsWhiteSpace(this.reader.Current))
				result.Append(this.reader.Current);
			return result.ToString();
		}
		string AccumulateUntilWhiteSpaceOr(params char[] needles)
		{
			Text.StringBuilder result = new Text.StringBuilder();
			while (this.reader.Next() && !char.IsWhiteSpace(this.reader.Current) && !needles.Contains(this.reader.Current))
				result.Append(this.reader.Current);
			return result.ToString();
		}
		string AccumulateUntil(string needle)
		{
			Text.StringBuilder result = new Text.StringBuilder();
			int position = 0;
			while (this.reader.Next())
				if (this.reader.Current == needle[position])
				{
					if (++position == needle.Length)
						break;
				}
				else if (position > 0)
				{
					result.Append(needle.Substring(0, position));
					position = 0;
				}
				else
					result.Append(this.reader.Current);
			return result.ToString();
		}
	}
}
