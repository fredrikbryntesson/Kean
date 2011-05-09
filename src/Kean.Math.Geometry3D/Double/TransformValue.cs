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

namespace Kean.Math.Geometry3D.Double
{
    public struct TransformValue :
        Abstract.ITransform<double>
    {
        double a;
        double b;
        double c;
        double d;
        double e;
        double f;
        double g;
        double h;
        double i;
        double j;
        double k;
        double l;

        public double A
        {
            get { return this.a; }
            set { this.a = value; }
        }
        public double B
        {
            get { return this.b; }
            set { this.b = value; }
        }
        public double C
        {
            get { return this.c; }
            set { this.c = value; }
        }
        public double D
        {
            get { return this.d; }
            set { this.d = value; }
        }
        public double E
        {
            get { return this.e; }
            set { this.e = value; }
        }
        public double F
        {
            get { return this.f; }
            set { this.f = value; }
        }
        public double G
        {
            get { return this.g; }
            set { this.g = value; }
        }
        public double H
        {
            get { return this.h; }
            set { this.h = value; }
        }
        public double I
        {
            get { return this.i; }
            set { this.i = value; }
        }
        public double J
        {
            get { return this.j; }
            set { this.j = value; }
        }
        public double K
        {
            get { return this.k; }
            set { this.k = value; }
        }
        public double L
        {
            get { return this.l; }
            set { this.l = value; }
        }
        public TransformValue(double a, double b, double c, double d, double e, double f, double g, double h, double i, double j, double k, double l)
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
        public static implicit operator TransformValue(Single.TransformValue value)
        {
            return new TransformValue(value.A, value.B, value.C, value.D, value.E, value.F, value.G, value.H, value.I, value.J, value.K, value.L);
        }
        public static implicit operator TransformValue(Integer.TransformValue value)
        {
            return new TransformValue(value.A, value.B, value.C, value.D, value.E, value.F, value.G, value.H, value.I, value.J, value.K, value.L);
        }
        public static explicit operator Single.TransformValue(TransformValue value)
        {
            return new Single.TransformValue((Kean.Math.Single)(value.A), (Kean.Math.Single)(value.B), (Kean.Math.Single)(value.C), (Kean.Math.Single)(value.D), (Kean.Math.Single)(value.E), (Kean.Math.Single)(value.F), (Kean.Math.Single)(value.G), (Kean.Math.Single)(value.H), (Kean.Math.Single)(value.I), (Kean.Math.Single)(value.J), (Kean.Math.Single)(value.K), (Kean.Math.Single)(value.L));
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
            TransformValue result = null;
            try
            {
                string[] values = value.Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 9)
                    result = new TransformValue(Kean.Math.Double.Parse(values[0]), Kean.Math.Double.Parse(values[3]), Kean.Math.Double.Parse(values[1]), Kean.Math.Double.Parse(values[4]), Kean.Math.Double.Parse(values[2]), Kean.Math.Double.Parse(values[5]), Kean.Math.Double.Parse(values[6]), Kean.Math.Double.Parse(values[7]), Kean.Math.Double.Parse(values[8]), Kean.Math.Double.Parse(values[9]), Kean.Math.Double.Parse(values[10]), Kean.Math.Double.Parse(values[11]));
            }
            catch
            {
                result = null;
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
                ^ this.F.GetHashCode()
                ^ this.G.GetHashCode()
                ^ this.H.GetHashCode()
                ^ this.I.GetHashCode()
                ^ this.J.GetHashCode()
                ^ this.K.GetHashCode()
                ^ this.L.GetHashCode();
        }
        public override string ToString()
        {
            return 
                    this.A.ToString() + ", " + this.D.ToString() + ", " + this.G.ToString() + ", " + this.J.ToString() + "; " 
                +   this.B.ToString() + ", " + this.E.ToString() + ", " + this.H.ToString() + ", " + this.K.ToString() + "; "
                +   this.C.ToString() + ", " + this.F.ToString() + ", " + this.I.ToString() + ", " + this.L.ToString() + "; "
                +   0 + ", " + 0 + ", " + 0 + ", " + 1;
        }
        #endregion
   }
}