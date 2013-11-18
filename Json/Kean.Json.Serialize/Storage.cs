//
//  Storage.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2011-2013 Simon Mika
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
namespace Kean.Json.Serialize
{
	public class Storage : 
		Core.Serialize.Storage
	{
		public Storage() :
			base(null, null, null)
		{
		}
		protected override Core.Serialize.Data.Node Load(Uri.Locator resource)
		{
			Dom.Item root = Dom.Item.Open(resource);
			return root.NotNull() ? Storage.Convert(root) : null;
		}
		protected override bool Store(Core.Serialize.Data.Node value, Uri.Locator resource)
		{
			Dom.Item item = Storage.Convert(value);
			if (!(item is Dom.Collection))
				item = new Dom.Object(KeyValue.Create(value.Name, item));
			return item.Save(resource);
		}

		#region Static Convert

		#region From Dom

		public static Core.Serialize.Data.Node Convert(Dom.Item item)
		{
			Core.Serialize.Data.Node result;
			if (item is Dom.Object)
				result = Storage.Convert(item as Dom.Object);
			else if (item is Dom.Primitive)
			{
				if (item is Dom.String)
					result = new Kean.Core.Serialize.Data.String((item as Dom.String).Value);
				else if (item is Dom.Number)
					result = new Kean.Core.Serialize.Data.Decimal((item as Dom.Number).Value);
				else if (item is Dom.Boolean)
					result = new Kean.Core.Serialize.Data.Boolean((item as Dom.Boolean).Value);
				else
					result = null;
			}
			else if (item is Dom.Array)
				result = Storage.Convert(item as Dom.Array);
			else
				result = null;
			return result;
		}
		static Core.Serialize.Data.Branch Convert(Dom.Object item)
		{
			Core.Serialize.Data.Branch result = new Core.Serialize.Data.Branch() { Region = item.Region };
			foreach (KeyValue<Json.Dom.Label, Dom.Item> child in item)
			{
				Core.Serialize.Data.Node c = Storage.Convert(child.Value);
				c.Name = child.Key;
				result.Nodes.Add(c);
			}
			return result;
		}
		static Core.Serialize.Data.Collection Convert(Dom.Array item)
		{
			Core.Serialize.Data.Collection result = new Core.Serialize.Data.Collection() { Region = item.Region };
			foreach (Dom.Item child in item)
				result.Nodes.Add(Storage.Convert(child));
			return result;
		}

		#endregion

		#region To Dom

		public static Dom.Item Convert(Core.Serialize.Data.Node item)
		{
			Dom.Item result;
			if (item is Core.Serialize.Data.Branch)
				result = Storage.Convert(item as Core.Serialize.Data.Branch);
			else if (item is Core.Serialize.Data.Leaf)
				result = Storage.Convert(item as Core.Serialize.Data.Leaf);
			else if (item is Core.Serialize.Data.Collection)
				result = Storage.Convert(item as Core.Serialize.Data.Collection);
			else if (item is Core.Serialize.Data.Link)
				result = Storage.Convert(item as Core.Serialize.Data.Link);
			else
				result = new Kean.Json.Dom.Null(item.Region);
			return result;
		}
		static Dom.Object Convert(Core.Serialize.Data.Branch item)
		{
			Dom.Object result = new Dom.Object(item.Region);
			if (item.Type.NotNull())
				result.Add("_type", (string)item.Type);
			item.Nodes.Apply(e => result.Add(e.Name, Storage.Convert(e)));
			return result;
		}
		static Dom.Primitive Convert(Core.Serialize.Data.Leaf item)
		{
			Dom.Primitive result;
			if (item is Core.Serialize.Data.Binary)
				result = new Dom.String((item as Core.Serialize.Data.Binary).Text);
			else if (item is Core.Serialize.Data.Boolean)
				result = new Dom.Boolean((item as Core.Serialize.Data.Boolean).Value);
			else if (item is Core.Serialize.Data.Byte)
				result = new Dom.Number((item as Core.Serialize.Data.Byte).Value);
			else if (item is Core.Serialize.Data.Character)
				result = new Dom.String((item as Core.Serialize.Data.Character).Text);
			else if (item is Core.Serialize.Data.DateTime)
				result = new Dom.String((item as Core.Serialize.Data.DateTime).Text);
			else if (item is Core.Serialize.Data.DateTimeOffset)
				result = new Dom.String((item as Core.Serialize.Data.DateTimeOffset).Text);
			else if (item is Core.Serialize.Data.Decimal)
				result = new Dom.Number((item as Core.Serialize.Data.Decimal).Value);
			else if (item is Core.Serialize.Data.Double)
				result = new Dom.Number((Decimal)(item as Core.Serialize.Data.Double).Value);
			else if (item is Core.Serialize.Data.Enumeration)
				result = new Dom.String((item as Core.Serialize.Data.Enumeration).Text);
			else if (item is Core.Serialize.Data.Integer)
				result = new Dom.Number((item as Core.Serialize.Data.Integer).Value);
			else if (item is Core.Serialize.Data.Long)
				result = new Dom.Number((item as Core.Serialize.Data.Long).Value);
			else if (item is Core.Serialize.Data.Short)
				result = new Dom.Number((item as Core.Serialize.Data.Short).Value);
			else if (item is Core.Serialize.Data.SignedByte)
				result = new Dom.Number((item as Core.Serialize.Data.SignedByte).Value);
			else if (item is Core.Serialize.Data.Single)
				result = new Dom.Number((Decimal)(item as Core.Serialize.Data.Single).Value);
			else if (item is Core.Serialize.Data.String)
				result = new Dom.String((item as Core.Serialize.Data.String).Value);
			else if (item is Core.Serialize.Data.TimeSpan)
				result = new Dom.String((item as Core.Serialize.Data.TimeSpan).Text);
			else if (item is Core.Serialize.Data.UnsignedInteger)
				result = new Dom.Number((item as Core.Serialize.Data.UnsignedInteger).Value);
			else if (item is Core.Serialize.Data.UnsignedLong)
				result = new Dom.Number((item as Core.Serialize.Data.UnsignedLong).Value);
			else if (item is Core.Serialize.Data.UnsignedShort)
				result = new Dom.Number((item as Core.Serialize.Data.UnsignedShort).Value);
			else
				result = new Kean.Json.Dom.Null();
			return result;
		}
		static Dom.Array Convert(Core.Serialize.Data.Collection item)
		{
			Dom.Array result = new Dom.Array(item.Region);
			item.Nodes.Apply(e => result.Add(Storage.Convert(e)));
			return result;
		}
		static Dom.Object Convert(Core.Serialize.Data.Link item)
		{
			Dom.Object result = new Dom.Object(item.Region);
			result.Add("_link", new Dom.String(item.Relative, item.Region));
			return result;
		}

		#endregion

		#endregion

		public static T Open<T>(Uri.Locator resource)
		{
			return new Storage().Load<T>(resource);
		}
		public static bool Save<T>(T value, Uri.Locator resource)
		{
			return new Storage().Store<T>(value, resource);
		}
		public static bool Save<T>(T value, Uri.Locator resource, string name)
		{
			return new Storage().Store<T>(value, resource, name);
		}
	}
}
