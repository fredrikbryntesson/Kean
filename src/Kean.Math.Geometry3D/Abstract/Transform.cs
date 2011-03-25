// 
//  ITransform.cs
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
using Kean.Core.Basis.Extension;

namespace Kean.Math.Geometry3D.Abstract
{
    public abstract class Transform<TransformType, TransformValue, R, V> :
        ITransform<V>,
        IEquatable<TransformType>
        where TransformType : Transform<TransformType, TransformValue, R, V>, ITransform<V>, new()
        where TransformValue : struct, ITransform<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        public R A { get; private set; }
        public R B { get; private set; }
        public R C { get; private set; }
        public R D { get; private set; }
        public R E { get; private set; }
        public R F { get; private set; }
        public R G { get; private set; }
        public R H { get; private set; }
        public R I { get; private set; }
        public R J { get; private set; }
        public R K { get; private set; }
        public R L { get; private set; }

        public R this[int x, int y]
        {
            get
            {
                R result;
                switch (x)
                {
                    case 0:
                        switch (y)
                        {
                            case 0: result = this.A; break;
                            case 1: result = this.B; break;
                            case 2: result = this.C; break;
                            case 3: result = Kean.Math.Abstract<R,V>.Zero; break;
                            default: throw new System.Exception(); // TODO: create new exception
                        }
                        break;
                    case 1:
                        switch (y)
                        {
                            case 0: result = this.D; break;
                            case 1: result = this.E; break;
                            case 2: result = this.F; break;
                            case 3: result = Kean.Math.Abstract<R,V>.Zero; break;
                            default: throw new System.Exception(); // TODO: create new exception
                        }
                        break;
                    case 2:
                        switch (y)
                        {
                            case 0: result = this.G; break;
                            case 1: result = this.H; break;
                            case 2: result = this.I; break;
                            case 3: result = Kean.Math.Abstract<R,V>.Zero; break;
                            default: throw new System.Exception(); // TODO: create new exception
                        }
                        break;
                    case 3:
                        switch (y)
                        {
                            case 0: result = this.J; break;
                            case 1: result = this.K; break;
                            case 2: result = this.L; break;
                            case 3: result = Kean.Math.Abstract<R,V>.One; break;
                            default: throw new System.Exception(); // TODO: create new exception
                        }
                        break;
                    default: throw new System.Exception(); // TODO: create new exception
                }
                return result;
            }
        }
        public abstract TransformValue Value { get; }
        public TransformType Inverse
        {
            get
            {
                R determinant = this.A * this.D - this.B * this.C;
                return new TransformType()
                {
                    A = this.D / determinant,
                    B = -this.B / determinant,
                    C = -this.C / determinant,
                    D = this.A / determinant,
                    E = (this.C * this.F - this.D * this.E) / determinant,
                    F = -(this.A * this.F - this.B * this.E) / determinant,
                };
            }
        }
        #region Transform Properties
        public V ScalingX { get { return (this.A.Squared() + this.B.Squared() + this.C.Squared()).SquareRoot(); } }
        public V ScalingY { get { return (this.C.Squared() + this.D.Squared() + this.E.Squared()).SquareRoot(); } }
        public V ScalingZ { get { return (this.G.Squared() + this.H.Squared() + this.I.Squared()).SquareRoot(); } }
        
        public V TransformScale { get { return ((R)this.ScalingX + (R)this.ScalingX) / Kean.Math.Abstract<R, V>.Two; } }
        public V TransformRotation { get { return ((R)this.B).ArcusTangensExtended(this.A); } }
        //public SizeType Translation { get { return new SizeType() { Width = this.E, Height = this.F, }; } }
        #endregion

