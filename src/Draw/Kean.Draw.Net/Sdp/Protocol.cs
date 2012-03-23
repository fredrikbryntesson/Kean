// 
//  Protocol.cs
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
using Collection = Kean.Core.Collection;
namespace Kean.Draw.Net.Sdp
{
    /*
      RTSP/1.0 200 OK
      CSeq: 1
      Content-Base: rtsp://example.com/media.mp4
      Content-Type: application/sdp
      m=video 0 RTP/AVP 96
      a=control:streamid=0
      a=range:npt=0-7.741000
      a=length:npt=7.741000
      a=rtpmap:96 MP4V-ES/5544
      a=mimetype:string;"video/MP4V-ES"
      a=AvgBitRate:integer;304018
      a=StreamName:string;"hinted video track"
      m=audio 0 RTP/AVP 97
      a=control:streamid=1
      a=range:npt=0-7.712000
      a=length:npt=7.712000
      a=rtpmap:97 mpeg4-generic/32000/2
      a=mimetype:string;"audio/mpeg4-generic"
      a=AvgBitRate:integer;65790
      a=StreamName:string;"hinted audio track"
       */
    /* Axis camera
      "RTSP/1.0 200 OK\r\n
     * CSeq: 0\r\n
     * Content-Type: application/sdp\r\n
     * Content-Base: rtsp://192.168.1.21:554/axis-media/media.amp/\r\n
     * Date: Wed, 05 Jan 2011 21:10:26 GMT\r\n
     * Content-Length: 342\r\n\r\n
     * v=0\r\n
     * o=- 1294261826758511 1294261826758511 IN IP4 192.168.1.21\r\n
     * s=Media Presentation\r\n
     * e=NONE\r\n
     * c=IN IP4 0.0.0.0\r\n
     * t=0 0\r\n
     * a=control:rtsp://192.168.1.21:554/axis-media/media.amp?videocodec=jpeg\r\n
     * a=range:npt=0.000000-\r\n
     * m=video 0 RTP/AVP 26\r\n
     * a=framesize:26 640-480\r\n
     * a=control:rtsp://192.168.1.21:554/axis-media/media.amp/trackID=1?videocodec=jpeg\r\n"
     */

    public class Protocol
    {
        public Session Session { get; set; }
        public Time Time { get; set; }
        public Collection.List<Media> Media { get; set; }
        public Protocol()
        {}
        public Protocol(string description)
        {
            string[] values = description.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Tuple<char, string>[] keyValues = values.Map(s => Tuple.Create(s[0], s.Substring(2)));
            int k = 0;
            while (k < keyValues.Length)
            {
                if (keyValues[k].Item1 == 't')
                {
                    this.Time = new Time() { Active = keyValues[k].Item2 };
                    if (++k < keyValues.Length && keyValues[k].Item1 == 'r')
                        this.Time.Repeat = keyValues[k].Item2;
                }
                else if (keyValues[k].Item1 == 'm')
                {
                    Media media = new Media() { Name = keyValues[k].Item2 };
                    while (++k < keyValues.Length)
                    {
                        char key = keyValues[k].Item1;
                        string value = keyValues[k].Item2;
                        if (key == 'i')
                            media.Title = value;
                        else if (key == 'b')
                            media.Bandwidth = value;
                        else if (key == 'c')
                            media.Connection = value;
                        else if (key == 'k')
                            media.EncryptionKey = value;
                        else if (key == 'a')
                        {
                            if (media.Attributes.IsNull())
                                media.Attributes = new Collection.List<string>();
                            media.Attributes.Add(value);
                        }
                        else
                            break;
                    }
                    if (this.Media.IsNull())
                        this.Media = new Collection.List<Media>();
                    this.Media.Add(media);
                }
                else
                {
                    
                    if(this.Session.IsNull())
                        this.Session = new Session();
                    while (k < keyValues.Length)
                    {
                        char key = keyValues[k].Item1;
                        string value = keyValues[k].Item2;
                        if (key == 'v')
                            this.Session.Version = value;
                        else if (key == 'o')
                            this.Session.OriginatorIdentifier = value;
                        else if (key == 's')
                            this.Session.Name = value;
                        else if (key == 'i')
                            this.Session.Information = value;
                        else if (key == 'u')
                            this.Session.Uri = value;
                        else if (key == 'e')
                            this.Session.Email = value;
                        else if (key == 'p')
                            this.Session.PhoneNumber = value;
                        else if (key == 'c')
                            this.Session.Connection = value;
                        else if (key == 'b')
                            this.Session.Bandwidth = value;
                        else if (key == 'z')
                            this.Session.TimeZoneAdjustment = value;
                        else if (key == 'k')
                            this.Session.EncryptionKey = value;
                        else if (key == 'a')
                        {
                            if (this.Session.Attributes.IsNull())
                                this.Session.Attributes = new Collection.List<string>();
                            this.Session.Attributes.Add(value);
                        }
                        else
                            break;
                        k++;
                    }
                }
            }
        }
        #region Object overrides
        public override string ToString()
        {
            string result;
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            if (this.Session.NotNull())
                builder.Append(this.Session.ToString());
            if (this.Time.NotNull())
                builder.Append(this.Time.ToString());
            if (this.Media.NotNull())
            {
                foreach(Media media in this.Media)
                builder.Append(media.ToString());
            }

            result = builder.ToString();
            return result;
        }
        #endregion
        #region Casts
        public static implicit operator Protocol(string value)
        {
            return value.NotEmpty() ? new Protocol(value) : null;
        }
        public static implicit operator string(Protocol protocol)
        {
            return protocol.NotNull() ? protocol.ToString() : null;
        }
        #endregion
    }
}
