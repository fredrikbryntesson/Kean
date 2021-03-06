// 
//  Point.cs (generated by template)
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2013 Simon Mika
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
using Kean.Extension;

namespace Kean.Math.Geometry3D.Single
{
	public struct Point:
	  IEquatable<Point>
	{
		public float X;
		public float Y;
		public float Z;

		public Point(float x, float y, float z) 
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}
		public Point(Geometry2D.Single.Point p, float z)
		{
			this.X = p.X;
			this.Y = p.Y;
			this.Z = z;
		}
		#region properties
		//public float Theta { get { return Math.Single.ArcusTangensExtended(Math.Single.SquareRoot()) } }
		public float Norm { get { return Math.Single.SquareRoot((Math.Single.Squared(this.X) + Math.Single.Squared(this.Y) + Math.Single.Squared(this.Z))); } }
		public float Azimuth { get { return Math.Single.ArcusTangensExtended(this.Y, this.X); } }
		public float Elevation
		{
			get
			{
				float result = new float();
				float r = this.Norm;
				if (r != result)
					result = Math.Single.ArcusCosinus(Math.Single.Clamp(this.Z / r, -1, 1));
				return result;
			}
		}
		#endregion
		#region Static Constants
		public static Point BasisX { get { return new Point(1, 0, 0); } }
		public static Point BasisY { get { return new Point(0, 1, 0); } }
		public static Point BasisZ { get { return new Point(0, 0, 1); } }
		#endregion
		#region Methods
		public float ScalarProduct(Point other)
		{
			return  this.X * other.X + this.Y * other.Y + this.Z * other.Z;
		}
		public Point VectorProduct(Point other)
		{
			return new Point(this.Y * other.Z - other.Y * this.Z, -(this.X * other.Z - other.X * this.Z), this.X * other.Y - other.X * this.Y);
		}
		public float Distance(Point other)
		{
			return (this - other).Norm;
		}
		#endregion
		#region Arithmetic Operators
		public static Point operator *(Transform left, Point right)
		{
			return new Point(left.A * right.X + left.D * right.Y + left.G * right.Z + left.J, left.B * right.X + left.E * right.Y + left.H * right.Z + left.K, left.C * right.X + left.F * right.Y + left.I * right.Z + left.L);
		}
		#endregion
		#region Static Creators
		public static Point Spherical(float radius, float azimuth, float elevation)
		{
			return new Point(radius * Math.Single.Cosine(azimuth) * Math.Single.Sine(elevation), radius * Math.Single.Sine(azimuth) * Math.Single.Sine(elevation), radius * Math.Single.Cosine(elevation));
		}
		public static Point Angles(float rx, float ry, float n)
		{
			float z = Math.Single.SquareRoot((n * n) / (1 + Math.Single.Squared(Math.Single.Tangens(ry)) + Math.Single.Squared(Math.Single.Tangens(rx))));
			//float z = n * Math.Single.Cosine(rx) * Math.Single.Cosine(ry);
			return new Point(z * Math.Single.Tangens(ry), z * Math.Single.Tangens(rx), z);
		}
		#endregion
		#region Arithmetic Vector - Vector Operators
		 public static Point operator +(Point left, Point right)
		{
			return new Point(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}
		public static Point operator +(Point left, Size right)
		{
			return new Point(left.X + right.Width, left.Y + right.Height, left.Z + right.Depth);
		}
		 public static Point operator +(Size left, Point right)
		{
			return new Point(left.Width + right.X, left.Height + right.Y, left.Depth + right.Z);
		}
		public static Point operator -(Point vector)
		{
			return new Point(-vector.X, -vector.Y, -vector.Z);
		}
		public static Point operator -(Point left, Size right)
		{
			return new Point(left.X - right.Width, left.Y - right.Height, left.Z - right.Depth);
		}
		public static Size operator -(Point left, Point right)
		{
			return new Size(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		}
		public static Point operator -(Size left, Point right)
		{
			return new Point(left.Width - right.X, left.Height - right.Y, left.Depth - right.Z);
		}
		public static Point operator *(Point left, Point right)
		{
			return new Point(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		}
		public static Point operator *(Point left, Size right)
		{
			return new Point(left.X * right.Width, left.Y * right.Height, left.Z * right.Depth);
		}
		public static Point operator *(Size left, Point right)
		{
			return new Point(left.Width * right.X, left.Height * right.Y,left.Depth * right.Z);
		}
		public static Point operator /(Point left, Point right)
		{
			return new Point(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
		}
		public static Point operator /(Point left, Size right)
		{
			return new Point(left.X / right.Width, left.Y / right.Height, left.Z / right.Depth);
		}
		public static Point operator /(Size left, Point right)
		{
			return new Point(left.Width / right.X, left.Height / right.Y,left.Depth / right.Z);
		}
		#endregion
		#region Arithmetic Vector and Scalar
		public static Point operator *(Point left, float right)
		{
			return new Point(left.X * right, left.Y * right, left.Z * right);
		}
		public static Point operator *(float left, Point right)
		{
			return  new Point(right.X * left, right.Y * left, right.Z * left);
		}
		public static Point operator /(Point left, float right)
		{
			return new Point(left.X / right, left.Y / right, left.Z / right);
		}
		#endregion
		#region Comparison Operators
		/// <summary>
		/// Defines equality.
		/// </summary>
		/// <param name="left">Point left of operator.</param>
		/// <param name="right">Point right of operator.</param>
		/// <returns>True if <paramref name="left"/> equals <paramref name="right"/> else false.</returns>
		public static bool operator ==(Point left, Point right)
		{
			return object.ReferenceEquals(left, right) ||
				!object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) && left.X == right.X && left.Y == right.Y && left.Z == right.Z;
		}
		/// <summary>
		/// Defines inequality.
		/// </summary>
		/// <param name="left">Point left of operator.</param>
		/// <param name="right">Point right of operator.</param>
		/// <returns>False if <paramref name="left"/> equals <paramref name="right"/> else true.</returns>
		public static bool operator !=(Point left, Point right)
		{
			return !(left == right);
		}
		 public static bool operator <(Point left, Point right)
		{
			return left.X < right.X && left.Y < right.Y && left.Z < right.Z;
		}
		public static bool operator >(Point left, Point right)
		{
			return left.X > right.X && left.Y > right.Y && left.Z > right.Z;
		}
		public static bool operator <=(Point left, Point right)
		{
			return left.X <= right.X && left.Y <= right.Y && left.Z <= right.Z;
		}
		public static bool operator >=(Point left, Point right)
		{
			return left.X >= right.X && left.Y >= right.Y && left.Z >= right.Z;
		}
		#endregion
		#region Object overides and IEquatable<VectorType>
		public override bool Equals(object other)
		{
			return (other is Point && this.Equals((Point)other));
		}
		// other is not null here.
		public bool Equals(Point other)
		{
			return this == other;
		}
		public override int GetHashCode()
		{
			return 33 * (33 * this.X.GetHashCode() ^ this.Y.GetHashCode()) ^ this.Z.GetHashCode();
		}
		public override string ToString()
		{
			return this.ToString("{0}, {1}, {2}");
		}
		public string ToString(string format)
		{
			return string.Format(format, ((float)this.X).ToString(), ((float)this.Y).ToString(), ((float)this.Z).ToString());
		}
		#endregion
		#region Casts
		public static implicit operator string(Point value)
		{
			return value.NotNull() ? value.ToString() : null;
		}
		public static implicit operator Point(string value)
		{
			Point result = new Point();
			if (value.NotEmpty())
			{

				try
				{
					string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
					if (values.Length == 3)
						result = new Point(Kean.Math.Single.Parse(values[0]), Kean.Math.Single.Parse(values[1]), Kean.Math.Single.Parse(values[2]));
				}
				catch
				{
				}
			}
			return result;
		}
		public static implicit operator Point(Size value)
		{
			return new Point(value.Width, value.Height, value.Depth);
		}
		
		public static implicit operator Point(Integer.Point value)
		{
			return new Point(value.X, value.Y, value.Z);
		}
		public static explicit operator Point(Double.Point value)
		{
			return new Point((float)value.X, (float)value.Y, (float)value.Z);
		}
		
		#endregion

		public Point Project(Transform transform, float planeZ)
		{
			return new Geometry3D.Single.Point(new Geometry2D.Single.Point(this.X, this.Y).Project(transform, planeZ), planeZ);
			//var transformed = transform * this;
			//var projected = transformed * planeZ / transformed.Z;
			//return projected; // TODO: Do this properly...
		}
	}
}
