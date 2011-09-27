// 
//  Storage.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using IO = Kean.IO;
using Uri = Kean.Core.Uri;

namespace Kean.Xml.Serialize
{
	public class Storage : 
		Core.Serialize.Storage
	{
		public Storage(params Core.Serialize.ISerializer[] serializers) :
			base(serializers)
		{ }
		protected override Core.Serialize.Data.Node Load(Uri.Locator locator)
		{
			Dom.Document document = Dom.Document.Open(locator);
			return this.Convert(document.Root);
		}
		Core.Serialize.Data.Node Convert(Dom.Element element)
		{
			return null;
		}
		protected override bool Store(Core.Serialize.Data.Node value, Uri.Locator locator)
		{
			throw new NotImplementedException();
		}
		Dom.Element Convert(Core.Serialize.Data.Node element)
		{
			return null;
		}
	}
}
