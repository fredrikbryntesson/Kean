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
        public Collection.IImmutableVector<TypeName> Arguments { get; private set; }
        string FullName
        {
            get
            {
                string result = "";
                // Mapping according to http://msdn.microsoft.com/en-US/library/ya5y69ds%28v=VS.80%29.aspx
                switch (this.Name)
                {
                    case "System.Boolean": result = "bool"; break;
                    case "System.Byte": result = "byte"; break;
                    case "System.SByte": result = "sbyte"; break;
                    case "System.Char": result = "char"; break;
                    case "System.Decimal": result = "decimal"; break;
                    case "System.Int16": result = "short"; break;
                    case "System.Uint16": result = "ushort"; break;
                    case "System.Int32": result = "int"; break;
                    case "System.UInt32": result = "uint"; break;
                    case "System.Int64": result = "long"; break;
                    case "System.UInt64": result = "ulong"; break;
                    case "System.Single": result = "float"; break;
                    case "System.Double": result = "double"; break;
                    case "System.String": result = "string"; break;
                    case "System.Object": result = "object"; break;
                    default:
                        System.Text.StringBuilder resultBuilder = null;
                        if (this.Arguments.Count > 0)
                        {
                            resultBuilder = new System.Text.StringBuilder().Append("<");
                            bool first = true;
                            foreach (Type name in this.Arguments)
                            {
                                if (!first)
                                    resultBuilder.Append(",");
                                else
                                    first = false;
                                resultBuilder.Append(name.FullName);
                            }
                            resultBuilder.Append(">");
                        }

                        if (this.Name.StartsWith(this.Assembly))
                            resultBuilder = new System.Text.StringBuilder(this.Assembly + ":" + this.Name.Substring(this.Assembly.Length + 1)).Append(resultBuilder);
                        else
                        {
                            resultBuilder = new System.Text.StringBuilder(this.Name).Append(resultBuilder);
                            if (this.Assembly.NotEmpty() && this.Assembly != "mscorlib")
                                resultBuilder.AppendFormat(" {0}", this.Assembly);
                        }
                        result = resultBuilder.ToString();
                        break;
                }
                return result;
            }
            set
            {
                int pointer = -1;
                while (++pointer < value.Length)
                    switch (value[pointer])
                    {
                        case '<':
                            pointer++;
                            int tail = value.Length;
                            while (value[--tail] != '>');
                            foreach (string argument in value.Substring(pointer, tail - pointer).Split(','))
                                this.arguments.Add(new TypeName() { FullName = argument.Trim(' ') });
                            pointer = tail;
                            break;
                        case ' ':
                            this.Assembly = value.Substring(pointer + 1).Trim();
                            pointer = value.Length;
                            break;
                        case ':':
                            this.Assembly = this.Name;
                            this.Name += '.';
                            break;
                        default:
                            this.Name += value[pointer];
                            break;
                    }
                string assembly = "mscorlib";
                switch (this.Name)
                {
                    // Mapping according to http://msdn.microsoft.com/en-US/library/ya5y69ds%28v=VS.80%29.aspx
                    case "bool": this.Name = "System.Boolean"; break;
                    case "byte": this.Name = "System.Byte"; break;
                    case "sbyte": this.Name = "System.SByte"; break;
                    case "char": this.Name = "System.Char"; break;
                    case "short": this.Name = "System.Int16"; break;
                    case "ushort": this.Name = "System.Uint16"; break;
                    case "int": this.Name = "System.Int32"; break;
                    case "uint": this.Name = "System.UInt32"; break;
                    case "long": this.Name = "System.Int64"; break;
                    case "ulong":this.Name = "System.UInt64"; break;
                    case "float": this.Name = "System.Single"; break;
                    case "double": this.Name = "System.Double"; break;
                    case "decimal": this.Name = "System.Decimal"; break;
                    case "string": this.Name = "System.String"; break;
                    case "object": this.Name = "System.Object"; break;
                    default: assembly = this.Assembly; break;
                }
                this.Assembly = assembly;
            }
        }
        string AssemblyQualifiedName
        {
            get
            {
                System.Text.StringBuilder resultBuilder = new System.Text.StringBuilder(this.Name);
                if (this.Arguments.Count > 0)
                {
                    resultBuilder = resultBuilder.AppendFormat("`{0}[", this.Arguments.Count);
                    bool first = true;
                    foreach (Type name in this.Arguments)
                    {
                        if (!first)
                            resultBuilder.Append(",");
                        else
                            first = false;
                        resultBuilder.AppendFormat("[{0}]", name.AssemblyQualifiedName);
                    }
                    resultBuilder.Append("]");
                }
                if (this.Assembly.NotEmpty() && this.Assembly != "mscorlib")
                    resultBuilder.AppendFormat(", {0}", this.Assembly);
                return resultBuilder.ToString();
            }
            set
            {
                int pointer = -1;
                while (++pointer < value.Length)
                    switch (value[pointer])
                    {
                        case '`':
                            while (value[++pointer] != '[') ;
                            pointer++;
                            int tail = value.Length;
                            while (value[--tail] != ']') ;
                            tail--;
                            foreach (string argument in value.Substring(pointer, tail - pointer).Split(','))
                                this.arguments.Add(new TypeName() { AssemblyQualifiedName = argument.Trim(' ', '[', ']') });
                            pointer = tail + 1;
                            break;
                        case ',':
                            this.Assembly = value.Substring(pointer + 1).Trim();
                            pointer = value.Length;
                            break;
                        default:
                            this.Name += value[pointer];
                            break;
                    }
            }
        }

        Type Type
        {
            get { return Type.GetType(this.AssemblyQualifiedName, false); }
            set
            {
                this.Name = value.Namespace + "." + value.Name.Split(new char[] { '`' }, 2)[0];
                this.Assembly = value.Assembly.GetName().Name;
                if (value.IsGenericType)
                    foreach (Type t in value.GetGenericArguments())
                        this.arguments.Add(new TypeName() { Type = t });
            }
        }

        TypeName()
        {
            this.arguments = new Collection.List<TypeName>();
			this.Arguments = new Collection.Wrap.ImmutableVector<TypeName>(this.arguments);
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
		public override string ToString ()
		{
			return this.FullName;
		}
		public override bool Equals(object other)
		{
			return base.Equals(other);
		}
		public override int GetHashCode ()
		{
			return this.FullName.GetHashCode();
		}
		#endregion
		#region IEquatable<Typename>, IEquatable<string>, IEquatable<Type>
		public bool Equals(TypeName other)
		{
			return other.NotNull() && this.FullName == other.FullName;
		}
		public bool Equals(string other)
		{
			return this.FullName == other;
		}
		public bool Equals(Type other)
		{
			return other.NotNull() && this.FullName == ((TypeName)other).FullName;
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
			return new TypeName() { FullName = value };
		}
		public static implicit operator string(TypeName value)
		{
			return value.FullName;
		}
		public static implicit operator TypeName(Type value)
		{
			return new TypeName() { Type = value };
		}
		public static implicit operator Type(TypeName value)
		{
			return value.Type;
		}
		#endregion
    }
}
