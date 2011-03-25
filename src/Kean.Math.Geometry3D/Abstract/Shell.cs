using System;

namespace Kean.Math.Geometry3D.Abstract
{
    public abstract class Shell<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> :
        IShell<V>
        where ShellType : Shell<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, IShell<V>, new()
        where ShellValue : struct, IShell<V>
        where TransformType : Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, ITransform<V>, new()
        where TransformValue : struct, ITransform<V>
        where BoxType : Box<TransformType, TransformValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, IBox<PointValue, SizeValue, V>, new()
        where BoxValue : struct, IBox<PointValue, SizeValue, V>
        where PointType : Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>, IPoint<V>, new()
        where PointValue : struct, IPoint<V>, IVector<V>
        where SizeType : Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, ISize<V>, new()
        where SizeValue : struct, ISize<V>, IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        R left;
        R right;
        R top;
        R bottom;
        R front;
        R back;
        public V Left { get { return this.left; } }
        public V Right { get { return this.right; } }
        public V Top { get { return this.top; } }
        public V Bottom { get { return this.bottom; } }
        public V Front { get { return this.front; } }
        public V Back { get { return this.back; } }
        public abstract ShellValue Value { get; }

        #region Constructors
        protected Shell()
        {
        }
        protected Shell(V left, V right, V top, V bottom, V front, V back)
        {
            this.left = (R)left;
            this.right = (R)right;
            this.top = (R)top;
            this.bottom = (R)bottom;
            this.front = (R)front;
            this.back = (R)back;
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
                (R)left.Bottom == right.Bottom &&
                (R)left.Front == right.Front &&
                (R)left.Back == right.Back;
                

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
            return this.Left.GetHashCode() ^ this.Right.GetHashCode() ^ this.Top.GetHashCode() ^ this.Bottom.GetHashCode() ^ this.Front.GetHashCode() ^ this.Back.GetHashCode();
        }
        #endregion
    }
}
