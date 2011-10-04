// 
//  Transform.cs
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
    public class Transform : 
        Abstract.Transform<Transform, TransformValue, Size, SizeValue, Kean.Math.Single, float>
    {
        public Transform() { }
        public Transform(float a, float b, float c, float d, float e, float f, float g, float h, float i, float j, float k, float l) : base(a, b, c, d, e, f, g, h, i, j, k, l) { }
        public override TransformValue Value { get { return (TransformValue)this; }}
        #region Casts
        public static implicit operator Transform(Integer.Transform value)
        {
            return new Transform(value.A, value.B, value.C, value.D, value.E, value.F, value.G, value.H, value.I, value.J, value.K, value.L);
        }
        public static explicit operator Integer.Transform(Transform value)
        {
            return new Integer.Transform((Kean.Math.Integer)(value.A), (Kean.Math.Integer)(value.B), (Kean.Math.Integer)(value.C), (Kean.Math.Integer)(value.D), (Kean.Math.Integer)(value.E), (Kean.Math.Integer)(value.F), (Kean.Math.Integer)(value.G), (Kean.Math.Integer)(value.H), (Kean.Math.Integer)(value.I), (Kean.Math.Integer)(value.J), (Kean.Math.Integer)(value.K), (Kean.Math.Integer)(value.L));
        }
        public static implicit operator Transform(TransformValue value)
        {
            return new Transform(value.A, value.B, value.C, value.D, value.E, value.F, value.G, value.H, value.I, value.J, value.K, value.L);
        }
        public static explicit operator TransformValue(Transform value)
        {
            return new TransformValue(value.A, value.B, value.C, value.D, value.E, value.F, value.G, value.H, value.I, value.J, value.K, value.L);
        }
        public static implicit operator string(Transform value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator Transform(string value)
        {
            Transform result = null;
            if (value.NotEmpty())
            {

                try
                {
                    string[] values = value.Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length == 12)
                        result = new Transform(
                            Kean.Math.Single.Parse(values[0]), Kean.Math.Single.Parse(values[1]), Kean.Math.Single.Parse(values[2]),
                            Kean.Math.Single.Parse(values[3]), Kean.Math.Single.Parse(values[4]), Kean.Math.Single.Parse(values[5]),
                            Kean.Math.Single.Parse(values[6]), Kean.Math.Single.Parse(values[7]), Kean.Math.Single.Parse(values[8]),
                            Kean.Math.Single.Parse(values[9]), Kean.Math.Single.Parse(values[10]), Kean.Math.Single.Parse(values[11]));
                }
                catch
                {
                    result = null;
                }
            }
            return result;
        }
        #endregion
    }
}
