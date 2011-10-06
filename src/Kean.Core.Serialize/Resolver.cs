﻿// 
//  Storage.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009-2011 Simon Mika
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
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Uri = Kean.Core.Uri;

namespace Kean.Core.Serialize
{
	public class Resolver
	{
		Collection.IDictionary<Uri.Locator, object> targets;
		Collection.IDictionary<Uri.Locator, Action<object>> looseEnds;

		public object this[Uri.Locator locator] 
		{ 
			get { return this.targets[locator]; }
			set
			{
				if (this.targets[locator].NotNull())
					new Exception.DuplicateIdentifier(locator).Throw();
				else
				{
					this.targets[locator] = value;
					this.looseEnds[locator].Call(value);
				}
			}
		}

		public Resolver()
		{
			this.targets = new Collection.Dictionary<Uri.Locator, object>();
			this.looseEnds = new Collection.Dictionary<Uri.Locator, Action<object>>();
		}

		public void Resolve(Uri.Locator locator, Action<object> set)
		{
			object target = this.targets[locator];
			if (target.NotNull())
				set(target);
			else
			{
				Action<object> looseEnd = this.looseEnds[locator];
				if (looseEnd.NotNull())
					looseEnd += set;
				else
					this.looseEnds[locator] = set;
			}
		}
	}
}