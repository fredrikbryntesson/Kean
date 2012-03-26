// 
//  Rtp.cs
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

namespace Kean.Draw.Net.Protocol
{
    public struct Rtp<T>
        where T : IProtocol
    {
        public int Version;
        public bool Padding;
        public bool Extension;
        public int ContributingSourceIdentifierCount;
        public bool Marker;
        public int PayloadType;
        public ushort SequenceNumber;
        public TimeSpan Timestamp;
        public uint SynchronizationSourceIdentifier;
        public T Payload;
        public Rtp(byte[] data)
        {
            this.Version = data[0] >> 6;
            this.Padding = (1 & (data[0] >> 5)) == 1;
            this.Extension = (1 & (data[0] >> 4)) == 1;
            this.ContributingSourceIdentifierCount = 0x1F & (data[0]);
            this.Marker = (data[1] >> 7) == 1;
            this.PayloadType = 127 & data[1];
            this.SequenceNumber = System.BitConverter.ToUInt16(new byte[] { data[3], data[2] }, 0);
            this.Timestamp = new TimeSpan(System.BitConverter.ToUInt32(new byte[]{data[7], data[6], data[5], data[4]}, 0));
            this.SynchronizationSourceIdentifier = System.BitConverter.ToUInt32(new byte[] { data[11], data[10], data[9], data[8] }, 0);
            if (!this.Extension)
            {
                this.Payload = default(T);
                this.Payload.Setup(data, 12, data.Length - 12);
            }
            else
                this.Payload = default(T);
        }
        public override string ToString()
        {
            return this.Marker + " " + this.SequenceNumber + " " + this.Timestamp;
        }
    }

}
