// 
//  Decompressor.cs
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
using Target = Kean.Draw.Jpeg;
namespace Kean.Draw.Jpeg.Test
{
    [TestFixture]
    public class Decompressor :
        Kean.Test.Fixture<Decompressor>
    {
        protected override void Run()
        {
            this.Run(
                this.Open
                );
        }
        [Test]
        public void Open()
        {
            using (Target.Decompressor decompressor = new Target.Decompressor())
            {
                Raster.Image image = decompressor.OpenResource("Data/original.jpg");
                image.Save("test2.png");
            }
        }
    }


}
