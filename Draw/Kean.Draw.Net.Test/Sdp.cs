// 
//  Sdp.cs
//  
//  Author:
//      Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2012 Anders Frisk
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
using Target = Kean.Draw.Net.Sdp;
namespace Kean.Draw.Net.Test
{
    [TestFixture]
    public class Sdp :
        Kean.Test.Fixture<Sdp>
    {
        string prefix = "Kean.Draw.Net.Test.Sdp.";
        protected override void Run()
        {
            this.Run(
                this.Parse
                );
        }
        [Test]
        public void Parse()
        {
            // "RTSP/1.0 200 OK\r\nCSeq: 0\r\nContent-Type: application/sdp\r\nContent-Base: rtsp://192.168.1.21:554/axis-media/media.amp/\r\nDate: Wed, 05 Jan 2011 22:08:21 GMT\r\nContent-Length: 342\r\n\r\nv=0\r\no=- 1294265301839884 1294265301839884 IN IP4 192.168.1.21\r\ns=Media Presentation\r\ne=NONE\r\nc=IN IP4 0.0.0.0\r\nt=0 0\r\na=control:rtsp://192.168.1.21:554/axis-media/media.amp?videocodec=jpeg\r\na=range:npt=0.000000-\r\nm=video 0 RTP/AVP 26\r\na=framesize:26 640-480\r\na=control:rtsp://192.168.1.21:554/axis-media/media.amp/trackID=1?videocodec=jpeg\r\n"
            string description = "v=0\r\no=- 1294265301839884 1294265301839884 IN IP4 192.168.1.21\r\ns=Media Presentation\r\ne=NONE\r\nc=IN IP4 0.0.0.0\r\nt=0 0\r\na=control:rtsp://192.168.1.21:554/axis-media/media.amp?videocodec=jpeg\r\na=range:npt=0.000000-\r\nm=video 0 RTP/AVP 26\r\na=framesize:26 640-480\r\na=control:rtsp://192.168.1.21:554/axis-media/media.amp/trackID=1?videocodec=jpeg\r\n";
            Target.Protocol protocol = description;
            Console.WriteLine();
            Console.WriteLine(protocol.ToString());
        }
    }
}
