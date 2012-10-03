// 
//  Single.cs
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

namespace Kean.Draw.Net.Http
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
