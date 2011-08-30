using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Extension;
using Kean.Core;

namespace Kean.Math.Regression.Test.Interpolation.Splines
{
    public class Geometry2D :
        AssertionHelper
    {
        string prefix = "Kean.Math.Regression.Test.Interpolation.Splines.Geometry2D.";
        [Test]
        public void PointValue()
        {
            Kean.Math.Regression.Interpolation.Splines.Geometry2D.Double.PointValue interpolate = new Kean.Math.Regression.Interpolation.Splines.Geometry2D.Double.PointValue(Kean.Math.Regression.Interpolation.Splines.Method.Periodic);
            interpolate.Measures = new Tuple<double, Kean.Math.Geometry2D.Double.PointValue>[] 
                { 
                   Tuple.Create<double, Kean.Math.Geometry2D.Double.PointValue>(0,new Kean.Math.Geometry2D.Double.PointValue(10, 0)), 
                   Tuple.Create<double, Kean.Math.Geometry2D.Double.PointValue>(1,new Kean.Math.Geometry2D.Double.PointValue(0, 10)), 
                   Tuple.Create<double, Kean.Math.Geometry2D.Double.PointValue>(2, new Kean.Math.Geometry2D.Double.PointValue(-10, 0)), 
                   Tuple.Create<double, Kean.Math.Geometry2D.Double.PointValue>(3, new Kean.Math.Geometry2D.Double.PointValue(0, -10)), 
                   Tuple.Create<double, Kean.Math.Geometry2D.Double.PointValue>(4, new Kean.Math.Geometry2D.Double.PointValue(10, 0)) 
                };
            Kean.Core.Collection.IList<Kean.Math.Geometry2D.Double.PointValue> points = new Kean.Core.Collection.List<Kean.Math.Geometry2D.Double.PointValue>();
            int count = 100;
            for (int i = 0; i <= count; i++)
            {
                double time = i / (double)count * 4;
                points.Add(interpolate.Interpolate(time));
            }
            System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
            file.WriteLine("clear all;");
            file.WriteLine("close all;");
            string pointsExport = "";
            foreach (Kean.Math.Geometry2D.Double.PointValue point in points)
                pointsExport += (string)point + ";";
            pointsExport = pointsExport.TrimEnd(';');
            file.WriteLine("points = [" + pointsExport + "];");
            file.WriteLine("plot(points(:,1), points(:,2),'r');");
            file.Close();
        }
        public void Run()
        {
            this.Run(
                this.PointValue
                );
        }
        internal void Run(params System.Action[] tests)
        {
            foreach (System.Action test in tests)
                if (test.NotNull())
                    test();
        }
        public static void Test()
        {
            Geometry2D fixture = new Geometry2D();
            fixture.Run();
        }
    }
}
