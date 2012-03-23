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
using Kean.Core.Extension;
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
        #region Object overrides
        public override string ToString()
        {
            string result;
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            if (this.Version.NotEmpty())
            {
                builder.Append("v=");
                builder.Append(this.Version);
                builder.Append("\r\n");
            }
            if (this.OriginatorIdentifier.NotEmpty())
            {
                builder.Append("o=");
                builder.Append(this.OriginatorIdentifier);
                builder.Append("\r\n");
            }
            if (this.Name.NotEmpty())
            {
                builder.Append("s=");
                builder.Append(this.Name);
                builder.Append("\r\n");
            }
            if (this.Information.NotEmpty())
            {
                builder.Append("i=");
                builder.Append(this.Information);
                builder.Append("\r\n");
            }
            if (this.Uri.NotEmpty())
            {
                builder.Append("u=");
                builder.Append(this.Uri);
                builder.Append("\r\n");
            }
            if (this.Email.NotEmpty())
            {
                builder.Append("e=");
                builder.Append(this.Email);
                builder.Append("\r\n");
            }
            if (this.PhoneNumber.NotEmpty())
            {
                builder.Append("p=");
                builder.Append(this.PhoneNumber);
                builder.Append("\r\n");
            }
            if (this.Connection.NotEmpty())
            {
                builder.Append("c=");
                builder.Append(this.Connection);
                builder.Append("\r\n");
            }
            if (this.Bandwidth.NotEmpty())
            {
                builder.Append("b=");
                builder.Append(this.Bandwidth);
                builder.Append("\r\n");
            }
            if (this.TimeZoneAdjustment.NotEmpty())
            {
                builder.Append("z=");
                builder.Append(this.TimeZoneAdjustment);
                builder.Append("\r\n");
            }
            if (this.EncryptionKey.NotEmpty())
            {
                builder.Append("k=");
                builder.Append(this.EncryptionKey);
                builder.Append("\r\n");
            }
            if (this.Attributes.NotNull())
            {
                foreach (string attribute in this.Attributes)
                {
                    builder.Append("a=");
                    builder.Append(attribute);
                    builder.Append("\r\n");
                }
            }
            result = builder.ToString();
            return result;
        }
        #endregion
    }

}
