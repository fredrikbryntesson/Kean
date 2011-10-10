using System;
using Color = Kean.Draw.Color;

namespace Kean.Draw.Cairo.Extension
{
	public static class ColorExtension
	{
		public static global::Cairo.Color AsCairo(this IColor me)
		{
			Color.Bgra bgra = me.Convert<Color.Bgra>();
			return new global::Cairo.Color(bgra.color.red / 255.0, bgra.color.red / 255.0, bgra.color.red / 255.0, bgra.alpha / 255.0);
		}
		public static IColor FromCairo(this global::Cairo.Color me)
		{
			return new Color.Bgra((byte)Math.Integer.Round(me.B * 255), (byte)Math.Integer.Round(me.G * 255), (byte)Math.Integer.Round(me.R * 255), (byte)Math.Integer.Round(me.A * 255));
		}
	}
}
