using System;
using Kean.Core.Basis.Extension;

namespace Kean.Core.Notify
{
	public class Proxy<T> :
		Abstract<T>
	{
		public Func<T> Get { get; set; }
		public Action<T> Set { get; set; }
		public override T Value
		{
			get { return this.Get(); }
			set
			{
				if (!value.Equals(this.Value) && this.OnChange(value))
				{
					this.Set.Call(value);
					this.Changed.Call(this.Get());
				}
			}
		}
		public override event Action<T> Changed;
		public override event OnChange<T> OnChange;
	}
}
