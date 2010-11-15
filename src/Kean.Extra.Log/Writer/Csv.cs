using System;

namespace Kean.Extra.Log.Writer
{
	public class Csv :
		Abstract
	{
		System.IO.TextWriter writer;
		public void Open(string filename)
		{
			this.writer = new System.IO.StreamWriter(filename);
			this.writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}", "Time", "Level", "Title", "Message", "File", "Line", "Column"); 
		}
		public override void Add(Kean.Core.Error.IError entry)
		{
			this.writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}", 
				entry.Time, 
				entry.Level, 
				entry.Title, 
				entry.Message, 
				entry.Trace.GetFrame(0).GetFileName(), 
				entry.Trace.GetFrame(0).GetFileLineNumber(), 
				entry.Trace.GetFrame(0).GetFileColumnNumber());
		}
		public override void Close()
		{
			writer.Close();
		}
	}
}
