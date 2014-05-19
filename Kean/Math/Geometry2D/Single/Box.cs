// 
// Box.cs (generated by template)
// 
// Author:
//		Simon Mika <smika@hx.se>
// 
// Copyright (c) 2011-2013 Simon Mika
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using Kean.Extension;

namespace Kean.Math.Geometry2D.Single
{
	public struct Box :
		IEquatable<Box>
	{
		public Point LeftTop;
		public Size Size;
		#region Sizes
		public float Width { get { return this.Size.Width; } }
		public float Height { get { return this.Size.Height; } }
		#endregion
		#region All sides
		public float Left { get { return this.LeftTop.X; } }
		public float Top { get { return this.LeftTop.Y; } }
		public float Right { get { return this.LeftTop.X + this.Size.Width; } }
		public float Bottom { get { return this.LeftTop.Y + this.Size.Height; } }
		#endregion
		#region All other corners
		public Point RightTop { get { return new Point(this.Right, this.Top); } }
		public Point LeftBottom { get { return new Point(this.Left, this.Bottom); } }
		public Point RightBottom { get { return this.LeftTop + this.Size; } }
		#endregion
		public Point Center { get { return this.LeftTop + (this.Size / 2); } }
		public bool Empty { get { return this.Size.Empty; } }

		public Box(Point leftTop, Size size)
		{
			this.LeftTop = leftTop;
			this.Size = size;
		}
		public Box(float left, float top, float width, float height) :
			this(new Point(left, top), new Size(width, height))
		{ }
		public Box(Size size) :
			this(new Point(), size)
		{ }
		#region Methods
		public Box Swap()
		{
			return new Box(this.LeftTop.Swap(), this.Size.Swap());
		}
		public Box Pad(float pad)
		{
			return this.Pad(pad, pad, pad, pad);
		}
		public Box Pad(Size padding)
		{
			return this.Pad(padding.Width, padding.Width, padding.Height, padding.Height);
		}
		public Box Pad(float left, float right, float top, float bottom)
		{
			return new Box(new Point(this.Left - left, this.Top - top), new Size(this.Size.Width + left + right, this.Size.Height + top + bottom));
		}
		public Box Intersection(Box other)
		{
			float left = this.Left > other.Left ? this.Left : other.Left;
			float top = this.Top > other.Top ? this.Top : other.Top;
			float width = Kean.Math.Single.Maximum((this.Right < other.Right ? this.Right : other.Right) - left, 0);
			float height = Kean.Math.Single.Maximum((this.Bottom < other.Bottom ? this.Bottom : other.Bottom) - top, 0);
			return new Box(left, top, width, height);
		}
		public Box Union(Box other)
		{
			float left = Kean.Math.Single.Minimum(this.Left, other.Left);
			float top = Kean.Math.Single.Minimum(this.Top, other.Top);
			float width = Kean.Math.Single.Maximum(this.Right, other.Right) - Kean.Math.Single.Minimum(this.Left, other.Left);
			float height = Kean.Math.Single.Maximum(this.Bottom, other.Bottom) - Kean.Math.Single.Minimum(this.Top, other.Top);
			return new Box(left, top, width, height);
		}
		public bool Contains(Integer.Point point)
		{
			return this.Left <= point.X && point.X < this.Right && this.Top <= point.Y && point.Y < this.Bottom;
		}
		public bool Contains(Single.Point point)
		{
			return this.Left <= point.X && point.X < this.Right && this.Top <= point.Y && point.Y < this.Bottom;
		}
		public bool Contains(Double.Point point)
		{
			return this.Left <= point.X && point.X < this.Right && this.Top <= point.Y && point.Y < this.Bottom;
		}
		public bool Contains(Box box)
		{
			return this.Intersection(box) == box;
		}
		public Box Round()
		{
			return new Box(this.LeftTop.Round(), this.Size.Round());
		}
		public Box Ceiling()
		{
			Point leftTop = this.LeftTop.Floor();
			return new Box(leftTop, (Size)(this.RightBottom.Ceiling() - leftTop));
		}
		public Box Floor()
		{
			return new Box(this.LeftTop.Round(), this.Size.Round());
		}
		#endregion
		#region Arithmetic operators
		public static Box operator +(Box left, Box right)
		{
			Box result;
			if (left.Empty)
				result = right;
			else if (right.Empty)
				result = left;
			else
				result = new Box(Kean.Math.Single.Minimum(left.Left, right.Left), Kean.Math.Single.Minimum(left.Top, right.Top), Kean.Math.Single.Maximum(left.Right, right.Right) - Kean.Math.Single.Minimum(left.Left, right.Left), Kean.Math.Single.Maximum(left.Bottom, right.Bottom) - Kean.Math.Single.Minimum(left.Top, right.Top));
			return result;
		}
		public static Box operator -(Box left, Box right)
		{
			Box result;
			if (!left.Empty && !right.Empty)
			{
				float l = Kean.Math.Single.Maximum(left.Left, right.Left);
				float r = Kean.Math.Single.Minimum(left.Right, right.Right);
				float t = Kean.Math.Single.Maximum(left.Top, right.Top);
				float b = Kean.Math.Single.Minimum(left.Bottom, right.Bottom);
				if (l < r && t < b)
					result = new Box(l, t, r - l, b - t);
				else
					result = new Box();
			}
			else
				result = new Box();
			return result;
		}
		public static Box operator +(Box left, Point right)
		{
			return new Box(left.LeftTop + right, left.Size);
		}
		public static Box operator -(Box left, Point right)
		{
			return new Box(left.LeftTop - right, left.Size);
		}
		public static Box operator +(Box left, Size right)
		{
			return new Box(left.LeftTop, left.Size + right);
		}
		public static Box operator -(Box left, Size right)
		{
			return new Box(left.LeftTop, left.Size - right);
		}
		public static Box operator *(Transform left, Box right)
		{
			return new Box(left * right.LeftTop, left * right.Size);
		}
		#endregion
		
		#region Comparison Operators
		/// <summary>
		/// Defines equality.
		/// </summary>
		/// <param name="Left">Point Left of operator.</param>
		/// <param name="Right">Point Right of operator.</param>
		/// <returns>True if <paramref name="Left"/> equals <paramref name="Right"/> else false.</returns>
		public static bool operator ==(Box left, Box right)
		{
			return left.Left == right.Left && left.Top == right.Top && left.Width == right.Width && left.Height == right.Height;
		}
		/// <summary>
		/// Defines inequality.
		/// </summary>
		/// <param name="Left">Point Left of operator.</param>
		/// <param name="Right">Point Right of operator.</param>
		/// <returns>False if <paramref name="Left"/> equals <paramref name="Right"/> else true.</returns>
		public static bool operator !=(Box left, Box right)
		{
			return !(left == right);
		}
		#endregion
		#region Static Operators
		public static Box operator -(Box left, Shell right)
		{
			return new Box(left.LeftTop + right.LeftTop, left.Size - right.Size);
		}
		public static Box operator +(Box left, Shell right)
		{
			return new Box(left.LeftTop - right.LeftTop, left.Size + right.Size);
		}
		#endregion
		#region Casts
		public static implicit operator Box(Integer.Box value)
		{
			return new Box(value.LeftTop, value.Size);
		}
		public static explicit operator Integer.Box(Box value)
		{
			return new Integer.Box((Integer.Point)(value.LeftTop), (Integer.Size)(value.Size));
		}
		public static implicit operator string(Box value)
		{
			return value.ToString();
		}
		public static explicit operator Box(string value)
		{
			Box result = new Box();
			if (value.NotEmpty())
			{
				try
				{
					string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
					switch (values.Length)
					{
						case 4:
							result = new Box((Point)(values[0] + " " + values[1]), (Size)(values[2] + " " + values[3]));
							break;
						case 2:
							result = new Box((Point)(values[0] + " " + values[1]), new Size()); 
							break;
						default:
							break;
					}
				}
				catch
				{
				}
			}
			return result;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return (other is Box) && this.Equals((Box)other);
		}
		public bool Equals(Box other)
		{
			return this == other;
		}
		public override string ToString()
		{
			return this.ToString("{0}, {1}, {2}, {3}");
		}
		public string ToString(string format)
		{
			return String.Format(format, Kean.Math.Single.ToString(this.Left), Kean.Math.Single.ToString(this.Top), Kean.Math.Single.ToString(this.Width), Kean.Math.Single.ToString(this.Height));
		}
		public override int GetHashCode()
		{
			return 33 * this.LeftTop.GetHashCode() ^ this.Size.GetHashCode();
		}
		#endregion
		#region Static Creators
		public static Box Create(Point leftTop, Size size)
		{
			return new Box(leftTop, size);
		}
		public static Box Create(float left, float top, float width, float height)
		{
			return new Box(left, top, width, height);
		}
		public static Box CreateAround(Point center, Size size)
		{
			return new Box(center - size / 2, size);
		}
		public static Box CreateAround(float center, float middle, float width, float height)
		{
			return new Box(center - width / 2, middle - height / 2, width, height);
		}
		public static Box Bounds(float left, float right, float top, float bottom)
		{
			return new Box(left, top, right - left, bottom - top);
		}
		public static Box Bounds(params Point[] points)
		{
			return Box.Bounds((System.Collections.Generic.IEnumerable<Point>)points);
		}
		public static Box Bounds(System.Collections.Generic.IEnumerable<Point> points)
		{
			float xMinimum = 0;
			float xMaximum = xMinimum;
			float yMinimum = xMinimum;
			float yMaximum = xMinimum;
			bool initialized = false;
			foreach (Point point in points)
			{
				if (!initialized)
				{
					initialized = true;
					xMinimum = point.X;
					xMaximum = point.X;
					yMinimum = point.Y;
					yMaximum = point.Y;
				}
				else
				{
					if (point.X < xMinimum)
						xMinimum = point.X;
					else if (point.X > xMaximum)
						xMaximum = point.X;
					if (point.Y < yMinimum)
						yMinimum = point.Y;
					else if (point.Y > yMaximum)
						yMaximum = point.Y;
				}
			}
			return new Box(xMinimum, yMinimum, xMaximum - xMinimum, yMaximum - yMinimum);
		}
		#endregion
	}
}
