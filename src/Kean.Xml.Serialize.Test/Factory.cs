// 
//  IData.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009-2011 Simon Mika
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
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Xml.Serialize.Test
{
	public abstract class Factory<T> :
		Kean.Test.Fixture<T>, 
		IFactory
		where T : Kean.Test.Fixture<T>, new()
	{
		protected void Test(Type type)
		{
			//Serialize.Storage(this.Factory.Filename(type), this.Factory.Create(type), this.Factory.Name(type));
			//FileAssert.AreEqual(Factory.ReferencePath(type), Factory.Filename(type), "Serializing test \"{0}\" failed.", type.Name);
		}


		public bool Boolean { get { return true; } }
		public int Integer { get { return 42; } }
		public float Float { get { return 13.37f; } }
		public Data.Enumerator Enumerator { get { return Data.Enumerator.Second; } }
		public string String { get { return "The Power of Attraction."; } }

		private int identifierCounter;

		internal Factory()
		{
		}

		public string Name(Type type)
		{
			return type.Name.Split('`')[0];
		}
		public string Filename(Type type)
		{
			return this.Name(type) + ".xml";
		}
		public string ReferencePath(string filename)
		{
			return "../../src/Test/Core/Attraction.Test.Core.Serializing/SerializedData/" + filename;
		}
		public string ReferencePath(Type type)
		{
			return this.ReferencePath(this.Filename(type));
		}
		public T Create<T>() where T : Data.IData
		{
			T result = (T)System.Activator.CreateInstance(typeof(T));
			result.Initilize(this, typeof(T).Name + this.identifierCounter++);
			return result;
		}
		internal object Create(Type type)
		{
			object result = null;
			if (!type.IsPrimitive && type.GetInterfaces().Contains(typeof(Data.IData)))
			{
				result = System.Activator.CreateInstance(type);
				(result as Data.IData).Initilize(this, type.Name + this.identifierCounter++);
			}
			else if (type == typeof(bool))
				result = this.Boolean;
			else if (type == typeof(int))
				result = this.Integer;
			else if (type == typeof(float))
				result = this.Float;
			else if (type == typeof(Data.Enumerator))
				result = this.Enumerator;
			else if (type == typeof(string))
				result = this.String;
			return result;
		}

		internal void Verify(object value)
		{
			Expect(value, Is.Not.Null);
			if (value is Data.IData)
				(value as Data.IData).Verify(this);
			else
			{
				Type type = value.GetType();
				if (type == typeof(bool))
					Expect(value, Is.EqualTo(this.Boolean));
				else if (type == typeof(int))
					Expect(value, Is.EqualTo(this.Integer));
				else if (type == typeof(float))
					Expect(value, Is.EqualTo(this.Float));
				else if (type == typeof(Data.Enumerator))
					Expect(value, Is.EqualTo(this.Enumerator));
				else if (type == typeof(string))
					Expect(value, Is.EqualTo(this.String));
			}
		}
	}
}
