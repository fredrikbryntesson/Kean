using System;
namespace Kean.Xml.Serialize.Test
{
	public interface IFactory
	{
		bool Boolean { get; }
		T Create<T>() where T : Kean.Xml.Serialize.Test.Data.IData;
		Kean.Xml.Serialize.Test.Data.Enumerator Enumerator { get; }
		string Filename(Type type);
		float Float { get; }
		int Integer { get; }
		string Name(Type type);
		string ReferencePath(Type type);
		string ReferencePath(string filename);
		string String { get; }
	}
}
