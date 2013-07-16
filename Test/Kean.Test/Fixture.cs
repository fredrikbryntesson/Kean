// 
//  Fixture.cs
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
using System;
using Kean.Core;
using Kean.Core.Extension;
using NUnit.Framework;
using Reflect = Kean.Core.Reflect;
using Kean.Core.Reflect.Extension;
namespace Kean.Test
{
	[TestFixture]
	public abstract class Fixture<T> :
		AssertionHelper
		where T : Fixture<T>, new()
	{
		protected string Prefix { get; private set; }
		string currentTestName;
		int currentTestCounter;
		protected string CurrentTestStep { get { return this.Prefix + this.currentTestName + "." + this.currentTestCounter; } }
		protected Fixture() :
			this(((Reflect.Type)typeof(T)).Name)
		{
		}
		protected Fixture(string prefix)
		{
			this.Prefix = prefix.NotEmpty() ? prefix + "." : "";
		}
		[TestFixtureSetUp]
		public virtual void Setup()
		{
		}
		[TestFixtureTearDown]
		public virtual void TearDown()
		{
		}
		[SetUp]
		public virtual void PreTest()
		{
		}
		[TearDown]
		public virtual void PostTest()
		{
		}
		protected abstract void Run();
		protected void Run(Test test)
		{
			this.currentTestCounter = 0;
			this.currentTestName = test.Name;
			test.Run(this.PreTest, this.PostTest);
			Console.Write(".");
		}
		protected void Run(params Test[] tests)
		{
			foreach (Test test in tests)
				this.Run(test);
		}
		protected void Run(params Action[] tests)
		{
			foreach (Action test in tests)
				this.Run((Test)test);
		}
		public static void Test()
		{
			T fixture = new T();
			Console.Write(fixture.Prefix);
			fixture.Setup();
			fixture.Run();
			fixture.TearDown();
			Console.WriteLine("done");
		}
		public static void VerifyAsResource(string filename, string resource, string message, params object[] arguments)
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetCallingAssembly();
			string plattformResource = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(resource), System.IO.Path.GetFileNameWithoutExtension(resource) + (Core.Environment.IsWindows ? ".windows" : ".mono") + System.IO.Path.GetExtension(resource));
			System.IO.Stream correct = assembly.GetManifestResourceStream(assembly.GetName().Name + plattformResource.Replace('/', '.')) ??
				assembly.GetManifestResourceStream(assembly.GetName().Name + resource.Replace('/', '.'));
			FileAssert.AreEqual(System.IO.File.Open(filename, System.IO.FileMode.Open), correct, message, arguments);
		}
		public void Verify(object actual, NUnit.Framework.Constraints.Constraint constraint, string message, params object[] arguments)
		{
			Expect(actual, constraint, message, arguments);
		}
		public void Verify(object actual, NUnit.Framework.Constraints.Constraint constraint)
		{
			try
			{
				Verify(actual, constraint, this.CurrentTestStep);
			}
			finally
			{
				this.currentTestCounter++;
			}
		}
	}
}
