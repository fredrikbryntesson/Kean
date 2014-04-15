// 
//  Box.cs (generated by template)
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
    public struct Box:
	IEquatable<Box>
    {
		public Point LeftTopFront;
		public Size Size;
		#region Sizes
		public float Width { get { return this.Size.Width; } }
		public float Height { get { return this.Size.Height; } }
		public float Depth { get { return this.Size.Depth; } }
		#endregion
		#region All sides
		public float Left { get { return this.LeftTopFront.X; } }
		public float Right { get { return this.LeftTopFront.X + this.Size.Width; } }
		public float Top { get { return this.LeftTopFront.Y; } }
		public float Bottom { get { return this.LeftTopFront.Y + this.Size.Height; } }
		public float Front { get { return this.LeftTopFront.Z; } }
		public float Back { get { return this.LeftTopFront.Z + this.Size.Depth; } }
		#endregion
	    public bool Empty { get { return this.Size.Empty; } }
		public Box(Point leftTopFront, Size size)
		{
			this.LeftTopFront = leftTopFront;
			this.Size = size;
		}
        public Box(float left, float top, float front, float width, float height, float depth) : 
			this(new Point(left, top, front), new Size(width, height, depth)) 
		{ }
        public Box(Size size) :
			this(new Geometry3D.Single.Point(), size) 
		{ }
        
        public  Box Pad(float left, float right, float top, float bottom, float front, float back)
        {
            return new Box(new Point(this.Left - left, this.Top - top, this.Front - front), new Size(this.Size.Width + left + right, this.Size.Height + top + bottom, this.Size.Depth + front + back));
        }
        public  Box Intersection(Box other)
        {
            float left = this.Left > other.Left ? this.Left : other.Left;
            float top = this.Top > other.Top ? this.Top : other.Top;
            float front = this.Front > other.Front ? this.Front: other.Front;
            float width = Kean.Math.Single.Maximum((this.Right < other.Right ? this.Right : other.Right) - left, 0);
            float height = Kean.Math.Single.Maximum((this.Bottom < other.Bottom ? this.Bottom : other.Bottom) - top, 0);
            float depth = Kean.Math.Single.Maximum((this.Back < other.Back ? this.Back : other.Back) - front, 0);
            return new Box(left, top, width, height, front, depth);
        }
		#region Arithmetic operators
		public static Box operator +(Box left, Box right)
		{
			Box result;
			if (left.Empty)
				result = right;
			else if (right.Empty)
				result = left;
			else
				result = new Box(Kean.Math.Single.Minimum(left.Left, right.Left), Kean.Math.Single.Minimum(left.Top, right.Top), Kean.Math.Single.Minimum(left.Front, right.Front), Kean.Math.Single.Maximum(left.Right, right.Right) - Kean.Math.Single.Minimum(left.Left, right.Left), Kean.Math.Single.Maximum(left.Bottom, right.Bottom) - Kean.Math.Single.Minimum(left.Top, right.Top), Kean.Math.Single.Maximum(left.Back, right.Back) - Kean.Math.Single.Minimum(left.Front, right.Front));
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
				 float u = Kean.Math.Single.Maximum(left.Front, right.Front);
				 float v = Kean.Math.Single.Minimum(left.Back, right.Back);
				if (l < r && t < b && u < v)
				{
					result = new Box(l, t, u, r - l, b - t, v - u);
				}
				else
					result = new Box();
			}
			else
				result = new Box();
			return result;
		}
		public static Box operator +(Box left, Point right)
		{
			return new Box(left.LeftTopFront + right, left.Size);
		}
		public static Box operator -(Box left, Point right)
		{
			return new Box(left.LeftTopFront - right, left.Size);
		}
		public static Box operator +(Box left, Size right)
		{
			return new Box(left.LeftTopFront, left.Size + right);
		}
		public static Box operator -(Box left, Size right)
		{
			return new Box(left.LeftTopFront, left.Size - right);
		}
		public static Box operator *(Transform left, Box right)
		{
			return new Box(left * right.LeftTopFront, left * right.Size);
		}
		#endregion
		#region Comparison Operators
		public static bool operator ==(Box left, Box  right)
		{
			return object.ReferenceEquals(left, right) ||
				!object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) &&
				left.LeftTopFront == right.LeftTopFront &&
				left.Size == right.Size;
		}
		public static bool operator !=(Box left, Box right)
		{
			return !(left == right);
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
			return this.ToString("{0}, {1}, {2}, {3}, {4}, {5}");
		}
		public string ToString(string format)
		{
			return string.Format(format, (this.LeftTopFront.X).ToString(), (this.LeftTopFront.Y).ToString(), (this.LeftTopFront.Z).ToString(), (this.Size.Width).ToString(), (this.Size.Height).ToString(), (this.Size.Depth).ToString());
		}
		public override int GetHashCode()
		{
			return 33 * this.LeftTopFront.GetHashCode() ^ this.Size.GetHashCode();
		}
		#endregion
		#region Static Methods
		public static Box Create(Point leftTopFront, Size size)
		{
			Box result = new Box();
			result.LeftTopFront = leftTopFront;
			result.Size = size;
			return result;
		}
		public static Box Bounds(float left, float right, float top, float bottom, float front, float back)
		{
			return new Box(left, top, front,right - left, bottom - top, back - front);
		}
		public static Box Bounds(params Point[] points)
		{
			Box result = new Box();
			if (points.Length > 0)
			{
				float xMinimum = 0;
				float xMaximum = xMinimum;
				float yMinimum = xMinimum;
				float yMaximum = xMinimum;
				float zMinimum = xMinimum;
				float zMaximum = xMinimum;
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
							zMinimum = point.Z;
							zMaximum = point.Z;
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
							if (point.Z < zMinimum)
								zMinimum = point.Z;
							else if (point.Z > zMaximum)
								zMaximum = point.Z;
						}
					
				}
				result = new Box(xMinimum, yMinimum, zMinimum, xMaximum - xMinimum, yMaximum - yMinimum, zMaximum - zMinimum);
			}
			return result;
		}
		#endregion
		#region Casts
		 public static implicit operator Box(string value)
        {
            Box result = new Box();
            if (value.NotEmpty())
            {

                try
                {
                    string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length == 6)
                        result = new Box((Point)(values[0] + " " + values[1] + " " + values[2]), (Size)(values[3] + " " + values[4] + " " + values[5]));
                }
                catch
                {
                }
            }
            return result;
        }
		public static implicit operator string(Box value)
        {
            return value.NotNull() ? value.ToString() : null;
        }


		 public static implicit operator Box(Integer.Box value)
        {
            return new Box(value.LeftTopFront, value.Size);
        }
        public static explicit operator Integer.Box(Box value)
        {
            return new Integer.Box((Integer.Point)(value.LeftTopFront), (Integer.Size)(value.Size));
        }
        
		
		
		
        #endregion
    }
}