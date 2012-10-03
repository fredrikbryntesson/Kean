// 
//  Rtsp.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
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
using Kean.Core;
using Kean.Core.Extension;
using Uri = Kean.Core.Uri;

namespace Kean.Draw.Net.Protocol
{
    public class Rtsp
    {
        public int Port { get; private set; }
        int counter = 0;
        Uri.Locator locator;
        Action<string> sender;
        Func<string> reciever;
        Uri.Locator video;
        string session;
        public Rtsp(Uri.Locator locator, Action<string> sender, Func<string> reciever)
        {
            this.locator = locator;    
            this.sender = sender;
            this.reciever = reciever;
        }
        // "RTSP/1.0 200 OK\r\nCSeq: 0\r\nPublic: DESCRIBE, GET_PARAMETER, PAUSE, PLAY, SETUP, SET_PARAMETER, TEARDOWN\r\nDate: Wed, 12 Jan 2011 05:28:33 GMT\r\n\r\n"
        public bool Options()
        {
            bool result = false;
            this.sender.Call("OPTIONS " + this.locator.ToString() + " RTSP/1.0\r\n" + "CSeq: " + (this.counter++).ToString() + "\r\n\r\n");
            string answer = this.reciever();
            if (result = this.CommandSucceded(answer))
                result &= answer.Contains("DESCRIBE") && answer.Contains("SETUP") && answer.Contains("PLAY") && answer.Contains("PAUSE") && answer.Contains("TEARDOWN"); 
            return result;
        }
        public bool Description()
        {
            bool result = false;
            this.sender.Call("DESCRIBE " + this.locator.ToString() + " RTSP/1.0\r\n" + "CSeq: " + (this.counter++).ToString() + "\r\n\r\n");
            Sdp.Protocol sdp = this.DescriptionAnswer(this.reciever());
            if (sdp.NotNull() && sdp.Media.NotNull())
            {
                foreach (Sdp.Media media in sdp.Media)
                    if (media.Name.Contains("RTP/AVP 26") && media.Attributes.NotNull())
                    {
                        string control = media.Attributes.Find(s => s.StartsWith("control:"));
                        if (!control.Contains(this.locator.Path.ToString()))
                        {
                            Uri.Locator video = this.locator.Copy();
                            video.Path.Add(control.Substring(8));
                            this.video = video;
                            result = true;
                        }
                        else
                        {
                            this.video = control.Substring(8);
                            result = true;
                        }
                    }
            }
            return result;
        }
        /*
         *  RTSP/1.0 200 OK
            CSeq: 0
            Content-Type: application/sdp
            Content-Base: rtsp://192.168.1.21:554/axis-media/media.amp/
            Date: Sat, 08 Jan 2011 15:23:28 GMT
            Content-Length: 342

            v=0
            o=- 1294500208759339 1294500208759339 IN IP4 192.168.1.21
            s=Media Presentation
            e=NONE
            c=IN IP4 0.0.0.0
            t=0 0
            a=control:rtsp://192.168.1.21:554/axis-media/media.amp?videocodec=jpeg
            a=range:npt=0.000000-
            m=video 0 RTP/AVP 26
            a=framesize:26 640-480
            a=control:rtsp://192.168.1.21:554/axis-media/media.amp/trackID=1?videocodec=jpeg
            */
        Sdp.Protocol DescriptionAnswer(string value)
        {
            Sdp.Protocol result = null;
            if (value.StartsWith("RTSP/1.0 200 OK"))
            {
                string part = value.Substring(value.IndexOf("\r\n\r\n") + 4);
                result = part;
            }
            return result;
        }
        public bool Setup()
        {
            bool result = false;
            if (this.video.NotNull())
            {
                int clientPort = 9000;
                string cast ="unicast";
                this.sender.Call("SETUP " + this.video.ToString() + " RTSP/1.0\r\n" + "CSeq: " + (this.counter++).ToString() + "\r\n" + "Transport: RTP/AVP;" + cast + ";client_port=" + clientPort + "-" + (clientPort + 1).ToString() + "\r\n\r\n");
                this.session = this.SetupAnswer(this.reciever());
                if (this.session.NotEmpty())
                {
                    this.Port = clientPort;
                    result = true;
                }
            }
            return result;
        }
        string SetupAnswer(string value)
        {
            string result = null;
            string[] splitted = value.Split(new char[] { '\r', '\n', ';' }, StringSplitOptions.RemoveEmptyEntries);
			if (splitted.Length > 0 && splitted[0].Contains("RTSP/1.0 200 OK"))
			{
				string session = splitted.Find(s => s.Contains("Session:"));
				if (session.NotEmpty())
					result = session.Replace("Session:", "").Trim();
			}
            return result;
        }
        public bool Play()
        {
            bool result = false;
            if(result = this.session.NotEmpty())
             this.sender.Call("PLAY " + this.locator.ToString() + " RTSP/1.0\r\n" + "CSeq: " + (this.counter++).ToString() + "\r\n" + "Session: " + this.session + "\r\n\r\n");
            result &= this.CommandSucceded(this.reciever());
            return result;
        }
        public bool Pause()
        {
            bool result = false;
            if (result = this.session.NotEmpty())
                this.sender.Call("PAUSE " + this.locator.ToString() + " RTSP/1.0\r\n" + "CSeq: " + (this.counter++).ToString() + "\r\n" + "Session: " + this.session + "\r\n\r\n");
            result &= this.CommandSucceded(this.reciever());
            return result;
        }
        public bool Teardown()
        {
            bool result = false;
            if (result = this.session.NotEmpty())
                this.sender.Call("TEARDOWN " + this.locator.ToString() + " RTSP/1.0\r\n" + "CSeq: " + (this.counter++).ToString() + "\r\n" + "Session: " + this.session + "\r\n\r\n");
            result &= this.CommandSucceded(this.reciever());
            return result;
            
        }
        bool CommandSucceded(string value)
        {
            return value.StartsWith("RTSP/1.0 200 OK");
        }
    }
}
