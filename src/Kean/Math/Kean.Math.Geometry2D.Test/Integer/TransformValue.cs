// 
//  TransformValue.cs
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
    public class TransformValue :
        Kean.Test.Fixture<TransformValue>
    {
        string prefix = "Kean.Math.Geometry2D.Test.Integer.TransformValue";
        float Precision { get { return 1e-4f; } }
        Target.Integer.TransformValue transform0;
        Target.Integer.TransformValue transform1;
        Target.Integer.TransformValue transform2;
        Target.Integer.TransformValue transform3;
        Target.Integer.TransformValue transform4;
        Target.Integer.PointValue point0;
        Target.Integer.PointValue point1;
        Target.Integer.SizeValue size0;

        [TestFixtureSetUp]
        public override void Setup()
        {
            this.transform0 = new Target.Integer.TransformValue(3, 1, 2, 1, 5, 7);
            this.transform1 = new Target.Integer.TransformValue(7, 4, 2, 5, 7, 6);
            this.transform2 = new Target.Integer.TransformValue(29, 11, 16, 7, 38, 20);
            this.transform3 = new Target.Integer.TransformValue(1, -1, -2, 3, 9, -16);
            this.transform4 = new Target.Integer.TransformValue(10, 20, 30, 40, 50, 60);
            this.point0 = new Target.Integer.PointValue(-7, 3);
            this.point1 = new Target.Integer.PointValue(2, -7);
            this.size0 = new Target.Integer.SizeValue(10, 10);
        }
        protected override void Run()
        {
            this.Run(
                 this.Equality,
                this.CreateZeroTransform,
                this.CreateIdentity,
                this.CreateRotation,
                this.CreateScale,
                this.CreateTranslation,
                this.Rotatate,
                this.Scale,
                this.Translatate,
                this.InverseTransform,
                this.MultiplicationTransformTransform,
                this.MultiplicationTransformPoint,
                this.GetValueValues,
                this.CastToArray,
                this.GetTranslation,
                this.GetScalingX,
                this.GetScalingY,
                this.GetScaling,
                this.GetRotation,
                this.Casting,
                this.Hash,
                this.Casts,
                this.StringCasts
                );
        }
        int Cast(double value)
        {
            return (int)value;
        }
        Target.Integer.TransformValue CastFromString(string value)
        {
            return (Target.Integer.TransformValue)value;
        }
        string CastToString(Target.Integer.TransformValue value)
        {
            return (string)value;
        }
        #region Equality
        [Test]
        public void Equality()
        {
            Target.Integer.TransformValue transform = new Target.Integer.TransformValue();
            Expect(this.transform0, Is.EqualTo(this.transform0));
            Expect(this.transform0.Equals(this.transform0 as object), Is.True);
            Expect(this.transform0 == this.transform0, Is.True);
            Expect(this.transform0 != this.transform1, Is.True);
            Expect(this.transform0 == transform, Is.False);
            Expect(transform == transform, Is.True);
            Expect(transform == this.transform0, Is.False);
        }
        #endregion
        #region Arithmetic
        [Test]
        public void InverseTransform()
        {
            Expect(this.transform0.Inverse, Is.EqualTo(this.transform3));
        }
        [Test]
        public void MultiplicationTransformTransform()
        {
            Expect(this.transform0 * this.transform1, Is.EqualTo(this.transform2));
        }
        [Test]
        public void MultiplicationTransformPoint()
        {
            Expect(this.transform0 * this.point0, Is.EqualTo(this.point1));
        }
        #endregion
        [Test]
        public void CreateZeroTransform()
        {
            Target.Integer.TransformValue transform = new Target.Integer.TransformValue();
            Expect(transform.A, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.0");
            Expect(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.1");
            Expect(transform.C, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.2");
            Expect(transform.D, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.3");
            Expect(transform.E, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.4");
            Expect(transform.F, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.5");
        }
        [Test]
        public void CreateIdentity()
        {
            Target.Integer.TransformValue transform = Target.Integer.TransformValue.Identity;
            Expect(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "CreateIdentity.0");
            Expect(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateIdentity.1");
            Expect(transform.C, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateIdentity.2");
            Expect(transform.D, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "CreateIdentity.3");
            Expect(transform.E, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateIdentity.4");
            Expect(transform.F, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateIdentity.5");
        }
        [Test]
        public void Rotatate()
        {
            Target.Integer.TransformValue identity = Target.Integer.TransformValue.Identity;
            int angle = Kean.Math.Integer.ToRadians(20);
            Target.Integer.TransformValue transform = Target.Integer.TransformValue.CreateRotation(angle);
            transform = transform.Rotate(-angle);
            Expect(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "Rotatate.0");
            Expect(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Rotatate.1");
            Expect(transform.C, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Rotatate.2");
            Expect(transform.D, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "Rotatate.3");
            Expect(transform.E, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Rotatate.4");
            Expect(transform.F, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Rotatate.5");
        }
        [Test]
        public void Scale()
        {
            Target.Integer.TransformValue identity = new Target.Integer.TransformValue(20, 0, 0, 20, 0, 0);
            int scale = 20;
            Target.Integer.TransformValue transform = Target.Integer.TransformValue.CreateScaling(scale, scale);
            transform = transform.Scale(5, 5);
            Expect(transform.A, Is.EqualTo(this.Cast(100)).Within(this.Precision), this.prefix + "Scale.0");
            Expect(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Scale.1");
            Expect(transform.C, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Scale.2");
            Expect(transform.D, Is.EqualTo(this.Cast(100)).Within(this.Precision), this.prefix + "Scale.3");
            Expect(transform.E, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Scale.4");
            Expect(transform.F, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Scale.5");
        }
        [Test]
        public void Translatate()
        {
            int xDelta = 40;
            int yDelta = -40;
            Target.Integer.TransformValue transform = Target.Integer.TransformValue.CreateTranslation(xDelta, yDelta);
            transform = transform.Translate(-xDelta, -yDelta);
            Expect(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "Translatate.0");
            Expect(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Translatate.1");
            Expect(transform.C, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Translatate.2");
            Expect(transform.D, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "Translatate.3");
            Expect(transform.E, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Translatate.4");
            Expect(transform.F, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Translatate.5");
        }
        [Test]
        public void CreateRotation()
        {
            int angle = Kean.Math.Integer.ToRadians(20);
            Target.Integer.TransformValue transform = Target.Integer.TransformValue.CreateRotation(angle);
            Expect(transform.A, Is.EqualTo(Kean.Math.Integer.Cosinus(angle)).Within(this.Precision), this.prefix + "CreateRotation.0");
            Expect(transform.B, Is.EqualTo(Kean.Math.Integer.Sinus(angle)).Within(this.Precision), this.prefix + "CreateRotation.1");
            Expect(transform.C, Is.EqualTo(-Kean.Math.Integer.Sinus(angle)).Within(this.Precision), this.prefix + "CreateRotation.2");
            Expect(transform.D, Is.EqualTo(Kean.Math.Integer.Cosinus(angle)).Within(this.Precision), this.prefix + "CreateRotation.3");
            Expect(transform.E, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateRotation.4");
            Expect(transform.F, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateRotation.5");
        }
        [Test]
        public void CreateScale()
        {
            int scale = 20;
            Target.Integer.TransformValue transform = Target.Integer.TransformValue.CreateScaling(scale, scale);
            Expect(transform.A, Is.EqualTo(scale).Within(this.Precision), this.prefix + "CreateScale.0");
            Expect(transform.B, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateScale.1");
            Expect(transform.C, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateScale.2");
            Expect(transform.D, Is.EqualTo(scale).Within(this.Precision), this.prefix + "CreateScale.3");
            Expect(transform.E, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateScale.4");
            Expect(transform.F, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateScale.5");
        }
        [Test]
        public void CreateTranslation()
        {
            int xDelta = 40;
            int yDelta = -40;
            Target.Integer.TransformValue transform = Target.Integer.TransformValue.CreateTranslation(xDelta, yDelta);
            Expect(transform.A, Is.EqualTo(1).Within(this.Precision), this.prefix + "CreateTranslation.0");
            Expect(transform.B, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateTranslation.1");
            Expect(transform.C, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateTranslation.2");
            Expect(transform.D, Is.EqualTo(1).Within(this.Precision), this.prefix + "CreateTranslation.3");
            Expect(transform.E, Is.EqualTo(xDelta).Within(this.Precision), this.prefix + "CreateTranslation.4");
            Expect(transform.F, Is.EqualTo(yDelta).Within(this.Precision), this.prefix + "CreateTranslation.5");
        }
        [Test]
        public void GetValueValues()
        {
            Target.Integer.TransformValue transform = this.transform0;
            Expect(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "GetValueValues.0");
            Expect(transform.B, Is.EqualTo(this.Cast(4)).Within(this.Precision), this.prefix + "GetValueValues.1");
            Expect(transform.C, Is.EqualTo(this.Cast(2)).Within(this.Precision), this.prefix + "GetValueValues.2");
            Expect(transform.D, Is.EqualTo(this.Cast(5)).Within(this.Precision), this.prefix + "GetValueValues.3");
            Expect(transform.E, Is.EqualTo(this.Cast(3)).Within(this.Precision), this.prefix + "GetValueValues.4");
            Expect(transform.F, Is.EqualTo(this.Cast(6)).Within(this.Precision), this.prefix + "GetValueValues.5");
        }
        [Test]
        public void GetScalingX()
        {
            int scale = this.transform0.ScalingX;
            Expect(scale, Is.EqualTo(this.Cast(4.1231)).Within(this.Precision), this.prefix + "GetScalingX.0");
        }
        [Test]
        public void GetScalingY()
        {
            int scale = this.transform0.ScalingY;
            Expect(scale, Is.EqualTo(this.Cast(5.38516474f)).Within(this.Precision), this.prefix + "GetScalingY.0");
        }
        [Test]
        public void GetScaling()
        {
            int scale = this.transform0.Scaling;
            Expect(scale, Is.EqualTo(this.Cast(4.75413513f)).Within(this.Precision), this.prefix + "GetScaling.0");
        }
        [Test]
        public void GetRotation()
        {
            int angle = Kean.Math.Integer.ToRadians(20);
            Target.Integer.TransformValue transform = Target.Integer.TransformValue.CreateRotation(angle);
            int value = Kean.Math.Integer.ToDegrees(transform.Rotation);
            Expect(value, Is.EqualTo(this.Cast(20)).Within(this.Precision), this.prefix + "GetRotation.0");
        }
        [Test]
        public void GetTranslation()
        {
            Target.Integer.SizeValue translation = this.transform0.Translation;
            Expect(translation.Width, Is.EqualTo(this.Cast(3)).Within(this.Precision), this.prefix + "GetTranslation.0");
            Expect(translation.Height, Is.EqualTo(this.Cast(6)).Within(this.Precision), this.prefix + "GetTranslation.1");
        }
        [Test]
        public void CastToArray()
        {
            int[,] values = (int[,])(Target.Integer.TransformValue.Identity);
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                    Expect(values[x, y], Is.EqualTo(this.Cast(x == y ? 1 : 0)).Within(this.Precision), this.prefix + "CastToArray.0");
        }
        [Test]
        public void Casting()
        {
            string value = "10, 20, 30, 40, 50, 60";
            Expect(this.CastToString(this.transform4), Is.EqualTo(value), this.prefix + "Casting.0");
            Expect(this.CastFromString(value), Is.EqualTo(this.transform4), this.prefix + "Casting.1");
        }
        #region Hash Code
        [Test]
        public void Hash()
        {
            Expect(this.transform0.Hash(), Is.Not.EqualTo(0));
        }
        #endregion

		[Test]
        public void Casts()
        { }
		[Test]
        public void StringCasts()
        {
            string textFromValue = new Target.Integer.TransformValue(10, 20, 30, 40, 50, 60);
            Expect(textFromValue, Is.EqualTo("10, 20, 30, 40, 50, 60"));
            Target.Integer.TransformValue @integerFromText = "10 20 30 40 50 60";
            Expect(@integerFromText.A, Is.EqualTo(10));
            Expect(@integerFromText.B, Is.EqualTo(20));
            Expect(@integerFromText.C, Is.EqualTo(30));
            Expect(@integerFromText.D, Is.EqualTo(40));
            Expect(@integerFromText.E, Is.EqualTo(50));
            Expect(@integerFromText.F, Is.EqualTo(60));
        }
    }
}

