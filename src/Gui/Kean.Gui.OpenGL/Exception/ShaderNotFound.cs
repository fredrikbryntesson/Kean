// 
//  ShaderNotFound.cs
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
using Error = Kean.Core.Error;

namespace Kean.Gui.OpenGL.Exception
{
	public class ShaderNotFound :
	   Exception
	{
		internal ShaderNotFound(string filename)
			: base(Error.Level.Warning, "Shader Not Found", "The shader file {0} was not found.", filename)
		{ }
		internal ShaderNotFound(System.Exception exception, string filename)
			: base(exception, Error.Level.Warning, "Shader Not Found", "The shader file {0} was not found.", filename)
		{ }
		internal ShaderNotFound(System.Reflection.Assembly assembly, string name)
			: base(Error.Level.Warning, "Shader Not Found", "The shader file with name {0} was not found in assembly {1} at {2}.", name, assembly.FullName, assembly.Location)
		{ }
		internal ShaderNotFound(System.Exception exception, System.Reflection.Assembly assembly, string name)
			: base(exception, Error.Level.Warning, "Shader Not Found", "The shader file with name {0} was not found in assembly {1} at {2}.", name, assembly.FullName, assembly.Location)
		{ }
	}
}
