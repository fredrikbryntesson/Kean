//
//  Shader.cs
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
using Collection = Kean.Core.Collection;
using Error = Kean.Core.Error;
using GL = OpenTK.Graphics.OpenGL.GL;

namespace Kean.Draw.OpenGL.Backend
{
	public abstract class Shader :
		Resource
	{
		internal int Identifier { get; private set; }
		protected Shader(Context context, ShaderType type) :
			base(context)
		{
			Shader.Free();
			this.Identifier = this.Create(type);
		}
		protected override void Dispose(bool disposing)
		{
			if (this.Identifier != 0)
			{
				lock (Shader.garbage)
					Shader.garbage.Add(this.Identifier);
				this.Identifier = 0;
			}
			base.Dispose(disposing);
		}
		protected abstract int Create(ShaderType type);
		public abstract string Compile(string code);
		public override string ToString()
		{
			return this.Identifier.ToString();
		}
		#region Garbage
		static Collection.IList<int> garbage = new Collection.List<int>();
		internal static void Free()
		{
			lock (Shader.garbage)
				while (Shader.garbage.Count > 0)
					GL.DeleteShader(Shader.garbage.Remove());
		}
		#endregion
	}
}
