// 
//  Vector.cs
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
namespace Abstract
{
	public class Vector<VectorType, R, V> :
        IEquatable<Vector<VectorType, R, V>>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
	{
		protected R X { get; private set; }
		protected R Y { get; private set; }
        #region Constructors
        protected Vector() 
        {
            this.X = new R().Zero;
            this.Y = new R().Zero;
        }
        protected Vector(R x, R y)
		{
			this.X = x;
			this.Y = y;
        }
        #endregion
        #region Object overides and IEquatable<VectorType>
        public override bool Equals(object other)
        {
            return (other is Vector<VectorType, R, V>) && this.Equals(other as Vector<VectorType, R, V>);
        }
        // other is not null here.
        public bool Equals(Vector<VectorType, R, V> other)
        {
            return this.X == other.X && this.Y == other.Y;
        }
        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
        public override string ToString()
        {
            return this.X.ToString() + " " + this.Y.ToString();
        }
        #endregion 
    }
}

