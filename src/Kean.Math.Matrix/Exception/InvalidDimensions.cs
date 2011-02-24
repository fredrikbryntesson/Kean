using System;
using Error = Kean.Core.Error;

namespace Kean.Math.Matrix.Exception
{

    public class InvalidDimensions : Error.Exception
    {
        public InvalidDimensions()
            : base(Error.Level.Warning, "Invalid Dimensions", "Invalid Dimensions")
        { }
    }
}
