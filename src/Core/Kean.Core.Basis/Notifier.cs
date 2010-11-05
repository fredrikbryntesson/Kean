// 
//  Notify.cs
//  
//  Author:
//       smika <${AuthorEmail}>
//  
//  Copyright (c) 2010 smika
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

namespace Kean.Core.Basis
{
	public class Notifier<T> :
		INotifier<T>
	{
		T value;
		event Action<T> changed;
		
		public Notifier()
		{ }
		public Notifier(T value)
		{
			this.value = value;
		}
		public Notifier(Action<T> changed)
		{
			this.changed = changed;
		}
		public Notifier(T value, Action<T> changed)
		{
			this.value = value;
			this.changed = changed;
		}
		
		#region INotfier<T>
		T INotifier<T>.Value
		{
			get { return this.value; }
			set
			{
				if (!object.Equals(value, this.value))
				{
					this.value = value;
					this.changed.Invoke(this.value);
				}
			}
		}
		event Action<T> INotifier<T>.Changed
		{
			add { this.changed += value; }
			remove { this.changed -= value; }
		}
		void INotifier<T>.Update(INotifier<T> changes)
		{
			if (!object.ReferenceEquals(this, changes) && !changes.Changed.NotNull())
				(this as INotifier<T>).Value = changes.value;
		}
		#endregion
		
		
		public static implicit operator Notifier<T>(T value)
		{
			return new Notifier(value);
		}
		public static implicit operator T(Notifier<T> value)
		{
			return value.value;
		}
		public static Notify<T> operator +(Notifier<T> left, Action<T> right)
		{
			left.changed += right;
			return left;
		}
		public static Notify<T> operator -(Notifier<T> left, Action<T> right)
		{
			left.changed -= right;
			return left;
		}
	}
}
