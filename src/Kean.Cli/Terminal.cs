// 
//  ITerminal.cs
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

namespace Kean.Cli
{
	public class Terminal :
		ITerminal
	{
		IO.ICharacterReader reader;
		IO.ICharacterWriter writer;
		#region Constructors
		public Terminal(IO.ICharacterDevice device) :
			this(device, device)
		{ }
		public Terminal(IO.ICharacterInDevice inDevice, IO.ICharacterOutDevice outDevice) :
			this(new IO.CharacterReader(inDevice), new IO.CharacterWriter(outDevice))
		{ }
		public Terminal(IO.ICharacterReader reader, IO.ICharacterWriter writer)
		{
			this.reader = reader;
			this.writer = writer;
		}
		#endregion
		#region ITerminal Members
		public bool Readable { get { return true; } }
		public bool Writeable { get { return true; } }
		#endregion
		#region ICharacterReader Members
		public string Resource { get { return this.reader.Resource; } }
		public int Row { get { return this.reader.Row; } }
		public int Column { get { return this.reader.Column; } }
		public char Last { get { return this.reader.Last; } }
		public bool Next() { return this.reader.Next(); }
		#endregion
		#region IInDevice Members
		public bool Empty { get { return this.reader.Empty; } }
		#endregion
		#region IDevice Members
		public bool Opened { get { return this.reader.Opened && this.writer.Opened; } }
		public bool Close() { return this.reader.Close() && this.writer.Close(); }
		#endregion
		#region IDisposable Members
		void IDisposable.Dispose() { this.Close(); }
		#endregion
		#region ICharacterWriter Members
		public char[] NewLine
		{
			get { return this.writer.NewLine; }
			set { this.writer.NewLine = value; }
		}
		public bool Write(params char[] buffer) { return this.writer.Write(buffer); }
		public bool Write(string value) { return this.writer.Write(value); }
		public bool Write<T>(T value) where T : IConvertible { return this.writer.Write(value); }
		public bool Write(System.Collections.Generic.IEnumerable<char> buffer) { return this.writer.Write(buffer); }
		public bool Write(string format, params object[] arguments) { return this.writer.Write(format, arguments); }
		public bool WriteLine() { return this.writer.WriteLine(); }
		public bool WriteLine(params char[] buffer) { return this.writer.WriteLine(buffer); }
		public bool WriteLine(string value) { return this.writer.WriteLine(value); }
		public bool WriteLine<T>(T value) where T : IConvertible { return this.writer.WriteLine(value); }
		public bool WriteLine(System.Collections.Generic.IEnumerable<char> buffer) { return this.writer.WriteLine(buffer); }
		public bool WriteLine(string format, params object[] arguments) { return this.writer.WriteLine(format, arguments); }
		#endregion
	}
}
