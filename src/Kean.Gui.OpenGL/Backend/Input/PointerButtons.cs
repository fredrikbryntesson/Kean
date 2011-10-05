﻿// 
//  PoinerButtons.cs
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

namespace Kean.Gui.OpenGL.Backend.Input
{
    class PointerButtons :
        Gui.Backend.PointerButtons
    {
        OpenTK.Input.MouseDevice targetPointer;

        public override int Count
        {
            get { return this.targetPointer.NumberOfButtons; }
        }
        public override bool this[int index]
        {
            get { return this.targetPointer[(OpenTK.Input.MouseButton)index]; }
        }
        internal PointerButtons(OpenTK.Input.MouseDevice targetPointer)
        {
            this.targetPointer = targetPointer;
        }
    }
}
