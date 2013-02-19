using System;
using NUnit.Framework;

using Kean.Core.Extension;
using Kean.Core;

namespace Kean.Math.Regression.Test.Interpolation.Splines
{
    public class Geometry2D :
        Kean.Test.Fixture<Geometry2D>
    {
        protected override void Run()
        {
            this.Run(
                this.PointValue
                );
        }
        string prefix = "Kean.Math.Regression.Test.Interpolation.Splines.Geometry2D.";
        [Test]
        public void PointValue()
        {
            Kean.Math.Regression.Interpolation.Splines.Geometry2D.Single.PointValue interpolate = new Kean.Math.Regression.Interpolation.Splines.Geometry2D.Single.PointValue(Kean.Math.Regression.Interpolation.Splines.Method.Periodic);
            interpolate.Measures = new Tuple<float, Kean.Math.Geometry2D.Single.PointValue>[] 
                { 
                   Tuple.Create<float, Kean.Math.Geometry2D.Single.PointValue>(0,new Kean.Math.Geometry2D.Single.PointValue(10, 0)), 
                   Tuple.Create<float, Kean.Math.Geometry2D.Single.PointValue>(1,new Kean.Math.Geometry2D.Single.PointValue(0, 10)), 
                   Tuple.Create<float, Kean.Math.Geometry2D.Single.PointValue>(2, new Kean.Math.Geometry2D.Single.PointValue(-10, 0)), 
                   Tuple.Create<float, Kean.Math.Geometry2D.Single.PointValue>(3, new Kean.Math.Geometry2D.Single.PointValue(0, -10)), 
                   Tuple.Create<float, Kean.Math.Geometry2D.Single.PointValue>(4, new Kean.Math.Geometry2D.Single.PointValue(10, 0)) 
                };
            int count = 100;
            for (int i = 0; i < count; i++)
            {
                float time = i / (float)count;
                Kean.Math.Geometry2D.Single.PointValue value = interpolate.Interpolate(time * 4);
                Kean.Math.Geometry2D.Single.PointValue onCircle = new Kean.Math.Geometry2D.Single.PointValue( 10 * Kean.Math.Single.Cosinus(Kean.Math.Single.Pi * 2 * time), 10 * Kean.Math.Single.Sinus(Kean.Math.Single.Pi * 2 * time));
                float distance = onCircle.Distance(value);
                Expect(distance, Is.LessThan(0.5f), this.prefix + "PointValue.0");
            }
            /*
            Kean.Core.Collection.IList<Kean.Math.Geometry2D.Double.PointValue> points = new Kean.Core.Collection.List<Kean.Math.Geometry2D.Double.PointValue>();
            int count = 100;
            for (int i = 0; i <= count; i++)
            {
                float time = i / (float)count * 4;
                points.Add(interpolate.Interpolate(time));
            }
            System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
            file.WriteLine("clear all;");
            file.WriteLine("close all;");
            string pointsExport = "";
            foreach (Kean.Math.Geometry2D.Single.PointValue point in points)
                pointsExport += (string)point + ";";
            pointsExport = pointsExport.TrimEnd(';');
            file.WriteLine("points = [" + pointsExport + "];");
            file.WriteLine("plot(points(:,1), points(:,2),'r');");
            file.WriteLine("axis equal;");
            file.Close();
            */
        }
    }
}
