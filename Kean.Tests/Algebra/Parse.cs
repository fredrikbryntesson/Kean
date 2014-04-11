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
				this.ConstantZero,
				this.ConstantSingleDigit,
				this.ConstantWithoutDecimals,
				this.ConstantWithDecimals,
				this.ConstantWithSpace,
				this.VariableSingleCharacter,
				this.VariableWithSpace,
				this.VariableMultipleCharacters,
				this.AddingConstants,
				this.AddingConstantsWithoutSpace,
				this.AddingConstantsWithDecimals,
				this.AddingVariables,
				this.AddingVariableConstant,
				this.AddingMultipleTerms,
				this.Parsing4,
				this.Parsing5,
				this.Parsing6,
				this.Parsing7,
				this.Parsing8,
				this.Parsing9,
				this.Parsing10,
				this.Parsing11,
				this.Parsing12
			);
		}

		#region Constant

		[Test]
		public void ConstantZero()
		{
			var expression = (Expression)"0";
			Verify(expression, Is.EqualTo((Expression)0f));
		}

		[Test]
		public void ConstantSingleDigit()
		{
			var expression = (Expression)"3";
			Verify(expression, Is.EqualTo((Expression)3f));
		}

		[Test]
		public void ConstantWithoutDecimals()
		{
			var expression = (Expression)"42";
			Verify(expression, Is.EqualTo((Expression)42f));
		}

		[Test]
		public void ConstantWithDecimals()
		{
			var expression = (Expression)"13.37";
			Verify(expression, Is.EqualTo((Expression)13.37f));
		}

		[Test]
		public void ConstantWithSpace()
		{
			var expression = (Expression)" 13.37 ";
			Verify(expression, Is.EqualTo((Expression)13.37f));
		}

		#endregion

		#region Variable

		[Test]
		public void VariableSingleCharacter()
		{
			var expression = (Expression)"x";
			Verify(expression, Is.EqualTo((Expression)"x"));
		}

		[Test]
		public void VariableWithSpace()
		{
			var expression = (Expression)" x ";
			Verify(expression, Is.EqualTo((Expression)"x"));
		}

		[Test]
		public void VariableMultipleCharacters()
		{
			var expression = (Expression)"variable";
			Verify(expression, Is.EqualTo((Expression)"variable"));
		}

		#endregion

		#region Adding

		[Test]
		public void AddingConstants()
		{
			var expression = (Expression)"2 + 3";
			Verify(expression, Is.EqualTo((Expression)2 + 3));
		}

		[Test]
		public void AddingConstantsWithoutSpace()
		{
			var expression = (Expression)"2+3";
			Verify(expression, Is.EqualTo((Expression)2 + 3));
		}

		[Test]
		public void AddingConstantsWithDecimals()
		{
			var expression = (Expression)"4.2 + 13.37";
			Verify(expression, Is.EqualTo((Expression)4.2f + 13.37f));
		}

		[Test]
		public void AddingVariables()
		{
			Expression expression = (Expression)"x + y";
			Verify(expression, Is.EqualTo(new Variable("x") + new Variable("y")));
		}

		[Test]
		public void AddingVariableConstant()
		{
			Expression expression = (Expression)"x + 2";
			Verify(expression, Is.EqualTo(new Variable("x") + 2));
		}

		[Test]
		public void AddingMultipleTerms()
		{
			Expression expression = (Expression)"1 + 2 + 3 + 4 + 5 + 6";
			Verify(expression, Is.EqualTo((Expression)1 + 2 + 3 + 4 + 5 + 6));
		}

		#endregion

		[Test]
		public void Parsing4()
		{
			Expression expression = (Expression)"x-2*x-3";
			Expression variable1 = new Variable("x");
			Expression correct = variable1 - 2 * variable1 - 3;
			Verify(expression, Is.EqualTo(correct));
		}

		[Test]
		public void Parsing5()
		{
			Expression expression = (Expression)"x+2*x/3-y";
			Expression variable1 = new Variable("x");
			Expression variable2 = new Variable("y");
			Expression correct = variable1 + 2 * variable1 / 3 - variable2;
			Verify(expression, Is.EqualTo(correct));
		}

		[Test]
		public void Parsing6()
		{
			Expression expression = (Expression)"x+2^y/3-y";
			Expression variable1 = new Variable("x");
			Expression variable2 = new Variable("y");
			Expression correct = variable1 + (Expression)2 ^ variable2 / 3 - variable2;
			Verify(expression, Is.EqualTo(correct));
		}

		[Test]
		public void Parsing7()
		{
			Expression expression = (Expression)"x+2+y/3-y";
			Expression variable1 = new Variable("x");
			Expression variable2 = new Variable("y");
			Expression correct = variable1 + 2 + variable2 / 3 - variable2;
			Verify(expression, Is.EqualTo(correct));
		}

		[Test]
		public void Parsing8()
		{
			Expression expression = (Expression)"x*2-y/3/y*5/6/65";
			Expression variable1 = new Variable("x");
			Expression variable2 = new Variable("y");
			Expression correct = variable1 * 2 - variable2 / 3 / variable2 * (Expression)5 / (Expression)6 / (Expression)65;
			Verify(expression, Is.EqualTo(correct));
		}

		[Test]
		public void Parsing9()
		{
			Expression expression = (Expression)"-x+-2+y/3-y";
			Expression variable1 = new Variable("x");
			Expression variable2 = new Variable("y");
			Expression correct = -variable1 + -(Expression)2 + variable2 / 3 - variable2;
			Verify(expression, Is.EqualTo(correct));
		}

		[Test]
		public void Parsing10()
		{
			Expression expression = (Expression)"x+(2+y/3)-y";
			Expression variable1 = new Variable("x");
			Expression variable2 = new Variable("y");
			Expression correct = variable1 + ((Expression)2 + variable2 / 3) - variable2;
			Verify(expression, Is.EqualTo(correct));
		}

		[Test]
		public void Parsing11()
		{
			Expression expression = (Expression)"x+((-2+y/3)-y)";
			Expression variable1 = new Variable("x");
			Expression variable2 = new Variable("y");
			Expression correct = variable1 + ((-(Expression)2 + variable2 / 3) - variable2);
			Verify(expression, Is.EqualTo(correct));
		}

		[Test]
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

