using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;

namespace Kean.Test.Math
{
    public abstract class Abstract<R, V> :
        AssertionHelper
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        public R A { get; set; }
        public R B { get; set; }
      
        [Test]
        public void Constructors()
        {
            R r = Kean.Math.Abstract<R, V>.One;
            Expect(r.Value, Is.EqualTo(1));
            r = new R();
            Expect(r.Value, Is.EqualTo(0));
        }
        [Test]
        public void Equality()
        {
            R r = Kean.Math.Abstract<R, V>.One;
            R s = Kean.Math.Abstract<R, V>.One;
            R t = Kean.Math.Abstract<R, V>.Two;
            R u = null;
            Expect(r.Equals(s), Is.True);
            Expect(r.Equals(s as object), Is.True);
            Expect(r == s, Is.True);
            Expect(r != t, Is.True);
            Expect(r == u, Is.False);
            Expect(u == u, Is.True);
            Expect(u == r, Is.False);
        }
        [Test]
        public void Copy()
        {
            Expect(this.A, Is.EqualTo(this.A.Copy()));
        }
        [Test]
        public void Arithmetics()
        {
            R zero = new R();
            R one = Kean.Math.Abstract<R, V>.One;
            R two = Kean.Math.Abstract<R, V>.Two;
            Expect(one - one, Is.EqualTo(zero));
            Expect(one + one, Is.EqualTo(two));
            Expect((two * (-two)).Value, Is.EqualTo(-4));
            Expect((two / (-two)).Value, Is.EqualTo(-1));
        }
        [Test]
        public void Absolute()
        {
            Expect(Kean.Math.Abstract<R, V>.Absolute(Kean.Math.Abstract<R, V>.Two), Is.EqualTo(Kean.Math.Abstract<R, V>.Two));
            Expect(Kean.Math.Abstract<R, V>.Absolute(-Kean.Math.Abstract<R, V>.Two), Is.EqualTo(Kean.Math.Abstract<R, V>.Two));
        }
        [Test]
        public void Minimum()
        {
            R zero = new R();
            R one = Kean.Math.Abstract<R, V>.One;
            R two = Kean.Math.Abstract<R, V>.Two;
            Expect(Kean.Math.Abstract<R, V>.Minimum(two), Is.EqualTo(two));
            Expect(Kean.Math.Abstract<R, V>.Minimum(zero, -one, two), Is.EqualTo(-one));
        }
        [Test]
        public void Maximum()
        {
            R zero = new R();
            R one = Kean.Math.Abstract<R, V>.One;
            R two = Kean.Math.Abstract<R, V>.Two;
            Expect(Kean.Math.Abstract<R, V>.Maximum(two), Is.EqualTo(two));
            Expect(Kean.Math.Abstract<R, V>.Maximum(zero, -one, two), Is.EqualTo(two));
        }
        [Test]
        public void Compare()
        {
            Expect(Kean.Math.Abstract<R, V>.One.LessThan(Kean.Math.Abstract<R, V>.Two), Is.True);
            Expect(Kean.Math.Abstract<R, V>.One.LessOrEqualThan(Kean.Math.Abstract<R, V>.Two), Is.True);
            Expect(Kean.Math.Abstract<R,V>.Two.LessOrEqualThan(Kean.Math.Abstract<R, V>.Two), Is.True);
            Expect(Kean.Math.Abstract<R,V>.Two.LessThan(Kean.Math.Abstract<R, V>.One), Is.False);
            Expect(Kean.Math.Abstract<R,V>.Two.GreaterThan(Kean.Math.Abstract<R, V>.One), Is.True);
            Expect(Kean.Math.Abstract<R,V>.Two.GreaterOrEqualThan(Kean.Math.Abstract<R, V>.One), Is.True);
            Expect(Kean.Math.Abstract<R,V>.Two.GreaterOrEqualThan(Kean.Math.Abstract<R, V>.Two), Is.True);
            Expect(Kean.Math.Abstract<R,V>.Two.GreaterOrEqualThan(Kean.Math.Abstract<R, V>.One), Is.True);
        }
        [Test]
        public void Clamp()
        {
            R zero = new R();
            R one = Kean.Math.Abstract<R, V>.One;
            R two = Kean.Math.Abstract<R, V>.Two;
            Expect(two.Clamp(zero, one), Is.EqualTo(one));
            Expect(one.Clamp(zero, two), Is.EqualTo(one));
            Expect(zero.Clamp(one, two), Is.EqualTo(one));
        }
        [Test]
        public void Logarithm()
        {
            R two = Kean.Math.Abstract<R, V>.Two;
            Expect(two.Logarithm(two).Value, Is.EqualTo(1));
        }
        public void Run()
        {
            this.Run(
                this.Constructors,
                this.Equality,
                this.Copy,
                this.Arithmetics,
                this.Absolute,
                this.Minimum,
                this.Maximum,
                this.Compare,
                this.Clamp,
                this.Logarithm
            );
        }
        internal void Run(params System.Action[] tests)
        {
            foreach (System.Action test in tests)
                if (test.NotNull())
                    test();
        }
    }
}
