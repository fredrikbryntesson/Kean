﻿// 
//  Shell.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
using System;

namespace Kean.Math.Geometry2D.Single
{
    public class Shell : Abstract.Shell<Transform, TransformValue, Shell, ShellValue, Box, BoxValue, Point, PointValue, Size, SizeValue, Kean.Math.Single, float>
    {
        public override ShellValue Value { get { return (ShellValue)this; } }
        public Shell() { }
        public Shell(Kean.Math.Single left, Kean.Math.Single right, Kean.Math.Single top, Kean.Math.Single bottom) : base(left, right, top, bottom) { }
        public Box Decrease(Size size)
        {
            return new Box(this.Left, this.Top, size.Width - this.Left - this.Right, size.Height - this.Top - this.Bottom);
        }
        public Box Increase(Size size)
        {
            return new Box(-this.Left, -this.Right, size.Width + this.Left + this.Right, size.Height + this.Top + this.Bottom);
        }
        #region Casts
        public static explicit operator ShellValue(Shell value)
        {
            return new ShellValue(value.Left, value.Right, value.Top, value.Bottom);
        }
        #endregion
    }
}