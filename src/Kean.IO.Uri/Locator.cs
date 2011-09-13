// 
//  Locator.cs
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

namespace Kean.IO.Uri
{
	public class Locator :
		IString,
		IEquatable<Locator>
	{
		public Scheme Scheme { get; set; }
		public Authority Authority { get; set; }
		public Path Path { get; set; }
		public Query Query { get; set; }
		public string Fragment { get; set; }

		#region IString Members
		public string String
		{
			get
			{
				System.Text.StringBuilder result = new System.Text.StringBuilder();
				if (this.Scheme != null)
					result.AppendFormat("{0}://", this.Scheme);
				if (this.Authority != null)
					result.Append(this.Authority);
				result.Append("/");
				if (this.Path != null)
					result.Append(this.Path);
				if (this.Query != null)
					result.AppendFormat("?{0}", this.Query);
				if (this.Fragment != null)
					result.AppendFormat("#{0}", this.Fragment);
				return result.ToString();
			}
			set
			{
				if (!value.NotEmpty())
				{
					this.Scheme = null;
					this.Authority = null;
					this.Path = null;
					this.Query = null;
					this.Fragment = null;
				}
				else
				{
					string[] splitted = value.Split(new string[] { "://" }, 2, StringSplitOptions.RemoveEmptyEntries);
					if (splitted.Length > 1) // has scheme
					{
						this.Scheme = splitted[0];
						value = splitted[1];
					}
					else
						value = splitted[0];
					if (!value.StartsWith("/"))
					{
						splitted = value.Split(new char[] { '/', '#', '?' }, 2);
						this.Authority = splitted[0];
						if (splitted.Length > 1)
							value = value.Substring(splitted[0].Length);
						else
							value = null;
					}
					if (value.NotEmpty())
					{
						value = value.TrimStart('/');
						if (value.NotEmpty())
						{
							if (value.StartsWith("#"))
							{
								this.Fragment = value.TrimStart('#');
								value = null;
							}
							else
							{
								splitted = value.Split(new char[] { '#' }, 2, StringSplitOptions.RemoveEmptyEntries);
								if (splitted.Length > 1)
								{
									this.Fragment = splitted[1];
									value = splitted[0];
								}
							}
							if (value.NotEmpty())
							{
								if (value.StartsWith("?"))
									this.Query = value.TrimStart('?');
								else
								{
									splitted = splitted[0].Split(new char[] { '?' }, 2, StringSplitOptions.RemoveEmptyEntries);
									if (splitted.Length > 1)
										this.Query = splitted[1];
									this.Path = splitted[0];
								}
							}
						}
					}
				}
			}
		}
		#endregion
		public Locator() { }
		public Locator(Scheme scheme, Path path) : this(scheme, null, path) { }
		public Locator(Scheme scheme, Authority authority, Path path) : this(scheme, authority, path, null, null) { }
		public Locator(Scheme scheme, Authority authority, Path path, Query query) : this(scheme, authority, path, query, null) { }
		public Locator(Scheme scheme, Authority authority, Path path, Query query, string fragment) :
			this()
		{
			this.Scheme = scheme;
			this.Authority = authority;
			this.Path = path;
			this.Query = query;
			this.Fragment = fragment;
		}
		public Locator Copy()
		{
			return new Locator(this.Scheme.IsNull() ? null : this.Scheme.Copy(), this.Authority.IsNull() ? null : this.Authority.Copy(), this.Path.IsNull() ? null : this.Path.Copy(), this.Query.IsNull() ? null : this.Query.Copy(), this.Fragment);
		}
		#region IEquatable<Locator> Members
		public bool Equals(Locator other)
		{
			return !object.ReferenceEquals(other, null) &&
				this.Scheme == other.Scheme &&
				this.Authority == other.Authority &&
				this.Path == other.Path &&
				this.Query == other.Query &&
				this.Fragment == other.Fragment;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is Locator && base.Equals(other as Locator);
		}
		public override int GetHashCode()
		{
			int result = 0;
			if (this.Scheme != null)
				result ^= this.Scheme.GetHashCode();
			if (this.Authority != null)
				result ^= this.Authority.GetHashCode();
			if (this.Path != null)
				result ^= this.Path.GetHashCode();
			if (this.Query != null)
				result ^= this.Query.GetHashCode();
			if (this.Fragment != null)
				result ^= this.Fragment.GetHashCode();
			return result;
		}
		public override string ToString()
		{
			return this.String;
		}
		#endregion
		public static Locator FromPlattformPath(string path)
		{
			return path.NotEmpty() ? new Locator("file", Path.FromPlattformPath(path)) : null;
		}
		public static implicit operator Locator(System.IO.DirectoryInfo directory)
		{
			return directory.NotNull() ?  new Locator("file", directory) : null;
		}
		#region Operators
		public static bool operator ==(Locator left, Locator right)
		{
			return left.Same(right) || (left.NotNull() && left.Equals(right));
		}
		public static bool operator !=(Locator left, Locator right)
		{
			return !(left == right);
		}
		public static implicit operator string(Locator locator)
		{
			return locator.IsNull() ? null : locator.String;
		}
		public static implicit operator Locator(string locator)
		{
			return locator.NotEmpty() ? new Locator() { String = locator } : null;
		}
		#endregion
	}
}
