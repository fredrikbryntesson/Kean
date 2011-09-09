// 
//  Reader.cs
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
namespace Kean.IO
{
	public interface ICharacterWriter :
		IOutDevice
	{
		char[] NewLine { get; set; }

		bool Write(char value);
		bool Write(string value);
		bool Write<T>(T value) where T : IConvertible;
		bool Write(char[] buffer);
		bool Write(char[] buffer, int index, int count);
		bool Write(string format, params object[] arguments);

		bool WriteLine();
		bool WriteLine(string value);
		bool WriteLine<T>(T value) where T : IConvertible;
		bool WriteLine(char[] buffer);
		bool WriteLine(char[] buffer, int index, int count);
		bool WriteLine(string format, params object[] arguments);
	}
}