        #region Constructors
        protected Transform() : this(new R(), new R(), new R(), new R(), new R(), new R()) { }
        protected Transform(R a, R b, R c, R d, R e, R f)
        {
            this.A = a;
            this.B = b;
            this.C = c;
            this.D = d;
            this.E = e;
            this.F = f;
        }
        #endregion
        #region Manipulations
        public TransformType Translate(V delta)
        {
            return this.Translate(delta, delta);
        }
        public TransformType Translate(IVector<V> delta)
        {
            return this.Translate(delta.X, delta.Y);
        }
        public TransformType Translate(V xDelta, V yDelta)
        {
            return Transform<TransformType, TransformValue, R, V>.Translation(xDelta, yDelta) * this;
        }
        public TransformType Scale(V xFactor, V yFactor)
        {
            return Transform<TransformType, TransformValue, R, V>.Scaling(xFactor, yFactor) * this;
        }
        public TransformType Rotate(V angle)
        {
            return Transform<TransformType, TransformValue, R, V>.Rotation(angle) * this;
        }
        public TransformType SkewX(V angle)
        {
            return Transform<TransformType, TransformValue, R, V>.SkewingX(angle) * this;
        }
        public TransformType SkewY(V angle)
        {
            return Transform<TransformType, TransformValue, R, V>.SkewingY(angle) * this;
        }
        #endregion
        #region Static Creators
        public static TransformType Identity
        {
            get
            {
                R zero = Kean.Math.Abstract<R, V>.Zero;
                R one = Kean.Math.Abstract<R, V>.One;
                return new TransformType() { A = one, B = zero, C = zero, D = one, E = zero, F = zero, };
            }
        }
        public static TransformType Translation(V xDelta, V yDelta)
        {
            R zero = Kean.Math.Abstract<R, V>.Zero;
            return new TransformType() { A = zero, B = zero, C = zero, D = zero, E = (R)xDelta, F = (R)yDelta };
        }
        public static TransformType Scaling(V xFactor, V yFactor)
        {
            R zero = Kean.Math.Abstract<R, V>.Zero;
            return new TransformType() { A = (R)xFactor, B = zero, C = zero, D = (R)yFactor, E = zero, F = zero };
        }
        public static TransformType Rotation(V angle)
        {
            R zero = Kean.Math.Abstract<R, V>.Zero;
            return new TransformType() { A = ((R)angle).Cosinus(), B = ((R)angle).Sinus(), C = -((R)angle).Sinus(), D = ((R)angle).Cosinus(), E = zero, F = zero };
        }
        public static TransformType SkewingX(V angle)
        {
            R zero = Kean.Math.Abstract<R, V>.Zero;
            R one = Kean.Math.Abstract<R, V>.One;
            return new TransformType() { A = one, B = zero, C = ((R)angle).Tangens(), D = one, E = zero, F = zero, };
        }
        public static TransformType SkewingY(V angle)
        {
            R zero = Kean.Math.Abstract<R, V>.Zero;
            R one = Kean.Math.Abstract<R, V>.One;
            return new TransformType() { A = one, B = ((R)angle).Tangens(), C = zero, D = one, E = zero, F = zero };
        }
        #endregion
        #region Arithmetic Operators
        public static TransformType operator *(Transform<TransformType, TransformValue, R, V> left, ITransform<V> right)
        {
            return new TransformType()
            {
                A = left.A * right.A + left.C * right.B,
                B = left.B * right.A + left.D * right.B,
                C = left.A * right.C + left.C * right.D,
                D = left.B * right.C + left.D * right.D,
                E = left.A * right.E + left.C * right.F + left.E,
                F = left.B * right.E + left.D * right.F + left.F,
            };
        }
        #endregion
        #region ITransform<V> Members
        V ITransform<V>.A { get { return this.A; } }
        V ITransform<V>.B { get { return this.B; } }
        V ITransform<V>.C { get { return this.C; } }
        V ITransform<V>.D { get { return this.D; } }
        V ITransform<V>.E { get { return this.E; } }
        V ITransform<V>.F { get { return this.F; } }
        #endregion
        #region Comparison Operators
        public static bool operator ==(Transform<TransformType, TransformValue, R, V> left, TransformType right)
        {
            return object.ReferenceEquals(left, right) ||
                !object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) &&
                left.Equals(right);
        }
        public static bool operator !=(Transform<TransformType, TransformValue, R, V> left, TransformType right)
        {
            return !(left == right);
        }
        #endregion
        #region IEquatable<TransformType> Members
        public bool Equals(TransformType other)
        {
            return other.NotNull() && this.A == other.A && this.B == other.B && this.C == other.C && this.D == other.D && this.E == other.E && this.F == other.F;
        }
        #endregion
        #region Object Overrides
        /// <summary>
        /// Return true if this object and <paramref name="other">other</paramref> are equal.
        /// </summary>
        /// <param name="other">Object to compare with</param>
        /// <returns>True if this object and <paramref name="other">other</paramref> are equal else false.</returns>
        public override bool Equals(object other)
        {
            return (other is TransformType) && this.Equals(other as TransformType);
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>Hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return this.A.GetHashCode()
                ^ this.B.GetHashCode()
                ^ this.C.GetHashCode()
                ^ this.D.GetHashCode()
                ^ this.E.GetHashCode()
                ^ this.F.GetHashCode();
        }
        public override string ToString()
        {
            return this.A.ToString() + ", " + this.B.ToString() + ", " + this.C.ToString() + ", " + this.D.ToString() + ", " + this.E.ToString() + ", " + this.F.ToString();
        }
        #endregion
    }
}
