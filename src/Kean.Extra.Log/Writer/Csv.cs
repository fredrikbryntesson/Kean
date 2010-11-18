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
using Error = Kean.Core.Error;

namespace Kean.Extra.Log.Writer
{
	public class Csv :
		Abstract
	{
		System.IO.TextWriter writer;
		public string Filename { get; set; }
		public override Action<Error.IError> Open()
		{
			this.writer = new System.IO.StreamWriter(this.Filename, true);
			this.writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8}", "Time", "Level", "Title", "Message", "Assembly", "Version", "Source", "Line", "Column"); 
			return (Error.IError entry) => {
				System.Reflection.AssemblyName assembly = entry.Assembly.GetName();
				this.writer.WriteLine("{0},{1},\"{2}\",\"{3}\",\"{4}\",{5},{6},{7},{8}", 
				entry.Time, 
				entry.Level,
				entry.Title.Replace("\"", "\"\""),
				entry.Message.Replace("\"", "\"\""),
				assembly.Name,
				assembly.Version,
				entry.Trace.GetFrame(0).GetFileName(), 
				entry.Trace.GetFrame(0).GetFileLineNumber(), 
				entry.Trace.GetFrame(0).GetFileColumnNumber()); 
			};
		}
		public override void Close()
		{
			writer.Close();
		}
	}
}
