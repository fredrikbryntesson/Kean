using System;
using Kean.Core.Extension;

namespace Kean.Xml.Dom
{
	public class Attribute :
		Object
	{
		public string Name { get; set; }
		public string Value { get; set; }
		public Attribute() { }
		public Attribute(string name) :
			this()
		{
			this.Name = name;
		}
		public Attribute(string name, string value) :
			this(name)
		{
			this.Value = value;
		}
        #region Object Overrides
        public override bool Equals(object other)
        {
            return this.Equals(other as Attribute);
        }
        public override int GetHashCode()
        {
            return this.Name.Hash() ^ this.Value.Hash();
        }
        public override string ToString()
        {
            return base.ToString();
        }
        #endregion
        #region IEquatable<Attribute> Members
        public bool Equals(Attribute other)
        {
            return other.NotNull() &&
                this.Name == other.Name &&
                this.Value == other.Value;
        }
        #endregion
        #region Operators
        public static bool operator ==(Attribute left, Attribute right)
        {
            return left.Same(right) || left.NotNull() && left.Equals(right);
        }
        public static bool operator !=(Attribute left, Attribute right)
        {
            return !(left == right);
        }
        #endregion
	}
}
