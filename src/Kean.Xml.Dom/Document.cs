using System;
using Kean.Core;
using Kean.Core.Extension;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Uri = Kean.Core.Uri;

namespace Kean.Xml.Dom
{
	public class Document :
		IEquatable<Document>
	{
		public float Version { get; set; }
		public string Encoding { get; set; }
		public bool? Standalone { get; set; }

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
		public Document()
		{
			this.Version = 1.0f;
		}
		public static explicit operator Document(string data)
		{
			return Document.Open(new Sax.Parser(data));
		}
		#region Static Open
		public static Document Open(Sax.Parser parser)
		{
			Document result = new Document();
			parser.OnDeclaration += (version, encoding, standalone, region) =>
			{
				result.Version = version;
				result.Encoding = encoding;
				result.Standalone = standalone;
			};
			result.Root = Element.Open(parser);
			return result;
		}
		public static Document OpenResource(System.Reflection.Assembly assembly, Uri.Path resource)
		{
			return Document.Open(new Sax.Parser(assembly, resource));
		}
		public static Document OpenResource(string name)
		{
			string[] splitted = name.Split(new char[] { ':' }, 2);
			Document result;
			if (splitted.Length > 1)
				result = Document.OpenResource(System.Reflection.Assembly.Load(splitted[0]), splitted[1]);
			else
				result = Document.OpenResource(System.Reflection.Assembly.GetCallingAssembly(), name);
			return result;
		}
		public static Document Open(string filename)
		{
			Document result;
			try
			{
				using (System.IO.Stream stream = new System.IO.StreamReader(filename).BaseStream)
					result = Document.Open(stream);
			}
			catch (System.IO.IOException)
			{
				result = null;
			}
			return result;
		}
		public static Document Open(System.IO.Stream stream)
		{
			return Document.Open(new Sax.Parser(stream));
		}
		public static Document Open(System.IO.Stream stream, string resource)
		{
			return Document.Open(new Sax.Parser(stream));
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return this.Equals(other as Document);
		}
		public override int GetHashCode()
		{
			return this.Version.GetHashCode() ^
				this.Encoding.Hash() ^
				this.Standalone.GetHashCode() ^
				this.Root.GetHashCode();
		}
		public override string ToString()
		{
			return base.ToString();
		}
		#endregion
		#region IEquatable<Document> Members
		public bool Equals(Document other)
		{
			return other.NotNull() &&
				this.Version == other.Version &&
				this.Encoding == other.Encoding &&
				this.Standalone == other.Standalone &&
				this.Root == other.Root;
		}
		#endregion
		#region Operators
		public static bool operator ==(Document left, Document right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Document left, Document right)
		{
			return !(left == right);
		}
		#endregion
	}
}
