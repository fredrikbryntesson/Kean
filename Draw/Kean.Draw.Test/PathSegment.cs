using System;
using System.Collections.Generic;
using Geometry2D = Kean.Math.Geometry2D;

using NUnit.Framework;

using Target = Kean.Draw.Color;

namespace Kean.Draw.Test
{
    public class PathSegment:
        Kean.Test.Fixture<PathSegment>
    {
        protected override void Run()
        {
            this.Run(
                this.Close
                );
        }
        [Test]
        public void Close()
        {
            Path path = new Path().Close();
            Verify(path.ToString(), Is.EqualTo("Z"));
        }
        [Test]
        public void CurveTo()
        {
        }
        [Test]
        public void EllipticalArcTo()
        {
        }
        [Test]
        public void LineTo()
        {
        }
        [Test]
        public void MoveTo()
        {
        }
    }
}
