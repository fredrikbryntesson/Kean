using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;

namespace Kean.Test.Math
{
    public abstract class Abstract<R, V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        public R A { get; set; }
        public R B { get; set; }
      
        [Test]
        public void Constructors()
        {
            R r = Kean.Math.Abstract<R, V>.One;
            Assert.That(r.Value, Is.EqualTo(1));
            r = new R();
            Assert.That(r.Value, Is.EqualTo(0));
        }
        [Test]
        public void Equality()
        {
            R r = Kean.Math.Abstract<R, V>.One;
            R s = Kean.Math.Abstract<R, V>.One;
            R t = Kean.Math.Abstract<R, V>.Two;
            R u = null;
            Assert.That(r.Equals(s), Is.True);
            Assert.That(r.Equals(s as object), Is.True);
            Assert.That(r == s, Is.True);
            Assert.That(r != t, Is.True);
            Assert.That(r == u, Is.False);
            Assert.That(u == u, Is.True);
            Assert.That(u == r, Is.False);
        }
        [Test]
        public void Copy()
        {
            Assert.That(this.A, Is.EqualTo(this.A.Copy()));
        }
        [Test]
        public void Arithmetics()
        {
            R zero = new R();
            R one = Kean.Math.Abstract<R, V>.One;
            R two = Kean.Math.Abstract<R, V>.Two;
            Assert.That(one - one, Is.EqualTo(zero));
            Assert.That(one + one, Is.EqualTo(two));
            Assert.That((two * (-two)).Value, Is.EqualTo(-4));
            Assert.That((two / (-two)).Value, Is.EqualTo(-1));
        }
        [Test]
        public void Absolute()
        {
            Assert.That(Kean.Math.Abstract<R, V>.Absolute(Kean.Math.Abstract<R, V>.Two), Is.EqualTo(Kean.Math.Abstract<R, V>.Two));
            Assert.That(Kean.Math.Abstract<R, V>.Absolute(-Kean.Math.Abstract<R, V>.Two), Is.EqualTo(Kean.Math.Abstract<R, V>.Two));
        }
        [Test]
        public void Minimum()
        {
            R zero = new R();
            R one = Kean.Math.Abstract<R, V>.One;
            R two = Kean.Math.Abstract<R, V>.Two;
            Assert.That(Kean.Math.Abstract<R, V>.Minimum(), Is.EqualTo(Kean.Math.Abstract<R, V>.PlusInfinity));
            Assert.That(Kean.Math.Abstract<R, V>.Minimum(two), Is.EqualTo(two));
            Assert.That(Kean.Math.Abstract<R, V>.Minimum(zero, -one, two), Is.EqualTo(-one));
        }
        [Test]
        public void Maximum()
        {
            R zero = new R();
            R one = Kean.Math.Abstract<R, V>.One;
            R two = Kean.Math.Abstract<R, V>.Two;
            Assert.That(Kean.Math.Abstract<R, V>.Maximum(), Is.EqualTo(Kean.Math.Abstract<R, V>.MinusInfinity));
            Assert.That(Kean.Math.Abstract<R, V>.Maximum(two), Is.EqualTo(two));
            Assert.That(Kean.Math.Abstract<R, V>.Maximum(zero, -one, two), Is.EqualTo(two));
        }
        [Test]
        public void Compare()
        {
            Assert.That(Kean.Math.Abstract<R, V>.One.LessThan(Kean.Math.Abstract<R, V>.Two), Is.True);
            Assert.That(Kean.Math.Abstract<R, V>.One.LessOrEqualThan(Kean.Math.Abstract<R, V>.Two), Is.True);
            Assert.That(Kean.Math.Abstract<R,V>.Two.LessOrEqualThan(Kean.Math.Abstract<R, V>.Two), Is.True);
            Assert.That(Kean.Math.Abstract<R,V>.Two.LessThan(Kean.Math.Abstract<R, V>.One), Is.False);
            Assert.That(Kean.Math.Abstract<R,V>.MinusInfinity.LessThan(new R()), Is.True);
            Assert.That(Kean.Math.Abstract<R,V>.Two.GreaterThan(Kean.Math.Abstract<R, V>.One), Is.True);
            Assert.That(Kean.Math.Abstract<R,V>.Two.GreaterOrEqualThan(Kean.Math.Abstract<R, V>.One), Is.True);
            Assert.That(Kean.Math.Abstract<R,V>.Two.GreaterOrEqualThan(Kean.Math.Abstract<R, V>.Two), Is.True);
            Assert.That(Kean.Math.Abstract<R,V>.Two.GreaterOrEqualThan(Kean.Math.Abstract<R, V>.One), Is.True);
            Assert.That(Kean.Math.Abstract<R, V>.PlusInfinity.GreaterThan(new R()), Is.True);
            Assert.That(Kean.Math.Abstract<R,V>.PlusInfinity.GreaterThan(Kean.Math.Abstract<R, V>.MinusInfinity), Is.True);
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
                this.Compare

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
