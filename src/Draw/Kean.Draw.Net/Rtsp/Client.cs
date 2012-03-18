using System;
using Kean.Core.Extension;
using Uri = Kean.Core.Uri;
using Raster = Kean.Draw.Raster;
using Parallel = Kean.Core.Parallel;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
namespace Kean.Draw.Net.Rtsp
{
    public class Client
    {
        bool stopped;
        public bool Running { get { return !this.stopped; } }
        Uri.Locator locator;
        System.Net.Sockets.TcpClient clientConnection;
        System.Net.Sockets.NetworkStream clientStream;
        int port = 9000;
        int counter = 0;
        string session;
        Parallel.Thread thread;
        public Client(Uri.Locator locator)
        {
            this.locator = locator;
            this.clientConnection = new System.Net.Sockets.TcpClient(this.locator.Authority.Endpoint.Host.ToString(), this.locator.Authority.Endpoint.Port.HasValue ? (int)this.locator.Authority.Endpoint.Port.Value : 554);
            this.clientStream = clientConnection.GetStream();
        }
        public void Start()
        {
            System.Net.Sockets.Socket server = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                     System.Net.Sockets.SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);
            System.Net.EndPoint endPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Parse("192.168.1.65"), this.port);
            server.Bind(endPoint);
            this.stopped = false;
            this.thread = Parallel.Thread.Start("Draw.Net", () =>
            {
                Console.WriteLine("Started");
                server.ReceiveBufferSize = 8192 * 15;
                server.ReceiveTimeout = 200;
                byte[] jpegData = new byte[512 * 1024];
                int jpegDataLength = 0;
                byte[] buffer = new byte[server.ReceiveBufferSize];
                bool frame = false;
                bool first = true;
                int index = 0;
                while (this.Running)
                {
                    if (server.Available > 0)
                    {
                        int read = server.Receive(buffer);
                        Rtp rtp = new Rtp(buffer, read);
                        if (rtp.Marker && !frame)
                            frame = true;
                        else if (!rtp.Marker && frame)
                        {
                            Jpeg jpeg = new Jpeg(buffer, rtp.PayloadOffset, rtp.PayloadLength, first);
                            // first
                            if (first)
                            {
                                byte[] header = jpeg.CreateHeader();    
                                Array.Copy(header, jpegData, header.Length);
                                jpegDataLength += header.Length;
                            }
                            first = false;
                            // between
                            Array.Copy(buffer, jpeg.PayloadOffset, jpegData, jpegDataLength, jpeg.PayloadLength);
                            jpegDataLength += jpeg.PayloadLength;
                        }
                        else if (rtp.Marker && frame)
                        {
                            // last
                            Jpeg jpeg = new Jpeg(buffer, rtp.PayloadOffset, rtp.PayloadLength, false);
                            Array.Copy(buffer, jpeg.PayloadOffset, jpegData, jpegDataLength, jpeg.PayloadLength);
                            jpegDataLength += jpeg.PayloadLength;
                            Raster.Image image = Raster.Image.Open(new System.IO.MemoryStream(jpegData, 0, jpegDataLength));
                            image.Save("test" + (index++) + ".png");
                            Console.WriteLine("image found " + image.NotNull() + " time " +rtp.Timestamp);
                            jpegDataLength = 0;
                            first = true;
                        }
                    }
                }
                Console.WriteLine("Stopped");
            });
        }
        public void Setup()
        {
            Console.WriteLine();
            string message = "DESCRIBE " + this.locator.ToString() + " RTSP/1.0\r\nCSeq: " + (this.counter++).ToString() + "\r\n\r\n";
            this.SendMessage(message);
            System.Threading.Thread.Sleep(200);
            string recieved = null;
            while ((recieved = this.RecieveMessage()).IsEmpty())
                System.Threading.Thread.Sleep(200);
            Console.WriteLine(recieved);
        }
        public void Play()
        {
            string message = "SETUP rtsp://192.168.1.21:554/axis-media/media.amp/trackID=1?videocodec=jpeg RTSP/1.0\r\nCSeq: " + (this.counter++).ToString() + "\r\nTransport: RTP/AVP;unicast;client_port=" + this.port + "-" + (this.port + 1).ToString() + "\r\n\r\n";
            this.SendMessage(message);
            System.Threading.Thread.Sleep(200);
            string recieved = null;
            while ((recieved = this.RecieveMessage()).IsEmpty())
                System.Threading.Thread.Sleep(200);
            Console.WriteLine(recieved);
            string[] splitted = recieved.Substring(recieved.IndexOf("Session:") + 8).Split(new char[] { ' ', '\r', '\n', ';' }, StringSplitOptions.RemoveEmptyEntries);
            this.session = splitted[0];
            Console.WriteLine("Session " + this.session);
            if (this.session.NotEmpty())
            {
                message = "PLAY " + this.locator.ToString() + " RTSP/1.0\r\nCSeq: " + (this.counter++).ToString() + "\r\nSession: " + this.session + "\r\n\r\n";
                this.SendMessage(message);
                System.Threading.Thread.Sleep(200);
                Console.WriteLine(this.RecieveMessage());
                System.Threading.Thread.Sleep(200);
            }
        }
        public void Pause()
        {
            if (this.session.NotEmpty())
            {
                string message = "PAUSE " + this.locator.ToString() + " RTSP/1.0\r\nCSeq: " + (this.counter++).ToString() + "\r\nSession: " + this.session + "\r\n\r\n";
                this.SendMessage(message);
                System.Threading.Thread.Sleep(200);
                string recieved = null;
                while ((recieved = this.RecieveMessage()).IsEmpty())
                    System.Threading.Thread.Sleep(200);
                Console.WriteLine(recieved);
            }
        }
        public void TearDown()
        {
            this.stopped = true;
            if (this.session.NotEmpty())
            {
                string message = "TEARDOWN " + this.locator.ToString() + " RTSP/1.0\r\nCSeq: " + (this.counter++).ToString() + "\r\nSession: " + this.session + "\r\n\r\n";
                this.SendMessage(message);
                System.Threading.Thread.Sleep(200);
                string recieved = null;
                while ((recieved = this.RecieveMessage()).IsEmpty())
                    System.Threading.Thread.Sleep(200);
                Console.WriteLine(recieved);
            }
        }
        void SendMessage(string message)
        {
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(message);
            this.clientStream.Write(buffer, 0, buffer.Length);
        }
        string RecieveMessage()
        {
            string result = null;
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            while (this.clientConnection.Connected && this.clientStream.CanRead && this.clientStream.DataAvailable)
            {
                int value = this.clientStream.ReadByte();
                if (value >= 0)
                    builder.Append(System.Text.Encoding.ASCII.GetChars(new byte[] { (byte)value }));
                else
                {
                    result = builder.ToString();
                    break;
                }
            }
            result = builder.ToString();
            return result;
        }

    }
}
