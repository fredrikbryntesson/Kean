using System;
using Raster = Kean.Draw.Raster;

namespace Kean.Draw.Net
{
    public interface ISource
    {
        event Action<Raster.Image> NewFrame;
        bool Running { get; }
        void Start();
        void Stop();
    }
}
