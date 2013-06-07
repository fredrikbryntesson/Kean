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
		Collection.Linked.List<PathLink, string>,
		IString,
		IEquatable<Path>
	{
		#region IString Members
		public string String
		{
			get
			{
				string result = "";
				PathLink tail = this.Last;
				while (tail.NotNull())
				{
					result = "/" + tail.Head + result;
					tail = tail.Tail;
				}
				if (result.StartsWith("/."))
					result = result.TrimStart('/');
				return result;
			}
			set
			{
				PathLink tail = null;
				if (value.NotNull())
					foreach (string folder in value.TrimStart('/').Split('/'))
						tail = new PathLink(folder, tail);
				this.Last = tail;
			}
		}
		#endregion
		public string PlatformPath
		{
			get
			{
				string result = "";
				PathLink tail = this.Last;
                if (tail.NotNull())
                {
                    string name;
                    while (tail.NotNull())
                    {
                        name = tail.Head ?? "";
                        if (tail.Tail.IsNull() && name.EndsWith(":"))
                            name += System.IO.Path.DirectorySeparatorChar;
                        result = System.IO.Path.Combine(name, result);
                        tail = tail.Tail;
                    }
                    if (System.IO.Path.DirectorySeparatorChar == '/' && !result.StartsWith("."))
                        result = System.IO.Path.DirectorySeparatorChar + result;
                }
				return Path.ResolveSpecialFolderVariables(result);
			}
			set
			{
                if (value.NotEmpty() && (value[0] == System.IO.Path.DirectorySeparatorChar || value[0] == System.IO.Path.AltDirectorySeparatorChar))
                    value = value.Substring(1);
				value = Path.InsertSpecialFoldersVariables(value);
				PathLink tail = null;
				if (value.NotNull())
					foreach (string folder in value.Split(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar))
						tail = new PathLink(folder, tail);
				this.Last = tail;
			}
		}
		public bool Empty { get { return this.Last.IsNull(); } }
		public bool Folder { get { return this.Last.NotNull() && this.Last.Head.IsEmpty(); } }
		public Path FolderPath { get { return this.Last.NotNull() ? new Path(this.Last.Tail) : new Path(); } }
		public string Extension
		{
			get { return this.Last.NotNull() ? this.Last.Head.Split('.').Last() : null; }
			set
			{
				if (this.Last.NotNull())
					this.Last = new PathLink();
				string old = this.Extension;
				if (old.NotEmpty())
					this.Last.Head = this.Last.Head.Substring(0, this.Last.Head.Length - old.Length);
				this.Last.Head += "." + value;
			}
		}
		public Path() { }
		public Path(params string[] path) :
			this()
		{
			PathLink last = null;
			if (path.NotEmpty())
			{
				path.Reverse();
				foreach (string name in path)
					last = new PathLink(name, last);
			}
			this.Last = last;
		}
		Path(PathLink last) :
			this()
		{
			this.Last = last;
		}
		public Path Copy()
		{
			Func<PathLink, PathLink> copy = null;
			copy = link => link.IsNull() ? null : new PathLink(link.Head, copy(link.Tail));
			return new Path(copy(this.Last));
		}
		public Path Resolve(Path absolute)
		{
			Path result;
			switch (this[0])
			{
				case ".":
				case "..":
					result = absolute.FolderPath + this;
					break;
				default:
					result = this;
					break;
			}
			return result;
		}
		#region IEquatable<Path> Members
		public bool Equals(Path other)
		{
			bool result = true;
			PathLink myTail = this.Last;
			PathLink otherTail = other.Last;
			while (myTail.NotNull() && otherTail.NotNull())
			{
				result &= myTail.Head == otherTail.Head;
				myTail = myTail.Tail;
				otherTail = otherTail.Tail;
			}
			result &= myTail.IsNull() && otherTail.IsNull();
			return result;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is Path && this.Equals(other as Path);
		}
		public override int GetHashCode()
		{
			int result = 0;
			PathLink tail = this.Last;
			while (tail.NotNull())
			{
				result = 33 * result ^ tail.Head.Hash();
				tail = tail.Tail;
			}
			return result;
		}
		public override string ToString()
		{
			return this.String;
		}
		#endregion
		public static Path FromPlatformPath(string path)
		{
			return new Path() { PlatformPath = path };
		}
        public static Path FromRelativePlatformPath(string path)
        {
            return Path.FromPlatformPath(System.IO.Path.GetFullPath(path));
		}
		#region Resolve & Insert Special Folders Variables
		static KeyValue<string, string>[] specialFolders = new KeyValue<string,string>[] {
			KeyValue.Create("Desktop", System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop)),
			KeyValue.Create("Documents", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)),
			KeyValue.Create("Videos", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyVideos)),
			KeyValue.Create("Pictures", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures)),
			KeyValue.Create("Music", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic)),
			KeyValue.Create("ApplicationPath", System.IO.Path.GetFullPath(System.Environment.GetCommandLineArgs()[0]))
		};
		static string ResolveSpecialFolderVariables(string platformPath)
		{
			string result = platformPath;
			if (result.NotEmpty())
				foreach (KeyValue<string, string> specialFolder in Path.specialFolders)
					if (specialFolder.Value.NotEmpty())
						result = result.Replace("$(" + specialFolder.Key + ")", specialFolder.Value + System.IO.Path.DirectorySeparatorChar);
			return result;
		}
		static string InsertSpecialFoldersVariables(string platformPath)
		{
			string result = platformPath;
			if (result.NotEmpty())
				foreach (KeyValue<string, string> specialFolder in Path.specialFolders)
					if (specialFolder.Value.NotEmpty())
						result = result.Replace(specialFolder.Value + System.IO.Path.DirectorySeparatorChar, "$(" + specialFolder.Key + ")");
			return result;
		}
		#endregion
		#region static operators
		#region Casts with System.IO.FileSystemInfo
		static PathLink Create(System.IO.DirectoryInfo directory)
		{
			return directory.IsNull() ? null : new PathLink() { Head = directory.Name.TrimEnd('\\'), Tail = directory.Parent.NotNull() ? Path.Create(directory.Parent) : null };
		}
		static PathLink Create(System.IO.FileInfo file)
		{
			return file.IsNull() ? null : new PathLink() { Head = file.Name.TrimEnd('\\'), Tail = file.Directory.NotNull() ? Path.Create(file.Directory) : null };
		}
		#region Casts with System.IO.FileSystemInfo
		public static implicit operator Path(System.IO.FileSystemInfo item)
		{
			return new Path(item is System.IO.DirectoryInfo ? Path.Create(item as System.IO.DirectoryInfo) : item is System.IO.FileInfo ? Path.Create(item as System.IO.FileInfo) : null);
		}
		public static explicit operator System.IO.DirectoryInfo(Path path)
		{
			return path.NotNull() ? new System.IO.DirectoryInfo(path.PlatformPath) : null;
		}
		#endregion
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
			Path result;
			if (right.NotNull() && right.Last.NotNull())
			{
				result = right.Copy();
				if (left.NotNull())
				{
					PathLink first = result.Last;
					while (first.Tail.NotNull())
						first = first.Tail;
					first.Tail = left.Copy().Last;
				}
			}
			else if (left.NotNull())
				result = left.Copy();
			else
				result = new Path();
			return result;
		}
		#endregion
		#endregion
	}
}