// 
//  Exception.cs
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
using Target = Kean.Core.Error;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
namespace Kean.Core.Error.Test
{
	[TestFixture]
	public class Error :
		AssertionHelper
	{
		string prefix = "Kean.Core.Error.Test.";
		public Error ()
		{
		}
		[Test]
		public void MetaDataImmediateThrow()
		{
			DateTime now = DateTime.Now;
			try
			{
				throw new Exception.Test(Target.Level.Critical, "Title", "Message {0} {1}", "argument0", "argument1");
			}
			catch (Exception.Abstract e)
			{
				Expect((e as Target.IError).Time, EqualTo(now).Within(new DateTime(100000)), prefix + "MetaDataImmediateThrow.0");
				Expect((e as Target.IError).Level, EqualTo(Target.Level.Critical), prefix + "MetaDataImmediateThrow.1");
				Expect((e as Target.IError).Title, EqualTo("Title"), prefix + "MetaDataImmediateThrow.2");
				Expect((e as Target.IError).Message, EqualTo("Message argument0 argument1"), prefix + "MetaDataImmediateThrow.3");
				Expect((e as Target.IError).AssemblyName, EqualTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name), prefix + "MetaDataImmediateThrow.4");
				Expect((e as Target.IError).AssemblyVersion, EqualTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()), prefix + "MetaDataImmediateThrow.5");
				Expect((e as Target.IError).Type, EqualTo("Kean.Core.Error.Test.Error"), prefix + "MetaDataImmediateThrow.6");
				Expect((e as Target.IError).Method, EqualTo("MetaDataImmediateThrow"), prefix + "MetaDataImmediateThrow.7");
			}
		}
		[Test]
		public void MetaDataFilteredThrow()
		{
			DateTime now = DateTime.Now;
			try
			{
				new Exception.Test(Target.Level.Critical, "Title", "Message {0} {1}", "argument0", "argument1").Throw();
			}
			catch (Exception.Abstract e)
			{
				Expect((e as Target.IError).Time, EqualTo(now).Within(new DateTime(100000)), prefix + "MetaDataFilteredThrow.0");
				Expect((e as Target.IError).Level, EqualTo(Target.Level.Critical), prefix + "MetaDataFilteredThrow.1");
				Expect((e as Target.IError).Title, EqualTo("Title"), prefix + "MetaDataFilteredThrow.2");
				Expect((e as Target.IError).Message, EqualTo("Message argument0 argument1"), prefix + "MetaDataFilteredThrow.3");
				Expect((e as Target.IError).AssemblyName, EqualTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name), prefix + "MetaDataFilteredThrow.4");
				Expect((e as Target.IError).AssemblyVersion, EqualTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()), prefix + "MetaDataFilteredThrow.5");
				Expect((e as Target.IError).Type, EqualTo("Kean.Core.Error.Test.Error"), prefix + "MetaDataFilteredThrow.6");
				Expect((e as Target.IError).Method, EqualTo("MetaDataFilteredThrow"), prefix + "MetaDataFilteredThrow.7");
			}
		}
		[Test]
		public void MetaDataIndirectThrow()
		{
			DateTime now = DateTime.Now;
			try
			{
				Exception.Test.Check(Target.Level.Critical, "Title", "Message {0} {1}", "argument0", "argument1");
			}
			catch (Exception.Abstract e)
			{
				Expect((e as Target.IError).Time, EqualTo(now).Within(new DateTime(100000)), prefix + "MetaDataIndirectThrow.0");
				Expect((e as Target.IError).Level, EqualTo(Target.Level.Critical), prefix + "MetaDataIndirectThrow.1");
				Expect((e as Target.IError).Title, EqualTo("Title"), prefix + "MetaDataIndirectThrow.2");
				Expect((e as Target.IError).Message, EqualTo("Message argument0 argument1"), prefix + "MetaDataIndirectThrow.3");
				Expect((e as Target.IError).AssemblyName, EqualTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name), prefix + "MetaDataIndirectThrow.4");
				Expect((e as Target.IError).AssemblyVersion, EqualTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()), prefix + "MetaDataIndirectThrow.5");
				Expect((e as Target.IError).Type, EqualTo("Kean.Core.Error.Test.Error"), prefix + "MetaDataIndirectThrow.6");
				Expect((e as Target.IError).Method, EqualTo("MetaDataIndirectThrow"), prefix + "MetaDataIndirectThrow.7");
			}
		}
		public static void Test()
		{
			Error fixture = new Error();
			fixture.MetaDataImmediateThrow();
			fixture.MetaDataFilteredThrow();
			fixture.MetaDataIndirectThrow();
		}
	}
}

