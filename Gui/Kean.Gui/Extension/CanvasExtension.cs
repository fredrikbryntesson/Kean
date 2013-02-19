using Geometry2D = Kean.Math.Geometry2D;
using Bitmap = Kean.Draw.Raster;
namespace Attraction.Gui.Base.Target.Extension
{
	public static class CanvasExtension
	{
		public static void PushAndTranslate(this ICanvas me, Geometry2D.Single.Box clip)
		{
			me.Push(clip.Intersection(me.Clip) - clip.LeftTop, Geometry2D.Single.Transform.CreateTranslation(clip.LeftTop));
		}
		public static void Push(this ICanvas me, Geometry2D.Single.Box clip)
		{
			me.Push(clip, Geometry2D.Single.Transform.Identity);
		}
		public static void Draw(this ICanvas me, ICache cache)
		{
			Geometry2D.Single.Box region = new Geometry2D.Single.Box(new Geometry2D.Single.Point(), cache.Resolution);
			me.Draw(cache, region, region);
		}

	}
}
