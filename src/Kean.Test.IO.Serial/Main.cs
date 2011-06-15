// 
//  Main.cs
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
using Target = Kean.IO.Serial;
using Kean.Core.Collection.Extension;

namespace Kean.Test.IO.Serial
{
	class MainClass
	{
		public static void Main(string[] arguments)
		{
			Target.Port.Available.Apply(p => Console.WriteLine(p.Device));
			Console.WriteLine("Hello World!");
			Target.Port device = new Target.Port("/dev/ttyACM0");
			device.Open("57600 8n1");
			device.WriteLine("AT");
			Console.Write(device.Read());
			device.Close();
		}
	}
}
