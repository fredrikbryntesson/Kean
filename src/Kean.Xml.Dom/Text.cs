using System;
using Kean.Core;
using Kean.Core.Extension;

namespace Kean.Xml.Dom
{
	public class Text :
		Node
	{
		public string Value { get; set; }
		public Text() { }
		public Text(string value)
		{
			this.Value = value;
		}
		#region Object Overrides
		public override bool Equals(object other)
		{
			return this.Equals(other as Element);
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
		#region IEquatable<Text> Members
		public bool Equals(Text other)
		{
			return other.NotNull() &&
				this.Value == other.Value;
		}
		#endregion
		#region Operators
		public static bool operator ==(Text left, Text right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Text left, Text right)
		{
			return !(left == right);
		}
		#endregion
	}
}
