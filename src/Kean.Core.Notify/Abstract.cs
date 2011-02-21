using System;

namespace Kean.Core.Notify
{
	public abstract class Abstract<T> :
		INotify<T>
	{
		#region INotify<T> Members
		public abstract T Value { get; set; }
		public abstract event Action<T> Changed;
		public abstract event OnChange<T> OnChange;
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
