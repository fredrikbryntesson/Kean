// 
//  Main.cs
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

namespace Kean.Test.Run
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Core.Collection.Vector.Test();
			Core.Collection.List.Test();
			Core.Collection.Queue.Test();
			Core.Collection.Stack.Test();
			Core.Collection.Linked.List.Test();
			Core.Collection.Linked.Queue.Test();
			Core.Collection.Linked.Stack.Test();
			Core.Collection.Array.Vector.Test();
			Core.Collection.Array.List.Test();
			Core.Collection.Array.Queue.Test();
			Core.Collection.Array.Stack.Test();

			Core.Error.Error.Test();
		}
	}
}
