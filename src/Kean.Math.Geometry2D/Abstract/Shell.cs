using System;

namespace Kean.Math.Geometry2D.Abstract
{  
		public abstract class Shell<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> :
        IShell<V>,
		IEquatable<ShellType>
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
        R left;
        R right;
        R top;
        R bottom;
        public V Left { get { return this.left; } }
        public V Right { get { return this.right; } }
        public V Top { get { return this.top; } }
        public V Bottom { get { return this.bottom; } }
        public abstract ShellValue Value { get; }

        #region Constructors
        protected Shell()
        {
        }
        protected Shell(V left, V right, V top, V bottom)
        {
            this.left = (R)left;
            this.right = (R)right;
            this.top = (R)top;
            this.bottom = (R)bottom;
        }
        #endregion
		#region Increase, Decrease
		public BoxType Decrease(ISize<V> size)
		{
			return Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(
				Point < TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V > .Create(this.Left, this.Top),
				Size < TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V > .Create(((R)size.Width) - this.Left - this.Right, ((R)size.Height) - this.Top - this.Bottom));
		}
		public BoxType Increase(ISize<V> size)
		{
			return Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(
				Point<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(((R)this.Left).Negate(), ((R)this.Right).Negate()),
				Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(((R)size.Width) + this.Left + this.Right, ((R)size.Height) + this.Top + this.Bottom));
		}
		#endregion
		#region Comparison Operators
		public static bool operator ==(Shell<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, IShell<V> right)
        {
            return object.ReferenceEquals(left, right) ||
                !object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) &&
                (R)left.Left == right.Left &&
                (R)left.Right == right.Right &&
                (R)left.Top == right.Top &&
                (R)left.Bottom == right.Bottom;

        }
        public static bool operator !=(Shell<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, IShell<V> right)
        {
            return !(left == right);
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
            return (other is ShellType) && this.Equals(other as ShellType);
        }
        /// <summary>
        /// Return true if this object and <paramref name="other">other</paramref> are equal.
        /// </summary>
        /// <param name="other">Object to compare with</param>
        /// <returns>True if this object and <paramref name="other">other</paramref> are equal else false.</returns>
        public bool Equals(ShellType other)
        {
            return this == other;
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>Hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return this.Left.GetHashCode() ^ this.Right.GetHashCode() ^ this.Top.GetHashCode() ^ this.Bottom.GetHashCode();
        }
		public override string ToString()
		{
			return this.ToString(true);
		}
        public string ToString(bool commaSeparated)
        {
			return ((R)this.Left).ToString() + (commaSeparated ? ", " : " ") + ((R)this.Right).ToString() + (commaSeparated ? ", " : " ") + ((R)this.Top).ToString() + (commaSeparated ? ", " : " ") + ((R)this.Bottom).ToString();
        }
        #endregion
        public static ShellType Create(V left, V right, V top, V bottom)
        {
            ShellType result = new ShellType();
            result.left = (R)left;
            result.right = (R)right;
            result.top = (R)top;
            result.bottom = (R)bottom;
            return result;
        }
    }
}
