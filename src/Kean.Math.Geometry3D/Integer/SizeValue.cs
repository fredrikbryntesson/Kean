﻿// 
//  SizeValue.cs
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
using Kean.Core.Extension;

namespace Kean.Math.Geometry3D.Integer
{
	public struct SizeValue :
		Abstract.ISize<int>, Abstract.IVector<int>
	{
		int width;
		int height;
        int depth;
        #region ISize<int>
        public int Width
		{
			get { return this.width; }
			set { this.width = value; }
		}
		public int Height
		{
			get { return this.height; }
			set { this.height = value; }
		}
        public int Depth
        {
            get { return this.depth; }
            set { this.depth = value; }
        }
        #endregion
        #region IVector<int> Members
        int Abstract.IVector<int>.X { get { return this.width; } }
        int Abstract.IVector<int>.Y { get { return this.height; } }
        int Abstract.IVector<int>.Z { get { return this.depth; } }
        #endregion
        public SizeValue(int width, int height, int depth)
		{
			this.width = width;
			this.height = height;
            this.depth = depth;
		}
        #region Casts
        public static implicit operator string(SizeValue value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator SizeValue(string value)
        {
            SizeValue result = new SizeValue();
            if (value.NotEmpty())
            {

                try
                {
					result = (SizeValue)(Size)value;
				}
                catch
                {
                }
            }
            return result;
        }
        #endregion
        #region Object Overrides
        public override int GetHashCode()
        {
            return this.Width.GetHashCode() ^ this.Height.GetHashCode() ^ this.Depth.GetHashCode();
        }
		public override string ToString()
		{
			return this.ToString(false);
		}
		public string ToString(bool commaSeparated)
		{
			return ((Size)this).ToString(commaSeparated);
		}
        #endregion
    }
}
