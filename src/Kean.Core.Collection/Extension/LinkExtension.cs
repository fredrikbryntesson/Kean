// 
//  Link.cs
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

namespace Kean.Core.Collection.Extension
{
	public static class LinkExtension
	{
		public static L Add<L, T>(this L link, T item)
			where L : class, Interface.ILink<L, T>, new() 
		{
			return new L()
			{
				Head = item,
				Tail = link,
			};
		}
		public static T Get<L, T>(this L link, int index)
			where L : class, Interface.ILink<L, T>, new() 
		{
			if (object.ReferenceEquals(link, null))
				throw new Exception.InvalidIndex();
			return index == 0 ? link.Head : Link.Get<L, T>(link.Tail, index - 1);
		}
		public static void Set<L, T>(this L link, int index, T item)
			where L : class, Interface.ILink<L, T>, new() 
		{
			if (object.ReferenceEquals(link, null))
				throw new Exception.InvalidIndex();
			else if (index == 0)
				link.Head = item;
			else
				Link.Set<L, T>(link.Tail, index - 1, item);
		}
        public static int Count<L, T>(this L link) 
			where L : class, Interface.ILink<L, T>, new() 
        {
            return object.ReferenceEquals(link, null) ? 0 : (1 + Link.Count<L, T>(link.Tail));
        }
		public static L Insert<L, T>(this L link, int index, T element)
			where L : class, Interface.ILink<L, T>, new() 
		{
			if (object.ReferenceEquals(link, null) && index > 0)
				throw new Exception.InvalidIndex();
			return (index == 0) ? Link.Add(link, element) : Link.Insert<L, T>(link.Tail, index - 1, element);
		}
		public static L Remove<L, T>(this L link, int index)
			where L : class, Interface.ILink<L, T>, new() 
		{
			if (object.ReferenceEquals(link, null))
				throw new Exception.InvalidIndex();
			return (index == 0) ? link.Tail : Link.Remove<L, T>(link.Tail, index - 1);
		}
		public static L Remove<L, T>(this L link, int index, out T element)
			where L : class, Interface.ILink<L, T>, new() 
		{
			L result;
			if (object.ReferenceEquals(link, null))
				throw new Exception.InvalidIndex();
			else if (index == 0)
			{
				element = link.Head;
				result = link.Tail;
			}
			else
				result = Link.Remove<L, T>(link.Tail, index - 1, out element);
			return result;
		}
		public static bool Equals<L, T>(this L link, L other)
			where L : class, Interface.ILink<L, T>, new() 
		{
			return object.ReferenceEquals(link, null) && object.ReferenceEquals(other, null) || object.ReferenceEquals(link, null) && object.ReferenceEquals(other, null) && link.Head.Equals(other.Head) && Link.Equals(link.Tail, other.Tail);
		}
        public static R Fold<L, T, R>(this L link, Func<T, R, R> function) 
			where L : class, Interface.ILink<L, T>, new() 
		{
			return Link.Fold(link, function, default(R));
		}
        public static R Fold<L, T, R>(this L link, Func<T, R, R> function, R initial) 
			where L : class, Interface.ILink<L, T>, new() 
		{
			return object.ReferenceEquals(link, null) ? initial : function(link.Head, Link.Fold(link.Tail, function, initial));
		}
        public static R FoldReverse<L, T, R>(this L link, Func<T, R, R> function) 
			where L : class, Interface.ILink<L, T>, new() 
        {
            return Link.FoldReverse(link, function, default(R));
        }
        public static R FoldReverse<L, T, R>(this L link, Func<T, R, R> function, R initial) 
			where L : class, Interface.ILink<L, T>, new() 
        {
            return object.ReferenceEquals(link, null) ? initial : Link.Fold(link.Tail, function, function(link.Head, initial));
		}
        public static void Apply<L, T>(this L link, Action<T> function) 
			where L : class, Interface.ILink<L, T>, new() 
        {
			if (!object.ReferenceEquals(link, null))
			{
				function(link.Head);
				Link.Apply(link.Tail, function);
			}
        }
        public static R Map<L, T, R, S>(this L link, Func<T, S> function) 
			where L : class, Interface.ILink<L, T>, new() 
			where R : class, Interface.ILink<R, S>, new()
        {
			return object.ReferenceEquals(link, null) ? null : new R() 
			{ 
				Head = function(link.Head),
				Tail = Link.Map<L, T, R, S>(link.Tail, function),
			};
        }
	}
}