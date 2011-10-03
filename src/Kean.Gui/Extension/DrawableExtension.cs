using System;
using Geometry2D = Kean.Math.Geometry2D;

namespace Attraction.Gui.Base.Target.Extension
{
	public static class DrawableExtension
	{
		public static void Draw(this IDrawable me)
		{
			me.Draw(new Geometry2D.Single.Point());
		}
		public static void Draw(this IDrawable me, Geometry2D.Single.Point position)
		{
			me.Draw(position, me.Bounds);
		}
		public static void Draw(this IDrawable me, Geometry2D.Single.Point position, Geometry2D.Single.Box part)
		{
			me.Draw(part, new Geometry2D.Single.Box(position, part.Size));
		}
	}
}
