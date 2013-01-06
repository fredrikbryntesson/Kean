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
		protected OperatingSystem OperatingSystem 
		{
			get
			{
				OperatingSystem result;
				switch (Environment.OSVersion.Platform)
        		{
            		case System.PlatformID.Unix:
		                if (System.IO.Directory.Exists("/Applications") && System.IO.Directory.Exists("/System") && System.IO.Directory.Exists("/Users") && System.IO.Directory.Exists("/Volumes"))
		                    result = OperatingSystem.Mac;
		                else
		                    result = OperatingSystem.Linux;
					break;
            		case PlatformID.MacOSX:
                		result = OperatingSystem.Mac;
					break;
	        		default:
                		result =  OperatingSystem.Windows;
					break;
        		}
				return result;
			}
		}
		protected Fixture() :
			this(((Reflect.Type)typeof(T)).Name)
		{ }
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
			this.currentTestCounter = -1;
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
			FileAssert.AreEqual(System.IO.File.Open(filename, System.IO.FileMode.Open), 
				assembly.GetManifestResourceStream(assembly.GetName().Name + ((string)resource).Replace('/', '.')), message, arguments);
		}
		public void Verify(object actual, NUnit.Framework.Constraints.Constraint constraint, string message, params object[] arguments)
		{
			Expect(actual, constraint, message, arguments);
		}
		public void Verify(object actual, NUnit.Framework.Constraints.Constraint constraint)
		{
			this.currentTestCounter++;
			Verify(actual, constraint, this.CurrentTestStep);
		}
	}
}
