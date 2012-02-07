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
			return this.Convert(Dom.Document.Open(locator).Root);
		}
		Core.Serialize.Data.Node Convert(Dom.Element element)
		{
			return null;
		}
		protected override bool Store(Core.Serialize.Data.Node value, Uri.Locator locator)
		{
			return new Dom.Document() { Root = this.Convert(value) }.Save(locator);
		}
		Dom.Element Convert(Core.Serialize.Data.Node element)
		{
			Dom.Element result = new Dom.Element(element.Name ?? "node");
			if (element.Type.NotNull())
				result.Attributes.Add(new Kean.Xml.Dom.Attribute("type", element.Type));
			if (element is Core.Serialize.Data.Leaf)
				result.Add(new Dom.Text((element as Core.Serialize.Data.Leaf).Text));
			else if (element is Core.Serialize.Data.Branch)
				(element as Core.Serialize.Data.Branch).Nodes.Apply(e => result.Add(this.Convert(e)));
			return result;
		}
	}
}
