using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Math.Test
{
    public class Fraction : 
        Kean.Test.Fixture<Fraction>
    {
        protected override void Run()
        {
            this.Run(this.ConstructorsCastsAndProperties);
        }
        [Test]
        public void ConstructorsCastsAndProperties()
        {
            Expect((Kean.Math.Fraction)("3:5") == (Kean.Math.Fraction)("6:10"), Is.True);
            Expect((Kean.Math.Fraction)("3:5") != (Kean.Math.Fraction)("5:10"), Is.True);
            Expect(((Kean.Math.Fraction)("3:5")).Equals((Kean.Math.Fraction)("6:10")), Is.True);

            Expect((float)(Kean.Math.Fraction)("3.5"), Is.EqualTo(3.5f));
			Expect((float)(Kean.Math.Fraction)("3,5"), Is.EqualTo(3.5));
			Expect(new Kean.Math.Fraction("3:5").Nominator, Is.EqualTo(3));
			Expect(new Kean.Math.Fraction("3:5").Denominator, Is.EqualTo(5));
			Expect((float)new Kean.Math.Fraction(3.245f), Is.EqualTo(3.245f));
			Expect((double)new Kean.Math.Fraction(3.245), Is.EqualTo(3.245));
			Expect((string)new Kean.Math.Fraction(3.245), Is.EqualTo("649:200"));
			Expect(((Kean.Math.Fraction)("3:5")).Nominator, Is.EqualTo(3));
			Expect(((Kean.Math.Fraction)("3:5")).Denominator, Is.EqualTo(5));
			Expect((string)((Kean.Math.Fraction)("3/5")), Is.EqualTo("3:5"));
			Expect((float)(Kean.Math.Fraction)(3.245f), Is.EqualTo(3.245f));
			Expect((double)(Kean.Math.Fraction)(3.245), Is.EqualTo(3.245));
			Expect((string)(Kean.Math.Fraction)(3.245), Is.EqualTo("649:200"));
			Expect(((Kean.Math.Fraction)("1989/867")).GCD, Is.EqualTo(51));
			Expect(((Kean.Math.Fraction)("1989/867")).ContinousFraction, Is.EqualTo("[2;3,2,2]"));
			Expect(new Kean.Math.Fraction("-3:5").Nominator, Is.EqualTo(-3));
			Expect(new Kean.Math.Fraction("3:-5").Denominator, Is.EqualTo(-5));
			Expect((float)new Kean.Math.Fraction(-3.245f), Is.EqualTo(-3.245f));
			Expect((double)new Kean.Math.Fraction(-3.245), Is.EqualTo(-3.245));
			Expect((string)new Kean.Math.Fraction(-3.245), Is.EqualTo("-649:200"));

			Expect(((Kean.Math.Fraction)("-3:5")).Nominator, Is.EqualTo(-3));
			Expect(((Kean.Math.Fraction)("3:-5")).Denominator, Is.EqualTo(-5));
			Expect((string)((Kean.Math.Fraction)("-3/5")), Is.EqualTo("-3:5"));
			Expect((float)(Kean.Math.Fraction)(-3.245f), Is.EqualTo(-3.245f));
			Expect((double)(Kean.Math.Fraction)(-3.245), Is.EqualTo(-3.245));
			Expect((string)(Kean.Math.Fraction)(-3.245), Is.EqualTo("-649:200"));
			Expect(((Kean.Math.Fraction)("-1989/867")).GCD, Is.EqualTo(51));
			Expect(((Kean.Math.Fraction)("-1989/867")).ContinousFraction, Is.EqualTo("[-2;3,2,2]"));
			Expect(((Kean.Math.Fraction)(Kean.Math.Double.SquareRoot(14))).ContinousFraction, Is.EqualTo("[3;1,2,1,6,1,2,1,7]"));
		}
    }
}