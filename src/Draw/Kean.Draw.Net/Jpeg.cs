using System;
using Kean.Core;
using Kean.Core.Extension;
using Uri = Kean.Core.Uri;
using Raster = Kean.Draw.Raster;

namespace Kean.Draw.Net
{
    public class Jpeg :
        Single,
        ISource
    {
        public Jpeg(Uri.Locator locator) : this(locator, null, null, 1024, 4) { }
        public Jpeg(Uri.Locator locator, int readSize, int attempts) : this(locator, null, null, readSize, attempts) { }
        public Jpeg(Uri.Locator locator, string login, string password, int readSize, int attempts) : base(locator, login, password, readSize, attempts) { }
          
        public override string Content { get { return "image/jpeg"; } }
        public event Action<Raster.Image> NewFrame;
        protected override void Initialize()
        {
            this.NewData += data => this.NewFrame.Call(Raster.Image.Open(new System.IO.MemoryStream(data)));
            base.Initialize();
        }
    }
}
