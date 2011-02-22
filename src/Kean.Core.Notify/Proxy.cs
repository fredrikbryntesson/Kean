using System;
using Kean.Core.Notify.Extension;
using Kean.Core.Basis.Extension;

namespace Kean.Core.Notify
{
	public class Proxy<T> :
		Abstract<T>
	{
		object @lock = new object();
		Func<T> get;
		Action<T> set;
		bool initialized;
		T value;
		public override T Value
		{
			get 
			{
				lock (this.@lock)
					return this.get();
			}
			set
			{
				bool update;
				lock (this.@lock)
					update = !(this.initialized && value.Equals(this.value));
				if (update && this.OnChange.Call(value))
				{
					this.set(value);
					lock (this.@lock)
					{
						this.initialized = true;
						this.value = value;
					}
					this.Changed.Call(this.Value);
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
