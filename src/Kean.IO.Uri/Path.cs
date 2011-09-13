// 
//  Path.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2011 Simon Mika
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

namespace Kean.IO.Uri
{
    public class Path :
		Collection.ILink<Path, string>,
        IString,
        IEquatable<Path>
    {
        #region ILink<Path, string> Members
		public string Head { get; set; }
		public Path Tail { get; set; }
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
                    string[] splitted = value.Split(new char[] { '/' }, 2);
                    this.Head = splitted[0];
                    this.Tail = splitted.Length > 1 ?  new Path() { String = splitted[1] } : null;
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
					this.Tail = splitted.Length > 1 ? new Path() { PlattformPath = splitted[1] } : null;
				}
			}
		}
		public Path() { }
		public Path(params string[] path) :
			this(0, path) { }
		Path(int skip, string[] path) :
			this()
		{
			if (path.NotNull() && path.Length > skip)
			{
				this.Head = path[skip];
				if (path.Length > skip + 1)
					this.Tail = new Path(skip + 1, path);
			}
		}
		public Path(string head, Path tail) :
			this()
		{
			this.Head = head;
			this.Tail = tail;
		}
		public Path Copy()
		{
			return new Path(this.Head, this.Tail.NotNull() ? this.Tail.Copy() : null);
		}
        #region IEquatable<Path> Members
        public bool Equals(Path other)
        {
            return !object.ReferenceEquals(other, null) && this.Head == other.Head && this.Tail == other.Tail;
        }
        #endregion
        #region Object Overrides
        public override bool Equals(object other)
        {
            return other is Path && this.Equals(other as Path);
        }
        public override int GetHashCode()
        {
            return this.Head.GetHashCode() ^ base.GetHashCode();
        }
        public override string ToString()
        {
            return this.String;
        }
        #endregion
		public static Path FromPlattformPath(string path)
		{
			return path.NotEmpty() ? new Path() { PlattformPath = path } : null;
		}
		#region Operators
		static Path Create(System.IO.DirectoryInfo directory, Path tail)
		{
			Path result = null;
			if (directory.NotNull())
			{
				result = new Path() { Head = directory.Name.TrimEnd('\\'), Tail = tail };
				if (directory.Parent.NotNull())
					result = Path.Create(directory.Parent, result);
			}
			return result;
		}
		static Path Create(System.IO.FileInfo file, Path tail)
		{
			Path result = null;
			if (file.NotNull())
			{
				result = new Path() { Head = file.Name, Tail = tail };
				if (file.Directory.NotNull())
					result = Path.Create(file.Directory, result);
			}
			return result;
		}
		public static implicit operator Path(System.IO.FileSystemInfo item)
		{
			return item is System.IO.DirectoryInfo ? Path.Create(item as System.IO.DirectoryInfo, null) : item is System.IO.FileInfo ? Path.Create(item as System.IO.FileInfo, null) : null;
		}
		public static explicit operator System.IO.DirectoryInfo(Path path)
		{
			return path.NotNull() ? new System.IO.DirectoryInfo(path.PlattformPath) : null;
		}
		public static implicit operator string(Path path)
		{
			return path.IsNull() ? null : path.String;
		}
		public static implicit operator Path(string path)
		{
			return path.IsEmpty() ? null : new Path() { String = path };
		}
		public static bool operator ==(Path left, Path right)
        {
            return object.ReferenceEquals(left, right) || (!object.ReferenceEquals(left, null) && left.Equals(right));
        }
        public static bool operator !=(Path left, Path right)
        {
            return !(left == right);
        }
        #endregion
    }
}