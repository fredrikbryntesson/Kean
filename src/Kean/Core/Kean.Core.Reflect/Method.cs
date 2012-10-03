// 
//  Method.cs
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

using Kean.Core.Extension;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
namespace Kean.Core.Reflect
{
	public class Method :
		Member,
		IComparable<Method>
	{
		Parameter[] parameters;
		public Parameter[] Parameters
		{
			get
			{
				if (this.parameters.IsNull())
					this.parameters = this.Information.GetParameters().Map(parameter => new Parameter(this, parameter));
				return this.parameters;
			}
		}
		protected System.Reflection.MethodInfo Information { get; private set; }
		public object Call(params object[] parameters)
		{
			return this.Information.Invoke(this.Parent, parameters);
		}
		internal Method(object parent, Type parentType, System.Reflection.MethodInfo information) :
			base(parent, parentType, information)
		{
			this.Information = information;
		}
		public Action<T> AsAction<T>() { return new Action<T>(this.Parent, this.ParentType, this.Information); }
		public Action<T1, T2> AsAction<T1, T2>() { return new Action<T1, T2>(this.Parent, this.ParentType, this.Information); }
		public Action<T1, T2, T3> AsAction<T1, T2, T3>() { return new Action<T1, T2, T3>(this.Parent, this.ParentType, this.Information); }
		public Action<T1, T2, T3, T4> AsAction<T1, T2, T3, T4>() { return new Action<T1, T2, T3, T4>(this.Parent, this.ParentType, this.Information); }
		public Action<T1, T2, T3, T4, T5> AsAction<T1, T2, T3, T4, T5>() { return new Action<T1, T2, T3, T4, T5>(this.Parent, this.ParentType, this.Information); }

		internal static Method Create(object parent, Type parentType, System.Reflection.MethodInfo information)
		{
			Method result;
			if (information.ReturnType == typeof(bool))
			{
				System.Reflection.ParameterInfo[] parameters = information.GetParameters();
				if (parameters.Length == 0)
					result = new Function<bool>(parent, parentType, information);
				else
					result = new Method(parent, parentType, information);
			}
			else
				result = new Method(parent, parentType, information);
			return result;
		}
		#region IComparable<Method> Members
		Order IComparable<Method>.Compare(Method other)
		{
			return this.Name.CompareWith(other.NotNull() ? other.Name : null);
		}
		#endregion
	}
}

