using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Extension;
using Target = Kean.Math.Complex.Double;
namespace Kean.Math.Complex.Test
{
    public class Double : AssertionHelper
    {
        class Comparer :
            System.Collections.IComparer
        {
            float tolerance;
            public Comparer(float tolerance)
            {
                this.tolerance = tolerance;
            }
            #region IComparer Members
            public int Compare(object x, object y)
            {
                return ((Target)x).Equals((Target)y, this.tolerance) ? 0 : 1;
            }
            #endregion
        }
        string prefix = "Kean.Math.Complex.Test.Double.";
        [Test]
        public void Constructors()
        {
            Target a = new Target();
            Expect(a.Real, Is.EqualTo(0), this.prefix + "Constructor.0");
            Expect(a.Imaginary, Is.EqualTo(0), this.prefix + "Constructor.1");
            a = new Target(1, 2);
            Expect(a.Real, Is.EqualTo(1), this.prefix + "Constructor.2");
            Expect(a.Imaginary, Is.EqualTo(2), this.prefix + "Constructor.3");
        }
        [Test]
        public void Equality()
        {
            Target a = new Target(1, 2);
            Target b = new Target(2, 3);
            Target c = new Target(1, 2);
            Expect(a, Is.Not.EqualTo(b), this.prefix + "Equality.0");
            Expect(b, Is.Not.EqualTo(a), this.prefix + "Equality.1");
            Expect(a, Is.EqualTo(c), this.prefix + "Equality.2");
            Expect(c, Is.EqualTo(a), this.prefix + "Equality.3");
        }
        [Test]
        public void BasicFunctions()
        {
            Expect(new Target(1, 2).Conjugate, Is.EqualTo(new Target(1, -2)), this.prefix + "BasicFunctions.0");
            Expect(new Target(1, 2).AbsoluteValue, Is.EqualTo(Kean.Math.Double.SquareRoot(5)), this.prefix + "BasicFunctions.1");
        }
        [Test]
        public void Arithmetics()
        {
            Target a = new Target(1, 2);
            Target b = new Target(2, 3);
            Target c = new Target(3, 5);
            Target d = new Target(-1, -1);
            Target e = new Target(-4, 7);
            Target f = new Target(0.615384615384615f, 0.076923076923077f);
            Expect(a + b, Is.EqualTo(c), this.prefix + "Arithmetics.0");
            Expect(a - b, Is.EqualTo(d), this.prefix + "Arithmetics.1");
            Expect(a - a, Is.EqualTo(new Target()), this.prefix + "Arithmetics.2");
            Expect(a * b, Is.EqualTo(e), this.prefix + "Arithmetics.3");
            Expect(a / b, Is.EqualTo(f).Comparer(new Comparer(1e-7f)), this.prefix + "Arithmetics.4");
        }
        [Test]
        public void Functions()
        {
            Target a = new Target(1, 2);
            Target b = new Kean.Math.Complex.Double(-1.131204383756814f, 2.471726672004819f);
            Target c = new Kean.Math.Complex.Double(0.804718956217050f, 1.107148717794090f);
            Expect(Kean.Math.Complex.Double.Exponential(a), Is.EqualTo(b).Comparer(new Comparer(1e-7f)), this.prefix + "Functions.0");
            Expect(Kean.Math.Complex.Double.Logarithm(a), Is.EqualTo(c).Comparer(new Comparer(1e-7f)), this.prefix + "Functions.1");
        }
        [Test]
        public void Casts()
        {
            Target a = 33f;
            Expect(a.Real, Is.EqualTo(33), this.prefix + "Casts.0");
            Expect(a.Imaginary, Is.EqualTo(0), this.prefix + "Casts.1");
            a = "33";
            Expect(a.Real, Is.EqualTo(33), this.prefix + "Casts.2");
            Expect(a.Imaginary, Is.EqualTo(0), this.prefix + "Casts.3");
            a = "33 + 22i";
            Expect(a.Real, Is.EqualTo(33), this.prefix + "Casts.4");
            Expect(a.Imaginary, Is.EqualTo(22), this.prefix + "Casts.5");
            a = "-33 -22*i";
            Expect(a.Real, Is.EqualTo(-33), this.prefix + "Casts.6");
            Expect(a.Imaginary, Is.EqualTo(-22), this.prefix + "Casts.7");
            a = "-22*i-33 ";
            Expect(a.Real, Is.EqualTo(-33), this.prefix + "Casts.8");
            Expect(a.Imaginary, Is.EqualTo(-22), this.prefix + "Casts.9");
            a = "-22f*i+33f ";
            Expect(a.Real, Is.EqualTo(33), this.prefix + "Casts.8");
            Expect(a.Imaginary, Is.EqualTo(-22), this.prefix + "Casts.9");
        }
        public void Run()
        {
            this.Run(
                this.Constructors,
                this.Equality,
                this.BasicFunctions,
                this.Arithmetics,
                this.Functions,
                this.Casts
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
            Double fixture = new Double();
            fixture.Run();
        }
    }
}
