using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kean.IO.Uri.Test
{
    public static class All
    {
        public static void Test()
        {
            Authority.Test();
            Domain.Test();
            Endpoint.Test();
            Locator.Test();
            Path.Test();
            Query.Test();
            Scheme.Test();
            User.Test();
        }
    }
}
