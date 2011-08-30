// 
//  Main.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009 Simon Mika
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
//  You should have received data copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;

namespace Kean.Test.Run
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Started");
            Xml.Dom.Test.Open.Test();

            Kean.Test.Math.Single.Test();
            Kean.Test.Math.Double.Test();
            Kean.Math.Complex.Test.Single.Test();
            Kean.Math.Complex.Test.Double.Test();
            Kean.Math.Complex.Test.Fourier.Single.Test();
            Kean.Math.Complex.Test.Fourier.Double.Test();

            Kean.Math.Geometry2D.Test.Integer.Point.Test();
            Kean.Math.Geometry2D.Test.Single.Point.Test();
            Kean.Math.Geometry2D.Test.Double.Point.Test();

            Kean.Math.Geometry2D.Test.Integer.Size.Test();
            Kean.Math.Geometry2D.Test.Single.Size.Test();
            Kean.Math.Geometry2D.Test.Double.Size.Test();
            
            Kean.Math.Geometry2D.Test.Integer.Box.Test();
            Kean.Math.Geometry2D.Test.Single.Box.Test();
            Kean.Math.Geometry2D.Test.Double.Box.Test();
            
            Kean.Math.Geometry2D.Test.Integer.Transform.Test();
            Kean.Math.Geometry2D.Test.Single.Transform.Test();
            Kean.Math.Geometry2D.Test.Double.Transform.Test();

            Kean.Math.Geometry3D.Test.Integer.Point.Test();
            Kean.Math.Geometry3D.Test.Single.Point.Test();
            Kean.Math.Geometry3D.Test.Double.Point.Test();

            Kean.Math.Geometry3D.Test.Integer.Size.Test();
            Kean.Math.Geometry3D.Test.Single.Size.Test();
            Kean.Math.Geometry3D.Test.Double.Size.Test();
          
            Kean.Math.Geometry3D.Test.Integer.Box.Test();
            Kean.Math.Geometry3D.Test.Single.Box.Test();
            Kean.Math.Geometry3D.Test.Double.Box.Test();

            Kean.Math.Matrix.Test.Single.Test();
            Kean.Math.Matrix.Test.Algorithms.Single.Test();
            Kean.Math.Matrix.Test.Double.Test();
            Kean.Math.Matrix.Test.Algorithms.Double.Test();

            Kean.Math.Geometry3D.Test.Single.Transform.Test();
            Kean.Math.Geometry3D.Test.Double.Transform.Test();
           
            Kean.Math.Geometry3D.Test.Double.Quaternion.Test();

            Kean.Math.Random.Test.Generator.Test();
            Kean.Math.Regression.Test.Ransac.Double.Test();
            Kean.Math.Regression.Test.Ransac.Single.Test();
            Kean.Math.Regression.Test.Minimization.Single.Test();
            Kean.Math.Regression.Test.Minimization.Double.Test();
            Kean.Math.Regression.Test.Interpolation.Splines.Geometry2D.Test();
			
            Kean.Core.Collection.Test.Vector.Test();
			Kean.Core.Collection.Test.List.Test();
			Kean.Core.Collection.Test.Queue.Test();
			Kean.Core.Collection.Test.Stack.Test();
			Kean.Core.Collection.Test.Dictionary.Test();
			Kean.Core.Collection.Test.Linked.List.Test();
			Kean.Core.Collection.Test.Linked.Queue.Test();
			Kean.Core.Collection.Test.Linked.Stack.Test();
			Kean.Core.Collection.Test.Array.Vector.Test();
			Kean.Core.Collection.Test.Array.List.Test();
			Kean.Core.Collection.Test.Array.Queue.Test();
			Kean.Core.Collection.Test.Array.Stack.Test();
			Kean.Core.Collection.Test.Sorted.List.Test();

			//Kean.Core.Error.Test.Error.Test();
			Console.WriteLine("Done");
		}
	}
}