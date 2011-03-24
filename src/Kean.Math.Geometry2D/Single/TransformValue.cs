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

namespace Kean.Math.Geometry2D.Single
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
        public TransformValue(float a, float b, float c, float d, float e, float f)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.e = e;
            this.f = f;
        }
   }
}