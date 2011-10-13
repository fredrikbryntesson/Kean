// 
//  Object.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
using Kean.Core;
using Kean.Core.Extension;
using Kean.Core.Reflect.Extension;
using Reflect = Kean.Core.Reflect;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;

namespace Kean.Cli.Processor
{
	class Object :
		Member,
		Collection.IReadOnlyVector<Member>
	{
		protected override char Delimiter { get { return '.'; } }
		object backend;
		Collection.IList<Member> members;
		Collection.IList<Member> Members
		{
			get
			{
				if (this.members.IsNull())
				{
					this.members = new Collection.Sorted.List<Member>();
					if (this.backend.NotNull())
						foreach (Reflect.Member member in this.backend.GetMembers(Reflect.MemberFilter.Instance | Reflect.MemberFilter.Public | Reflect.MemberFilter.Method | Reflect.MemberFilter.Property))
						{
							MemberAttribute[] attributes = member.GetAttributes<MemberAttribute>();
							if (attributes.Length == 1)
							{
								if (attributes[0] is ObjectAttribute && member is Reflect.Property && (member as Reflect.Property).Readable && (member as Reflect.Property).Data.NotNull())
									this.members.Add(new Object(attributes[0] as ObjectAttribute, member as Reflect.Property, this));
								else if (attributes[0] is PropertyAttribute)
									this.members.Add(new Property(attributes[0] as PropertyAttribute, member as Reflect.Property, this));
								else if (attributes[0] is MethodAttribute)
									this.members.Add(new Method(attributes[0] as MethodAttribute, member as Reflect.Method, this));
							}
						}
				}
				return this.members;
			}
		}
		public Object(object backend) :
			base(null, null, null)
		{
			this.backend = backend;
		}
		public Object(ObjectAttribute attribute, Reflect.Property backend, Object parent) :
			base(attribute, backend, parent)
		{
			this.backend = backend.Data;
		}
		internal Reflect.Event GetEvent(string name)
		{
			return this.backend.GetEvent(name);
		}
		public override bool Execute(Editor editor, string[] parameters)
		{
			if (parameters.Length == 0 || parameters.Length == 1 && parameters[0].IsEmpty())
				editor.Current = this;
			else
				editor.Error(this, "Error Parsing:", parameters);
			return true;
		}
		public override string Complete(string[] parameters)
		{
			string incomplete = parameters.Length > 0 ? parameters[0] ?? "" : "";
			Collection.List<string> alternatives = new Collection.List<string>();
			foreach (Member member in this)
				if (member.Name.StartsWith(incomplete))
					alternatives.Add(member.Name + (member is Object ? "." : " "));
			string result = "";
			if (alternatives.Count > 0)
				for (int i = 0; i < alternatives[0].Length && alternatives.All(s => s[i] == alternatives[0][i]); i++)
					result += alternatives[0][i];
			return result;
		}
		public override string Help(string[] parameters)
		{
			string incomplete = parameters.Length > 0 ? parameters[0] ?? "" : "";
			Collection.List<Tuple<string, string>> results = new Collection.List<Tuple<string, string>>();
			foreach (Member member in this)
				if (member.Name.StartsWith(incomplete))
					results.Add(Tuple.Create(member.Name, member.Description));
			return results.Fold((m, r) => r + m.Item1 + "\t" + m.Item2 + "\n", "");
		}
		#region IReadOnlyVector<Member> Members
		public int Count
		{
			get { return this.Members.Count; }
		}
		public Member this[int index]
		{
			get { return this.Members[index]; }
		}
		#endregion
		#region IEnumerable<Member> Members
		public System.Collections.Generic.IEnumerator<Member> GetEnumerator()
		{
			return this.Members.GetEnumerator();
		}
		#endregion
		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Members.GetEnumerator();
		}
		#endregion
		#region IEquatable<IVector<member>> Members
		public bool Equals(Collection.IVector<Member> other)
		{
			return this.Members.Equals(other);
		}
		#endregion
		#region IEquatable<IReadOnlyVector<Member>> Members
		public bool Equals(Kean.Core.Collection.IReadOnlyVector<Member> other)
		{
			return this.Members.Equals(other);
		}
		#endregion
	}
}
