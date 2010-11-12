// 
//  Exception.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009 Simon Mika
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
namespace Kean.Core.Error
{
	public abstract class Exception :
		System.ApplicationException,
		IError
	{
		public static Action<IError> Log { get; set; }
		static Level threshold = Level.Warning;
		public static Level Threshold 
		{
			get { return Exception.threshold; }
			set { if (value < Level.Recoverable) Exception.threshold = value; }
		}
		public Level Level { get; private set; }
		public string Title { get; private set; }
		public System.Diagnostics.StackTrace Trace { get; private set; }
        protected Exception(Level level, string title, string message, params object[] arguments) : this(null, level, title, message, arguments) { }
        protected Exception(System.Exception exception, Level level, string title, string message, params object[] arguments) : 
            base(System.String.Format(message, arguments), exception)
        {
            this.Level = level;
            this.Title = title;
			this.Trace = new System.Diagnostics.StackTrace(1, true);
			if (Exception.Log != null)
				Exception.Log(this);
		}
        public void Throw()
        {
			if (this.Level >= Exception.Threshold)
				throw this;
        }
	}
}
