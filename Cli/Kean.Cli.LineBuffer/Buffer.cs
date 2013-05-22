// 
//  Buffer.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2013 Simon Mika
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;

namespace Kean.Cli.LineBuffer
{
	class Buffer
	{
		Collection.IList<char> builder = new Collection.Linked.List<char>();

		public Buffer()
		{ }

		public override string ToString()
		{
			return this.builder.Fold<char, string>((c, result) => result + c, "");
		}
		public void Insert(int index, char c)
		{
			this.builder.Insert(index, c);
		}
		public void Append(char c)
		{
			this.builder.Add(c);
		}
		public string Substring(int index, int length)
		{
			string result = "";
			if (length != 0)
			{
				int count = this.builder.Count();
				int sliceIndex = (index >= 0) ? index : count + index;
				int sliceLength = (length > 0) ? length : count + length - sliceIndex;
				result = ((Collection.IVector<char>)this.builder.Slice(sliceIndex, sliceLength)).Fold<char, string>((c, res) => res + c, "");
			}
			return result;
		}
		public string Substring(int index)
		{
			return index >= 0 ?
				this.Substring(index, this.builder.Count - index) :
				this.Substring(this.builder.Count + index, -index);
		}
		public void Delete(int index)
		{
			this.builder.Remove(index);
		}
		public void Clear()
		{
			this.builder.Clear();
		}
		public void Set(string value)
		{
			this.builder = new Collection.Linked.List<char>();
			this.builder.Add(value.ToCharArray());
		}
	}
}
