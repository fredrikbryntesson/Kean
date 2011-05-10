// 
//  TransformValue.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
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
using Kean.Core.Basis.Extension;

namespace Kean.Math.Geometry2D.Integer
{
    public struct TransformValue :
        Abstract.ITransform<int>
    {
        int a;
        int b;
        int c;
        int d;
        int e;
        int f;
        public int A
        {
            get { return this.a; }
            set { this.a = value; }
        }
        public int B
        {
            get { return this.b; }
            set { this.b = value; }
        }
        public int C
        {
            get { return this.c; }
            set { this.c = value; }
        }
        public int D
        {
            get { return this.d; }
            set { this.d = value; }
        }
        public int E
        {
            get { return this.e; }
            set { this.e = value; }
        }
        public int F
        {
            get { return this.f; }
            set { this.f = value; }
        }
        public TransformValue(int a, int b, int c, int d, int e, int f)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.e = e;
            this.f = f;
        }
        #region Casts
        public static implicit operator string(TransformValue value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator TransformValue(string value)
        {
            TransformValue result = new TransformValue();
            try
            {
                string[] values = value.Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 9)
                    result = new TransformValue(Kean.Math.Integer.Parse(values[0]), Kean.Math.Integer.Parse(values[3]), Kean.Math.Integer.Parse(values[1]), Kean.Math.Integer.Parse(values[4]), Kean.Math.Integer.Parse(values[2]), Kean.Math.Integer.Parse(values[5]));
            }
            catch
            {
            }
            return result;
        }
        #endregion
        #region Object Overrides
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>Hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return this.A.GetHashCode()
                ^ this.B.GetHashCode()
                ^ this.C.GetHashCode()
                ^ this.D.GetHashCode()
                ^ this.E.GetHashCode()
                ^ this.F.GetHashCode();
        }
        public override string ToString()
        {
            return Kean.Math.Integer.ToString(this.A) + ", " + Kean.Math.Integer.ToString(this.C) + ", " + Kean.Math.Integer.ToString(this.E) + "; " + Kean.Math.Integer.ToString(this.B) + ", " + Kean.Math.Integer.ToString(this.D) + ", " + Kean.Math.Integer.ToString(this.F) + "; " + 0 + ", " + 0 + ", " + 1;
        }
        #endregion
   }
}