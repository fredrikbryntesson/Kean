// 
//  SizeValue.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2012 Anders Frisk
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
using NUnit.Framework;
using NUnit.Framework;
using Target = Kean.Math.Geometry2D;
using Kean.Core.Extension;

namespace Kean.Math.Geometry2D.Test.Integer
{
    [TestFixture]
    public class SizeValue :
        Kean.Test.Fixture<SizeValue>
    {
        string prefix = "Kean.Math.Geometry2D.Test.Integer.SizeValue";
        float Precision { get { return 1e-4f; } }
        Target.Integer.SizeValue CastFromString(string value)
        {
            return value;
        }
        string CastToString(Target.Integer.SizeValue value)
        {
            return value;
        }
        Target.Integer.SizeValue vector0 = new Target.Integer.SizeValue((int)22.221f, (int)-3.1f);
        Target.Integer.SizeValue vector1 = new Target.Integer.SizeValue((int)12.221f, (int)13.1f);
        Target.Integer.SizeValue vector2 = new Target.Integer.SizeValue((int)34.442f, (int)10.0f);
        Target.Integer.SizeValue vector3 = new Target.Integer.SizeValue((int)10, (int)20);

        protected override void Run()
        {
            this.Run(
                this.Polar0,
                this.Polar1,
                this.Polar2,
                this.Polar3,
                this.Polar4,
                this.Polar5,
                this.Angles,
                this.Casts,
                this.StringCasts
                );
        }
        #region Equality
        [Test]
        public void Equality()
        {
            Target.Integer.SizeValue point = new Target.Integer.SizeValue();
            Expect(this.vector0, Is.EqualTo(this.vector0));
            Expect(this.vector0.Equals(this.vector0 as object), Is.True);
            Expect(this.vector0 == this.vector0, Is.True);
            Expect(this.vector0 != this.vector1, Is.True);
            Expect(this.vector0 == point, Is.False);
            Expect(point == point, Is.True);
            Expect(point == this.vector0, Is.False);
        }
        #endregion
        #region Arithmetic
        [Test]
        public void Addition()
        {
            Expect((this.vector0 + this.vector1).Width, Is.EqualTo(this.vector2.Width).Within(this.Precision));
            Expect((this.vector0 + this.vector1).Height, Is.EqualTo(this.vector2.Height).Within(this.Precision));
        }
        [Test]
        public void Subtraction()
        {
            Expect(this.vector0 - this.vector0, Is.EqualTo(new Target.Integer.SizeValue()));
        }
        [Test]
        public void ScalarMultiplication()
        {
            Expect((-1) * this.vector0, Is.EqualTo(-this.vector0));
        }
        [Test]
        public void ScalarDivision()
        {
            Expect(this.vector0 / (-1), Is.EqualTo(-this.vector0));
        }
        #endregion
        #region Hash Code
        [Test]
        public void Hash()
        {
            Expect(this.vector0.Hash(), Is.Not.EqualTo(0));
        }
        #endregion

        [Test]
        public void GetValues()
        {
            Expect(this.vector0.Width, Is.EqualTo((int)(22.221)).Within(this.Precision), this.prefix + "GetValues.0");
            Expect(this.vector0.Height, Is.EqualTo((int)(-3.1)).Within(this.Precision), this.prefix + "GetValues.1");
        }
        [Test]
        public void Swap()
        {
            Target.Integer.SizeValue result = this.vector0.Swap();
            Expect(result.Width, Is.EqualTo(this.vector0.Height), this.prefix + "Swap.0");
            Expect(result.Height, Is.EqualTo(this.vector0.Width), this.prefix + "Swap.1");
        }
        [Test]
        public void Casting()
        {
            string value = "10, 20";
            Expect(this.CastToString(this.vector3), Is.EqualTo(value), this.prefix + "Casting.0");
            Expect(this.CastFromString(value), Is.EqualTo(this.vector3), this.prefix + "Casting.1");
        }
        [Test]
        public void CastingNull()
        {
            string value = null;
            Target.Integer.SizeValue point = null;
            Expect(this.CastToString(point), Is.EqualTo(value), this.prefix + "CastingNull.0");
            Expect(this.CastFromString(value), Is.EqualTo(point), this.prefix + "CastingNull.1");
        }

        #region Polar Representation
        [Test]
        public void Polar0()
        {
            Target.Integer.SizeValue point = new Target.Integer.SizeValue();
            Expect(point.Norm, Is.EqualTo(0));
            Expect(point.Azimuth, Is.EqualTo(0));
        }
        [Test]
        public void Polar1()
        {
            Target.Integer.SizeValue point = new Target.Integer.SizeValue(1, 0);
            Expect(point.Norm, Is.EqualTo(1));
            Expect(point.Azimuth, Is.EqualTo(0));
        }
        [Test]
        public void Polar2()
        {
            Target.Integer.SizeValue point = new Target.Integer.SizeValue(0, 1);
            Expect(point.Norm, Is.EqualTo(1));
            Expect(point.Azimuth, Is.EqualTo(Kean.Math.Integer.ToRadians(90)));
        }
        [Test]
        public void Polar3()
        {
            Target.Integer.SizeValue point = new Target.Integer.SizeValue(0, -5);
            Expect(point.Norm, Is.EqualTo(5));
            Expect(point.Azimuth, Is.EqualTo(Kean.Math.Integer.ToRadians(-90)));
        }
        [Test]
        public void Polar4()
        {
            Target.Integer.SizeValue point = new Target.Integer.SizeValue(-1, 0);
            Expect(point.Norm, Is.EqualTo(1));
            Expect(point.Azimuth, Is.EqualTo(Kean.Math.Integer.ToRadians(180)));
        }
        [Test]
        public void Polar5()
        {
            Target.Integer.SizeValue point = new Target.Integer.SizeValue(-3, 0);
            int radius = point.Norm;
            int azimuth = point.Azimuth;
            Target.Integer.SizeValue point2 = Target.Integer.SizeValue.Polar(radius, azimuth);
            Expect(point.Distance(point2), Is.EqualTo(0).Within(this.Precision));
        }
        #endregion
        [Test]
        public void Angles()
        {
            Expect(Target.Single.SizeValue.BasisX.Angle(Target.Single.SizeValue.BasisX), Is.EqualTo(0).Within(this.Precision));
            Expect(Target.Single.SizeValue.BasisX.Angle(Target.Single.SizeValue.BasisY), Is.EqualTo(Kean.Math.Single.Pi / 2).Within(this.Precision));
            Expect(Target.Single.SizeValue.BasisX.Angle(-Target.Single.SizeValue.BasisY), Is.EqualTo(-Kean.Math.Single.Pi / 2).Within(this.Precision));
            Expect(Target.Single.SizeValue.BasisX.Angle(-Target.Single.SizeValue.BasisX), Is.EqualTo(Kean.Math.Single.Pi).Within(this.Precision));
        }
        [Test]
        public void Casts()
        { }
        [Test]
        public void StringCasts()
        {
            string textFromValue = new Target.Integer.SizeValue(10, 20);
            Expect(textFromValue, Is.EqualTo("10, 20"));
            Target.Integer.SizeValue @integerFromText = "10, 20";
            Expect(@integerFromText.Width, Is.EqualTo(10));
            Expect(@integerFromText.Height, Is.EqualTo(20));
        }
    }
}
