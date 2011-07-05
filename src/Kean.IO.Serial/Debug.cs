// 
//  Debug.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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
using Kean.Core.Collection.Extension;

namespace Kean.IO.Serial
{
	public class Debug :
		IPort,
		IDisposable
	{
		IPort backend;
		public Debug(IPort backend)
		{
			this.backend = backend;
		}

		#region IPort implementation
		public bool Open(Settings settings)
		{
			bool result = this.backend.Open(settings);
			Console.WriteLine((result ? "# Succeded opening: " : "# Failed opening: ") + settings);
			return result;
		}

		public bool Close()
		{
			bool result = this.backend.Close();
			Console.WriteLine(result ? "# Succeded closing" : "# Failed closing");
			return result;
		}

		public void Write(params byte[] value)
		{
			Console.Write("> " + value.Fold((byte b, System.Text.StringBuilder a) => a.Append((char)b), new System.Text.StringBuilder()));
			this.backend.Write(value);
		}

		public void Write(string value)
		{
			Console.Write("> " + value);
			this.backend.Write(value);
		}

		public void WriteLine(string value)
		{
			Console.WriteLine("> " + value);
			this.backend.WriteLine(value);
		}

		public string Read()
		{
			string result = this.backend.Read();
			Console.Write("< " + result);
			return result;
		}

		public bool IsOpen { get { return this.backend.IsOpen; } }

		public string Device { get { return this.Device; } }

		public Settings Settings { get { return this.Settings; } }
		#endregion

		#region IDisposable implementation
		public void Dispose ()
		{
			this.backend.Dispose();
		}
		#endregion
	}
}

