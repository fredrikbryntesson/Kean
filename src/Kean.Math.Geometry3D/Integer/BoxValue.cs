// 
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Kean.Core.Basis.Extension;

namespace Kean.Math.Geometry3D.Integer
{
    public struct BoxValue :
        Abstract.IBox<PointValue, SizeValue, int>
    {
        PointValue leftTopFront;
        SizeValue size;
        public PointValue LeftTopFront
        {
            get { return this.leftTopFront; }
            set { this.leftTopFront = value; }
        }
        public SizeValue Size
        {
            get { return this.size; }
            set { this.size = value; }
        }
        public int Left { get { return this.leftTopFront.X; } }
        public int Top { get { return this.leftTopFront.Y; } }
        public int Front { get { return this.leftTopFront.Z; } }
        public int Width { get { return this.size.Width; } }
        public int Height { get { return this.Size.Height; } }
        public int Depth { get { return this.Size.Depth; } }
        public BoxValue(int left, int top, int front, int width, int height, int depth)
        {
            this.leftTopFront = new PointValue(left, top, front);
            this.size = new SizeValue(width, height, depth);
        }
        public BoxValue(PointValue leftTopFront, SizeValue size)
        {
            this.leftTopFront = leftTopFront;
            this.size = size;
        }
        #region Casts
        public static implicit operator string(BoxValue value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator BoxValue(string value)
        {
            BoxValue result = new BoxValue();
            try
            {
                string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 6)
                    result = new BoxValue((PointValue)(values[0] + " " + values[1] + " " + values[2]), (SizeValue)(values[3] + " " + values[4] + " " + values[5]));
            }
            catch
            {
            }
            return result;
        }
        #endregion
        #region Object Overrides
        public override string ToString()
        {
            return this.leftTopFront.ToString() + " " + this.size.ToString();
        }
        public override int GetHashCode()
        {
            return this.leftTopFront.GetHashCode() ^ this.size.GetHashCode();
        }
        #endregion
    }
}
