// 
//  Double.cs
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
    public partial class Double :
        System.IEquatable<Double>
    {
        Geometry2D.Integer.SizeValue dimensions;
        public Geometry2D.Integer.SizeValue Dimensions { get { return this.dimensions; } private set { this.dimensions = value; } }
        // Matrix elements are supposed to be in column major order 
        public bool IsSquare { get { return this.Dimensions.Width == this.Dimensions.Height; } }
        public int Order { get { return Kean.Math.Integer.Minimum(this.Dimensions.Width,this.Dimensions.Height); } }
        public double Norm
        {
            get
            {
                double result = 0;
                for (int i = 0; i < this.elements.Length; i++)
                    result += Kean.Math.Double.Squared(this.elements[i]);
                return Kean.Math.Double.SquareRoot(result);
            }
        }
        public double Distance(Double other)
        {
            return (this - other).Norm;
        }
        double[] elements;
        public Double() : this(0) { }
        public Double(int order) : this(order, order) { }
        public Double(int width, int height) : this(new Geometry2D.Integer.SizeValue(width, height)) { }
        public Double(Geometry2D.Integer.SizeValue dimensions) : this(dimensions, new double[dimensions.Area]) { }
        public Double(int width, int height, double[] elements) : this(new Geometry2D.Integer.SizeValue(width, height), elements) { }
        public Double(Geometry2D.Integer.SizeValue dimensions, double[] elements)
        {
            this.Dimensions = dimensions;
            int minimum = Kean.Math.Integer.Minimum(elements.Length, this.Dimensions.Area);
            this.elements = new double[this.Dimensions.Area];
            Array.Copy(elements, 0, this.elements, 0, minimum);
        }
        public double this[int x, int y]
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
        public static Double operator +(Double left, Double right)
        {
            if (left.Dimensions != right.Dimensions)
                new Exception.InvalidDimensions();
            Double result = new Double(left.Dimensions);
            for (int i = 0; i < result.elements.Length; i++)
                result.elements[i] = left.elements[i] + right.elements[i];
            return result;
        }
        public static Double operator *(Double left, Double right)
        {
            if (left.Dimensions.Width != right.Dimensions.Height)
                new Exception.InvalidDimensions();
            Double result = new Double(right.Dimensions.Width, left.Dimensions.Height);
            for (int x = 0; x < right.Dimensions.Width; x++)
                for (int y = 0; y < left.Dimensions.Height; y++)
                    for (int z = 0; z < left.Dimensions.Width; z++)
                        result[x, y] = result[x, y] + left[z, y] * right[x, z];
            return result;
        }
        public static Double operator -(Double left, Double right)
        {
            if (left.Dimensions != right.Dimensions)
                new Exception.InvalidDimensions();
            Double result = new Double(left.Dimensions);
            for (int i = 0; i < result.elements.Length; i++)
                result.elements[i] = left.elements[i] - right.elements[i];
            return result;

        }
        public static Double operator *(double left, Double right)
        {
            Double result = new Double(right.Dimensions);
            for (int i = 0; i < result.elements.Length; i++)
                result.elements[i] = left * right.elements[i];
            return result;
        }
        public static Double operator *(Double left, double right)
        {
            return right * left;
        }
        public static Double operator /(Double left, double right)
        {
            return left * (1 / right);
        }
        public static Double operator -(Double value)
        {
            return (-1) * value;
        }
        #endregion
        #region Matrix Operations
        public Double Transpose()
        {
            Double result = new Double(this.Dimensions.Height, this.Dimensions.Width);
            for (int x = 0; x < result.Dimensions.Width; x++)
                for (int y = 0; y < result.Dimensions.Height; y++)
                    result[x, y] = this[y, x];
            return result;
        }
        public Double Adjoint()
        {
            Double result = new Double(this.Dimensions.Width, this.Dimensions.Height);
            for (int x = 0; x < result.Dimensions.Width; x++)
                for (int y = 0; y < result.Dimensions.Height; y++)
                    result[x, y] = Kean.Math.Double.Power(-1, x + 1 + y + 1) * this.Minor(y, x).Determinant();
            return result;

        }
        public Double Minor(int x, int y)
        {
            if (this.Dimensions.Width < 1 || this.Dimensions.Height < 1)
                new Exception.InvalidDimensions();
            Double result = new Double(this.Dimensions.Height - 1, this.Dimensions.Width - 1);
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
        public Double Extract(int left, int top)
        {
            return this.Extract(left, this.Dimensions.Width, top, this.Dimensions.Height);
        }
        public Double Extract(int left, int right, int top, int bottom)
        {
            Double result = new Double(right-left, bottom- top);
            for (int x = left; x < right; x++)
                for (int y = top; y < bottom; y++)
                    result[x-left, y-top] = this[x, y];
            return result;
        }
        public Double Paste(int x, int y, Double submatrix)
        {
            return this.Paste(new Geometry2D.Integer.Point(x, y), submatrix);
        }
        public Double Paste(Geometry2D.Integer.Point leftTop, Double submatrix)
        {
            Double result = this.Copy();
            for (int x = 0; x < submatrix.Dimensions.Width; x++)
                for (int y = 0; y < submatrix.Dimensions.Height; y++)
                    result[x + leftTop.X, y + leftTop.Y] = submatrix[x, y];
            return result;
        }
        public void Set( int left, int top, Double submatrix)
        {
            for (int x = 0; x < submatrix.Dimensions.Width; x++)
                for (int y = 0; y < submatrix.Dimensions.Height; y++)
                    this[x + left, y + top] = submatrix[x, y];
        }
        public Double Copy()
        {
            Double result = new Double(this.Dimensions);
            Array.Copy(this.elements, result.elements, this.elements.Length);
            return result;
        }
        
        #endregion
        #region Static Constructors
        public static Double Identity(int order)
        {
            Double result = new Double(order, order);
            for (int i = 0; i < order; i++)
                result[i, i] = 1;
            return result;
        }
        public static Double Diagonal(params Double[] matrices)
        {
            int width = 0;
            int height = 0;
            for (int i = 0; i < matrices.Length; i++)
            {
                width += matrices[i].Dimensions.Width;
                height += matrices[i].Dimensions.Height;
            }
            Double result = new Double(width, height);
            int k = 0;
            int l = 0;
            for (int i = 0; i < matrices.Length; i++)
            {
                result.Set(k, l, matrices[i]);
                k += matrices[i].Dimensions.Width;
                l += matrices[i].Dimensions.Height;
            }
            return result;
        }
        public static Double Basis(int length, int index)
        {
            return Double.Basis(length, index, true);
        }
        public static Double Basis(int length, int index, bool column)
        {
            Double result = new Double(1, length);
            result[0, index] = 1;
            if (!column)
                result = result.Transpose();
            return result;
        }
        #endregion
        #region Object overides and IEquatable<Double>
        public override bool Equals(object other)
        {
            return (other is Double) && this.Equals(other as Double);
        }
        // other is not null here.
        public bool Equals(Double other)
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
                    builder.Append((Kean.Math.Double.ToString(this[x, y])));
                    builder.Append((x == this.Dimensions.Width - 1) ? ((y == this.Dimensions.Height - 1) ? "" : "; ") : ", ");
                }
            return builder.ToString();
        }
        #endregion
        #region Comparison Functions and IComparable<Double>
        public static bool operator ==(Double left, Double right)
        {
            return
                left.Same(right) || (left.NotNull() && right.NotNull()) &&
                left.Equals(right);
        }
        public static bool operator !=(Double left, Double right)
        {
            return !(left == right);
        }
        #endregion
        #region Casts
        public static explicit operator Double(double[] value)
        {
            Double result = new Double(1, value.Length);
            Array.Copy(value, result.elements, value.Length);
            return result;
        }
        public static explicit operator double[](Double value)
        {
            double[] result = new double[value.elements.Length];
            Array.Copy(value.elements, result, result.Length);
            return result;
        }
        public static explicit operator double[,](Double value)
        {
            double[,] result = new double[value.Dimensions.Width, value.Dimensions.Height];
            for (int x = 0; x < value.Dimensions.Width; x++)
                for (int y = 0; y < value.Dimensions.Height; y++)
                    result[x, y] = value[x, y];
            return result;
        }
        public static explicit operator Double(double[,] value)
        {
            Double result = new Double(value.GetLength(0), value.GetLength(1));
            for (int x = 0; x < result.Dimensions.Width; x++)
                for (int y = 0; y < result.Dimensions.Height; y++)
                    result[x, y] = value[x, y];
            return result;
        }
        #endregion
    }
}
