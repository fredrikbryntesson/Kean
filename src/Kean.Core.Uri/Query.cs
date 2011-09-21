// 
//  Query.cs
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
using Kean.Core.Collection.Linked.Extension;

namespace Kean.Core.Uri
{
    public class Query :
		Collection.ILink<Query, KeyValue<string, string>>,
        IString,
        IEquatable<Query>
    {
        public string this[string key]
        {
            get { return this.Head.Key == key ? this.Head.Value : (this.Tail.NotNull() ? this.Tail[key] : null); }
            set
            {
                if (this.Head.Key == key)
                    this.Head = KeyValue.Create(key, value);
                else if (this.Tail.NotNull())
                    this.Tail[key] = value;
                else
                    this.Tail = new Query(key, value);
            }
        }
        public Query()
        {
        }
		public Query(KeyValue<string, string> head) :
			this()
		{
            this.Head = head;
		}
		public Query(string key, string value) :
			this(KeyValue.Create(key, value))
		{ }
		public Query(KeyValue<string, string> head, Query tail) :
			this(head)
		{
			this.Tail = tail;
		}
		public Query(string key, string value, Query tail) :
			this(KeyValue.Create(key, value), tail)
		{ }
		public Query Copy()
		{
			return new Query(this.Head, this.Tail.NotNull() ? this.Tail.Copy() : null);
		}
        public void Add(string key, string value)
        {
            this.Add(KeyValue.Create(key, value)); 
        }
        #region ILink<Query, KeyValue<string, string>> Members
		public KeyValue<string, string> Head { get; set; }
        public Query Tail { get; set; }
        #endregion
        #region IString Members
        public string String
        {
            get
            {
                System.Text.StringBuilder result = new System.Text.StringBuilder(this.Head.Key);
                if (this.Head.Value.NotNull())
                {
                    result.Append("=");
                    result.Append(this.Head.Value);
                }
                if (this.Tail != null)
                {
                    result.Append("&");
                    result.Append(this.Tail.String);
                }
                return result.ToString();
            }
            set
            {
                if (value.IsEmpty())
                {
                    this.Head = KeyValue.Create<string, string>(null, null);
                    this.Tail = null;
                }
                else
                {
                    string[] splitted = value.Split(new char[] { '&', ';' }, 2);
                    string[] keyValue = splitted[0].Split(new char[] { '=' }, 2);
                    this.Head = KeyValue.Create(keyValue[0], keyValue.Length > 1 ? keyValue[1] : null);
                    this.Tail = splitted.Length > 1 ? new Query() { String = splitted[1] } : null;
                }
            }
        }
        #endregion
        #region IEquatable<Query> Members
        public bool Equals(Query other)
        {
            return other.NotNull() && this.Head == other.Head && this.Tail == other.Tail;
        }
        #endregion
        #region Object Overrides
        public override bool Equals(object other)
        {
            return other is Query && this.Equals(other as Query);
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
        #region Operators
        public static bool operator ==(Query left, Query right)
        {
            return left.SameOrEquals(right);
        }
        public static bool operator !=(Query left, Query right)
        {
            return !(left == right);
        }
        public static implicit operator string(Query query)
        {
            return query.IsNull() ? null : query.String;
        }
        public static implicit operator Query(string query)
        {
            return query.IsEmpty() ? null : new Query() { String = query };
        }
        #endregion
    }
}