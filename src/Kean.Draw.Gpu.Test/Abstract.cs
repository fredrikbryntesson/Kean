using System;

namespace Kean.Draw.Gpu.Test
{
	public abstract class Abstract<T> :
		Kean.Test.Fixture<T>
		where T : Abstract<T>, new()
	{
		Gpu.Window window;
		public override void Setup()
		{
			base.Setup();
			this.window = new Window();
		}
		public override void TearDown()
		{
			this.window.Dispose();
			base.TearDown();
		}
	}
}
