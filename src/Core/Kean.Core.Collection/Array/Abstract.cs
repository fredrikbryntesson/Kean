// 
//  Abstract.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009 Simon Mika
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

namespace Kean.Core.Collection.Array
{
	public abstract class Abstract<T>
	{
		T items;
		static readonly T[] EmptyArray = new T[0]; 
		
		protected T this[int index]
		{
			get 
			{
				if 
			}
			set
			{
			}
		}
		protected int Count { get; private set; }
		public int Capacity
		{
			get { return this.items.Length; }
			set
			{
				if (value < this.Count)
					throw new Exception.InvalidArgument("Capacity can't be set to {0}, because collection contains {1} elements.", value, this.Count);
				System.Array.Resize<T>(ref this.items, value);
			}
		}

		protected Abstract()
		{
		}
		
		private void VerifyIndex(int index)
		{
			if ((uint) index >= 
		}
		
		public void Trim()
		{
			this.Capacity = this.Count;
		}
	}
}
