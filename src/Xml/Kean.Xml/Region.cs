using System;
using Kean.Core.Extension;

namespace Kean.Xml
{
	public class Region :
		IEquatable<Region>
	{
		public string Resource { get; private set; }
		public Position Start { get; private set; }
		public Position End { get; private set; }
		public Region(string resource, Position start, Position end)
		{
			this.Resource = resource;
			this.Start = start;
			this.End = end;
		}
		#region Object Overrides
		public override bool Equals(object other)
		{
			return this.Equals(other as Region);
		}
		public override int GetHashCode()
		{
			return this.Resource.Hash() ^ this.Start.Hash() ^ this.End.Hash();
		}
		public override string ToString()
		{
			return string.Format("{0} ({1} - {2})", this.Resource, this.Start, this.End);
		}
		#endregion
		#region IEquatable<Region> Members
		public bool Equals(Region other)
		{
			return other.NotNull() && this.Resource == other.Resource && this.Start == other.Start && this.End == other.End;
		}
		#endregion
		#region Operators
		public static bool operator ==(Region left, Region right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Region left, Region right)
		{
			return !(left == right);
		}
		#endregion
	}
}
