// 
//  Dictionary.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Core.Collection;

namespace Kean.Core.Collection.Test.Base
{
	public abstract class Dictionary<T, D> :
        Kean.Test.Fixture<T>
        where T : Kean.Test.Fixture<T>, new()
        where D : Target.IDictionary<string, int>
	{
		public string[] Correct { get; private set; }
		public string Prefix { get; set; }
		protected Dictionary()
		{
			this.Correct = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen" };
		}

		protected abstract D Create(int size);

        protected override void Run()
        {
            this.Run(
                this.SingleElement,
                this.TwoElements,
                this.TwelveElements,
                this.Equality,
				this.NullValue
            );
        }
        
        [Test]
		public void SingleElement()
		{
			D target = this.Create(10);
			Expect(target.Contains(this.Correct[1]), Is.EqualTo(false), this.Prefix + "SingleElement.0");
			Expect(target[this.Correct[1]], Is.EqualTo(0), this.Prefix + "SingleElement.1");
			Expect(target.Remove(this.Correct[1]), Is.EqualTo(false), this.Prefix + "SingleElement.2");
			target[this.Correct[1]] = 1;
			Expect(target.Contains(this.Correct[2]), Is.EqualTo(false), this.Prefix + "SingleElement.3");
			Expect(target[this.Correct[2]], Is.EqualTo(0), this.Prefix + "SingleElement.4");
			Expect(target.Remove(this.Correct[2]), Is.EqualTo(false), this.Prefix + "SingleElement.5");
			Expect(target.Contains(this.Correct[1]), Is.EqualTo(true), this.Prefix + "SingleElement.6");
			Expect(target[this.Correct[1]], Is.EqualTo(1), this.Prefix + "SingleElement.7");
			Expect(target.Remove(this.Correct[1]), Is.EqualTo(true), this.Prefix + "SingleElement.8");
			Expect(target.Contains(this.Correct[1]), Is.EqualTo(false), this.Prefix + "SingleElement.9");
			Expect(target[this.Correct[1]], Is.EqualTo(0), this.Prefix + "SingleElement.10");
			Expect(target.Remove(this.Correct[1]), Is.EqualTo(false), this.Prefix + "SingleElement.11");
		}
		[Test]
		public void TwoElements()
		{
			D target = this.Create(10);
			target[this.Correct[1]] = 1;
			target[this.Correct[2]] = 2;
			Expect(target.Contains(this.Correct[3]), Is.EqualTo(false), this.Prefix + "TwoElements.0");
			Expect(target[this.Correct[3]], Is.EqualTo(0), this.Prefix + "TwoElements.1");
			Expect(target.Remove(this.Correct[3]), Is.EqualTo(false), this.Prefix + "TwoElements.2");
			Expect(target.Contains(this.Correct[1]), Is.EqualTo(true), this.Prefix + "TwoElements.3");
			Expect(target.Contains(this.Correct[2]), Is.EqualTo(true), this.Prefix + "TwoElements.4");
			Expect(target[this.Correct[1]], Is.EqualTo(1), this.Prefix + "TwoElements.5");
			Expect(target[this.Correct[2]], Is.EqualTo(2), this.Prefix + "TwoElements.6");
			Expect(target.Remove(this.Correct[1]), Is.EqualTo(true), this.Prefix + "TwoElements.7");
			Expect(target.Remove(this.Correct[2]), Is.EqualTo(true), this.Prefix + "TwoElements.8");
			Expect(target.Contains(this.Correct[1]), Is.EqualTo(false), this.Prefix + "TwoElements.9");
			Expect(target[this.Correct[1]], Is.EqualTo(0), this.Prefix + "TwoElements.10");
			Expect(target.Remove(this.Correct[1]), Is.EqualTo(false), this.Prefix + "TwoElements.11");
			Expect(target.Contains(this.Correct[2]), Is.EqualTo(false), this.Prefix + "SingleElement.12");
			Expect(target[this.Correct[2]], Is.EqualTo(0), this.Prefix + "SingleElement.13");
			Expect(target.Remove(this.Correct[2]), Is.EqualTo(false), this.Prefix + "SingleElement.14");
		}
		[Test]
		public void TwelveElements()
		{
			D target = this.Create(10);
			for (int i = 1; i < 13; i++)
				Expect(target.Contains(this.Correct[i]), Is.EqualTo(false), this.Prefix + "TwelveElement." + (i - 1));
			for (int i = 1; i < 13; i++)
				Expect(target[this.Correct[i]], Is.EqualTo(0), this.Prefix + "SingleElement." + 11 + i);
			for (int i = 1; i < 13; i++)
				Expect(target.Remove(this.Correct[i]), Is.EqualTo(false), this.Prefix + "SingleElement." + (23 + i));
			for (int i = 1; i < 13; i++)
				target[this.Correct[i]] = i;
			Expect(target.Contains(this.Correct[13]), Is.EqualTo(false), this.Prefix + "SingleElement.36");
			Expect(target[this.Correct[13]], Is.EqualTo(0), this.Prefix + "SingleElement.37");
			Expect(target.Remove(this.Correct[13]), Is.EqualTo(false), this.Prefix + "SingleElement.38");
			for (int i = 1; i < 13; i++)
				Expect(target.Contains(this.Correct[i]), Is.EqualTo(true), this.Prefix + "SingleElement." + (38 + i));
			for (int i = 1; i < 13; i++)
				Expect(target[this.Correct[i]], Is.EqualTo(i), this.Prefix + "SingleElement." + (50 + i));
			for (int i = 1; i < 13; i++)
				Expect(target.Remove(this.Correct[i]), Is.EqualTo(true), this.Prefix + "SingleElement." + (62 + i));
			for (int i = 1; i < 13; i++)
				Expect(target.Contains(this.Correct[i]), Is.EqualTo(false), this.Prefix + "SingleElement." + (74 + i));
			for (int i = 1; i < 13; i++)
				Expect(target[this.Correct[i]], Is.EqualTo(0), this.Prefix + "SingleElement." + (86 + i));
			for (int i = 1; i < 13; i++)
				Expect(target.Remove(this.Correct[i]), Is.EqualTo(false), this.Prefix + "SingleElement." + (98 + i));

		}
        [Test]
        public void Equality()
        {
            D a = this.Create(10);
            D b = this.Create(10);
            for (int i = 0; i < 100; i++)
            {
                a["index" + i] = i;
                b["index" + (99 - i)] = 99 - i;
            }
            Expect(a, Is.EqualTo(b),  this.Prefix + "Equality." + 0);
            Expect(b, Is.EqualTo(a), this.Prefix + "Equality." + 1);
            b["index100"] = 100;
            Expect(a, Is.Not.EqualTo(b), this.Prefix + "Equality." + 2);
            Expect(b, Is.Not.EqualTo(a), this.Prefix + "Equality." + 3);
            b.Remove("index100");
            Expect(a, Is.EqualTo(b), this.Prefix + "Equality." + 4);
            Expect(b, Is.EqualTo(a), this.Prefix + "Equality." + 5);
            b.Remove("index50");
            Expect(a, Is.Not.EqualTo(b), this.Prefix + "Equality." + 6);
            Expect(b, Is.Not.EqualTo(a), this.Prefix + "Equality." + 7);
        }
		[Test]
		public void NullValue()
		{
			Collection.IDictionary<string, string> d = new Collection.Dictionary<string, string>();
			d["hello"] = "goodbye";
			d["kean"] = null; 
			Expect(d.Contains("hello"), Is.True, this.Prefix + "NullValue." + 0);
			Expect(d.Contains("kean"), Is.True, this.Prefix + "NullValue." + 1);
			Expect(d.Remove("kean"), Is.True, this.Prefix + "NullValue." + 2);
			Expect(d.Contains("kean"), Is.False, this.Prefix + "NullValue." + 3);
			Expect(d["kean"], Is.Null, this.Prefix + "NullValue." + 4);


		}
	}
}

