using System;

namespace Kean.Xml.Sax.Reader
{
	class Resource :
		Stream
	{
		public System.Reflection.Assembly Assembly { get; private set; }
		public string Filename { get; set; }

		public Resource(System.Reflection.Assembly assembly, string filename) :
			this(assembly, assembly.GetName().Name + "." + filename.Replace('\\', '.').Replace('/', '.'), filename)
		{
		}
		Resource(System.Reflection.Assembly assembly, string resource, string filename) :
			base(assembly.GetManifestResourceStream(filename))
		{
			this.Assembly = assembly;
			this.Filename = filename;
		}

		public override Text Include(string argument)
		{
			return new Resource(this.Assembly, System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Filename), argument));
		}
	}
}
