using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;
using Target = Kean.Math.Matrix.Single;

namespace Kean.Test.Math.Matrix.Algorithms
{
    public class Single :
        AssertionHelper
    {
        string prefix = "Kean.Test.Math.Matrix.Algorithms.";
        [Test]
        public void Cholesky()
        {
            Target a = new Kean.Math.Matrix.Single(5,5, new float[] {1,1,1,1,1,1,2,3,4,5,1,3,6,10,15,1,4,10,20,35,1,5,15,35,70} );
            Target c = a.Cholesky();
            Expect(c * c.Transpose(), Is.EqualTo(a), this.prefix + "Cholesky.0");
            Target d = c.Transpose();
            Expect(d.Transpose() * d, Is.EqualTo(a), this.prefix + "Cholesky.1");
        }
        [Test]
        public void LeastSquare1()
        {
            Target a = new Kean.Math.Matrix.Single(5, 5, new float[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target y = new Kean.Math.Matrix.Single(1, 5, new float[] { -1, 2, -3, 4, 5 });
            Target correct = new Kean.Math.Matrix.Single(1, 5, new float[] { -70, 231, -296, 172, -38 }); 
            Target x = a.LeastSquare(y);
            Expect(x, Is.EqualTo(correct), this.prefix + "LeastSquare.0");
        }
        public void Run()
        {
            this.Run(
                this.Cholesky,
                this.LeastSquare1
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
            Single fixture = new Single();
            fixture.Run();
        }
    }
}
