// 
//  Abstract.cs
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
using Raster = Kean.Draw.Raster;
using Parallel = Kean.Core.Parallel;

namespace Kean.Draw.Net.Http
{
    public abstract class Abstract
    {
        protected abstract string Content { get; }
        int attempts;
        string login;
        string password;
        Uri.Locator locator;
        Parallel.Thread thread;
        int readSize;
        bool stopped;
        public Abstract()
        {}
        public Abstract(Uri.Locator locator, string login, string password, int readSize, int attempts)
        {
            this.locator = locator;
            this.login = login;
            this.password = password;
            this.readSize = readSize;
            this.attempts = attempts;
        }
        public abstract event Action<byte[]> NewData;
        public bool Running { get { return !this.stopped; } }
        public bool Start()
        {
            this.Initialize();
            this.stopped = false;
            this.thread = Parallel.Thread.Start("Draw.Net", () =>
            {
                int attempt = 0;
                do
                {
                    try
                    {
                        byte[] buffer = new byte[512 * 1024];
                        while (this.Running)
                        {
                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(this.locator.ToString()) as System.Net.HttpWebRequest;
                            request.Credentials = this.login.NotEmpty() && this.password.NotEmpty() ? new System.Net.NetworkCredential(this.login, this.password) : null;
                            // get response
                            System.Net.WebResponse response = request.GetResponse();
                            string content = response.ContentType;
                            if (!content.StartsWith(this.Content))
                            {
                                this.stopped = true;
                                break;
                            }
                            this.StreamParser(response, buffer, this.readSize);
                            response.Close();
                            request.Abort();
                        }
                    }
                    catch (System.Exception)
                    { }
                } while (attempt++ < this.attempts);
            });
            return true;
        }
        public bool Stop()
        {
			bool result;
			if (result = this.thread.NotNull())
			{
				this.thread.Abort();
				this.thread.Join(100);
				this.thread.Dispose();
				this.thread = null;
            	this.stopped = true;
			}
            return result;
        }
        protected abstract void StreamParser(System.Net.WebResponse response, byte[] buffer, int readSize);
        protected virtual void Initialize()
        { }
    }
}
