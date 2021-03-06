﻿//
//  Negation.cs
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

namespace Kean.Math.Algebra
{
	public class Negation:
	UnaryOperator
	{
		public override int Precedence { get { return 4; } }
		protected override string Symbol { get { return "-"; } }
		internal Negation()
		{
		}
		public Negation(Expression argument) : 
			base(argument)
		{
		}
		public override float Evaluate(params KeyValue<string, float>[] variables)
		{
			return -this.Argument.Evaluate(variables);
		}
		public override Expression Derive(string variable)
		{
			return new Negation(this.Argument.Derive(variable));
		}
		public override Expression Simplify()
		{
			Expression result = this.Argument.Simplify();
			if (result is Constant)
				result = -(result as Constant).Value;
			else
				result = new Negation(result);
			return result;
		}
	}
}

