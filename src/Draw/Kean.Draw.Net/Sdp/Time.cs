// 
//  Time.cs
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
using Kean.Core.Extension;

namespace Kean.Draw.Net.Sdp
{  
    /*  
        Time description
        t=  (time the session is active)
        r=* (zero or more repeat times)
    */
    public class Time
    {
        public string Active { get; set; }
        public string Repeat { get; set; }
        
        #region Object overrides
        public override string ToString()
        {
            string result;
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            if (this.Active.NotEmpty())
            {
                builder.Append("t=");
                builder.Append(this.Active);
                builder.Append("\r\n");
            }
            if (this.Repeat.NotEmpty())
            {
                builder.Append("r=");
                builder.Append(this.Repeat);
                builder.Append("\r\n");
            }
            result = builder.ToString();
            return result;
        }
        #endregion
    }
}
