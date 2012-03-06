using System;
using Kean.Core;
using Kean.Core.Extension;
using Uri = Kean.Core.Uri;

namespace Kean.Draw.Net
{
    public abstract class Single : 
        Abstract
    {
        public override event Action<byte[]> NewData;
        public Single(Uri.Locator locator, string login, string password, int readSize, int attempts) : base(locator, login, password, readSize, attempts) { }
        protected override void StreamParser(System.Net.WebResponse response, byte[] buffer, int readSize)
        {
            System.IO.Stream stream = response.GetResponseStream();
            int total = 0;
            // loop
            // safe check. Flush the buffer if do not have enough space left.
            while (total + readSize < buffer.Length)
            {
                int read = 0;
                // read == 0 means end of stream.
                if ((read = stream.Read(buffer, total, readSize)) == 0)
                    break;
                total += read;
            }
            if (total != 0 && total < buffer.Length)
            {
                byte[] data = new byte[total];
                Array.Copy(buffer, data, total);
                this.NewData.Call(data);
            }
        }
    }
}
