using System;
using Kean.Core;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Cairo
{
	public abstract class Image :
		Draw.Image
	{
		internal global::Cairo.Surface Backend { get; private set; }

		Canvas canvas;
		public override Draw.Canvas Canvas
		{
			get 
			{ 
				if (this.canvas.IsNull())
					this.canvas = new Canvas(this);
				return this.canvas;
			}
		}
		protected Image(global::Cairo.Surface backend, Geometry2D.Integer.Size size) :
			base(size, CoordinateSystem.Default)
		{
			this.Backend = backend;
		}
		#region Draw.Image overrides
		public override T Convert<T>()
		{
			return null;
		}
		public override Draw.Image ResizeTo(Geometry2D.Integer.Size size)
		{
			return null;
		}
		public override Draw.Image Copy()
		{
			return null;
		}
		public override Draw.Image Copy(Geometry2D.Integer.Size size, Geometry2D.Single.Transform transform)
		{
			return null;
		}
		public override void Shift(Geometry2D.Integer.Size offset) { }
		#endregion
		public void Save(string filename)
		{
			this.Backend.WriteToPng(filename);
		}
	}
}
