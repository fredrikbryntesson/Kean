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
        public static implicit operator Transform(TransformValue value)
        {
            return new Transform(value.A, value.B, value.C, value.D, value.E, value.F, value.G, value.H, value.I, value.J, value.K, value.L);
        }
        public static explicit operator TransformValue(Transform value)
        {
            return new TransformValue(value.A, value.B, value.C, value.D, value.E, value.F, value.G, value.H, value.I, value.J, value.K, value.L);
        }
        #endregion
   }
}