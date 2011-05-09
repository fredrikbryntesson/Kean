// 
//  Point.cs
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

namespace Kean.Math.Geometry3D.Double
{
    public class Point : Abstract.Point<Transform, TransformValue, Point, PointValue, Size, SizeValue, Kean.Math.Double, double>
    {
        public override PointValue Value { get { return (PointValue)this; } }
        public Point() { }
        public Point(double x, double y, double z) : base(x, y, z) { }
        #region Casts
        public static implicit operator Point(Single.Point value)
        {
            return new Point(value.X, value.Y, value.Z);
        }
        public static implicit operator Point(Integer.Point value)
        {
            return new Point(value.X, value.Y, value.Z);
        }
        public static explicit operator Single.Point(Point value)
        {
            return new Single.Point((Kean.Math.Single)(value.X), (Kean.Math.Single)(value.Y), (Kean.Math.Single)(value.Z));
        }
        public static explicit operator Integer.Point(Point value)
        {
            return new Integer.Point((Kean.Math.Integer)(value.X), (Kean.Math.Integer)(value.Y), (Kean.Math.Integer)(value.Z));
        }
        public static implicit operator Point(PointValue value)
        {
            return new Point(value.X, value.Y, value.Z);
        }
        public static explicit operator PointValue(Point value)
        {
            return new PointValue(value.X, value.Y, value.Z);
        }
        public static implicit operator string(Point value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator Point(string value)
        {
            Point result = null;
            try
            {
                string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 3)
                    result = new Point(Kean.Math.Double.Parse(values[0]), Kean.Math.Double.Parse(values[1]), Kean.Math.Double.Parse(values[2]));
            }
            catch
            {
                result = null;
            }
            return result;
        }
        #endregion
    }
}
