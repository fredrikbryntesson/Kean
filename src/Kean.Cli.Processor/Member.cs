// 
//  Member.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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
using Kean.Core;
using Kean.Core.Extension;
using Reflect = Kean.Core.Reflect;

namespace Kean.Cli.Processor
{
	abstract class Member :
		Kean.Core.IComparable<Member>
	{
		public string Name { get; private set; }
		Reflect.Member backend;
		protected Object Parent { get; set; }
		protected Member(MemberAttribute attribute, Reflect.Member backend, Object parent)
		{
			this.Name = attribute.NotNull() ? attribute.Name : null;
			this.backend = backend;
			this.Parent = parent;
		}

		#region IComparable<Member> Members
		public Order Compare(Member other)
		{
			return this.Name.CompareWith(other.Name);
		}
		#endregion
	}
}
