// 
//  ResizeDirectionExtension.cs
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

namespace Kean.Gui.OpenGL.Backend.Extension
{
	static class ResizeDirectionExtension
	{
		public static OpenTK.ResizeDirection Convert(this Gui.Backend.ResizeDirection me)
		{
			OpenTK.ResizeDirection result;
			switch (me)
			{
				default:
				case Gui.Backend.ResizeDirection.Left:
					result = OpenTK.ResizeDirection.Left;
					break;
				case Gui.Backend.ResizeDirection.Right:
					result = OpenTK.ResizeDirection.Right;
					break;
				case Gui.Backend.ResizeDirection.Top:
					result = OpenTK.ResizeDirection.Top;
					break;
				case Gui.Backend.ResizeDirection.Bottom:
					result = OpenTK.ResizeDirection.Bottom;
					break;
				case Gui.Backend.ResizeDirection.LeftTop:
					result = OpenTK.ResizeDirection.LeftTop;
					break;
				case Gui.Backend.ResizeDirection.LeftBottom:
					result = OpenTK.ResizeDirection.LeftBottom;
					break;
				case Gui.Backend.ResizeDirection.RightTop:
					result = OpenTK.ResizeDirection.RightTop;
					break;
				case Gui.Backend.ResizeDirection.RightBottom:
					result = OpenTK.ResizeDirection.RightBottom;
					break;
			}
			return result;
		}
	}
}
