// 
//  MotionJpeg.cs
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

namespace Kean.Draw.Net.Http
{
    public class MotionJpeg :
         Multi,
        ISource
    {
        protected override string SubContent { get { return "image/jpeg"; } }
        public MotionJpeg(Uri.Locator locator) : this(locator, null, null, 1024, 4) { }
        public MotionJpeg(Uri.Locator locator, int readSize, int attempts) : this(locator, null, null, readSize, attempts) { }
        public MotionJpeg(Uri.Locator locator, string login, string password, int readSize, int attempts) : base(locator, login, password, readSize, attempts) { }
        public event Action<Raster.Image> NewFrame;
        protected override void Initialize()
        {
            this.NewData += data => this.NewFrame.Call(Raster.Image.Open(new System.IO.MemoryStream(data)));
            base.Initialize();
        }
    }
}
