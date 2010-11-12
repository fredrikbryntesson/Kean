//
//  List.cs
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
namespace Kean.Core.Collection.Hooked
{
	public class List<T> :
		Vector<T>,
		IList<T>
	{
		IList<T> data;
		public event Action<T> Added;
		public event Func<T, bool> OnAdd;
		public event Action<int, T> Removed;
		public event Func<int, T, bool> OnRemove;
		public List (IList<T> data) :
			base(data)
		{
			this.data = data;
		}
		public void Add(T item)
		{
			bool add = true;
			if (this.OnAdd.NotNull ()) {
				Delegate[] onAdd = this.OnAdd.GetInvocationList ();
				for (int i = 0; add && i < onAdd.Length; i++)
					add &= (onAdd[i] as Func<T, bool>) (item);
			}
			if (add) {
				this.data.Add(item);
				this.Added.Call(item);
				}
		}
		public T Remove()
		{
			T result = this.data[this.Count - 1];
			bool remove = true;
			if (this.OnRemove.NotNull ()) {
				Delegate[] onRemove = this.OnRemove.GetInvocationList ();
				for (int i = 0; remove && i < onRemove.Length; i++)
					remove &= (onAdd[i] as Func<int, T, bool>) (this.Count - 1, result);
			}
			if (remove) {
				result = this.data.Remove();
				this.Removed.Call(result);
			} else
				result = default(T);
			return result;
		}
		public T Remove(int index)
		{
			T result = this.data[index];
			bool remove = true;
			if (this.OnRemove.NotNull ()) {
				Delegate[] onRemove = this.OnRemove.GetInvocationList ();
				for (int i = 0; remove && i < onRemove.Length; i++)
					remove &= (onAdd[i] as Func<int, T, bool>) (index, result);
			}
			if (remove) {
				result = this.data.Remove(index);
				this.Removed.Call(result);
			} else
				result = default(T);
			return result;
		}
	}
}

