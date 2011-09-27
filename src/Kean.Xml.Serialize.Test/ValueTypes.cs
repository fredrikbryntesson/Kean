using System;
using NUnit.Framework;
using Kean.Core.Extension;

namespace Kean.Xml.Serialize.Test
{
	public class ValueTypes :
		Factory<ValueTypes>
	{
		protected override void  Run()
		{
			this.Run(
				this.Integer,
				this.Single,
				this.String,
				this.Enumerator
				);
		}

		[Test]
		public void Integer()
		{
			this.Test(typeof(int));
		}
		[Test]
		public void Single()
		{
			this.Test(typeof(float));
		}
		[Test]
		public void String()
		{
			this.Test(typeof(string));
		}
		[Test]
		public void Enumerator()
		{
			this.Test(typeof(Data.Enumerator));
		}
	}
}
