// 
//  Tuple.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 Simon Mika
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

namespace Kean.Core
{
	public static class Tuple
	{
		public static Tuple<T1> Create<T1>(T1 item1) { return new Tuple<T1>(item1); }
		public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2) { return new Tuple<T1, T2>(item1, item2); }
		public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) { return new Tuple<T1, T2, T3>(item1, item2, item3); }
		public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) { return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4); }
		public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) { return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5); }
		public static Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) { return new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6); }
		public static Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) { return new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7); }
		public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) { return new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>>(item1, item2, item3, item4, item5, item6, item7, new Tuple<T8>(item8)); }
	}
	public class Tuple<T1> :
		ITuple
	{
		public T1 Item1 { get; private set; }
		public Tuple(T1 item1)
		{
			this.Item1 = item1;
		}
		#region Object overrides
		public override bool Equals (object other)
		{
			return other is Tuple<T1> && (other as Tuple<T1>).Item1.SameOrEquals(this.Item1);
		}
		public override int GetHashCode ()
		{
			return this.Item1.Hash ();
		}
		public override string ToString ()
		{
			return string.Format("({0})", this.Item1);
		}
		#endregion
	}
	public class Tuple<T1, T2> :
		ITuple
	{
		public T1 Item1 { get; private set; }
		public T2 Item2 { get; private set; }
		public Tuple(T1 item1, T2 item2)
		{
			this.Item1 = item1;
			this.Item2 = item2;
		}
		#region Object overrides
		public override bool Equals (object other)
		{
			return other is Tuple<T1, T2> && 
				(other as Tuple<T1, T2>).Item1.SameOrEquals(this.Item1) && 
				(other as Tuple<T1, T2>).Item2.SameOrEquals(this.Item2);
		}
		public override int GetHashCode ()
		{
			return this.Item1.Hash () ^
				this.Item2.Hash ();
		}
		public override string ToString ()
		{
			return string.Format("({0}, {1})", this.Item1, this.Item2);
		}
		#endregion
	}
	public class Tuple<T1, T2, T3> :
		ITuple
	{
		public T1 Item1 { get; private set; }
		public T2 Item2 { get; private set; }
		public T3 Item3 { get; private set; }
		public Tuple(T1 item1, T2 item2, T3 item3)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
		}
		#region Object overrides
		public override bool Equals (object other)
		{
			return other is Tuple<T1, T2, T3> && 
				(other as Tuple<T1, T2, T3>).Item1.SameOrEquals(this.Item1) && 
				(other as Tuple<T1, T2, T3>).Item2.SameOrEquals(this.Item2) && 
				(other as Tuple<T1, T2, T3>).Item3.SameOrEquals(this.Item3);
		}
		public override int GetHashCode ()
		{
			return this.Item1.Hash () ^
				this.Item2.Hash () ^
				this.Item3.Hash ();
		}
		public override string ToString ()
		{
			return string.Format("({0}, {1}, {2})", this.Item1, this.Item2, this.Item3);
		}
		#endregion
	}
	public class Tuple<T1, T2, T3, T4> :
		ITuple
	{
		public T1 Item1 { get; private set; }
		public T2 Item2 { get; private set; }
		public T3 Item3 { get; private set; }
		public T4 Item4 { get; private set; }
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
		}
		#region Object overrides
		public override bool Equals (object other)
		{
			return other is Tuple<T1, T2, T3, T4> && 
				(other as Tuple<T1, T2, T3, T4>).Item1.SameOrEquals(this.Item1) && 
				(other as Tuple<T1, T2, T3, T4>).Item2.SameOrEquals(this.Item2) && 
				(other as Tuple<T1, T2, T3, T4>).Item3.SameOrEquals(this.Item3) && 
				(other as Tuple<T1, T2, T3, T4>).Item4.SameOrEquals(this.Item4);
		}
		public override int GetHashCode ()
		{
			return this.Item1.Hash () ^
				this.Item2.Hash () ^
				this.Item3.Hash () ^
				this.Item4.Hash ();
		}
		public override string ToString ()
		{
			return string.Format("({0}, {1}, {2}, {3})", this.Item1, this.Item2, this.Item3, this.Item4);
		}
		#endregion
	}
	public class Tuple<T1, T2, T3, T4, T5> :
		ITuple
	{
		public T1 Item1 { get; private set; }
		public T2 Item2 { get; private set; }
		public T3 Item3 { get; private set; }
		public T4 Item4 { get; private set; }
		public T5 Item5 { get; private set; }
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
		}
		#region Object overrides
		public override bool Equals (object other)
		{
			return other is Tuple<T1, T2, T3, T4, T5> && 
				(other as Tuple<T1, T2, T3, T4, T5>).Item1.SameOrEquals(this.Item1) && 
				(other as Tuple<T1, T2, T3, T4, T5>).Item2.SameOrEquals(this.Item2) && 
				(other as Tuple<T1, T2, T3, T4, T5>).Item3.SameOrEquals(this.Item3) && 
				(other as Tuple<T1, T2, T3, T4, T5>).Item4.SameOrEquals(this.Item4) && 
				(other as Tuple<T1, T2, T3, T4, T5>).Item5.SameOrEquals(this.Item5);
		}
		public override int GetHashCode ()
		{
			return this.Item1.Hash () ^
				this.Item2.Hash () ^
				this.Item3.Hash () ^
				this.Item4.Hash () ^
				this.Item5.Hash ();
		}
		public override string ToString ()
		{
			return string.Format("({0}, {1}, {2}, {3}, {4})", this.Item1, this.Item2, this.Item3, this.Item4, this.Item5);
		}
		#endregion
	}
	public class Tuple<T1, T2, T3, T4, T5, T6> :
		ITuple
	{
		public T1 Item1 { get; private set; }
		public T2 Item2 { get; private set; }
		public T3 Item3 { get; private set; }
		public T4 Item4 { get; private set; }
		public T5 Item5 { get; private set; }
		public T6 Item6 { get; private set; }
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
			this.Item6 = item6;
		}
		#region Object overrides
		public override bool Equals (object other)
		{
			return other is Tuple<T1, T2, T3, T4, T5, T6> && 
				(other as Tuple<T1, T2, T3, T4, T5, T6>).Item1.SameOrEquals(this.Item1) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6>).Item2.SameOrEquals(this.Item2) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6>).Item3.SameOrEquals(this.Item3) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6>).Item4.SameOrEquals(this.Item4) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6>).Item5.SameOrEquals(this.Item5) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6>).Item6.SameOrEquals(this.Item6);
		}
		public override int GetHashCode ()
		{
			return this.Item1.Hash () ^
				this.Item2.Hash () ^
				this.Item3.Hash () ^
				this.Item4.Hash () ^
				this.Item5.Hash () ^
				this.Item6.Hash ();
		}
		public override string ToString ()
		{
			return string.Format("({0}, {1}, {2}, {3}, {4}, {5})", this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6);
		}
		#endregion
	}
	public class Tuple<T1, T2, T3, T4, T5, T6, T7> :
		ITuple
	{
		public T1 Item1 { get; private set; }
		public T2 Item2 { get; private set; }
		public T3 Item3 { get; private set; }
		public T4 Item4 { get; private set; }
		public T5 Item5 { get; private set; }
		public T6 Item6 { get; private set; }
		public T7 Item7 { get; private set; }
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
			this.Item6 = item6;
			this.Item7 = item7;
		}
		#region Object overrides
		public override bool Equals (object other)
		{
			return other is Tuple<T1, T2, T3, T4, T5, T6, T7> && 
				(other as Tuple<T1, T2, T3, T4, T5, T6, T7>).Item1.SameOrEquals(this.Item1) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6, T7>).Item2.SameOrEquals(this.Item2) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6, T7>).Item3.SameOrEquals(this.Item3) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6, T7>).Item4.SameOrEquals(this.Item4) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6, T7>).Item5.SameOrEquals(this.Item5) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6, T7>).Item6.SameOrEquals(this.Item6) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6, T7>).Item7.SameOrEquals(this.Item7);
		}
		public override int GetHashCode ()
		{
			return this.Item1.Hash () ^
				this.Item2.Hash () ^
				this.Item3.Hash () ^
				this.Item4.Hash () ^
				this.Item5.Hash () ^
				this.Item6.Hash () ^
				this.Item7.Hash ();
		}
		public override string ToString ()
		{
			return string.Format("({0}, {1}, {2}, {3}, {4}, {5}, {6})", this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7);
		}
		#endregion
	}
	public class Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>  :
		ITuple
		where TRest : ITuple
	{
		public T1 Item1 { get; private set; }
		public T2 Item2 { get; private set; }
		public T3 Item3 { get; private set; }
		public T4 Item4 { get; private set; }
		public T5 Item5 { get; private set; }
		public T6 Item6 { get; private set; }
		public T7 Item7 { get; private set; }
		public TRest Rest { get; private set; }
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
			this.Item6 = item6;
			this.Item7 = item7;
			if (rest.IsNull())
				throw new ArgumentNullException("rest");
			this.Rest = rest;
		}
		#region Object overrides
		public override bool Equals (object other)
		{
			return other is Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> && 
				(other as Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>).Item1.SameOrEquals(this.Item1) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>).Item2.SameOrEquals(this.Item2) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>).Item3.SameOrEquals(this.Item3) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>).Item4.SameOrEquals(this.Item4) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>).Item5.SameOrEquals(this.Item5) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>).Item6.SameOrEquals(this.Item6) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>).Item7.SameOrEquals(this.Item7) && 
				(other as Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>).Rest.Equals(this.Item7);
		}
		public override int GetHashCode ()
		{
			return this.Item1.Hash () ^
				this.Item2.Hash () ^
				this.Item3.Hash () ^
				this.Item4.Hash () ^
				this.Item5.Hash () ^
				this.Item6.Hash () ^
				this.Item7.Hash () ^
				this.Rest.GetHashCode ();
		}
		public override string ToString ()
		{
			string rest = this.Rest.ToString();
			return string.Format("({0}, {1}, {2}, {3}, {4}, {5}, {6})", this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7, rest.Substring(1, rest.Length - 1));
		}
		#endregion
	}
}
