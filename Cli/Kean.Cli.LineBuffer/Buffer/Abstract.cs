// 
//  Abstract.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2013 Simon Mika
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

namespace Kean.Cli.LineBuffer.Buffer
{
	abstract class Abstract
	{
		protected abstract string Value { get; }
		protected Abstract()
		{ }
		public abstract void MoveCursorHome();
		public abstract void MoveCursorLeft();
		public abstract void MoveCursorEnd();
		public abstract void MoveCursorRight();
		public abstract void DeletePreviousCharacter();
		public abstract void MoveCursorLeftAndNotDelete();
		public abstract void DeleteCurrentCharacter();
		public abstract void Insert(char value);
		public abstract void Insert(string value);
		public abstract void Renew(string line);
		public abstract void MoveCursor(int steps);
		public abstract void RemoveAndNotDelete();
		public abstract void Write();
		public abstract Abstract Copy();
		#region Object overides and IEquatable<Buffer>
		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}
		public override string ToString()
		{
			return this.Value;
		}
		#endregion
		#region  IEquatable<Buffer.Abstract>
		public override bool Equals(object other)
		{
			return this.Equals(other as Abstract);
		}
		public bool Equals(Abstract other)
		{
			return other.NotNull() && this.Value == other.Value;
		}
		#endregion
		#region Comparison Functions and IComparable<Buffer>
		public static bool operator ==(Abstract left, Abstract right)
		{
			return left.SameOrEquals(right);
		}
		public static bool operator !=(Abstract left, Abstract right)
		{
			return !(left == right);
		}
		#endregion
	}
}
