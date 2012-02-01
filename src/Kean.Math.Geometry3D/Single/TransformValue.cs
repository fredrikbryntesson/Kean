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
using Kean.Core.Extension;

namespace Kean.Math.Geometry3D.Single
{
    public struct TransformValue :
        Abstract.ITransform<float>
    {
        float a;
        float b;
        float c;
        float d;
        float e;
        float f;
        float g;
        float h;
        float i;
        float j;
        float k;
        float l;

        public float A
        {
            get { return this.a; }
            set { this.a = value; }
        }
        public float B
        {
            get { return this.b; }
            set { this.b = value; }
        }
        public float C
        {
            get { return this.c; }
            set { this.c = value; }
        }
        public float D
        {
            get { return this.d; }
            set { this.d = value; }
        }
        public float E
        {
            get { return this.e; }
            set { this.e = value; }
        }
        public float F
        {
            get { return this.f; }
            set { this.f = value; }
        }
        public float G
        {
            get { return this.g; }
            set { this.g = value; }
        }
        public float H
        {
            get { return this.h; }
            set { this.h = value; }
        }
        public float I
        {
            get { return this.i; }
            set { this.i = value; }
        }
        public float J
        {
            get { return this.j; }
            set { this.j = value; }
        }
        public float K
        {
            get { return this.k; }
            set { this.k = value; }
        }
        public float L
        {
            get { return this.l; }
            set { this.l = value; }
        }
        public TransformValue(float a, float b, float c, float d, float e, float f, float g, float h, float i, float j, float k, float l)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.e = e;
            this.f = f;
            this.g = g;
            this.h = h;
            this.i = i;
            this.j = j;
            this.k = k;
            this.l = l;
        }
        #region Casts
        public static implicit operator TransformValue(Integer.TransformValue value)
        {
            return new TransformValue(value.A, value.B, value.C, value.D, value.E, value.F, value.G, value.H, value.I, value.J, value.K, value.L);
        }
        public static explicit operator Integer.TransformValue(TransformValue value)
        {
            return new Integer.TransformValue((Kean.Math.Integer)(value.A), (Kean.Math.Integer)(value.B), (Kean.Math.Integer)(value.C), (Kean.Math.Integer)(value.D), (Kean.Math.Integer)(value.E), (Kean.Math.Integer)(value.F), (Kean.Math.Integer)(value.G), (Kean.Math.Integer)(value.H), (Kean.Math.Integer)(value.I), (Kean.Math.Integer)(value.J), (Kean.Math.Integer)(value.K), (Kean.Math.Integer)(value.L));
        }
        public static implicit operator string(TransformValue value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator TransformValue(string value)
        {
            TransformValue result = new TransformValue();
            if (value.NotEmpty())
            {
                try
                {
					result = (TransformValue)(Transform)(value);
				}
                catch
                {
                }
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
            return 33 * (33 * (33 * (33 * (33 * (33 * (33 * (33 * (33 * (33 * (33 * this.A.GetHashCode() ^ this.B.GetHashCode()) ^ this.C.GetHashCode()) ^ this.D.GetHashCode()) ^ this.E.GetHashCode()) ^ this.F.GetHashCode()) ^ this.G.GetHashCode()) ^ this.H.GetHashCode()) ^ this.I.GetHashCode()) ^ this.J.GetHashCode()) ^ this.K.GetHashCode()) ^ this.L.GetHashCode();
        }
		public override string ToString()
		{
			return this.ToString(false);
		}
		public string ToString(bool matlab)
		{
			return ((Transform)this).ToString(matlab);
		}
        #endregion
   }
}