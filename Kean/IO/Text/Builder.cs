// 
//  Builder.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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
using Collection = Kean.Collection;
using Kean;
using Kean.Extension;
using Kean.Collection.Extension;

namespace Kean.IO.Text
{
    public class Builder
    {
        Collection.IList<string> data;
		public int Length { get { return this.data.Fold((item, result) => result + item.Length, 0); } }
        public Builder()
        { 
            this.data = new Collection.Linked.List<string>();
        }
        public Builder(string value) :
            this()
        {
            this.Append(value);
        }
        protected Builder(Builder original) :
            this()
        {
            original.data.Apply(item => this.Append(item));
        }
        public Builder Copy()
        {
            return new Builder(this);
        }
        #region Append
        public Builder Append(Builder value) 
        {
            this.data.Add(value.data);
            return this;
        }
        public Builder Append(char value) { return this.Append(new string(value, 1)); }
        public Builder Append(string format, params object[] arguments) { return this.Append(string.Format(format, arguments)); }
        public Builder Append(string value)
        {
            this.data.Add(value);
            return this;
        }
        #endregion
        #region Prepend
        public Builder Prepend(Builder value)
        {
            value.data.Apply(item => this.Prepend(item));
            return this;
        }
        public Builder Prepend(char value) { return this.Prepend(new string(value, 1)); }
        public Builder Prepend(string format, params object[] arguments) { return this.Prepend(string.Format(format, arguments)); }
        public Builder Prepend(string value)
        {
            this.data.Insert(0, value);
            return this;
        }
        #endregion
        #region Casts
        public static implicit operator string(Builder builder)
        {
			string result;
			if (builder.NotNull())
			{
				result = builder.data.Fold((item, accumulation) => accumulation + item, "");
				builder.data = new Collection.Linked.List<string>();
				builder.Append(result);
			}
			else
				result = "";
            return result;
        }
        public static implicit operator Builder(string value)
        {
            return new Builder(value);
        }
        #endregion
        #region Binary operators
        public static Builder operator +(Builder left, Builder right)
        {
            return left.NotNull() ? left.Copy().Append(right) : new Builder().Append(right);
        }
        public static Builder operator +(Builder left, string right)
        {
            return left.NotNull() ? left.Copy().Append(right) : new Builder(right);
        }
        public static Builder operator +(string left, Builder right)
        {
			return right.NotNull() ? right.Copy().Prepend(left) : new Builder(left);
        }
        #endregion

		#region Object Overrides
		public override int GetHashCode()
		{
			return ((string)this).GetHashCode();
		}
		public override bool Equals(object other)
		{
			return ((string)this).Equals(other);
		}
		public override string ToString()
		{
			return this;
		}
		#endregion
	}
}
