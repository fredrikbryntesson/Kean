// 
//  Generator.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika 2012
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

namespace Kean.Math.Random
{
	public abstract class Generator<T> :
		IGenerator<T>
	{
	    static ulong[][] datums;
        static int counter = 0;

		int a, b, c;
		ulong x, y, z;
		ulong w;
		static Generator()
		{
			Generator<T>.datums = new ulong[][]
	        {
				new ulong[] { 5, 14, 1,  36243606, 521288629, 88675123 },
				new ulong[] { 15, 4, 21, 30633188, 541324838, 92181608 },
	            new ulong[] { 23, 24, 3, 81499928, 853670779, 40506423 },
				new ulong[] { 5, 12, 29, 04594501, 135437015, 00321629 }
	        };
		}
        protected Generator() : this((ulong)DateTime.Now.Ticks)
        { }
        protected Generator(ulong seed)
        {
			lock (Generator<T>.datums)
			{
	            this.a = (int)Generator<T>.datums[Generator<T>.counter][0];
	            this.b = (int)Generator<T>.datums[Generator<T>.counter][1];
	            this.c = (int)Generator<T>.datums[Generator<T>.counter][2];
				this.x = seed;
	            this.y = Generator<T>.datums[Generator<T>.counter][3];
	            this.z = Generator<T>.datums[Generator<T>.counter][4];
	            this.w = Generator<T>.datums[Generator<T>.counter][5];
	            Generator<T>.counter = (Generator<T>.counter + 1) %  Generator<T>.datums.Length;
			}
        }

		protected ulong Next()
		{
            ulong t = (this.x ^ (this.x << this.a));
            this.x = this.y;
            this.y = this.z;
            this.z = this.w;
            return this.w = (this.w ^ (this.w >> this.c)) ^ (t ^ (t >> this.b));
		}
		protected ulong Next(int bits)
		{
			return this.Next() >> (64 - bits);
		}
		public abstract T Generate();
		public virtual T[] Generate(int count)
		{
			T[] result = new T[count];
			for (int i = 0; i < count; i++)
				result[i] = this.Generate();
			return result;
		}
		public virtual T[] GenerateUnique(int count)
		{
			T[] result = new T[count];
			for (int i = 0; i < count; i++)
			{
				result[i] = this.Generate();
				for (int j = 0; j < i; j++)
					if (result[j].Equals(result[i]))
						j = --i;
			}
			return result;
		}

	}
}