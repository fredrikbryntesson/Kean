// 
//  Single.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
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
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Core.Basis.Extension;
namespace Kean.Math.Matrix
{
    public partial class Single :
        System.IEquatable<Single>
    {
        Geometry2D.Integer.Size dimensions;
        public Geometry2D.Integer.Size Dimensions { get { return this.dimensions; } private set { this.dimensions = value; } }
        // Matrix elements are supposed to be in column major order 
        public bool IsSquare { get { return this.Dimensions.Width == this.Dimensions.Height; } }
        #region Matrix Invariants
        public float Trace
        {
            get
            {
                float result = 0;
                int diagonal = Math.Integer.Minimum(this.Dimensions.Width, this.Dimensions.Height);
                for (int i = 0; i < diagonal; i++)
                    result += this[i, i];
                return result;
            }
        }
        public float Determinant
        {
            get
            {
                float result = 0;
                if (!this.IsSquare)
                    new Exception.InvalidDimensions();
                else
                {
                    int order = this.Dimensions.Width;
                    if (order > 0)
                    {
                        for (int x = 0; x < this.Dimensions.Width; x++)
                            result += this[x, 0] *
                                 Kean.Math.Single.Power(-1, x + 1 + 1) * this.Minor(x, 0).Determinant;
                    }
                    else
                        result = 1;
                }
                return result;
            }
        }
        #endregion
        public float Norm
        {
            get
            {
                float result = 0;
                for (int i = 0; i < this.elements.Length; i++)
                    result += Kean.Math.Single.Squared(this.elements[i]);
                return Kean.Math.Single.SquareRoot(result);
            }
        }
        public float Distance(Single other)
        {
            return (this - other).Norm;
        }
        float[] elements;
        public Single() : this(0) { }
        public Single(int order) : this(order, order) { }
        public Single(int width, int height) : this(new Kean.Math.Geometry2D.Integer.Size(width, height)) { }
        public Single(Geometry2D.Integer.Size dimensions) : this(dimensions, new float[dimensions.Area]) { }
        public Single(int width, int height, float[] elements) : this(new Geometry2D.Integer.Size(width, height), elements) { }
        public Single(Geometry2D.Integer.Size dimensions, float[] elements)
        {
            this.Dimensions = dimensions;
            int minimum = Kean.Math.Integer.Minimum(elements.Length, this.Dimensions.Area);
            this.elements = new float[this.Dimensions.Area];
            Array.Copy(elements, 0, this.elements, 0, minimum);
        }
        public float this[int x, int y]
        {
            get { return this.elements[this.Index(x, y)]; }
            set { this.elements[this.Index(x, y)] = value; }
        }
        int Index(int x, int y)
        {
            return x * this.Dimensions.Height + y; // Column major order 
            // Use Y * this.Dimensions.Width + X for row major order
        }
        #region Arithmetic Matrix - Matrix Operators
        public static Single operator +(Single left, Single right)
        {
            if (left.Dimensions != right.Dimensions)
                new Exception.InvalidDimensions();
            Single result = new Single(left.Dimensions);
            for (int i = 0; i < result.elements.Length; i++)
                result.elements[i] = left.elements[i] + right.elements[i];
            return result;
        }
        public static Single operator *(Single left, Single right)
        {
            if (left.Dimensions.Width != right.Dimensions.Height)
                new Exception.InvalidDimensions();
            Single result = new Single(right.Dimensions.Width, left.Dimensions.Height);
            for (int x = 0; x < right.Dimensions.Width; x++)
                for (int y = 0; y < left.Dimensions.Height; y++)
                    for (int z = 0; z < left.Dimensions.Width; z++)
                        result[x, y] = result[x, y] + left[z, y] * right[x, z];
            return result;
        }
        public static Single operator -(Single left, Single right)
        {
            if (left.Dimensions != right.Dimensions)
                new Exception.InvalidDimensions();
            Single result = new Single(left.Dimensions);
            for (int i = 0; i < result.elements.Length; i++)
                result.elements[i] = left.elements[i] - right.elements[i];
            return result;

        }
        public static Single operator *(float left, Single right)
        {
            Single result = new Single(right.Dimensions);
            for (int i = 0; i < result.elements.Length; i++)
                result.elements[i] = left * right.elements[i];
            return result;
        }
        public static Single operator *(Single left, float right)
        {
            return right * left;
        }
        public static Single operator /(Single left, float right)
        {
            return left * (1 / right);
        }
        public static Single operator -(Single value)
        {
            return (-1) * value;
        }
        #endregion
        #region Matrix Operations
        public Single Transpose()
        {
            Single result = new Single(this.Dimensions.Height, this.Dimensions.Width);
            for (int x = 0; x < result.Dimensions.Width; x++)
                for (int y = 0; y < result.Dimensions.Height; y++)
                    result[x, y] = this[y, x];
            return result;
        }
        public Single Adjoint()
        {
            Single result = new Single(this.Dimensions.Width, this.Dimensions.Height);
            for (int x = 0; x < result.Dimensions.Width; x++)
                for (int y = 0; y < result.Dimensions.Height; y++)
                    result[x, y] = Kean.Math.Single.Power(-1, x + 1 + y + 1) * this.Minor(y, x).Determinant;
            return result;

        }
        public Single Inverse()
        {
            float determinant = this.Determinant;
            if (determinant == 0)
                new Exception.DivisionByZero();
            return this.Adjoint() / determinant;
        }
        public Single Minor(int x, int y)
        {
            if (this.Dimensions.Width < 1 || this.Dimensions.Height < 1)
                new Exception.InvalidDimensions();
            Single result = new Single(this.Dimensions.Height - 1, this.Dimensions.Width - 1);
            for (int xx = 0; xx < x; xx++)
            {
                for (int yy = 0; yy < y; yy++)
                    result[xx, yy] = this[xx, yy];
                for (int yy = y + 1; yy < this.Dimensions.Height; yy++)
                    result[xx, yy - 1] = this[xx, yy];
            }
            for (int xx = x + 1; xx < this.Dimensions.Width; xx++)
            {
                for (int yy = 0; yy < y; yy++)
                    result[xx - 1, yy] = this[xx, yy];
                for (int yy = y + 1; yy < this.Dimensions.Height; yy++)
                    result[xx - 1, yy - 1] = this[xx, yy];
            }
            return result;
        }
        public Single Extract(Geometry2D.Integer.Box region)
        {
            Single result = new Single(region.Size);
            for (int x = 0; x < region.Size.Width; x++)
                for (int y = 0; y < region.Size.Height; y++)
                    result[x, y] = this[x + region.Left, y + region.Top];
            return result;
        }
        public void Paste(int x, int y, Single submatrix)
        {
            this.Paste(new Geometry2D.Integer.Point(x, y), submatrix);
        }
        public void Paste(Geometry2D.Integer.Point leftTop, Single submatrix)
        {
            for (int x = 0; x < submatrix.Dimensions.Width; x++)
                for (int y = 0; y < submatrix.Dimensions.Height; y++)
                    this[x + leftTop.X, y + leftTop.Y] = submatrix[x, y];
        }
        #endregion
        #region Static Methods
        public static Single Identity(int order)
        {
            Single result = new Single(order, order);
            for (int i = 0; i < order; i++)
                result[i, i] = 1;
            return result;
        }
        #endregion
        #region Object overides and IEquatable<Single>
        public override bool Equals(object other)
        {
            return (other is Single) && this.Equals(other as Single);
        }
        // other is not null here.
        public bool Equals(Single other)
        {
            bool result = this.Dimensions == other.Dimensions;
            if (result)
            {
                for (int x = 0; x < this.Dimensions.Width; x++)
                    for (int y = 0; y < this.Dimensions.Height; y++)
                        result &= this[x, y] == other[x, y];
            }
            return result;
        }
        public override int GetHashCode()
        {
            int result = this.Dimensions.GetHashCode();
            for (int i = 0; i < this.Dimensions.Area; i++)
                result ^= this.elements[i].GetHashCode();
            return result;
        }
        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int y = 0; y < this.Dimensions.Height; y++)
                for (int x = 0; x < this.Dimensions.Width; x++)
                {
                    builder.Append((Kean.Math.Single.ToString(this[x, y])));
                    builder.Append((x == this.Dimensions.Width - 1) ? ((y == this.Dimensions.Height - 1) ? "" : "; ") : ", ");
                }
            return builder.ToString();
        }
        #endregion
        #region Comparison Functions and IComparable<Single>
        public static bool operator ==(Single left, Single right)
        {
            return
                left.Same(right) || (left.NotNull() && right.NotNull()) &&
                left.Equals(right);
        }
        public static bool operator !=(Single left, Single right)
        {
            return !(left == right);
        }
        #endregion
        #region Casts
        public static explicit operator Single(float[] value)
        {
            Single result = new Single(1, value.Length);
            Array.Copy(value, result.elements, value.Length);
            return result;
        }
        public static explicit operator float[](Single value)
        {
            float[] result = new float[value.elements.Length];
            Array.Copy(value.elements, result, result.Length);
            return result;
        }
        public static explicit operator float[,](Single value)
        {
            float[,] result = new float[value.Dimensions.Width, value.Dimensions.Height];
            for (int x = 0; x < value.Dimensions.Width; x++)
                for (int y = 0; y < value.Dimensions.Height; y++)
                    result[x, y] = value[x, y];
            return result;
        }
        public static explicit operator Single(float[,] value)
        {
            Single result = new Single(value.GetLength(0), value.GetLength(1));
            for (int x = 0; x < result.Dimensions.Width; x++)
                for (int y = 0; y < result.Dimensions.Height; y++)
                    result[x, y] = value[x, y];
            return result;
        }
        #endregion
    }
}
