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
            R r = new R().One;
            Assert.That(r.Value, Is.EqualTo(1));
            r = new R();
            Assert.That(r.Value, Is.EqualTo(0));
        }
        [Test]
        public void Equality()
        {
            R r = new R().One;
            R s = new R().One;
            R t = new R().Two;
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
            R one = new R().One;
            R two = new R().Two;
            Assert.That(one - one, Is.EqualTo(zero));
            Assert.That(one + one, Is.EqualTo(two));
            Assert.That((two * (-two)).Value, Is.EqualTo(-4));
            Assert.That((two / (-two)).Value, Is.EqualTo(-1));
        }
        [Test]
        public void MinimumMaximum()
        {
            R zero = new R();
            R one = new R().One;
            R two = new R().Two;
            Assert.That(Kean.Math.Abstract<R, V>.Maximum(zero, -one, two), Is.EqualTo(two));
            Assert.That(Kean.Math.Abstract<R, V>.Minimum(zero, -one, two), Is.EqualTo(-one));
         }

        public void Run()
        {
            this.Run(
                this.Constructors,
                this.Equality,
                this.Copy,
                this.Arithmetics,
                this.MinimumMaximum
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
