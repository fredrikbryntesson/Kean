// 
//  Action.cs
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

namespace Kean.Core.Basis.Extension
{
	public static class ActionExtension
	{
		public static void Call(this Action me)
		{
			if (me.NotNull())
				me.Invoke();
		}
		public static void Call<T>(this Action<T> me, T argument)
		{
			if (me.NotNull())
				me.Invoke(argument);
		}
		public static void Call<T, S>(this Action<T, S> me, T argument1, S argument2)
		{
			if (me.NotNull())
				me.Invoke(argument1, argument2);
		}
	}
}
