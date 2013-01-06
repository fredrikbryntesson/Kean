// 
//  PointValue.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
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
using NUnit.Framework;

using Target = Kean.Math.Geometry2D;
using Kean.Core.Extension;

namespace Kean.Math.Geometry2D.Test.Double
{
    [TestFixture]
    public class PointValue :
        Kean.Test.Fixture<PointValue>
    {
        string prefix = "Kean.Math.Geometry2D.Test.Double.PointValue";
        float Precision { get { return 1e-4f; } }
        Target.Double.PointValue CastFromString(string value)
        {
            return value;
        }
        string CastToString(Target.Double.PointValue value)
        {
            return value;
        }
        Target.Double.PointValue vector0 = new Target.Double.PointValue((double)22.221f, (double)-3.1f);
        Target.Double.PointValue vector1 = new Target.Double.PointValue((double)12.221f, (double)13.1f);
        Target.Double.PointValue vector2 = new Target.Double.PointValue((double)34.442f, (double)10.0f);
        Target.Double.PointValue vector3 = new Target.Double.PointValue((double)10, (double)20);

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
            Target.Double.PointValue point = new Target.Double.PointValue();
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
            Expect((this.vector0 + this.vector1).X, Is.EqualTo(this.vector2.X).Within(this.Precision));
            Expect((this.vector0 + this.vector1).Y, Is.EqualTo(this.vector2.Y).Within(this.Precision));
        }
        [Test]
        public void Subtraction()
        {
            Expect(this.vector0 - this.vector0, Is.EqualTo(new Target.Double.PointValue()));
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
            Expect(this.vector0.X, Is.EqualTo((double)(22.221)).Within(this.Precision), this.prefix + "GetValues.0");
            Expect(this.vector0.Y, Is.EqualTo((double)(-3.1)).Within(this.Precision), this.prefix + "GetValues.1");
        }
        [Test]
        public void Swap()
        {
            Target.Double.PointValue result = this.vector0.Swap();
            Expect(result.X, Is.EqualTo(this.vector0.Y), this.prefix + "Swap.0");
            Expect(result.Y, Is.EqualTo(this.vector0.X), this.prefix + "Swap.1");
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
            Target.Double.PointValue point = null;
            Expect(this.CastToString(point), Is.EqualTo(value), this.prefix + "CastingNull.0");
            Expect(this.CastFromString(value), Is.EqualTo(point), this.prefix + "CastingNull.1");
        }

        #region Polar Representation
        [Test]
        public void Polar0()
        {
            Target.Double.PointValue point = new Target.Double.PointValue();
            Expect(point.Norm, Is.EqualTo(0));
            Expect(point.Azimuth, Is.EqualTo(0));
        }
        [Test]
        public void Polar1()
        {
            Target.Double.PointValue point = new Target.Double.PointValue(1, 0);
            Expect(point.Norm, Is.EqualTo(1));
            Expect(point.Azimuth, Is.EqualTo(0));
        }
        [Test]
        public void Polar2()
        {
            Target.Double.PointValue point = new Target.Double.PointValue(0, 1);
            Expect(point.Norm, Is.EqualTo(1));
            Expect(point.Azimuth, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
        }
        [Test]
        public void Polar3()
        {
            Target.Double.PointValue point = new Target.Double.PointValue(0, -5);
            Expect(point.Norm, Is.EqualTo(5));
            Expect(point.Azimuth, Is.EqualTo(Kean.Math.Double.ToRadians(-90)));
        }
        [Test]
        public void Polar4()
        {
            Target.Double.PointValue point = new Target.Double.PointValue(-1, 0);
            Expect(point.Norm, Is.EqualTo(1));
            Expect(point.Azimuth, Is.EqualTo(Kean.Math.Double.ToRadians(180)));
        }
        [Test]
        public void Polar5()
        {
            Target.Double.PointValue point = new Target.Double.PointValue(-3, 0);
            double radius = point.Norm;
            double azimuth = point.Azimuth;
            Target.Double.PointValue point2 = Target.Double.PointValue.Polar(radius, azimuth);
            Expect(point.Distance(point2), Is.EqualTo(0).Within(this.Precision));
        }
        #endregion
        [Test]
        public void Angles()
        {
            Expect(Target.Single.PointValue.BasisX.Angle(Target.Single.PointValue.BasisX), Is.EqualTo(0).Within(this.Precision));
            Expect(Target.Single.PointValue.BasisX.Angle(Target.Single.PointValue.BasisY), Is.EqualTo(Kean.Math.Single.Pi / 2).Within(this.Precision));
            Expect(Target.Single.PointValue.BasisX.Angle(-Target.Single.PointValue.BasisY), Is.EqualTo(-Kean.Math.Single.Pi / 2).Within(this.Precision));
            Expect(Target.Single.PointValue.BasisX.Angle(-Target.Single.PointValue.BasisX), Is.EqualTo(Kean.Math.Single.Pi).Within(this.Precision));
        }
        [Test]
        public void Casts()
        {
            // integer - double
            {
                Target.Integer.PointValue integer = new Target.Integer.PointValue(10, 20);
                Target.Double.PointValue @double = integer;
                Expect(@double.X, Is.EqualTo(10));
                Expect(@double.Y, Is.EqualTo(20));
                Expect((Target.Integer.PointValue)@double, Is.EqualTo(integer));
            }
            // single - double
            {
                Target.Single.PointValue single = new Target.Single.PointValue(10, 20);
                Target.Double.PointValue @double = single;
                Expect(@double.X, Is.EqualTo(10));
                Expect(@double.Y, Is.EqualTo(20));
                Expect((Target.Single.PointValue)@double, Is.EqualTo(single));
            }
        }
		[Test]
        public void StringCasts()
        {
            string textFromValue = new Target.Double.PointValue(10, 20);
            Expect(textFromValue, Is.EqualTo("10, 20"));
            Target.Double.PointValue @integerFromText = "10, 20";
            Expect(@integerFromText.X, Is.EqualTo(10));
            Expect(@integerFromText.Y, Is.EqualTo(20));
        }
    }
}

