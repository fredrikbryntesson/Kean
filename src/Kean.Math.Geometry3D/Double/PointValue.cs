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
using Kean.Core.Extension;

namespace Kean.Math.Geometry3D.Double
{
	public struct PointValue :
        Abstract.IPoint<double>, Abstract.IVector<double>
	{
		double x;
		double y;
        double z;
		public double X
		{
			get { return this.x; }
			set { this.x = value; }
		}
		public double Y
		{
			get { return this.y; }
			set { this.y = value; }
		}
        public double Z
        {
            get { return this.z; }
            set { this.z = value; }
        }
        public PointValue(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
            this.z = z;
        }
        #region Casts
        public static implicit operator PointValue(Single.PointValue value)
        {
            return new PointValue(value.X, value.Y, value.Z);
        }
        public static implicit operator PointValue(Integer.PointValue value)
        {
            return new PointValue(value.X, value.Y, value.Z);
        }
        public static explicit operator Single.PointValue(PointValue value)
        {
            return new Single.PointValue((Kean.Math.Single)(value.X), (Kean.Math.Single)(value.Y), (Kean.Math.Single)(value.Z));
        }
        public static explicit operator Integer.PointValue(PointValue value)
        {
            return new Integer.PointValue((Kean.Math.Integer)(value.X), (Kean.Math.Integer)(value.Y), (Kean.Math.Integer)(value.Z));
        }
        public static implicit operator string(PointValue value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator PointValue(string value)
        {
            PointValue result = new PointValue();
            if (value.NotEmpty())
            {

                try
                {
					result = (PointValue)(Point)value;
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
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
        }
		public override string ToString()
		{
			return ((Point)this).ToString();
		}
		public string ToString(string format)
		{
			return ((Point)this).ToString(format);
		}
		#endregion
    }
}
