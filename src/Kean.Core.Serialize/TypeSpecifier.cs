// 
//  TypeSpecifier.cs
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
using Kean.Core.Basis.Extension;
using Kean.Core.Collection.Extension;

namespace Kean.Core.Serialize
{
    public class TypeSpecifier :
        Configure.IString
    {
        public string Assembly { get; set; }
        public string Name { get; set; }
        public Collection.IList<TypeSpecifier> Arguments { get; set; }
        public string FullName
        {
            get
            {
                string result = "";
                // Mapping according to http://msdn.microsoft.com/en-US/library/ya5y69ds%28v=VS.80%29.aspx
                switch (this.Name)
                {
                    case "System.Boolean":
                        result = "bool";
                        break;
                    case "System.Byte":
                        result = "byte";
                        break;
                    case "System.SByte":
                        result = "sbyte";
                        break;
                    case "System.Char":
                        result = "char";
                        break;
                    case "System.Decimal":
                        result = "decimal";
                        break;
                    case "System.Int16":
                        result = "short";
                        break;
                    case "System.Uint16":
                        result = "ushort";
                        break;
                    case "System.Int32":
                        result = "int";
                        break;
                    case "System.UInt32":
                        result = "uint";
                        break;
                    case "System.Int64":
                        result = "long";
                        break;
                    case "System.UInt64":
                        result = "ulong";
                        break;
                    case "System.Single":
                        result = "float";
                        break;
                    case "System.Double":
                        result = "double";
                        break;
                    case "System.String":
                        result = "string";
                        break;
                    case "System.Object":
                        result = "object";
                        break;
                    default:
                        System.Text.StringBuilder resultBuilder = null;
                        if (this.Arguments.Count > 0)
                        {
                            resultBuilder = new System.Text.StringBuilder().Append("<");
                            bool first = true;
                            foreach (TypeSpecifier name in this.Arguments)
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
                                this.Arguments.Add(new TypeSpecifier() { FullName = argument.Trim(' ') });
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
                    case "System.SByte":
                        this.Name = "sbyte";
                        break;
                    case "byte":
                        this.Name = "System.Byte";
                        break;
                    case "char":
                        this.Name = "System.Char";
                        break;
                    case "short":
                        this.Name = "System.Int16";
                        break;
                    case "ushort":
                        this.Name = "System.Uint16";
                        break;
                    case "int":
                        this.Name = "System.Int32";
                        break;
                    case "uint":
                        this.Name = "System.UInt32";
                        break;
                    case "long":
                        this.Name = "System.Int64";
                        break;
                    case "ulong":
                        this.Name = "System.UInt64";
                        break;
                    case "float":
                        this.Name = "System.Single";
                        break;
                    case "double":
                        this.Name = "System.Double";
                        break;
                    case "decimal":
                        this.Name = "System.Decimal";
                        break;
                    case "bool":
                        this.Name = "System.Boolean";
                        break;
                    case "string":
                        this.Name = "System.String";
                        break;
                    case "object":
                        this.Name = "System.Object";
                        break;
                    default:
                        assembly = this.Assembly;
                        break;
                }
                this.Assembly = assembly;
            }
        }
        public string AssemblyQualifiedName
        {
            get
            {
                System.Text.StringBuilder resultBuilder = new System.Text.StringBuilder(this.Name);
                if (this.Arguments.Count > 0)
                {
                    resultBuilder = resultBuilder.AppendFormat("`{0}[", this.Arguments.Count);
                    bool first = true;
                    foreach (TypeSpecifier name in this.Arguments)
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
                                this.Arguments.Add(new TypeSpecifier() { AssemblyQualifiedName = argument.Trim(' ', '[', ']') });
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

        public Type Type
        {
            get { return Type.GetType(this.AssemblyQualifiedName, false); }
            set
            {
                this.Name = value.Namespace + "." + value.Name.Split(new char[] { '`' }, 2)[0];
                this.Assembly = value.Assembly.GetName().Name;
                if (value.IsGenericType)
                    foreach (Type t in value.GetGenericArguments())
                        this.Arguments.Add(new TypeSpecifier() { Type = t });
            }
        }

        #region Configure.IString Members
        string Configure.IString.String
        {
            get { return this.FullName; }
            set { this.FullName = value; }
        }
        #endregion

        public TypeSpecifier()
        {
            this.Arguments = new Collection.List<TypeSpecifier>();
        }
    }
}
