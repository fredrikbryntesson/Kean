//
//  Depth.cs
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

using Kean.Core.Collection.Extension;
using System;
using Collection = Kean.Core.Collection;
using Error = Kean.Core.Error;
using GL = OpenTK.Graphics.OpenGL.GL;
using Geometry2D = Kean.Math.Geometry2D;
using Raster = Kean.Draw.Raster;

namespace Kean.Draw.OpenGL.Backend
{
	public abstract class Depth :
		Resource
	{
		internal int Identifier { get; private set; }
		public Geometry2D.Integer.Size Size { get; protected set; }
		protected Depth(Context context) :
			base(context)
		{
			Depth.Free();
			this.Identifier = this.CreateIdentifier();
		}
		#region Implementors Interface
		protected abstract int CreateIdentifier();
		public abstract void Use();
		public abstract void UnUse();
		public abstract void Create(Geometry2D.Integer.Size size);
		#endregion
		#region Garbage
		static Collection.IList<int> garbage = new Collection.List<int>();
		internal static void Free()
		{
			lock (Depth.garbage)
			{
				int[] garbage = Depth.garbage.ToArray();
				if (garbage.Length > 0)
					GL.DeleteTextures(garbage.Length, garbage);
				Depth.garbage.Clear();
			}
		}
		#endregion
	}
}
