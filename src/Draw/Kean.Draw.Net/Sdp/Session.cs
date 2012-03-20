// 
//  Session.cs
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
using Collection = Kean.Core.Collection;

namespace Kean.Draw.Net.Sdp
{
    /*
     *  Session description
        v=  (protocol version)
        o=  (originator and session identifier)
        s=  (session name)
        i=* (session information)
        u=* (URI of description)
        e=* (email address)
        p=* (phone number)
        c=* (connection information—not required if included in all media)
        b=* (zero or more bandwidth information lines)
        One or more time descriptions ("t=" and "r=" lines; see below)
        z=* (time zone adjustments)
        k=* (encryption key)
        a=* (zero or more session attribute lines)
     */
    public class Session
    {
        public string Version { get; set; }
        public string OriginatorIdentifier { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public string Uri { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Connection { get; set; }
        public string Bandwidth { get; set; }
        public string TimeZoneAdjustment { get; set; }
        public string EncryptionKey { get; set; }
        public Collection.List<string> Attributes { get; set; }
    }
}
