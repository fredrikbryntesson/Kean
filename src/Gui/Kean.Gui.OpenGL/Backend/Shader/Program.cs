using System;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Core.Extension;
using Gpu = Kean.Draw.Gpu;
using Error = Kean.Core.Error;

namespace Kean.Gui.OpenGL.Backend.Shader
{
	public class Program :
		Gpu.Backend.IShader
	{
		public enum ArrayType
		{
			Uniform1,
			Uniform2,
			Uniform3,
			Uniform4,
		}
		int identifier;
		Vertex vertex;
		Fragment fragment;
		public Program(string fragment) :
			this(null, fragment)
		{ }
		public Program(string vertex, string fragment) :
			this(vertex.NotEmpty() ? new Vertex(vertex) : null, fragment.NotEmpty() ? new Fragment(fragment) : null)
		{ }
		public Program(Fragment fragment) :
			this(null, fragment)
		{ }
		public Program(Vertex vertex, Fragment fragment)
		{
			this.fragment = fragment;
			this.vertex = vertex;
			this.identifier = Program.Create();
			if (vertex.NotNull())
				vertex.Attach(this.identifier);
			if (fragment.NotNull())
				fragment.Attach(this.identifier);
		}
		~Program()
		{
			Error.Log.Wrap((Action)this.Dispose)();
		}
		public void Dispose()
		{
			if (this.vertex.NotNull())
			{
				//this.vertex.Deattach(this.identifier);
				this.vertex.Dispose();
				this.vertex = null;
			}
			if (this.fragment.NotNull())
			{
				//this.fragment.Deattach(this.identifier);
				this.fragment.Dispose();
				this.fragment = null;
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
		public void Unuse()
		{
			Program.Reset();
		}
		public void BindChannels(params Gpu.Image[] images)
		{
			int k = 0;
			for (int i = 0; i < images.Length; i++)
			{
				Gpu.Image image = images[i];
				if (image is Gpu.Packed)
				{
					string name = null;
					if (image is Gpu.Bgra)
						name = "bgr";
					else if (image is Gpu.Bgr)
						name = "bgr";
					else if (image is Gpu.Bgr)
						name = "monochrome";
					this.SetVariable(name + i, k++);
				}
				else if (image is Gpu.Planar)
				{
					if (image is Gpu.Yuv420)
					{
						this.SetVariable("monochrome" + i + "Y" + k, k++);
						this.SetVariable("monochrome" + i + "U" + k, k++);
						this.SetVariable("monochrome" + i + "V" + k, k++);
					}
				}
			}
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
			this.Unuse();
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
			this.Unuse();
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
			this.Unuse();
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
