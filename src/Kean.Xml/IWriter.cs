using System;
using Collection = Kean.Core.Collection;

namespace Kean.Xml
{
	public interface IWriter<T>
	{
		T WriteComment(string comment);
		T WriteData(string data);
		T WriteDocument(float version, string encoding, T content);
		T WriteDocument(T content);
		T WriteElement(string name, Collection.IDictionary<string, string> attributes, T content);
		T WriteProcessingInstruction(string target, string data);
		T WriteText(string text);
	}
}
