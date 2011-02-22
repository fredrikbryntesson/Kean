using System;

namespace Kean.Core.Notify
{
	public abstract class Abstract<T>
	{
		public abstract T Value { get; set; }
		public abstract event Action<T> Changed;
		public abstract event OnChange<T> OnChange;
		public void Update(Abstract<T> changes)
		{
			if (!object.ReferenceEquals(this, changes))
				this.Value = changes.Value;
		}
		#region Object Overrides
		public override string ToString()
		{
			return this.Value.ToString();
		}
		#endregion
		#region Casts
		public static implicit operator Abstract<T>(T value)
		{
			return new Notifier<T>(value);
		}
		public static implicit operator T(Abstract<T> value)
		{
			return value.Value;
		}
		public static Abstract<T> operator +(Abstract<T> left, Action<T> right)
		{
			left.Changed += right;
			return left;
		}
		public static Abstract<T> operator -(Abstract<T> left, Action<T> right)
		{
			left.Changed -= right;
			return left;
		}
		public static Abstract<T> operator +(Abstract<T> left, OnChange<T> right)
		{
			left.OnChange += right;
			return left;
		}
		public static Abstract<T> operator -(Abstract<T> left, OnChange<T> right)
		{
			left.OnChange -= right;
			return left;
		}
		#endregion
	}
}
