using System;
using Collection = Kean.Core.Collection;

namespace Kean.Xml.Sax
{
	interface IParser<T>
	{
		event Action<string> OnComment;
		event Action<string> OnData;
		event Action<string> OnElementEnd;
		event Action<string, Collection.IDictionary<string, string>> OnElementStart;
		event Action<string, string> OnProccessingInstruction;
		event Action<string> OnText;
		bool Parse(T input);
	}
}
