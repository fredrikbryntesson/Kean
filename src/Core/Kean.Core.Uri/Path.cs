// 
//  Path.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2012 Simon Mika
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
using Kean.Core;
using Kean.Core.Extension;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;

namespace Kean.Core.Uri
{
    public class Path :
        IString,
        IEquatable<Path>
    {
		PathLink head;
        #region IString Members
        public string String
        {
			get { return this.head.NotNull() ? this.head.String : ""; }
            set { this.head = new PathLink() { String = value }; }
        }
        #endregion
		public string PlattformPath
		{
			get { return this.head.NotNull() ? this.head.PlattformPath : ""; }
			set { this.head = new PathLink() { PlattformPath = value }; }
		}
		public bool Empty { get { return this.head.IsNull(); } }
		public bool IsFolder { get { return this.head.NotNull() && this.head.IsFolder; } }
		public Path Folder { get { return this.head.NotNull() ? new Path(this.head.Folder) : new Path(); } }
		public string Extension { get { return this.head.Extension; } set { this.head.Extension = value; } }
		public Path() { }
		public Path(params string[] path) :
			this(new PathLink(path))
		{ }
		Path(PathLink last) :
			this()
		{
			this.head = last;
		}
		public Path Copy()
		{
			return new Path(this.head.Copy());
		}
        #region IEquatable<Path> Members
        public bool Equals(Path other)
        {
            return other.NotNull() && this.head == other.head;
        }
        #endregion
        #region Object Overrides
        public override bool Equals(object other)
        {
            return other is Path && this.Equals(other as Path);
        }
        public override int GetHashCode()
        {
			return this.head.Hash();
        }
        public override string ToString()
        {
            return this.String;
        }
        #endregion
		public static Path FromPlattformPath(string path)
		{
			return new Path() { PlattformPath = path };
		}
		#region static operators
		#region Casts with System.IO.FileSystemInfo
		public static implicit operator Path(System.IO.FileSystemInfo item)
		{
			return new Path((PathLink)item);
		}
		public static explicit operator System.IO.DirectoryInfo(Path path)
		{
			return path.NotNull() ? (System.IO.DirectoryInfo)path.head : null;
		}
		#endregion
		#region Casts with string
		public static implicit operator string(Path path)
		{
			return path.IsNull() ? null : path.String;
		}
		public static implicit operator Path(string path)
		{
			return path.IsEmpty() ? null : new Path() { String = path };
		}
		#endregion
		#region Equality Operators
		public static bool operator ==(Path left, Path right)
        {
            return left.SameOrEquals(right);
        }
        public static bool operator !=(Path left, Path right)
        {
            return !(left == right);
		}
		#endregion
		#region Add Operator
		public static Path operator +(Path left, Path right)
		{
			return left.IsNull() ? right : right.IsNull() ? left : new Path(left.head + right.head);
		}
		#endregion
		#endregion
	}
}