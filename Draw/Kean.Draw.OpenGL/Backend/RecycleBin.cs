//
//  RecycleBin.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika
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
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Kean.Core;
using Kean.Core.Extension;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Recycle = Kean.Core.Recycle;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.OpenGL.Backend
{
	class RecycleBin<T> :
		WasteBin<T>
		where T : ITexture
	{
		Recycle.IBin<T, Geometry2D.Integer.Size> monochrome;
		Recycle.IBin<T, Geometry2D.Integer.Size> argb;
		Recycle.IBin<T, Geometry2D.Integer.Size> rgb;

		public bool On { get; set; }

		public RecycleBin(Action<T> free) :
			base(free)
		{
			this.monochrome = this.CreateBin();
			this.argb = this.CreateBin();
			this.rgb = this.CreateBin();
		}

		public override void Add(T item)
		{
			lock (this.Lock)
			{
				if (this.On)
				{
					switch (item.Type)
					{
						case TextureType.Monochrome: this.monochrome.Recycle(item); break;
						case TextureType.Argb: this.argb.Recycle(item); break;
						case TextureType.Rgb: this.rgb.Recycle(item); break;
					}
				}
				else
					base.Add(item);
			}
		}
		public override void Free()
		{
			lock (this.Lock)
			{
				if (!this.On)
				{
					this.monochrome.Free();
					this.argb.Free();
					this.rgb.Free();
				}
				base.Free();
			}
		}
		public T Recycle(TextureType type, Geometry2D.Integer.Size size)
		{
			T result;
			lock (this.Lock)
				switch (type)
				{
					case TextureType.Monochrome: result = this.monochrome.Find(size); break;
					default:
					case TextureType.Argb: result = this.argb.Find(size); break;
					case TextureType.Rgb: result = this.rgb.Find(size); break;
				}
			return result;
		}

		Recycle.IBin<T, Geometry2D.Integer.Size> CreateBin()
		{
			return new Recycle.Bins<T, Geometry2D.Integer.Size>(
			10,
			3,
			(item, size) => item.Size == size,
			base.Add,
			item => {
				int pixels = item.Size.Area;
				return pixels < 10000 ? 0 : pixels < 100000 ? 1 : 2;
			},
			size =>
			{
				int pixels = size.Area;
				return pixels < 10000 ? 0 : pixels < 100000 ? 1 : 2;
			}
			);
		}
	}
}
