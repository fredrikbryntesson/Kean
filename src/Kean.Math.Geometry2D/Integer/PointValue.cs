﻿// 
//  PointValue.cs
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
using System;
using Kean.Core.Basis.Extension;

namespace Kean.Math.Geometry2D.Integer
{
	public struct PointValue :
		Abstract.IPoint<int>, Abstract.IVector<int>
	{
		int x;
		int y;
		public int X
		{
			get { return this.x; }
			set { this.x = value; }
		}
		public int Y
		{
			get { return this.y; }
			set { this.y = value; }
		}
		public PointValue(int x, int y)
		{
			this.x = x;
			this.y = y;
        }
        #region Casts
        public static implicit operator string(PointValue value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator PointValue(string value)
        {
            PointValue result = new PointValue();
            try
            {
                string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 2)
                    result = new PointValue(Kean.Math.Integer.Parse(values[0]), Kean.Math.Integer.Parse(values[1]));
            }
            catch
            {
            }
            return result;
        }
        #endregion
        #region Object Overrides
        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
        public override string ToString()
        {
            return Kean.Math.Integer.ToString(this.X) + " " + Kean.Math.Integer.ToString(this.Y);
        }
        #endregion
    }
}
