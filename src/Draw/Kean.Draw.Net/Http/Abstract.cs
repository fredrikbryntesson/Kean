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
        public abstract string Content { get; }
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
        public void Start()
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
                    catch (System.Exception e)
                    { }
                } while (attempt++ < this.attempts);
            });

        }
        public void Stop()
        {
            this.stopped = true;
        }
        protected abstract void StreamParser(System.Net.WebResponse response, byte[] buffer, int readSize);
        protected virtual void Initialize()
        { }
    }
}
