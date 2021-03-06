﻿//
//  Resource.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika
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
using Kean.Extension;
using Error = Kean.Error;

namespace Kean.Draw.OpenGL.Backend
{
	public abstract class Resource :
		IDisposable
	{
		protected Context Context { get; private set; }
		protected Resource(Context context)
		{
			this.Context = context;
		}
		protected Resource(Resource original) :
			this(original.Context)
		{
			original.Context = null;
		}
		~Resource()
		{
			Error.Log.Wrap((Action<bool>)this.Dispose)(false);
		}
		protected abstract void Dispose(bool disposing);
		public void Dispose()
		{
			this.Dispose(true);
		}
		protected internal virtual void Delete()
		{
			this.Context = null;
		}
	}
}
