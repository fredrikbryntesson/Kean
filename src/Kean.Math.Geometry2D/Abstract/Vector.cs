// 
//  Vector.cs
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

namespace Kean.Math.Geometry2D.Abstract
{
    public abstract class Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> :
		IEquatable<Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>>,
        IVector<V>
		where VectorType : Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, IVector<V>, new()
		where VectorValue : struct, IVector<V>
		where TransformType : Transform<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, ITransform<V>, new()
        where TransformValue : struct, ITransform<V>
		where ShellType : Shell<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, IShell<V>, new()
		where ShellValue : struct, IShell<V>
		where BoxType : Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, IBox<PointValue, SizeValue, V>, new()
        where BoxValue : struct, IBox<PointValue, SizeValue, V>
		where PointType : Point<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, IPoint<V>, new()
        where PointValue : struct, IPoint<V>, IVector<V>
		where SizeType : Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, ISize<V>, new()
        where SizeValue : struct, ISize<V>, IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        protected R X { get; private set; }
        protected R Y { get; private set; }
        public abstract VectorValue Value { get; }
        public R Norm { get { return (this.X.Squared() + this.Y.Squared()).SquareRoot(); } }
        public R Azimuth { get { return this.Y.ArcusTangensExtended(this.X); } }
        #region IVector<V> Members
        V IVector<V>.X { get { return this.X; } }
        V IVector<V>.Y { get { return this.Y; } }
        #endregion
        #region Static Constants
        public static VectorType BasisX { get { return new VectorType() { X = Kean.Math.Abstract<R, V>.One, Y = new R() }; } }
        public static VectorType BasisY { get { return new VectorType() { X = new R(), Y = Kean.Math.Abstract<R, V>.One }; } }
        #endregion
        #region Constructors
        protected Vector()
        {
            this.X = Kean.Math.Abstract<R, V>.Zero;
            this.Y = Kean.Math.Abstract<R, V>.Zero;
        }
        protected Vector(R x, R y)
        {
            this.X = x;
            this.Y = y;
        }
        #endregion
        #region Methods
        protected VectorType Create(R x, R y)
        {
            return new VectorType() { X = x, Y = y };
        }
        public VectorType Swap()
        {
            return new VectorType() { X = this.Y, Y = this.X };
        }
        public R Angle(VectorType other)
        {
            R result = new R();
            if (other.NotNull())
            {
                result = this.ScalarProduct(other) / (this.Norm * other.Norm);
                R sign = this.X * other.Y - this.Y * other.X;
                result = result.Clamp(Kean.Math.Abstract<R, V>.One.Negate(), Kean.Math.Abstract<R, V>.One).ArcusCosinus();
                result *= sign < Kean.Math.Abstract<R, V>.Zero ? Kean.Math.Abstract<R, V>.One.Negate() : Kean.Math.Abstract<R, V>.One;
            }
            return result;
        }
        public R ScalarProduct(VectorType other)
        {

            return other.IsNull() ? null : this.X * other.X + this.Y * other.Y;
        }
        public R Distance(VectorType other)
        {
            return other.IsNull() ? null : (this - other).Norm;
        }
		public VectorType Round()
		{
			return new VectorType() { X = this.X.Round(), Y = this.Y.Round() };
		}
		public VectorType Ceiling()
		{
			return new VectorType() { X = this.X.Ceiling(), Y = this.Y.Ceiling() };
		}
		public VectorType Floor()
		{
			return new VectorType() { X = this.X.Floor(), Y = this.Y.Floor() };
		}
        public VectorType Minimum(IVector<V> floor)
        {
            return new VectorType() { X = Kean.Math.Abstract<R,V>.Minimum((R)this.X, (R)floor.X), Y = Kean.Math.Abstract<R,V>.Minimum((R)this.Y, (R)floor.Y) };
        }
        public VectorType Maximum(IVector<V> ceiling)
        {
            return new VectorType() { X = Kean.Math.Abstract<R, V>.Maximum((R)this.X, (R)ceiling.X), Y = Kean.Math.Abstract<R, V>.Maximum((R)this.Y, (R)ceiling.Y) };
        }
	    public VectorType Clamp(IVector<V> floor, IVector<V> ceiling)
        {
            return new VectorType().Create(((R)this.X).Clamp((R)floor.X, (R)ceiling.X), ((R)this.Y).Clamp((R)floor.Y, (R)ceiling.Y));
        }
       	#endregion
        #region Arithmetic Vector - Vector Operators
		public static VectorType operator +(Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, IVector<V> right)
        {
            return (left.IsNull() || right.IsNull()) ? null : 
                new VectorType()
                {
                    X = left.X + right.X,
                    Y = left.Y + right.Y,
                };
        }
		public static VectorType operator -(Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> vector)
        {
            return vector.IsNull() ? null : new VectorType()
             {
                 X = -vector.X,
                 Y = -vector.Y,
             };
        }
        public static VectorType operator -(Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, IVector<V> right)
        {
            return (left.IsNull() || right.IsNull()) ? null : new VectorType()
                {
                    X = left.X - right.X,
                    Y = left.Y - right.Y,
                };
        }
        public static VectorType operator *(Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, IVector<V> right)
        {
            return (left.IsNull() || right.IsNull()) ? null : new VectorType()
                {
                    X = left.X * right.X,
                    Y = left.Y * right.Y,
                };
        }
        #endregion
        #region Arithmetic Vector and Scalar
        public static VectorType operator *(Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, R right)
        {
            return (left.IsNull() || right.IsNull()) ? null : new VectorType()
                {
                    X = left.X * right,
                    Y = left.Y * right,
                };
        }
        public static VectorType operator /(Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, R right)
        {
            return (left.IsNull() || right.IsNull()) ? null : new VectorType()
                {
                    X = left.X / right,
                    Y = left.Y / right,
                };
        }
        public static VectorType operator *(R left, Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
            return right * left;
        }
        #endregion
        #region Comparison Operators
        /// <summary>
        /// Defines equality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>True if <paramref name="Left"/> equals <paramref name="Right"/> else false.</returns>
        public static bool operator ==(Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, IVector<V> right)
        {
            return object.ReferenceEquals(left, right) ||
                !object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) && left.X == right.X && left.Y == right.Y;
        }
        /// <summary>
        /// Defines inequality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>False if <paramref name="Left"/> equals <paramref name="Right"/> else true.</returns>
        public static bool operator !=(Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, IVector<V> right)
        {
            return !(left == right);
        }
        #endregion
        #region Object overides and IEquatable<VectorType>
        public override bool Equals(object other)
        {
            return (other is Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>) && this.Equals(other as Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>);
        }
        // other is not null here.
        public bool Equals(Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> other)
        {
            return this == other;
        }
        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
        public override string ToString()
        {
			return this.ToString("{0}, {1}");
		}
		public string ToString(string format)
		{
			return String.Format(format, ((R)this.X).ToString(), ((R)this.Y).ToString());
		}
		#endregion
        #region Casts
        public static explicit operator V[](Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> value)
        {
            return new V[] { value.X, value.Y };
        }
        public static explicit operator Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>(V[] value)
        {
            return new VectorType() { X = (R)value[0], Y = (R)value[1] };
        }
        public static explicit operator Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>(VectorValue value)
        {
            return new VectorType() { X = (R)value.X, Y = (R)value.Y };
        }
		public static implicit operator VectorType(Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> value)
		{
			return new VectorType() { X = (R)value.X, Y = (R)value.Y };
		}
		#endregion
        public static VectorType Create(V x, V y)
        {
            return new VectorType() { X = (R)x, Y = (R)y };
        }
    }
}

