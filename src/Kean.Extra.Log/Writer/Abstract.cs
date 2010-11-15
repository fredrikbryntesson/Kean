using System;
using Error = Kean.Core.Error;

namespace Kean.Extra.Log.Writer
{
	public abstract class Abstract :
		IDisposable
	{
		protected Abstract()
		{
		}
		~Abstract()
		{
			(this as IDisposable).Dispose();
		}
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion

		public abstract void Add(Error.IError entry);
		public abstract void Close();
	}
}
