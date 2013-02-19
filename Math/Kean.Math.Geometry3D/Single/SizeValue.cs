// 
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

namespace Kean.Math.Geometry3D.Single
{
	public struct SizeValue :
		Abstract.ISize<float>, Abstract.IVector<float>
	{
		float width;
		float height;
        float depth;
        #region ISize<float>
        public float Width
		{
			get { return this.width; }
			set { this.width = value; }
		}
		public float Height
		{
			get { return this.height; }
			set { this.height = value; }
		}
        public float Depth
        {
            get { return this.depth; }
            set { this.depth = value; }
        }
        #endregion
        #region IVector<float> Members
        float Abstract.IVector<float>.X { get { return this.width; } }
        float Abstract.IVector<float>.Y { get { return this.height; } }
        float Abstract.IVector<float>.Z { get { return this.depth; } }
        #endregion
        public SizeValue(float width, float height, float depth)
		{
			this.width = width;
			this.height = height;
            this.depth = depth;
		}
        #region Casts
        public static implicit operator SizeValue(Integer.SizeValue value)
        {
            return new SizeValue(value.Width, value.Height, value.Depth);
        }
        public static explicit operator Integer.SizeValue(SizeValue value)
        {
            return new Integer.SizeValue((Kean.Math.Integer)(value.Width), (Kean.Math.Integer)(value.Height), (Kean.Math.Integer)(value.Depth));
        }
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
            return 33 * (33 * this.Width.GetHashCode() ^ this.Height.GetHashCode()) ^ this.Depth.GetHashCode();
        }
		public override string ToString()
		{
			return ((Size)this).ToString();
		}
		public string ToString(string format)
		{
			return ((Size)this).ToString(format);
		}
		#endregion
    }
}
