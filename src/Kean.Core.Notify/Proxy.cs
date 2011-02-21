using System;
using Kean.Core.Basis.Extension;

namespace Kean.Core.Notify
{
	public class Proxy<T> :
		Abstract<T>
	{
		Func<T> get;
		Action<T> set;
		public override T Value
		{
			get { return this.get(); }
			set
			{
				if (!value.Equals(this.Value) && this.OnChange(value))
				{
					this.set(value);
					this.Changed.Call(this.get());
				}
			}
		}
		public override event Action<T> Changed;
		public override event OnChange<T> OnChange;
		#region Constructors
		public Proxy(Func<T> get, Action<T> set)
		{
			this.get = get;
			this.set = set;
		}
		#endregion
	}
}
