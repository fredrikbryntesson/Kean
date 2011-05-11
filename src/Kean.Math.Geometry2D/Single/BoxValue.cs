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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
using System;
using Kean.Core.Basis.Extension;

namespace Kean.Math.Geometry2D.Single
{
    public struct BoxValue :
        Abstract.IBox<PointValue, SizeValue, float>
    {
        PointValue leftTop;
        SizeValue size;
        public PointValue LeftTop
        {
            get { return this.leftTop; }
            set { this.leftTop = value; }
        }
        public SizeValue Size
        {
            get { return this.size; }
            set { this.size = value; }
        }
        public float Width { get { return this.size.Width; } }
        public float Height { get { return this.Size.Height; } }

        public float Left { get { return this.leftTop.X; } }
        public float Top { get { return this.leftTop.Y; } }
        public float Right { get { return this.leftTop.X + this.size.Width; } }
        public float Bottom { get { return this.leftTop.Y + this.size.Height; } }
        public BoxValue(float left, float top, float width, float height)
        {
            this.leftTop = new PointValue(left, top);
            this.size = new SizeValue(width, height);
        }
        public BoxValue(PointValue leftTop, SizeValue size)
        {
            this.leftTop = leftTop;
            this.size = size;
        }
        #region Casts
        public static implicit operator BoxValue(Integer.BoxValue value)
        {
            return new BoxValue(value.LeftTop, value.Size);
        }
        public static explicit operator Integer.BoxValue(BoxValue value)
        {
            return new Integer.BoxValue((Integer.PointValue)(value.LeftTop), (Integer.SizeValue)(value.Size));
        }
        public static implicit operator string(BoxValue value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator BoxValue(string value)
        {
            BoxValue result = new BoxValue();
            if (value.NotEmpty())
            {
                try
                {
                    string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length == 4)
                        result = new BoxValue((PointValue)(values[0] + " " + values[1]), (SizeValue)(values[2] + " " + values[3]));
                }
                catch
                {
                }
            }
            return result;
        }
        #endregion
        #region Object Overrides
        public override string ToString()
        {
            return this.leftTop.ToString() + " " + this.size.ToString();
        }
        public override int GetHashCode()
        {
            return this.leftTop.GetHashCode() ^ this.size.GetHashCode();
        }
        #endregion
    }
}
