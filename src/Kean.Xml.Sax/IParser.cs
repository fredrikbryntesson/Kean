using System;
using Kean.Core;
using Collection = Kean.Core.Collection;

namespace Kean.Xml.Sax
{
	public interface IParser
	{
		event Action<float, string, bool?, Region> OnDeclaration;
		event Action<string, Region> OnComment;
		event Action<string, Region> OnData;
		event Action<string, Region> OnElementEnd;
		event Action<string, Collection.IDictionary<string, Tuple<string, Region>>, Region> OnElementStart;
		event Action<string, string, Region> OnProccessingInstruction;
		event Action<string, Region> OnText;
		string Resource { get; }
		Position Position { get; }
		bool Parse();
	}
}
