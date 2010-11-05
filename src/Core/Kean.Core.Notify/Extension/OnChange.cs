// 
//  OnChange.cs
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
using Kean.Core.Basis.Extension;
using Kean.Core.Notify.Extension;

namespace Kean.Core.Notify.Extension
{
	public static class OnChange
	{
		/// <summary>
		/// Invokes the delegates in the multicast list until one return false.
		/// </summary>
		/// <param name="me">
		/// A <see cref="OnChange<T>"/> with delegates to call.
		/// </param>
		/// <param name="value">
		/// A <see cref="T"/> that contains the new value.
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/> that is true if all delegates in me return true, else false.
		/// </returns>
		public static bool Call<T>(this OnChange<T> me, T value)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] delegates = me.GetInvocationList();
				for (int i = 0; i < delegates.Length && result; i++)
					result &= (delegates[i] as OnChange<T>).Call(value);
			}
			return result;
		}
	}
}
