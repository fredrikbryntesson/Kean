using System;
using NUnit.Framework;
using NUnit.Framework;
using Kean.Core.Extension;

namespace Kean.Math.Geometry3D.Test.Double
{
    [TestFixture]
    public class Transform :
        Kean.Math.Geometry3D.Test.Abstract.Transform<Transform, Kean.Math.Geometry3D.Double.Transform, Kean.Math.Geometry3D.Double.TransformValue, Kean.Math.Geometry3D.Double.Point, Kean.Math.Geometry3D.Double.PointValue, Kean.Math.Geometry3D.Double.Size, Kean.Math.Geometry3D.Double.SizeValue, Kean.Math.Double, double>
    {
        protected override Kean.Math.Geometry3D.Double.Transform CastFromString(string value)
        {
            return (Kean.Math.Geometry3D.Double.Transform)value;
        }
        protected override string CastToString(Kean.Math.Geometry3D.Double.Transform value)
        {
            return (string)value;
        }
        [TestFixtureSetUp]
        public override void Setup()
        {
            this.Transform0 = new Kean.Math.Geometry3D.Double.Transform(-1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            this.Transform1 = new Kean.Math.Geometry3D.Double.Transform(-1, 2, 3, 4, 5, 6, 7, 8, -5, 10, 11, 12);
            this.Transform2 = new Kean.Math.Geometry3D.Double.Transform(30, 32, 36, 58, 81, 96, -10, 14, 24, 128, 182, 216);
            this.Transform3 = new Kean.Math.Geometry3D.Double.Transform(-0.5f, 1, -0.5f, 1, -5, 3, -0.5f, 3.66666666666666f, -2.16666666666667f, 0, 1, -2);
            this.Point0 = new Kean.Math.Geometry3D.Double.Point(34, 10, 30);
            this.Point1 = new Kean.Math.Geometry3D.Double.Point(226, 369, 444);
        }
        protected override void Run()
        {
            base.Run();
            this.Run(
                this.InverseTransform1,
                this.InverseTransform2,
                this.InverseTransform3
                );
        }
        protected override double Cast(double value)
        {
            return (double)value;
        }
        [Test]
        public void InverseTransform1()
        {

            Kean.Math.Geometry3D.Double.Transform transform = 
                new Kean.Math.Geometry3D.Double.Transform(0.035711678574190f, 0.849129305868777f, 0.933993247757551f, 0.678735154857773f, 0.757740130578333f, 0.743132468124916f, 0.392227019534168f, 0.655477890177557f,   0.171186687811562f, 0.706046088019609f, 0.031832846377421f, 0.276922984960890f);
            Kean.Math.Geometry3D.Double.Transform transformInverseCorrect = new Kean.Math.Geometry3D.Double.Transform(-1.304260393891308f, 1.703723523873863f, -0.279939209639535f, 0.639686782697661f, -1.314595978968342f, 2.216619899417434f, 0.538976631155336f, 1.130007253038916f, -2.004511083979782f, 0.751249880258891f, -1.473984978790241f, 0.682183855876876f);
                
            Kean.Math.Matrix.Double transformInverseMatrix1 = (Kean.Math.Matrix.Double)(double[,])transform.Inverse;
            Kean.Math.Matrix.Double transformInverseMatrix2 = (Kean.Math.Matrix.Double)(double[,])transformInverseCorrect;
            Expect(transformInverseMatrix1.Distance(transformInverseMatrix2), Is.LessThan(0.00001));
        }
        [Test]
        public void InverseTransform2()
        {
            Kean.Math.Geometry3D.Double.Transform transform =
                new Kean.Math.Geometry3D.Double.Transform(-0.829516518295923f, -0.207909983627822f, -0.518339449185655f, -0.176317665787488f, 0.978147963606672f, -0.110175505550793f, 0.529919264233205f, 0, -0.848048096156426f, 0, 0, 0);
            Kean.Math.Geometry3D.Double.Transform transformInverseCorrect =
                new Kean.Math.Geometry3D.Double.Transform(-0.829516518295923f, -0.176317665787488f, 0.529919264233205f, -0.207909983627822f, 0.978147963606672f, -1.15118542239879e-17f, -0.518339449185655f, -0.110175505550793f, -0.848048096156426f, 0, 0 ,0);
            Kean.Math.Matrix.Double transformInverseMatrix1 = (Kean.Math.Matrix.Double)(double[,])transform.Inverse;
            Kean.Math.Matrix.Double transformInverseMatrix2 = (Kean.Math.Matrix.Double)(double[,])transformInverseCorrect;
            Expect(transformInverseMatrix1.Distance(transformInverseMatrix2), Is.LessThan(0.0000001));
        }
        [Test]
        public void InverseTransform3()
        {
            Kean.Math.Matrix.Double transformMatrix = (Kean.Math.Matrix.Double)(double[,])this.Transform0;
            Kean.Math.Matrix.Double transformInverseMatrix1 = transformMatrix.Inverse();
            Kean.Math.Matrix.Double transformInverseMatrix2 = (Kean.Math.Matrix.Double)(double[,])(this.Transform0.Inverse);
            Expect(transformInverseMatrix1.Distance(transformInverseMatrix2), Is.LessThan(0.0000001));
        }
    }
}
