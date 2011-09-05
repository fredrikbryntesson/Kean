using System;
using Kean.Core.Extension;

namespace Kean.Xml.Dom
{
	public class Comment :
		Node
	{
		public string Value { get; set; }
		public Comment() { }
		public Comment(string value)
		{
			this.Value = value;
		}
        #region Object Overrides
        public override bool Equals(object other)
        {
            return this.Equals(other as Comment);
        }
        public override int GetHashCode()
        {
            return this.Value.Hash();
        }
        public override string ToString()
        {
            return base.ToString();
        }
        #endregion
        #region IEquatable<Comment> Members
        public bool Equals(Comment other)
        {
            return other.NotNull() &&
                this.Value == other.Value;
        }
        #endregion
        #region Operators
        public static bool operator ==(Comment left, Comment right)
        {
            return left.Same(right) || left.NotNull() && left.Equals(right);
        }
        public static bool operator !=(Comment left, Comment right)
        {
            return !(left == right);
        }
        #endregion
	}
}
