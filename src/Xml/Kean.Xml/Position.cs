using System;
using Kean.Core;
using Kean.Core.Extension;

namespace Kean.Xml
{
	public class Position :
		IEquatable<Position>
	{
		public int Line { get; private set; }
		public int Column { get; private set; }
		public Position(int line, int column)
		{
			this.Line = line;
			this.Column = column;
		}
		#region Object Overrides
		public override bool Equals(object other)
		{
			return this.Equals(other as Position);
		}
		public override int GetHashCode()
		{
			return this.Line.GetHashCode() ^ this.Column.GetHashCode();
		}
		public override string ToString()
		{
			return string.Format("Ln{0} Col{1}", this.Line, this.Column);
		}
		#endregion
		#region IEquatable<Position> Members
		public bool Equals(Position other)
		{
			return other.NotNull() && this.Line == other.Line && this.Column == other.Column;
		}
		#endregion
		#region Operators
		public static bool operator ==(Position left, Position right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Position left, Position right)
		{
			return !(left == right);
		}
		#endregion
	}
}
