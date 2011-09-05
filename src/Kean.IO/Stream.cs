using System;

namespace Kean.IO
{
	public abstract class Stream :
		IDisposable
	{
		public abstract bool Opened { get; }
		public abstract bool Ended { get; }
		protected Stream()
		{ }
		~Stream()
		{
			(this as IDisposable).Dispose();
		}
		public abstract int Read();
		public abstract int Peek();
		public abstract void Write(byte value);
		public abstract void Close();


		#region IDisposable Members
		void IDisposable.Dispose()
		{
			if (this.Opened)
				this.Close();
		}
		#endregion
	}
}
