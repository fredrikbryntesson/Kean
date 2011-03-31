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
using Kean.Core.Basis.Extension;

namespace Kean.Math.Geometry2D.Integer
{
    public class Transform : Abstract.Transform<Transform, TransformValue, Size, SizeValue, Kean.Math.Integer, int>
    {
        public Transform() { }
        public Transform(int a, int b, int c, int d, int e, int f) : base(a, b, c, d, e, f) { }
        public override TransformValue Value { get { return (TransformValue)this; } }
         #region Casts
        public static explicit operator TransformValue(Transform value)
        {
            return new TransformValue(value.A, value.B, value.C, value.D, value.E, value.F);
        }
        public static implicit operator string(Transform value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator Transform(string value)
        {
            Transform result = null;
            try
            {
                string[] values = value.Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 4)
                    result = new Transform(Kean.Math.Integer.Parse(values[0]), Kean.Math.Integer.Parse(values[3]), Kean.Math.Integer.Parse(values[1]), Kean.Math.Integer.Parse(values[4]), Kean.Math.Integer.Parse(values[2]), Kean.Math.Integer.Parse(values[5]));
            }
            catch
            {
                result = null;
            }
            return result;
        }
        #endregion 
    }
}
