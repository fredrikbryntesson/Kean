using System;

namespace Kean.Draw.Net.Rtsp
{
    public struct Rtp
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
        public int PayloadOffset;
        public int PayloadLength;
        public Rtp(byte[] data, int length)
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
                this.PayloadOffset = 12;
                this.PayloadLength = length - 12;
            }
            else
            {
                this.PayloadOffset = 0;
                this.PayloadLength = 0;
            }
        }
        public override string ToString()
        {
            return this.Marker + " " + this.SequenceNumber + " " + this.Timestamp;
        }
    }

}
