// 
//  Open.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Kean.Core;
using Kean.Core.Extension;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
namespace Kean.Xml.Dom.Test
{
    [TestFixture]
    public class Open :
        Kean.Test.Fixture<Open>
    {
        Factory factory;
        public override void Setup()
        {
            base.Setup();
            this.factory = new Factory();
        }
        [Test]
        public void Valid001()
        {
            Expect(this.factory.Open("valid001"), Is.EqualTo(this.factory.Create("valid001")));
        }
        [Test]
        public void Valid002()
        {
            Expect(this.factory.Open("valid002"), Is.EqualTo(this.factory.Create("valid002")));
        }
        [Test]
        public void Valid003()
        {
            Expect(this.factory.Open("valid003"), Is.EqualTo(this.factory.Create("valid003")));
        }
        [Test]
        public void Valid004()
        {
            Expect(this.factory.Open("valid004"), Is.EqualTo(this.factory.Create("valid004")));
        }
        [Test]
        public void Valid005()
        {
            Expect(this.factory.Open("valid005"), Is.EqualTo(this.factory.Create("valid005")));
        }
        [Test]
        public void Valid006()
        {
            Expect(this.factory.Open("valid006"), Is.EqualTo(this.factory.Create("valid006")));
        }
        [Test]
        public void Valid007()
        {
            Expect(this.factory.Open("valid007"), Is.EqualTo(this.factory.Create("valid007")));
        }
        protected override void Run()
        {
            this.Run(
                this.Valid001,
                this.Valid002,
                this.Valid003,
                this.Valid004,
                this.Valid005,
                this.Valid006,
                this.Valid007
                );
        }
    }
}

