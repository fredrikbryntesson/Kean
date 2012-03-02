// 
//  Abstract.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
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
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Core.Extension;

namespace Kean.Gui.OpenGL.Backend.Shader
{
	public abstract class Abstract :
		IString,
		IDisposable
	{
		protected abstract OpenTK.Graphics.OpenGL.ShaderType ShaderType { get; }

		protected int Identifier { get; private set; }
		#region IString Members
		string IString.String { get { return this.Code; } set { this.Code = value; } }
		#endregion
		string code;
		public string Code
		{
			get { return this.code; }
			set
			{
				this.code = value;
				this.Identifier = Abstract.Create(this.ShaderType);
				// Compile vertex shader
				OpenTK.Graphics.OpenGL.GL.ShaderSource(this.Identifier, this.code);
				OpenTK.Graphics.OpenGL.GL.CompileShader(this.Identifier);
				string compilerMesage;
				OpenTK.Graphics.OpenGL.GL.GetShaderInfoLog(this.Identifier, out compilerMesage);
				int statusCode;
				OpenTK.Graphics.OpenGL.GL.GetShader(this.Identifier, OpenTK.Graphics.OpenGL.ShaderParameter.CompileStatus, out statusCode);
				if (statusCode != 1)
					throw new Exception.ShaderNotCompiled(compilerMesage);

			}
		}
		protected Abstract() { }
		protected Abstract(string code)
		{
			this.Code = code;
		}
		internal void Attach(int program)
		{
			OpenTK.Graphics.OpenGL.GL.AttachShader(program, this.Identifier);
			OpenTK.Graphics.OpenGL.GL.LinkProgram(program);
		}
		internal void Deattach(int program)
		{
			OpenTK.Graphics.OpenGL.GL.DetachShader(program, this.Identifier);
		}

		~Abstract()
		{
			this.Dispose();
		}
		public void Dispose()
		{
			if (this.Identifier != 0)
			{
				Abstract.Delete(this.Identifier);
				this.Identifier = 0;
			}
		}
		#region Dispose Handling
		static Collection.IList<int> garbage = new Collection.List<int>();
		static int Create(OpenTK.Graphics.OpenGL.ShaderType shaderType)
		{
			while (Abstract.garbage.Count > 0)
				OpenTK.Graphics.OpenGL.GL.DeleteShader(Abstract.garbage.Remove());
			return OpenTK.Graphics.OpenGL.GL.CreateShader(shaderType);
		}
		static void Delete(int shader)
		{
			Abstract.garbage.Add(shader);
		}
		#endregion
	}
	public abstract class Abstract<T> :
		Abstract
		where T : Abstract<T>, new()
	{
		public Abstract() { }
		public Abstract(string code) : base(code) { }
		public static T CreateFromFile(string filename)
		{
			try
			{
				return new T() { Code = System.IO.File.ReadAllText(filename) };
			}
			catch (System.Exception e)
			{
				throw new Exception.ShaderNotFound(e, filename);
			}
		}
		public static T CreateFromAssembly(string name)
		{
			System.IO.TextReader stream;
			try
			{
				stream = new System.IO.StreamReader(System.Reflection.Assembly.GetCallingAssembly().GetManifestResourceStream(name));
			}
			catch (System.Exception e)
			{
				throw new Exception.ShaderNotFound(e, System.Reflection.Assembly.GetCallingAssembly(), name);
			}
			T result = new T() { Code = stream.ReadToEnd() };
			stream.Close();
			stream.Dispose();
			return result;
		}
	}
}
