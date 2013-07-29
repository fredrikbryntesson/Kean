//
// 
//  Nullable.cs
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
using NUnit.Framework;


namespace Kean.Core.Serialize.Test.Data
{
	public class Nullable :
		IData
	{
		[Core.Serialize.Parameter]
		public bool? Boolean { get; set; }
		[Core.Serialize.Parameter]
		public bool? BooleanNull { get; set; }
		[Core.Serialize.Parameter]
		public int? Integer { get; set; }
		[Core.Serialize.Parameter]
		public int? IntegerNull { get; set; }
		[Core.Serialize.Parameter]
		public float? Float { get; set; }
		[Core.Serialize.Parameter]
		public float? FloatNull { get; set; }
		[Core.Serialize.Parameter]
		public Enumerator? Enumerator { get; set; }
		[Core.Serialize.Parameter]
		public Enumerator? EnumeratorNull { get; set; }
		[Core.Serialize.Parameter]
		public Structure? Structure { get; set; }
		[Core.Serialize.Parameter]
		public Structure? StructureNull { get; set; }

		#region IData
		public virtual void Initilize(IFactory factory)
		{
			this.Boolean = factory.Create<bool>();
			this.Integer = factory.Create<int>();
			this.Float = factory.Create<float>();
			this.Enumerator = factory.Create<Enumerator>();
			this.Structure = factory.Create<Structure>();
		}
		public virtual void Verify(IFactory factory, string message, params object[] arguments)
		{
			factory.Verify(this.Boolean, message, arguments);
			factory.Verify(this.BooleanNull, Is.Null, message, arguments);
			factory.Verify(this.Integer, message, arguments);
			factory.Verify(this.IntegerNull, Is.Null, message, arguments);
			factory.Verify(this.Float, message, arguments);
			factory.Verify(this.FloatNull, Is.Null, message, arguments);
			factory.Verify(this.Enumerator, message, arguments);
			factory.Verify(this.EnumeratorNull, Is.Null, message, arguments);
			factory.Verify(this.Structure, message, arguments);
			factory.Verify(this.StructureNull, Is.Null, message, arguments);
		}
		#endregion
	}
}
