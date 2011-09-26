using System;
using Collection = Kean.Core.Collection;
using Kean.Core;
using Kean.Core.Extension;
using Text = System.Text;
using Uri = Kean.Core.Uri;
using IO = Kean.IO;

namespace Kean.Xml.Sax
{
	public class Parser
	{
		public event Action<float, string, bool?, Region> OnDeclaration;
		public event Action<string, Collection.IDictionary<string, Tuple<string, Region>>, Region> OnElementStart;
		public event Action<string, Region> OnElementEnd;
		public event Action<string, Region> OnText;
		public event Action<string, Region> OnData;
		public event Action<string, string, Region> OnProccessingInstruction;
		public event Action<string, Region> OnComment;

		IO.CharacterReader reader;

		public Uri.Locator Resource { get { return this.reader.Resource; } }
		public Position Position { get { return new Position(this.reader.Row, this.reader.Column); } }

		public Parser(System.Reflection.Assembly assembly, Uri.Path path) :
			this(IO.ByteDevice.Open(assembly, path)) { }
		public Parser(System.IO.Stream stream) :
			this(new IO.ByteDevice(stream)) { }

		public Parser(Uri.Locator resource) :
			this(IO.ByteDevice.Open(resource)) { }
		public Parser(IO.IByteDevice device) :
			this(new IO.CharacterDevice(device)) { }
		public Parser(IO.ICharacterDevice device) :
			this(new IO.CharacterReader(device)) { }
		public Parser(IO.CharacterReader reader)
		{
			this.reader = reader;
		}
		Mark Mark()
		{
			return new Mark(this);
		}
		public bool Parse()
		{
			this.reader.Next();
			while (!this.reader.Empty)
			{
				Mark mark = this.Mark();
				switch (this.reader.Last)
				{
					case '<':
						if (!this.reader.Next())
							return false;
						switch (this.reader.Last)
						{
							case '/': // end element
								this.OnElementEnd(this.AccumulateUntil('>').Trim(), mark);
								this.reader.Next();
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
									this.OnDeclaration.Call(version, null, null, mark);
								}
								else
									this.OnProccessingInstruction.Call(target, this.AccumulateUntil("?>").Trim(), mark);
								this.reader.Next();
								break;
							case '!': // CDATA section or comment
								if (!this.reader.Next())
									return false;
								switch (this.reader.Last)
								{
									case '[': // CDATA section
										if (!this.Verify("CDATA["))
											return false;
										this.OnData.Call(this.AccumulateUntil("]]>"), mark);
                                        this.reader.Next();
										break;
									case '-': // comment
										if (!this.Verify("-"))
											return false;
										this.OnComment.Call(this.AccumulateUntil("-->"), mark);
                                        this.reader.Next();
										break;
								}
								break;
							default: // element name
								string name = this.reader.Last + this.AccumulateUntilWhiteSpaceOr('/', '>').Trim();
								Collection.IDictionary<string, Tuple<string, Region>> attributes = new Collection.Dictionary<string, Tuple<string, Region>>();
                                if (this.reader.Last != '/' && this.reader.Last != '>')
                                    do
                                    {
                                        Mark attributeMark = this.Mark();
                                        string key = this.AccumulateUntil('=').Trim();
                                        this.AccumulateUntil('"');
                                        attributes[key] = Tuple.Create(this.AccumulateUntil('"'), (Region)attributeMark);
                                    } while (this.reader.Next() && this.reader.Last != '/' && this.reader.Last != '>');
								this.OnElementStart.Call(name, attributes, mark);
								if (this.reader.Last == '/' && this.Verify(">"))
								{
									this.OnElementEnd(name, mark);
									this.reader.Next(); // Might have been the last element in fragment, eol acceptable.
								}
								else if (this.reader.Last != '>' || !this.reader.Next())
									return false;
								break;
						}
						break;
					default:
						string text = (this.reader.Last + this.AccumulateUntil('<')).Trim();
						if (text.NotEmpty())
							this.OnText.Call(text, mark);
						break;
				}
			}
			return true; // returns false on errors within the function body
		}
		bool Verify(string value)
		{
			bool result = true;
			for (int i = 0; i < value.Length; i++)
				if (!(result = this.reader.Next() && this.reader.Last == value[i]))
					break;
			return result;
		}
		string AccumulateUntil(params char[] needles)
		{
			Text.StringBuilder result = new Text.StringBuilder();
			while (this.reader.Next() && !needles.Contains(this.reader.Last))
				result.Append(this.reader.Last);
			return result.ToString();
		}
		string AccumulateUntilWhiteSpace()
		{
			Text.StringBuilder result = new Text.StringBuilder();
			while (this.reader.Next() && !char.IsWhiteSpace(this.reader.Last))
				result.Append(this.reader.Last);
			return result.ToString();
		}
		string AccumulateUntilWhiteSpaceOr(params char[] needles)
		{
			Text.StringBuilder result = new Text.StringBuilder();
			while (this.reader.Next() && !char.IsWhiteSpace(this.reader.Last) && !needles.Contains(this.reader.Last))
				result.Append(this.reader.Last);
			return result.ToString();
		}
		string AccumulateUntil(string needle)
		{
			Text.StringBuilder result = new Text.StringBuilder();
			int position = 0;
			while (this.reader.Next())
				if (this.reader.Last == needle[position])
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
					result.Append(this.reader.Last);
			return result.ToString();
		}
	}
}
