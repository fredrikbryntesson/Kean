using System;
using NUnit.Framework;

namespace Kean.Algebra.Test
{
	public class Parse :
	Kean.Test.Fixture<Parse>
	{
		protected override void Run()
		{
			this.Run(
				//this.Parsing1,
				//this.Parsing2,
				//this.Parsing3,
				//this.Parsing4,
				//this.Parsing5,
				//this.Parsing6,
				//this.Parsing7,
				//this.Parsing8,
				//this.Parsing9,
				this.Parsing10,
				this.Parsing11,
				this.Parsing12
			);
		}

		[Test]
		public void Parsing1()
		{
			Expression expression = (Expression)"2+3";
			Verify(expression, Is.EqualTo((Expression)2 + 3));
		}

		public void Parsing2()
		{
			Expression expression = (Expression)"x+y";
			Expression variable1 = new Variable("x");
			Expression variable2 = new Variable("y");
			Verify(expression, Is.EqualTo(variable1 + variable2));
		}

		public void Parsing3()
		{
			Expression expression = (Expression)"x+2";
			Expression variable1 = new Variable("x");
			Verify(expression, Is.EqualTo(variable1 + 2));
		}

		public void Parsing4()
		{
			Expression expression = (Expression)"x-2*x-3";
			Expression variable1 = new Variable("x");
			Expression correct = variable1 - 2 * variable1 - 3;
			Verify(expression, Is.EqualTo(correct));
		}

		public void Parsing5()
		{
			Expression expression = (Expression)"x+2*x/3-y";
			Expression variable1 = new Variable("x");
			Expression variable2 = new Variable("y");
			Expression correct = variable1 + 2 * variable1 / 3 - variable2;
			Verify(expression, Is.EqualTo(correct));
		}

		public void Parsing6()
		{
			Expression expression = (Expression)"x+2^y/3-y";
			Expression variable1 = new Variable("x");
			Expression variable2 = new Variable("y");
			Expression correct = variable1 + (Expression)2 ^ variable2 / 3 - variable2;
			Verify(expression, Is.EqualTo(correct));
		}

		public void Parsing7()
		{
			Expression expression = (Expression)"x+2+y/3-y";
			Expression variable1 = new Variable("x");
			Expression variable2 = new Variable("y");
			Expression correct = variable1 + 2 + variable2 / 3 - variable2;
			Verify(expression, Is.EqualTo(correct));
		}

		public void Parsing8()
		{
			Expression expression = (Expression)"x*2-y/3/y*5/6/65";
			Expression variable1 = new Variable("x");
			Expression variable2 = new Variable("y");
			Expression correct = variable1 * 2 - variable2 / 3 / variable2 * (Expression)5 / (Expression)6 / (Expression)65;
			Verify(expression, Is.EqualTo(correct));
		}

		public void Parsing9()
		{
			Expression expression = (Expression)"-x+-2+y/3-y";
			Expression variable1 = new Variable("x");
			Expression variable2 = new Variable("y");
			Expression correct = -variable1 + -(Expression)2 + variable2 / 3 - variable2;
			Verify(expression, Is.EqualTo(correct));
		}

		public void Parsing10()
		{
			Expression expression = (Expression)"x+(2+y/3)-y";
			Expression variable1 = new Variable("x");
			Expression variable2 = new Variable("y");
			Expression correct = variable1 + ((Expression)2 + variable2 / 3) - variable2;
			Verify(expression, Is.EqualTo(correct));
		}

		public void Parsing11()
		{
			Expression expression = (Expression)"x+((-2+y/3)-y)";
			Expression variable1 = new Variable("x");
			Expression variable2 = new Variable("y");
			Expression correct = variable1 + ((-(Expression)2 + variable2 / 3) - variable2);
			Verify(expression, Is.EqualTo(correct));
		}

		public void Parsing12()
		{
			Expression expression = (Expression)"x+((-2+y/3)-y)+2*y-z";
			Expression variable1 = new Variable("x");
			Expression variable2 = new Variable("y");
			Expression variable3 = new Variable("z");
			Expression correct = variable1 + ((-(Expression)2 + variable2 / 3) - variable2) + (Expression)2 * variable2 - variable3;
			Verify(expression, Is.EqualTo(correct));
		}
	}
}

