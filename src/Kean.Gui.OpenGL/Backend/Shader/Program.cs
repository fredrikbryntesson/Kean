using System;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Core.Extension;

namespace Kean.Gui.OpenGL.Backend.Shader
{
	public class Program :
		IDisposable
	{
		public enum ArrayType
		{
			Uniform1,
			Uniform2,
			Uniform3,
			Uniform4,
		}
		int identifier;
		public Collection.Hooked.IList<Shader.Abstract> Shaders { get; private set; }
		public Program()
		{
			this.Shaders = new Collection.Hooked.List<Shader.Abstract>();
			(this.Shaders as Collection.Hooked.List<Shader.Abstract>).Added += (int index, Shader.Abstract shader) =>
			{
				OpenTK.Graphics.OpenGL.GL.AttachShader(this.identifier, shader.Identifier);
				OpenTK.Graphics.OpenGL.GL.LinkProgram(this.identifier);
			};
			(this.Shaders as Collection.Hooked.List<Shader.Abstract>).Removed += (int index, Shader.Abstract shader) =>
			{
				OpenTK.Graphics.OpenGL.GL.DetachShader(this.identifier, shader.Identifier);
			};
			this.identifier = Program.Create();
		}
		public Program(Shader.Abstract shader)
			: this()
		{
			this.Shaders.Add(shader);
		}
		~Program()
		{
			this.Dispose();
		}
		public void Dispose()
		{
			if (this.Shaders != null)
			{
				foreach (Shader.Abstract shader in this.Shaders)
					shader.Dispose();
				this.Shaders = null;
			}
			if (this.identifier != 0)
			{
				Program.Delete(this.identifier);
				this.identifier = 0;
			}
		}
		public void Use()
		{
			OpenTK.Graphics.OpenGL.GL.UseProgram(this.identifier);
		}
		public void UnUse()
		{
			Program.Reset();
		}
		public void SetVariable(string variableName, params Kean.Math.Geometry2D.Single.Point[] points)
		{
			float[] values = new float[points.Length * 2];
			for (int i = 0; i < points.Length; i++)
			{
				values[2 * i + 0] = points[i].X;
				values[2 * i + 1] = points[i].Y;
			}
			this.SetVariable(variableName, ArrayType.Uniform2, values);
		}
		public void SetVariable(string variableName, ArrayType variable, params float[] values)
		{
			this.Use();
			switch (variable)
			{
				default:
				case ArrayType.Uniform1:
					OpenTK.Graphics.OpenGL.GL.Uniform1(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.identifier, variableName), values.Length, values);
					break;
				case ArrayType.Uniform2:
					OpenTK.Graphics.OpenGL.GL.Uniform2(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.identifier, variableName), values.Length, values);
					break;
				case ArrayType.Uniform3:
					OpenTK.Graphics.OpenGL.GL.Uniform3(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.identifier, variableName), values.Length, values);
					break;
				case ArrayType.Uniform4:
					OpenTK.Graphics.OpenGL.GL.Uniform4(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.identifier, variableName), values.Length, values);
					break;
			}
			this.UnUse();
		}
		public void SetVariable(string variableName, params int[] values)
		{
			this.Use();
			switch (values.Length)
			{
				default:
				case 1:
					OpenTK.Graphics.OpenGL.GL.Uniform1(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.identifier, variableName), values[0]);
					break;
				case 2:
					OpenTK.Graphics.OpenGL.GL.Uniform2(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.identifier, variableName), values[0], values[1]);
					break;
				case 3:
					OpenTK.Graphics.OpenGL.GL.Uniform3(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.identifier, variableName), values[0], values[1], values[2]);
					break;
				case 4:
					OpenTK.Graphics.OpenGL.GL.Uniform4(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.identifier, variableName), values[0], values[1], values[2], values[3]);
					break;
			}
			this.UnUse();
		}
		public void SetVariable(string variableName, params float[] values)
		{
			this.Use();
			switch (values.Length)
			{
				default:
				case 1:
					OpenTK.Graphics.OpenGL.GL.Uniform1(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.identifier, variableName), values[0]);
					break;
				case 2:
					OpenTK.Graphics.OpenGL.GL.Uniform2(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.identifier, variableName), values[0], values[1]);
					break;
				case 3:
					OpenTK.Graphics.OpenGL.GL.Uniform3(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.identifier, variableName), values[0], values[1], values[2]);
					break;
				case 4:
					OpenTK.Graphics.OpenGL.GL.Uniform4(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.identifier, variableName), values[0], values[1], values[2], values[3]);
					break;
			}
			this.UnUse();
		}
		public static void Reset()
		{
			OpenTK.Graphics.OpenGL.GL.UseProgram(0);
		}
		#region Dispose Handling
		private static Collection.IList<int> garbage = new Collection.List<int>();
		private static int Create()
		{
			while (Program.garbage.Count > 0)
				OpenTK.Graphics.OpenGL.GL.DeleteProgram(Program.garbage.Remove());
			return OpenTK.Graphics.OpenGL.GL.CreateProgram();
		}
		private static void Delete(int program)
		{
			Program.garbage.Add(program);
		}
		#endregion
	}
}
