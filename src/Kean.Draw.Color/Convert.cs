// 
//  Convert.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public Licenseusing System;
using System;
using Function = Kean.Math;
using Kean.Core.Extension;

namespace Kean.Draw.Color
{
	public static class Convert
	{
		static int[][] yuvToBgr;
		static void YuvToBgrInitialize()
		{
			if (Convert.yuvToBgr.IsNull())
			{
				// The color transforms uses [0,255] in all y,u,v channels. The output from several video files and capture unit has 
				// given y-values in [0,255] including the endpoints.
				// Shader version 
				//   mat4 matrix = mat4(1, 1, 1, 0,
				//                      0, -0.3441, 1.7720, 0,
				//                      1.4020, -0.7141, 0, 0,
				//                      0, 0, 0, 1);   
				// Integer version (multiplication times 256).
				Convert.yuvToBgr = new int[3][];
				for (int i = 0; i < 3; i++)
					Convert.yuvToBgr[i] = new int[3 * 256];
				for (int j = 0; j < 256; j++)
				{
					Convert.yuvToBgr[2][j + 0] = Convert.yuvToBgr[1][j + 0] = Convert.yuvToBgr[0][j + 0] = 256 * j;
					Convert.yuvToBgr[0][j + 512] = 358 * (j - 128);
					Convert.yuvToBgr[1][j + 256] = -88 * (j - 128);
					Convert.yuvToBgr[1][j + 512] = -182 * (j - 128);
					Convert.yuvToBgr[2][j + 256] = 453 * (j - 128);
				}
			}
		}
		public static Action<Yuv> FromYuv(Action<Bgr> action)
		{
			Convert.YuvToBgrInitialize();
			return action.IsNull() ? (Action<Yuv>)null : color => action(new Bgr(
				(byte)(Function.Integer.Clamp(0, 255, (Convert.yuvToBgr[2][color.y] + Convert.yuvToBgr[2][256 + color.u] + Convert.yuvToBgr[2][512 + color.v]) >> 8)),
				(byte)(Function.Integer.Clamp(0, 255, (Convert.yuvToBgr[1][color.y] + Convert.yuvToBgr[1][256 + color.u] + Convert.yuvToBgr[1][512 + color.v]) >> 8)),
				(byte)(Function.Integer.Clamp(0, 255, (Convert.yuvToBgr[0][color.y] + Convert.yuvToBgr[0][256 + color.u] + Convert.yuvToBgr[0][512 + color.v]) >> 8))
				));
		}
		public static Action<Y> FromY(Action<Bgr> action)
		{
			Convert.YuvToBgrInitialize();
			return action.IsNull() ? (Action<Y>)null : value => action(new Bgr(value, value, value));
		}

		static int[][] bgrToYuv;
		static void BgrToYuvInititalize()
		{
			if (Convert.bgrToYuv.IsNull())
			{
				// Shader version 
				//  mat4 matrix =  mat4(0.299, -0.168736,  0.5, 0.0,
				//		                0.587, -0.331264, -0.418688, 0.0,
				//		                0.114,   0.5000, -0.081312, 0.0,
				//		                0.0,		0.0,      0.0, 1.0); 
				// Integer version (multiplication times 256).
				Convert.bgrToYuv = new int[3][];
				for (int i = 0; i < 3; i++)
					Convert.bgrToYuv[i] = new int[3 * 256];
				for (int j = 0; j < 256; j++)
				{
					Convert.bgrToYuv[0][j + 0] = 76 * j;
					Convert.bgrToYuv[0][j + 256] = 150 * j;
					Convert.bgrToYuv[0][j + 512] = 29 * j;
					Convert.bgrToYuv[1][j + 0] = -43 * j;
					Convert.bgrToYuv[1][j + 256] = -85 * j;
					Convert.bgrToYuv[1][j + 512] = 128 * j;
					Convert.bgrToYuv[2][j + 0] = 128 * j;
					Convert.bgrToYuv[2][j + 256] = -107 * j;
					Convert.bgrToYuv[2][j + 512] = -21 * j;
				}
			}
		}
		public static Action<Bgr> FromBgr(Action<Yuv> action)
		{
			Convert.BgrToYuvInititalize();
			return action.IsNull() ? (Action<Bgr>)null : color => action(new Yuv(
				(byte)Function.Integer.Clamp(0, 255, ((Convert.bgrToYuv[0][color.red] + Convert.bgrToYuv[0][256 + color.green] + Convert.bgrToYuv[0][512 + color.blue]) >> 8)),
				(byte)Function.Integer.Clamp(0, 255, ((Convert.bgrToYuv[1][color.red] + Convert.bgrToYuv[1][256 + color.green] + Convert.bgrToYuv[1][512 + color.blue]) >> 8) + 128),
				(byte)Function.Integer.Clamp(0, 255, ((Convert.bgrToYuv[2][color.red] + Convert.bgrToYuv[2][256 + color.green] + Convert.bgrToYuv[2][512 + color.blue]) >> 8) + 128)
				));
		}
		public static Action<Y> FromY(Action<Yuv> action)
		{
			return action.IsNull() ? (Action<Y>)null : (Y value) => action(new Yuv(value, 128, 128));
		}

		public static Action<Bgr> FromBgr(Action<Y> action)
		{
			Convert.BgrToYuvInititalize();
			return action.IsNull() ? (Action<Bgr>)null : color => action((Y)(byte)Function.Integer.Clamp(0, 255, ((Convert.bgrToYuv[0][color.red] + Convert.bgrToYuv[0][256 + color.green] + Convert.bgrToYuv[0][512 + color.blue]) >> 8)));
		}
		public static Action<Yuv> FromYuv(Action<Y> action)
		{
			return action.IsNull() ? (Action<Yuv>)null : color => action(color.y);
		}
	}
}
