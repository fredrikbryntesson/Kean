// 
//  Single.cs
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
using Target = Kean.Draw.Net;

namespace Kean.Draw.Net.Test
{
    [TestFixture]
    public class Jpeg :
        Kean.Test.Fixture<Jpeg>
    {
        string prefix = "Kean.Draw.Net.Single.";
        protected override void Run()
        {
            this.Run(
                this.Open
                );
        }
        [Test]
        public void Open()
        {
            //Target.Jpeg single = new Kean.Draw.Net.Jpeg("http://192.168.1.21/axis-cgi/jpg/image.cgi?resolution=640x480");
            Target.Jpeg single = new Kean.Draw.Net.Jpeg("http://194.218.96.90/axis-cgi/jpg/image.cgi?resolution=480x360");
            int counter = 0;
            single.NewFrame += (image) => 
            {
                counter++;
                //image.Save("test.jpg");
                Console.WriteLine("new image " + counter);
            };
            single.Start();
            while(counter < 20)
                System.Threading.Thread.Sleep(100);
            single.Stop();
        }
    }

}
