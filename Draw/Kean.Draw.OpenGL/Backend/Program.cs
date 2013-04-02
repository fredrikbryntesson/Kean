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

namespace Kean.Draw.OpenGL.Backend
{
	public abstract class Program :
		Resource
	{
		internal int Identifier { get; private set; }
		protected Program(Context context) :
			base(context)
		{
			Program.Free();
			this.Identifier = GL.CreateProgram();
		}
		protected override void Dispose(bool disposing)
		{
			if (this.Identifier != 0)
			{
				lock (Program.garbage)
					Program.garbage.Add(this.Identifier);
				this.Identifier = 0;
			}
			base.Dispose(disposing);
		}
		public abstract void Use();
		public abstract void UnUse();
		public abstract void Attach(Shader shader);
		public abstract void Detach(Shader shader);
		public abstract void Link();
		public abstract void SetTexture(string name, int number, ITexture texture);
		public abstract void UnSetTexture(int number);
		public abstract void SetVariable(string name, float[,] values);
		public abstract void SetVariable(string name, params int[] values);
		public abstract void SetVariable(string name, params float[] values);
		public override string ToString()
		{
			return this.Identifier.ToString();
		}
		#region Garbage
		static Collection.IList<int> garbage = new Collection.List<int>();
		internal static void Free()
		{
			lock (Program.garbage)
				while (Program.garbage.Count > 0)
					OpenTK.Graphics.OpenGL.GL.DeleteProgram(Program.garbage.Remove());
		}
		#endregion
	}
}
