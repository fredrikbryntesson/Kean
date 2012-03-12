// 
//  PathLink.cs
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
	class PathLink :
		Collection.ILink<PathLink, string>,
		IString,
		IEquatable<PathLink>
	{
		#region ILink<PathLink, string> Members
		public string Head { get; set; }
		public PathLink Tail { get; set; }
		#endregion
		#region IString Members
		public string String
		{
			get
			{
				System.Text.StringBuilder result = new System.Text.StringBuilder(this.Head);
				if (this.Tail.NotNull())
				{
					result.Append("/");
					result.Append(this.Tail.String);
				}
				return result.ToString();
			}
			set
			{
				if (value.IsEmpty())
				{
					this.Head = null;
					this.Tail = null;
				}
				else
				{
					string[] splitted = value.Replace('+', ' ').Split(new char[] { '/' }, 2);
					this.Head = splitted[0];
					this.Tail = splitted.Length > 1 ? new PathLink() { String = splitted[1] } : null;
				}
			}
		}
		#endregion
		public string PlattformPath
		{
			get
			{
				string name = this.Head ?? "";
				if (name.EndsWith(":"))
					name += System.IO.Path.DirectorySeparatorChar;
				return this.Tail.NotNull() ? System.IO.Path.Combine(name, this.Tail.PlattformPath) : name;
			}
			set
			{
				if (value.IsEmpty())
				{
					this.Head = null;
					this.Tail = null;
				}
				else
				{
					string[] splitted = value.Split(new char[] { System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar }, 2);
					this.Head = splitted[0];
					this.Tail = splitted.Length > 1 ? new PathLink() { PlattformPath = splitted[1] } : null;
				}
			}
		}
		public bool IsFolder { get { return this.Tail.IsNull() ? this.Head.IsEmpty() : this.Tail.IsFolder; } }
		public PathLink Folder { get { return this.Tail.IsNull() ? new PathLink() : new PathLink(this.Head, this.Tail.Folder); } }
		public string Extension 
		{ 
			get { return this.Tail.IsNull() ? (this.Head.NotEmpty() ? this.Head.Split('.').Last() : "") : this.Tail.Extension; }
			set
			{
				if (this.Tail.IsNull())
				{
					string old = this.Extension;
					if (old.NotEmpty())
						this.Head = this.Head.Substring(0, this.Head.Length - old.Length);
					this.Head += "." + value;
				}
				else
					this.Tail.Extension = value;
			}
		}
		public PathLink() { }
		public PathLink(params string[] PathLink) :
			this(0, PathLink) { }
		PathLink(int skip, string[] PathLink) :
			this()
		{
			if (PathLink.NotNull() && PathLink.Length > skip)
			{
				this.Head = PathLink[skip];
				if (PathLink.Length > skip + 1)
					this.Tail = new PathLink(skip + 1, PathLink);
			}
		}
		public PathLink(string head, PathLink tail) :
			this()
		{
			this.Head = head;
			this.Tail = tail;
		}
		public PathLink Copy()
		{
			return new PathLink(this.Head, this.Tail.NotNull() ? this.Tail.Copy() : null);
		}
		#region IEquatable<PathLink> Members
		public bool Equals(PathLink other)
		{
			return other.NotNull() && this.Head == other.Head && this.Tail == other.Tail;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is PathLink && this.Equals(other as PathLink);
		}
		public override int GetHashCode()
		{
			return this.Head.Hash() ^ this.Tail.Hash();
		}
		public override string ToString()
		{
			return this.String;
		}
		#endregion
		public static PathLink FromPlattformPathLink(string PathLink)
		{
			return PathLink.NotEmpty() ? new PathLink() { PlattformPath = PathLink } : null;
		}
		#region Operators
		static PathLink Create(System.IO.DirectoryInfo directory, PathLink tail)
		{
			PathLink result = null;
			if (directory.NotNull())
			{
				result = new PathLink() { Head = directory.Name.TrimEnd('\\'), Tail = tail };
				if (directory.Parent.NotNull())
					result = PathLink.Create(directory.Parent, result);
			}
			return result;
		}
		static PathLink Create(System.IO.FileInfo file, PathLink tail)
		{
			PathLink result = null;
			if (file.NotNull())
			{
				result = new PathLink() { Head = file.Name, Tail = tail };
				if (file.Directory.NotNull())
					result = PathLink.Create(file.Directory, result);
			}
			return result;
		}
		#region Casts with System.IO.FileSystemInfo
		public static implicit operator PathLink(System.IO.FileSystemInfo item)
		{
			return item is System.IO.DirectoryInfo ? PathLink.Create(item as System.IO.DirectoryInfo, null) : item is System.IO.FileInfo ? PathLink.Create(item as System.IO.FileInfo, null) : null;
		}
		public static explicit operator System.IO.DirectoryInfo(PathLink PathLink)
		{
			return PathLink.NotNull() ? new System.IO.DirectoryInfo(PathLink.PlattformPath) : null;
		}
		#endregion
		#region Casts with string
		public static implicit operator string(PathLink PathLink)
		{
			return PathLink.IsNull() ? null : PathLink.String;
		}
		public static implicit operator PathLink(string PathLink)
		{
			return PathLink.IsEmpty() ? null : new PathLink() { String = PathLink };
		}
		#endregion
		#region Equality Operators
		public static bool operator ==(PathLink left, PathLink right)
		{
			return object.ReferenceEquals(left, right) || (!object.ReferenceEquals(left, null) && left.Equals(right));
		}
		public static bool operator !=(PathLink left, PathLink right)
		{
			return !(left == right);
		}
		#endregion
		#region Add Operator
		public static PathLink operator +(PathLink left, PathLink right)
		{
			if (left.IsNull() || left.Tail.IsNull() && left.Head.IsEmpty())
				left = right;
			else
				left.Tail = left.Tail + right;
			return left;
		}
		#endregion
		#endregion
	}
}