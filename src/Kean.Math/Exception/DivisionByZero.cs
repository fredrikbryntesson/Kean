﻿using System;
using Error = Kean.Core.Error;

namespace Kean.Math.Exception
{

    public class DivisionByZero : Error.Exception
    {
        public DivisionByZero()
            : base(Error.Level.Warning, "Division by zero", "Division by zero")
        { }
    }
}
