using System;
using Error = Kean.Core.Error;

namespace Kean.Math.Matrix.Exception
{

    public class DivisionByZero : Error.Exception
    {
        public DivisionByZero()
            : base(Error.Level.Warning, "Division by zero", null)
        { }
    }
}
