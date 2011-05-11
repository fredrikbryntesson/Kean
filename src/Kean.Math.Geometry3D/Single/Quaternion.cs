// 
//  Quaternion.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
using System;
using Kean.Core.Basis.Extension;

namespace Kean.Math.Geometry3D.Single
{
    public class Quaternion :
        Abstract.Quaternion<Transform, TransformValue, Quaternion, Point, PointValue, Size, SizeValue, Kean.Math.Single, float>
    {
        public Quaternion() { }
        public Quaternion(float real, Point imaginary) :
            base(real, imaginary) { }
        public Quaternion(float x, float y, float z, float w) :
            base(x, new Point(y, z, w)) { }
        #region Casts
        public static implicit operator string(Quaternion value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator Quaternion(string value)
        {
            Quaternion result = null;
            if (value.NotEmpty())
            {

                try
                {
                    string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length == 4)
                        result = new Quaternion(Kean.Math.Single.Parse(values[0]), Kean.Math.Single.Parse(values[1]), Kean.Math.Single.Parse(values[2]), Kean.Math.Single.Parse(values[3]));
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
