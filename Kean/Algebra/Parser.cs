//
//  Parser.cs
//
//  Author:
//       Simon Mika <simon@mika.se>
//
//  Copyright (c) 2014 Simon Mika
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
using Kean.Extension;

namespace Kean.Algebra
{
	class Parser
	{
		public Parser()
		{
		}
		Collection.IStack<Expression> stack = new Collection.Stack<Expression>();
		public Expression Parse(string expression)
		{
			return this.Parse(null, Parser.Tokenize(expression));
		}
		Expression Parse(Expression operator1, Collection.IQueue<Expression> arguments)
		{
			Expression left = null;
			Expression right = null;
			Expression operator2 = null;
			if (operator1.IsNull())
				operator1 = this.PushOnStack(operator1, arguments);
			if (operator2.IsNull() && !(operator1 is RightParanthesis))
				operator2 = this.PushOnStack(operator2, arguments);
			if (!(operator1 is RightParanthesis) && !(operator2 is RightParanthesis))
				operator2 = this.PushOnStack(operator2, arguments);
			if (!(this.stack.Empty))
				right = this.stack.Pop();
			if (!(this.stack.Empty))
				left = this.stack.Pop();
			if (operator1.NotNull() && right.NotNull() && left.NotNull())
			{
				if (operator2.NotNull())
				{
					if (operator2 is RightParanthesis)
						this.stack.Push(((BinaryOperator)operator1).Build(left, right));
					else
					{
						if (operator1.Precedence < operator2.Precedence && !(operator1 is RightParanthesis))
							this.stack.Push(((BinaryOperator)operator2).Build(left, right));
						else if (operator1.Precedence > operator2.Precedence || operator1.Precedence == operator2.Precedence)
						{
							this.stack.Push(((BinaryOperator)operator1).Build(this.stack.Pop(), left));
							this.stack.Push(right);
							operator1 = operator2;
						}
						this.Parse(operator1, arguments);
					}
				}
				else
				{
					this.stack.Push(((BinaryOperator)operator1).Build(left, right));
					operator1 = null;
				}
			}
			else
				this.stack.Push(right);
			return this.stack.Peek();
		}
		public Expression PushOnStack(Expression @operator, Collection.IQueue<Expression> arguments)
		{
			Expression result = @operator;
			if (arguments.Count > 0)
			{
				if (arguments.Peek() is Constant || arguments.Peek() is Variable)
				{
					this.stack.Push(arguments.Dequeue());
					result = PushOnStack(@operator, arguments);
				}
				else if ((arguments.Peek() is BinaryOperator || (arguments.Peek() is RightParanthesis)) && @operator.IsNull())
					result = arguments.Dequeue();
				else if (arguments.Peek() is UnaryOperator)
				{
					Expression value = null;
					arguments.Dequeue();
					if (arguments.Peek() is Constant || arguments.Peek() is Variable)
						value = arguments.Dequeue();
					this.stack.Push(new Negation(value));
					result = PushOnStack(@operator, arguments);
				}
				else if (arguments.Peek() is LeftParanthesis)
				{
					arguments.Dequeue();
					this.Parse(null, arguments);
					result = PushOnStack(result, arguments);
				}
			}
			return result;
		}
		public static Collection.IQueue<Expression> Tokenize(string data)
		{
			var digit = new System.Text.StringBuilder();
			var arguments = new Collection.Queue<Expression>();
			bool flag = false;
			foreach (char c in data)
			{
				if (char.IsDigit(c))
					digit.Append(c);
				else if (c == '.')
					digit.Append(c);
				else
				{
					if (digit.Length > 0)
					{
						arguments.Enqueue(new Constant(float.Parse(digit.ToString())));
						digit = new System.Text.StringBuilder();
						flag = false;
					}
					if (char.IsLetter(c))
					{
						arguments.Enqueue(new Variable(c.ToString()));
						flag = false;
					}
					else
						switch (c)
						{
							case '-':
								{

									if (flag || arguments.Empty)
										arguments.Enqueue(new Negation());
									else
									{
										arguments.Enqueue(new Subtraction());
										flag = true;
									}
									break;
								}
							case '+':
								arguments.Enqueue(new Addition());
								flag = true;
								break;
							case '*':
								arguments.Enqueue(new Multiplication());
								flag = true;
								break;
							case '/':
								arguments.Enqueue(new Division());
								flag = true;
								break;
							case '^':
								arguments.Enqueue(new Power());
								flag = true;
								break;
							case '(':
								arguments.Enqueue(new LeftParanthesis());
								flag = true;
								break;
							case ')':
								arguments.Enqueue(new RightParanthesis());
								break;
						}
				}
			}
			if (digit.Length > 0)
			{
				arguments.Enqueue(new Constant(float.Parse(digit.ToString())));
				digit = new System.Text.StringBuilder();
			}
			return arguments;
		}
	}
}
