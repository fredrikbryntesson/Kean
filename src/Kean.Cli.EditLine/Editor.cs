// 
//  Editor.cs
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
using Kean.Core;
using Kean.Core.Extension;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using IO = Kean.IO;

namespace Kean.Cli.EditLine
{
	public class Editor
	{
		Object root;
		Object current;

		IO.Reader reader;
		System.IO.TextWriter writer;
		public Editor(object root, IO.Stream reader, System.IO.TextWriter writer) :
			this(root, new IO.Reader(reader), writer)
		{ }
		public Editor(object root, IO.Reader reader, System.IO.TextWriter writer)
		{
			this.current = this.root = new Object(root);
			this.reader = reader;
			this.writer = writer;
		}
		public void Read()
		{
			bool exit = false;
			System.Text.StringBuilder line = new System.Text.StringBuilder();
			while (this.reader.Next() && !exit)
			{
				switch (this.reader.Current)
				{
					case (char)4:
						exit = true;
						break;
					default:
						this.writer.WriteLine(this.reader.Current + ": " + ((int)this.reader.Current));
						break;
				}
			}
		}
	}
}
