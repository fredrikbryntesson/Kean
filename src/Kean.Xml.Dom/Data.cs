using System;
using Kean.Core.Extension;

namespace Kean.Xml.Dom
{
	public class Data :
		Node
	{
		public string Value { get; set; }
		public Data() { }
		public Data(string value)
		{
			this.Value = value;
		}
        #region Object Overrides
        public override bool Equals(object other)
        {
            return this.Equals(other as Data);
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
        #region IEquatable<Data> Members
        public bool Equals(Data other)
        {
            return other.NotNull() &&
                this.Value == other.Value;
        }
        #endregion
        #region Operators
        public static bool operator ==(Data left, Data right)
        {
            return left.Same(right) || left.NotNull() && left.Equals(right);
        }
        public static bool operator !=(Data left, Data right)
        {
            return !(left == right);
        }
        #endregion
	}
}
