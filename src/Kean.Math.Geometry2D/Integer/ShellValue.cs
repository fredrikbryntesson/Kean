// 
//  ShellValue.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
namespace Kean.Math.Geometry2D.Integer
{
    public struct ShellValue :
        Abstract.IShell<int>
    {
        int left;
        int right;
        int top;
        int bottom;
        public int Left { get { return this.left; } }
        public int Right { get { return this.right; } }
        public int Top { get { return this.top; } }
        public int Bottom { get { return this.bottom; } }

        public ShellValue(int left, int right, int top, int bottom)
        {
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
        }
        #region Casts
        public static implicit operator Shell(ShellValue value)
        {
            return new Shell(value.Left, value.Right, value.Top, value.Bottom);
        }
        public static explicit operator ShellValue(Shell value)
        {
            return new ShellValue(value.Left, value.Right, value.Top, value.Bottom);
        }
        #endregion
    }
}
