using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;

namespace Kean.Test.Math.Geometry3D.Abstract
{
    public abstract class Size<SizeType, R, V> : Vector<SizeType, R, V>
        where SizeType : Kean.Math.Geometry3D.Abstract.Size<SizeType, R, V>, new()
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        [Test]
        public void GetValues()
        {
            Assert.That(this.Vector0.Width, Is.EqualTo(this.Cast(22.221)).Within(this.Precision));
            Assert.That(this.Vector0.Height, Is.EqualTo(this.Cast(-3.1)).Within(this.Precision));
            Assert.That(this.Vector0.Depth, Is.EqualTo(this.Cast(10)).Within(this.Precision));
        }
       
        public void Run()
        {
            this.Run(
                this.Equality,
                this.Addition,
                this.Subtraction,
                this.ScalarMultitplication,
                this.GetValues
                );
        }
    }
}
