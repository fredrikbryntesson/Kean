// 
//  ResizeDirection.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009-2011 Simon Mika
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

namespace Kean.Gui.Backend
{
    [Flags]
    public enum ResizeDirection
    {
        None = 0x0,
        Left = 0x2,
        Right = 0x3,
        Top = 0x4,
        Bottom = 0xc,
        LeftTop = Left | Top,
        LeftBottom = Left | Bottom,
        RightTop = Right | Top,
        RightBottom = Right | Bottom,
    }
}
