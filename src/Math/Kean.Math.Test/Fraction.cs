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
            this.Run(this.Constructors, this.Casts, this.Continuous);
        }
        [Test]
        public void Constructors()
        {
            Expect(new Kean.Math.Fraction(3.2).Value, Is.EqualTo(3.2));
            Expect(new Kean.Math.Fraction(3.2).String, Is.EqualTo("3.2"));
            Expect(new Kean.Math.Fraction("3/5").Value, Is.EqualTo(0.6));
            Expect(new Kean.Math.Fraction("3/5").String, Is.EqualTo("3/5"));
        }
        [Test]
        public void Casts()
        {
            Expect((double)new Kean.Math.Fraction(3.2), Is.EqualTo(3.2));
            Expect((string)new Kean.Math.Fraction(3.2).String, Is.EqualTo("3.2"));
            Expect((double)new Kean.Math.Fraction("3/5").Value, Is.EqualTo(0.6));
            Expect((string)new Kean.Math.Fraction("3/5").String, Is.EqualTo("3/5"));
        }
        [Test]
        public void Continuous()
        {
            Kean.Math.Fraction fraction = new Kean.Math.Fraction(3.245, 10, 1e-4);
            Kean.Math.Fraction fraction2 = (Kean.Math.Fraction)"[3; 4, 12, 4]";
            Kean.Math.Fraction fraction3 = (Kean.Math.Fraction)"[3;1,2,1,6,1,2,1,6";
        }
    }
}