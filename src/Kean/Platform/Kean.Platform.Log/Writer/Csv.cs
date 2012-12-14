//
//  Csv
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 Simon Mika
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
using Kean.Core.Extension;
using Error = Kean.Core.Error;

namespace Kean.Platform.Log.Writer
{
	public class Csv :
		Abstract
	{
		bool append;
		System.IO.TextWriter writer;
		public string Filename { get; set; }
		public override Action<Error.IError> Open()
		{
			if (this.writer.IsNull())
			{
				this.writer = new System.IO.StreamWriter(this.Filename, this.append);
				if (!this.append)
					this.writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", "Time", "Level", "Title", "Message", "Assembly", "Version", "Class", "Method", "Source", "Line", "Column");
				this.append = true;
			}
			return (Error.IError entry) =>
			{
				if (this.writer.NotNull())
					this.writer.WriteLine("{0},{1},\"{2}\",\"{3}\",\"{4}\",{5},{6},{7},{8},{9},{10}",
					entry.Time,
					entry.Level,
					entry.Title.Replace("\"", "\"\""),
					entry.Message.Replace("\"", "\"\""),
					entry.AssemblyName,
					entry.AssemblyVersion,
					entry.Type,
					entry.Method,
					entry.Filename,
					entry.Line,
					entry.Column);
			};
		}
		public override void Close()
		{
			if (this.writer.NotNull())
			{
				this.writer.Close();
				this.writer = null;
			}
		}
	}
}
