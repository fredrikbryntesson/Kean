using System;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Gui.OpenGL.Backend.OpenGL21
{
	class Window :
		Backend.Window
	{
		public Window(Geometry2D.Integer.Size size, string title) :
			base(size, title, OpenTK.GameWindowFlags.Default, OpenTK.Graphics.GraphicsMode.Default, OpenTK.DisplayDevice.Default)
		{ }
		protected override OpenTK.Graphics.GraphicsContext CreateContext(OpenTK.Platform.IWindowInfo windowInformation)
		{
			return new OpenTK.Graphics.GraphicsContext(OpenTK.Graphics.GraphicsMode.Default, windowInformation, 1, 0, OpenTK.Graphics.GraphicsContextFlags.Default);
		}
	}
}
