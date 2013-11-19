//
//  Data2D.cs
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

using Kean.Collection.Extension;
using System;
using Collection = Kean.Collection;
using Error = Kean.Error;
using GL = OpenTK.Graphics.OpenGL.GL;
using Geometry2D = Kean.Math.Geometry2D;
using Geometry3D = Kean.Math.Geometry3D;
using Raster = Kean.Draw.Raster;
using Kean.Extension;

namespace Kean.Draw.OpenGL.Backend
{
	public abstract class Data1D :
		Resource,
		IData
	{
		protected int Identifier { get; private set; }
		public int Size { get; private set; }
		protected Data1D(Context context, byte[] data) :
			this(context)
		{
			this.Use();
			this.Update(data);
			this.UnUse();
		}
		protected Data1D(Context context) : 
			base(context)
		{
			this.Identifier = this.CreateIdentifier();
		}

		public void Update(byte[] data)
		{
			if (this.Size != data.Length)
			{
				this.Size = data.Length;
				this.Allocate(data);
			}
			else
				this.Load(data);
		}
		#region Implementors Interface
		protected abstract int CreateIdentifier();
		protected abstract void Allocate(byte[] data);
		protected abstract void Load(byte[] data);
		public abstract void Use();
		public abstract void UnUse();
		protected internal override void Delete()
		{
			this.Identifier = 0;
			base.Delete();
		}
		#endregion
		protected override void Dispose(bool disposing)
		{
			if (this.Identifier != 0)
				this.Delete();
		}
	}
}
