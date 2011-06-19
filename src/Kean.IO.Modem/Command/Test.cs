// 
//  Test.cs
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
using Collection = Kean.Core.Collection;
using Kean.Core.Basis.Extension;

namespace Kean.IO.Modem.Command
{
	public class Test :
		Abstract
	{
		public Collection.IImmutableVector<string> Arguments { get; private set; }
		protected override string Command { get { return this.Name + "=?"; } }

		internal Test(string name) :
			base(name) { }

		protected override bool Parse (string response)
		{
			if (response.NotEmpty() && response.StartsWith(this.Name + ": (") && response.EndsWith(")"))
			{
				int start = this.Name.Length + 2;
				this.Arguments = new Collection.Wrap.ImmutableVector<string>(response.Substring(start, response.Length - start - 2).Split(new string[] { "),(" }));
			}
			else
				this.Arguments = new Collection.Vector<string>(0);
		}
	}
}

