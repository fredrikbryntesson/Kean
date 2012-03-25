// 
//  Tokenizer.cs
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
using Collection = Kean.Core.Collection;

namespace Kean.Fubl
{
    public class Tokenizer
    {
        public Tokenizer()
        { }
        public System.Collections.Generic.IEnumerable<Token.Abstract> Tokenize(IO.ICharacterReader reader)
        {
            reader.Next();
            while (!reader.Empty)
            {
                IO.Text.Mark mark = new IO.Text.Mark(reader);
                if (char.IsWhiteSpace(reader.Last))
                    reader.Next();
                else if (char.IsSymbol(reader.Last))
                {
                    yield return new Token.Symbol(reader.Last, mark);
                    reader.Next();
                }
                else if (char.IsDigit(reader.Last))
                {
                }
                else if (char.IsLetter(reader.Last))
                {
                }
            }
        }

    }
}
