using Error = Kean.Core.Error;

namespace Kean.Gui.OpenGL.Exception
{
	public class ContextNotCreatable :
		Exception
	{
		internal ContextNotCreatable(System.Exception inner)
			: base(inner, Error.Level.Recoverable, "Unable to Create Context", "Unable to create context: \"{0}\"", inner.Message)
		{ }
	}
}
