using Error = Kean.Core.Error;

namespace Kean.Gui.OpenGL.Exception
{
	public class Exception :
		Error.Exception
	{
		internal Exception(Error.Level level, string title, string message, params string[] arguments) : this(null, level, title, message, arguments) { }
		internal Exception(System.Exception innerException, Error.Level level, string title, string message, params string[] arguments) : base(innerException, level, title, message, arguments) { }
	}
}
