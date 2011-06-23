// 
//  TypeName.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010, 2011 Simon Mika
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
using Kean.Core.Basis.Extension;
using Kean.Core.Collection.Extension;

namespace Kean.Core.Reflect
{
	public class TypeName :
		IEquatable<TypeName>,
		IEquatable<string>,
		IEquatable<Type>
	{
		public string Assembly { get; private set; }
		public string Name { get; private set; }
		Collection.IList<TypeName> arguments;
		public Collection.IReadOnlyVector<TypeName> Arguments { get; private set; }

		Type type;
		TypeName()
		{
			this.arguments = new Collection.List<TypeName>();
            this.Arguments = new Collection.Wrap.ReadOnlyVector<TypeName>(this.arguments);
		}


		TypeName(Type type) :
			this()
		{
			this.Name = type.Namespace + "." + type.Name.Split(new char[] { '`' }, 2) [0];
			this.Assembly = type.Assembly.GetName().Name;
			if (type.IsGenericType)
				foreach (Type t in type.GetGenericArguments())
					this.arguments.Add(t);
		}


		TypeName(string name) :
			this()
		{
			int pointer = -1;
			while (++pointer < name.Length)
				switch (name [pointer])
				{
				default:
					this.Name += name [pointer];
					break;
				case '<':
					pointer++;
					int tail = name.Length;
					while (name[--tail] != '>')
						;
					foreach (string argument in name.Substring(pointer, tail - pointer).Split(','))
						this.arguments.Add(new TypeName(argument.Trim(' ')));
					pointer = tail;
					break;
				case ':':
					this.Assembly = this.Name;
					this.Name += '.';
					break;
				case ' ':
					this.Assembly = name.Substring(pointer + 1).Trim();
					pointer = name.Length;
					break;
				}
			string assembly = "mscorlib";
			switch (this.Name)
			{
				// http://msdn.microsoft.com/en-US/library/ya5y69ds%28v=VS.80%29.aspx
				case "bool": this.Name = "System.Boolean"; break;
				case "byte": this.Name = "System.Byte";	break;
				case "sbyte": this.Name = "System.SByte"; break;
				case "char": this.Name = "System.Char";	break;
				case "short": this.Name = "System.Int16"; break;
				case "ushort": this.Name = "System.Uint16";	break;
				case "int":	this.Name = "System.Int32";	break;
				case "uint": this.Name = "System.UInt32"; break;
				case "long": this.Name = "System.Int64"; break;
				case "ulong": this.Name = "System.UInt64"; break;
				case "float": this.Name = "System.Single"; break;
				case "double": this.Name = "System.Double";	break;
				case "decimal":	this.Name = "System.Decimal"; break;
				case "string": this.Name = "System.String";	break;
				case "object": this.Name = "System.Object";	break;
				default: assembly = this.Assembly; break;
			}
			this.Assembly = assembly;
		}
		public TypeName(string assembly, string name, params TypeName[] arguments) :
			this()
		{
			this.Assembly = assembly;
			this.Name = name;
			this.arguments.Add(arguments);
		}

		public T Create<T>()
		{
			return (T)this.Create();
		}
		public object Create()
		{
			return System.Activator.CreateInstance(this);
		}

		#region Object Overrides
		public override string ToString()
		{
			return this;
		}
		public override bool Equals(object other)
		{
			return base.Equals(other);
		}
		public override int GetHashCode()
		{
			return ((string)this).GetHashCode();
		}

		#endregion
		#region IEquatable<Typename>, IEquatable<string>, IEquatable<Type>
		public bool Equals(TypeName other)
		{
			return other.NotNull() && (string)this == (string)other;
		}
		public bool Equals(string other)
		{
			return this == other;
		}
		public bool Equals(Type other)
		{
			return other.NotNull() && this == (TypeName)other;
		}

		#endregion
		#region Binary Operators
		public static bool operator ==(TypeName left, TypeName right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(TypeName left, TypeName right)
		{
			return !(left == right);
		}
		public static bool operator ==(TypeName left, string right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(TypeName left, string right)
		{
			return !(left == right);
		}
		public static bool operator ==(string left, TypeName right)
		{
			return right == left;
		}
		public static bool operator !=(string left, TypeName right)
		{
			return !(left == right);
		}
		public static bool operator ==(TypeName left, Type right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(TypeName left, Type right)
		{
			return !(left == right);
		}
		public static bool operator ==(Type left, TypeName right)
		{
			return right == left;
		}
		public static bool operator !=(Type left, TypeName right)
		{
			return !(left == right);
		}
		#endregion
		#region Casts
		public static implicit operator TypeName(string value)
		{
			return new TypeName(value);
		}
		public static implicit operator string(TypeName value)
		{
			string result = "";
			// http://msdn.microsoft.com/en-US/library/ya5y69ds%28v=VS.80%29.aspx
			switch (value.Name)
			{
			case "System.Boolean": result = "bool"; break;
			case "System.Byte": result = "byte"; break;
			case "System.SByte": result = "sbyte"; break;
			case "System.Char":	result = "char"; break;
			case "System.Decimal": result = "decimal"; break;
			case "System.Int16": result = "short"; break;
			case "System.Uint16": result = "ushort"; break;
			case "System.Int32": result = "int"; break;
			case "System.UInt32": result = "uint"; break;
			case "System.Int64": result = "long"; break;
			case "System.UInt64": result = "ulong";	break;
			case "System.Single": result = "float";	break;
			case "System.Double": result = "double"; break;
			case "System.String": result = "string"; break;
			case "System.Object": result = "object"; break;
			default:
				System.Text.StringBuilder resultBuilder = null;
				if (value.Arguments.Count > 0)
				{
					resultBuilder = new System.Text.StringBuilder().Append("<");
					bool first = true;
					foreach (Type name in value.Arguments)
					{
						if (!first)
							resultBuilder.Append(",");
						else
							first = false;
						resultBuilder.Append(name.FullName);
					}
					resultBuilder.Append(">");
				}

				if (value.Name.StartsWith(value.Assembly))
					resultBuilder = new System.Text.StringBuilder(value.Assembly + ":" + value.Name.Substring(value.Assembly.Length + 1)).Append(resultBuilder);
				else
				{
					resultBuilder = new System.Text.StringBuilder(value.Name).Append(resultBuilder);
					if (value.Assembly.NotEmpty() && value.Assembly != "mscorlib")
						resultBuilder.AppendFormat(" {0}", value.Assembly);
				}
				result = resultBuilder.ToString();
				break;
			}
			return result;
		}
		public static implicit operator TypeName(Type value)
		{
			return new TypeName(value);
		}
		public static implicit operator Type(TypeName value)
		{
			if (value.type.IsNull())
			{
				System.Text.StringBuilder name = new System.Text.StringBuilder(value.Name);
				if (value.Arguments.Count > 0)
				{
					name = name.AppendFormat("`{0}[", value.Arguments.Count);
					bool first = true;
					foreach (TypeName argument in value.Arguments)
					{
						if (first)
							first = false;
						else
							name.Append(",");
						name.AppendFormat("[{0}]", ((Type)argument).AssemblyQualifiedName);
					}
					name.Append("]");
				}
				if (value.Assembly.NotEmpty() && value.Assembly != "mscorlib")
					name.AppendFormat(", {0}", value.Assembly);
				value.type = Type.GetType(name.ToString(), false);
			}
			return value.type;
		}
		#endregion
	}
}
