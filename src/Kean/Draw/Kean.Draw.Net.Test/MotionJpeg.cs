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
using Target = Kean.Draw.Net.Http;

namespace Kean.Draw.Net.Test
{
    [TestFixture]
    public class MotionJpeg :
        Kean.Test.Fixture<MotionJpeg>
    {
        string prefix = "Kean.Draw.Net.MotionJpeg.";
        protected override void Run()
        {
            this.Run(
                this.Open
                );
        }
        [Test]
        public void Open()
        {
            //Target.MotionJpeg multi = new Kean.Draw.Net.MotionJpeg("http://192.168.1.21/mjpg/video.mjpg");
            Target.MotionJpeg multi = new Target.MotionJpeg("http://194.218.96.90/axis-cgi/mjpg/video.cgi?resolution=480x360");
            int counter = 0;
            multi.NewFrame += (Raster.Image image) =>
            {
                counter++;
                Console.Write(".");
            };
            multi.Start();
            while (counter < 10)
                System.Threading.Thread.Sleep(100);
            multi.Stop();
        }
    }

}
