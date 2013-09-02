//
//  Program.cs
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
using Kean.Core.Extension;

namespace Kean.Draw.OpenGL.Backend
{
	public abstract class Program :
		Resource
	{
		internal int Identifier { get; private set; }
		protected Program(Context context) :
			base(context)
		{
			this.Identifier = GL.CreateProgram();
		}
		protected Program(Program original) :
			base(original)
		{
			this.Identifier = original.Identifier;
			original.Identifier = 0;
		}

		protected override void Dispose(bool disposing)
		{
			if (this.Context.NotNull())
				this.Context.Recycle(this.Refurbish());
		}
		#region Implementors Interface
		public abstract void Use();
		public abstract void UnUse();
		public abstract void Attach(Shader shader);
		public abstract void Detach(Shader shader);
		public abstract void Link();
		public abstract void SetTexture(string name, int number, IData texture);
		public abstract void UnSetTexture(int number);
		public abstract void SetVariable(string name, float[,] values);
		public abstract void SetVariable(string name, params int[] values);
		public abstract void SetVariable(string name, params float[] values);
		protected abstract Program Refurbish();
		protected internal override void Delete()
		{
			this.Identifier = 0;
			base.Delete();
		}
		#endregion
		public override string ToString()
		{
			return this.Identifier.ToString();
		}
	}
}
