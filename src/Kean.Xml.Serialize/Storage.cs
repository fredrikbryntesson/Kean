// 
//  Storage.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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
		protected override Core.Serialize.Data.Node Load(Uri.Locator resource)
		{
			return this.Convert(Dom.Document.Open(resource).Root, resource);
		}
		Core.Serialize.Data.Node Convert(Dom.Element element, Uri.Locator resource)
		{
			resource = resource.Copy();
			resource.Fragment = (resource.Fragment.NotEmpty() ? resource.Fragment + "." : "") + element.Name;
			Core.Serialize.Data.Node result = null;
			if (element.All(n => !(n is Dom.Element)))
			{
				string value = element.Fold((n, s) => s + (n is Dom.Text ? (n as Dom.Text).Value : n is Dom.Data ? (n as Dom.Data).Value : ""), "");
				result = new Core.Serialize.Data.String(value) { Name = element.Name, Locator = resource };
			}
			else
			{
				result = new Core.Serialize.Data.Branch() { Name = element.Name, Locator = resource };
				foreach (Dom.Node node in element)
					if (node is Dom.Element)
						(result as Core.Serialize.Data.Branch).Nodes.Add(this.Convert(node as Dom.Element, resource));
			}
			Dom.Attribute type = element.Attributes.Find(a => a.Name == "type");
			if (type.NotNull())
				result.Type = type.Value;
			return result;
		}
		protected override bool Store(Core.Serialize.Data.Node value, Uri.Locator resource)
		{
			return new Dom.Document() { Root = this.Convert(value) }.Save(resource);
		}
		Dom.Element Convert(Core.Serialize.Data.Node element)
		{
			Dom.Element result = null;
			if (element.NotNull())
			{
				result = new Dom.Element(element.Name ?? "node");
				if (element is Core.Serialize.Data.Link)
				{
					result.Attributes.Add(new Kean.Xml.Dom.Attribute("type", element.Type));
					result.Add(new Dom.Text((element as Core.Serialize.Data.Link).Target));
				}
				else
				{
					if (element.Type.NotNull())
						result.Attributes.Add(new Kean.Xml.Dom.Attribute("type", element.Type));
					if (element is Core.Serialize.Data.Leaf)
						result.Add(new Dom.Text((element as Core.Serialize.Data.Leaf).Text));
					else if (element is Core.Serialize.Data.Branch)
						(element as Core.Serialize.Data.Branch).Nodes.Apply(e => result.Add(this.Convert(e)));
				}
			}
			return result;
		}
	}
}
