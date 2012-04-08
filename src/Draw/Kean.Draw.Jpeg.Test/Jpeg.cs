// 
//  Jpeg.cs
//  
//  Author:
//      Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
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
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Draw.Jpeg;
namespace Kean.Draw.Jpeg.Test
{
    [TestFixture]
    public class Jpeg :
        Kean.Test.Fixture<Jpeg>
    {
        string prefix = "Kean.Draw.Jpeg.";
        protected override void Run()
        {
            this.Run(
                this.Save,
                this.Open
                );
        }
        [Test]
        public void Open()
        {
            Raster.Image image = Target.Decompress.OpenResource("Data/original.jpg");
            image.Save("test2.png");
        }
        [Test]
        public void Save()
        {
            Raster.Image image = Target.Decompress.OpenResource("Data/original.jpg");
            Target.Compress.Save(image);
        }
    }


}
