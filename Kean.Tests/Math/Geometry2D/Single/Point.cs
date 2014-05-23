// 
//  Point.cs
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
using Kean.Extension;

namespace Kean.Math.Geometry2D.Test.Single
{
	[TestFixture]
	public class Point :
		Kean.Test.Fixture<Point>
	{
		string prefix = "Kean.Math.Geometry2D.Test.Single.Point";
		float Precision { get { return 1e-4f; } }
		Target.Single.Point CastFromString(string value)
		{
			return (Target.Single.Point)value;
		}
		string CastToString(Target.Single.Point value)
		{
			return value;
		}
		Target.Single.Point vector0 = new Target.Single.Point((float)22.221f, (float)-3.1f);
		Target.Single.Point vector1 = new Target.Single.Point((float)12.221f, (float)13.1f);
		Target.Single.Point vector2 = new Target.Single.Point((float)34.442f, (float)10.0f);
		Target.Single.Point vector3 = new Target.Single.Point((float)10, (float)20);

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
				this.StringCasts,
				this.Projection,
				this.Project
				//this.ProjectLines
				);
		}
		#region Equality
		[Test]
		public void Equality()
		{
#pragma warning disable 1718
			Target.Single.Point point = new Target.Single.Point();
			Verify(this.vector0, Is.EqualTo(this.vector0));
			Verify(this.vector0.Equals(this.vector0 as object), Is.True);
			Verify(this.vector0 == this.vector0, Is.True);
			Verify(this.vector0 != this.vector1, Is.True);
			Verify(this.vector0 == point, Is.False);
			Verify(point == point, Is.True);
			Verify(point == this.vector0, Is.False);
#pragma warning restore 1718
		}
		#endregion
		#region Arithmetic
		[Test]
		public void Addition()
		{
			Verify((this.vector0 + this.vector1).X, Is.EqualTo(this.vector2.X).Within(this.Precision));
			Verify((this.vector0 + this.vector1).Y, Is.EqualTo(this.vector2.Y).Within(this.Precision));
		}
		[Test]
		public void Subtraction()
		{
			Verify(this.vector0 - this.vector0, Is.EqualTo(new Target.Single.Point()));
		}
		[Test]
		public void ScalarMultiplication()
		{
			Verify((-1) * this.vector0, Is.EqualTo(-this.vector0));
		}
		[Test]
		public void ScalarDivision()
		{
			Verify(this.vector0 / (-1), Is.EqualTo(-this.vector0));
		}
		#endregion
		#region Hash Code
		[Test]
		public void Hash()
		{
			Verify(this.vector0.Hash(), Is.Not.EqualTo(0));
		}
		#endregion

		[Test]
		public void GetValues()
		{
			Verify(this.vector0.X, Is.EqualTo((float)(22.221)).Within(this.Precision), this.prefix + "GetValues.0");
			Verify(this.vector0.Y, Is.EqualTo((float)(-3.1)).Within(this.Precision), this.prefix + "GetValues.1");
		}
		[Test]
		public void Swap()
		{
			Target.Single.Point result = this.vector0.Swap();
			Verify(result.X, Is.EqualTo(this.vector0.Y), this.prefix + "Swap.0");
			Verify(result.Y, Is.EqualTo(this.vector0.X), this.prefix + "Swap.1");
		}
		[Test]
		public void Casting()
		{
			string value = "10, 20";
			Verify(this.CastToString(this.vector3), Is.EqualTo(value), this.prefix + "Casting.0");
			Verify(this.CastFromString(value), Is.EqualTo(this.vector3), this.prefix + "Casting.1");
		}
		[Test]
		public void CastingNull()
		{
			string value = null;
			Target.Single.Point point = new Target.Single.Point();
			Verify(this.CastToString(point), Is.EqualTo(value), this.prefix + "CastingNull.0");
			Verify(this.CastFromString(value), Is.EqualTo(point), this.prefix + "CastingNull.1");
		}

		#region Polar Representation
		[Test]
		public void Polar0()
		{
			Target.Single.Point point = new Target.Single.Point();
			Verify(point.Norm, Is.EqualTo(0));
			Verify(point.Azimuth, Is.EqualTo(0));
		}
		[Test]
		public void Polar1()
		{
			Target.Single.Point point = new Target.Single.Point(1, 0);
			Verify(point.Norm, Is.EqualTo(1));
			Verify(point.Azimuth, Is.EqualTo(0));
		}
		[Test]
		public void Polar2()
		{
			Target.Single.Point point = new Target.Single.Point(0, 1);
			Verify(point.Norm, Is.EqualTo(1));
			Verify(point.Azimuth, Is.EqualTo(Kean.Math.Single.ToRadians(90)));
		}
		[Test]
		public void Polar3()
		{
			Target.Single.Point point = new Target.Single.Point(0, -5);
			Verify(point.Norm, Is.EqualTo(5));
			Verify(point.Azimuth, Is.EqualTo(Kean.Math.Single.ToRadians(-90)));
		}
		[Test]
		public void Polar4()
		{
			Target.Single.Point point = new Target.Single.Point(-1, 0);
			Verify(point.Norm, Is.EqualTo(1));
			Verify(point.Azimuth, Is.EqualTo(Kean.Math.Single.ToRadians(180)));
		}
		[Test]
		public void Polar5()
		{
			Target.Single.Point point = new Target.Single.Point(-3, 0);
			float radius = point.Norm;
			float azimuth = point.Azimuth;
			Target.Single.Point point2 = Target.Single.Point.Polar(radius, azimuth);
			Verify(point.Distance(point2), Is.EqualTo(0).Within(this.Precision));
		}
		#endregion
		[Test]
		public void Angles()
		{
			Verify(Target.Single.Point.BasisX.Angle(Target.Single.Point.BasisX), Is.EqualTo(0).Within(this.Precision));
			Verify(Target.Single.Point.BasisX.Angle(Target.Single.Point.BasisY), Is.EqualTo(Kean.Math.Single.Pi / 2).Within(this.Precision));
			Verify(Target.Single.Point.BasisX.Angle(-Target.Single.Point.BasisY), Is.EqualTo(-Kean.Math.Single.Pi / 2).Within(this.Precision));
			Verify(Target.Single.Point.BasisX.Angle(-Target.Single.Point.BasisX), Is.EqualTo(Kean.Math.Single.Pi).Within(this.Precision));
		}
		[Test]
		public void Casts()
		{
			// integer - single
			{
				Target.Integer.Point integer = new Target.Integer.Point(10, 20);
				Target.Single.Point @single = integer;
				Verify(@single.X, Is.EqualTo(10));
				Verify(@single.Y, Is.EqualTo(20));
				Verify((Target.Integer.Point)@single, Is.EqualTo(integer));
			}
		}
		[Test]
		public void StringCasts()
		{
			string textFromValue = new Target.Single.Point(10, 20);
			Verify(textFromValue, Is.EqualTo("10, 20"));
			Target.Single.Point @integerFromText = (Target.Single.Point)"10, 20";
			Verify(@integerFromText.X, Is.EqualTo(10));
			Verify(@integerFromText.Y, Is.EqualTo(20));
		}
		[Test]
		public void Projection()
		{
			Target.Single.Point point = new Target.Single.Point(0, 0);
			Target.Single.Size size = new Target.Single.Size(640, 480);
			Target.Single.Size fieldOfView = new Target.Single.Size(45f, 45f);
			float zPlane = (float)size.Width / (Math.Single.Tangens(fieldOfView.Width / 2f) * 2f);
			Geometry3D.Single.Transform transform = Geometry3D.Single.Transform.Identity;
			Verify(point.Project(transform, zPlane), Is.EqualTo(point));
			transform = Geometry3D.Single.Transform.CreateTranslation(10, 10, 0);
			Verify(point.Project(transform, zPlane), Is.EqualTo(new Target.Single.Point(10, 10)));
			transform = Geometry3D.Single.Transform.CreateTranslation(10, 10, -zPlane/2);
			Verify(point.Project(transform, zPlane), Is.EqualTo(new Target.Single.Point(20, 20)));
		}
		[Test]
		public void Project()
		{
			var size = new Target.Integer.Size(2, 2);
			var fieldOfView = new Target.Single.Size(90f, 90f);
			float zPlane = ((float)size.Width / 2f) / (Math.Single.Tangens(Kean.Math.Single.ToRadians(fieldOfView.Width) / 2f));
			var cells = new Target.Integer.Size(3, 3);

			var camTransform = Geometry3D.Single.Transform.CreateTranslation(-size.Width / 2f, -size.Height / 2f, 0);
			var transforms = new Kean.Collection.Array.List<Geometry3D.Single.Transform>();
			transforms.Add(Geometry3D.Single.Transform.Identity * camTransform);
			transforms.Add(Geometry3D.Single.Transform.CreateTranslation(size.Width / 2f, 0, 0) * camTransform);
			transforms.Add(Geometry3D.Single.Transform.CreateTranslation(0, size.Height / 2f, 0) * camTransform);
			transforms.Add(Geometry3D.Single.Transform.CreateTranslation(0, 0, -zPlane / 2f) * camTransform);
			transforms.Add(Geometry3D.Single.Transform.CreateRotationX(Kean.Math.Single.ToRadians(45)) * camTransform);
			transforms.Add(Geometry3D.Single.Transform.CreateRotationY(Kean.Math.Single.ToRadians(45)) * camTransform);
			transforms.Add(Geometry3D.Single.Transform.CreateRotationZ(Kean.Math.Single.ToRadians(45)) * camTransform);
			
			string toCSV = "";

			Geometry2D.Single.Point[,] points = new Geometry2D.Single.Point[cells.Area, transforms.Count];
			for (int y = 0; y < cells.Height; y++)
			{
				for (int x = 0; x < cells.Width; x++)
				{
					for (int t = 0; t < transforms.Count; t++)
					{
						points[y * cells.Width + x, t] = new Target.Single.Point((x * size.Width / (cells.Width - 1f)), y * size.Height / (cells.Height - 1f)).Project(transforms[t], zPlane);
						toCSV += points[y * cells.Width + x, t].ToString() + "\t";
					}
					toCSV += "\n";
				}
			}
			var locator = Kean.Uri.Locator.FromPlatformPath("$(Documents)/points");
			toCSV.Save((Uri.Locator.FromPlatformPath(locator.PlatformPath.ToString() + ".Stats.txt")));
		}
		[Test]
		public void ProjectLines()
		{
			var size = new Target.Integer.Size(640, 480);
			var fieldOfView = new Target.Single.Size(45f, 45f);
			float zPlane = ((float)size.Width / 2f) / (Math.Single.Tangens(Kean.Math.Single.ToRadians(fieldOfView.Width) / 2f));
			var cells = new Target.Integer.Size(5, 5);

			var camTransform = Geometry3D.Single.Transform.CreateTranslation(-size.Width / 2f, -size.Height / 2f, 0);
			var transforms = new Kean.Collection.Array.List<Geometry3D.Single.Transform>();
			transforms.Add(Geometry3D.Single.Transform.CreateRotationY(Kean.Math.Single.ToRadians(-15)) * camTransform);
			transforms.Add(Geometry3D.Single.Transform.CreateRotationY(Kean.Math.Single.ToRadians(-10)) * camTransform);
			transforms.Add(Geometry3D.Single.Transform.CreateRotationY(Kean.Math.Single.ToRadians(-5)) * camTransform);
			transforms.Add(Geometry3D.Single.Transform.CreateRotationY(Kean.Math.Single.ToRadians(0)) * camTransform);
			transforms.Add(Geometry3D.Single.Transform.CreateRotationY(Kean.Math.Single.ToRadians(5)) * camTransform);
			transforms.Add(Geometry3D.Single.Transform.CreateRotationY(Kean.Math.Single.ToRadians(10)) * camTransform);
			transforms.Add(Geometry3D.Single.Transform.CreateRotationY(Kean.Math.Single.ToRadians(15)) * camTransform);

			string toCSV = "";

			Geometry3D.Single.Point[,] points = new Geometry3D.Single.Point[cells.Area, transforms.Count];
			for (int y = 0; y < cells.Height; y++)
			{
				for (int x = 0; x < cells.Width; x++)
				{
					for (int t = 0; t < transforms.Count; t++)
					{
						var point = new Target.Single.Point((x * size.Width / (cells.Width - 1f)), y * size.Height / (cells.Height - 1f)).Project(transforms[t], zPlane);
						var point3 = new Geometry3D.Single.Point(point, zPlane);
						//points[y * cells.Width + x, t] = new Geometry3D.Single.Point(point3.RY, point3.RX, point3.Norm);
						toCSV += points[y * cells.Width + x, t].ToString() + "\t";
					}
					toCSV += "\n";
				}
			}
			var locator = Kean.Uri.Locator.FromPlatformPath("$(Documents)/points");
			toCSV.Save((Uri.Locator.FromPlatformPath(locator.PlatformPath.ToString() + ".Stats.txt")));
		}
	}
}

