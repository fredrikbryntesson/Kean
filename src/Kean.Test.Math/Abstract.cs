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
        }
        [Test]
        public void Equality()
        {
            R r = new R().One;
            R s = new R().One;
            R t = new R().Two;
            Assert.That(r.Equals(s), Is.True);
            Assert.That(r.Equals(s as object), Is.True);
            Assert.That(r == s, Is.True);
            Assert.That(r != t, Is.True);
        }
        [Test]
        public void Copy()
        {
            Assert.That(this.A, Is.EqualTo(this.A.Copy()));
        }
        public void Run()
        {
            this.Run(
                this.Constructors,
                this.Equality,
                this.Copy
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
