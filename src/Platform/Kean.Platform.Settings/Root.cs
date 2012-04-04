// 
//  Root.cs
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
using Uri = Kean.Core.Uri;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;

namespace Kean.Platform.Settings
{
	class Root : 
		Dynamic
	{
		Module module;

		internal Object Object { get; set; }
		public string Title { get; set; }
		public Root(Module module)
		{
			this.Title = "Settings Reference Manual";
			this.module = module;
		}

		[Method("close", "Closes application.", "Shuts down the current application instance.")]
		public bool Close()
		{
			return this.module.Application.NotNull() && this.module.Application.Close();
		}

		[Method("help", "Opens help in browser.", "Launches browser viewing help.")]
		public void Help()
		{
			Uri.Locator filename = "file:///c:/help.html";
			new Kean.Xml.Dom.Document(new Xml.Dom.Element("html", 
				new Xml.Dom.Element("head",
					new Xml.Dom.Element("title", new Xml.Dom.Text("Help")),
					new Xml.Dom.Element("style", new Xml.Dom.Text(""))
					), 
				new Xml.Dom.Element("body", this.GetHelp(null, this.Object, true)))).Save(filename);
			System.Diagnostics.Process.Start(filename);
		}
		Collection.IVector<Xml.Dom.Node> GetHelp(string prefix, Object @object, bool topLevel)
		{
			Collection.IList<Xml.Dom.Node> result = new Collection.List<Kean.Xml.Dom.Node>();
			Collection.IReadOnlyVector<Property> properties = @object.Properties;
			Collection.IReadOnlyVector<Method> methods = @object.Methods;
			Collection.IReadOnlyVector<Object> objects = @object.Objects;
			string name = (prefix + "." + @object.Name).Trim('.');
			if (name.IsEmpty())
				result.Add(new Xml.Dom.Element("h1", new Xml.Dom.Text(this.Title)));
			else if (topLevel && !(topLevel = !(properties.Count > 0 || methods.Count > 0)))
				result.Add(new Xml.Dom.Element("h2", new Xml.Dom.Text(name)));
			if (properties.Count > 0)
			{
				result.Add(new Xml.Dom.Element("h3", new Xml.Dom.Text("Properties")));
				Xml.Dom.Element list = new Xml.Dom.Element("dl");
				properties.Apply(p => list.Add(this.GetHelp(name, p)));
				result.Add(list);
			}
			if (methods.Count > 0)
			{
				result.Add(new Xml.Dom.Element("h3", new Xml.Dom.Text("Methods")));
				Xml.Dom.Element list = new Xml.Dom.Element("dl");
				methods.Apply(m => list.Add(this.GetHelp(name, m)));
				result.Add(list);
			}
			objects.Apply(o => result.Add(this.GetHelp(name, o, topLevel)));
			return result;
		}
		Collection.IVector<Xml.Dom.Node> GetHelp(string prefix, Method method)
		{
			return new Collection.Vector<Xml.Dom.Node>(
			new Xml.Dom.Element("dt", new Xml.Dom.Text((prefix + "." + method.Name).Trim('.'))),
			new Xml.Dom.Element("dd", new Xml.Dom.Text(method.Usage)));
		}
		Collection.IVector<Xml.Dom.Node> GetHelp(string prefix, Property property)
		{
			return new Collection.Vector<Xml.Dom.Node>(
			new Xml.Dom.Element("dt", new Xml.Dom.Text((prefix + "." + property.Name).Trim('.'))),
			new Xml.Dom.Element("dd", new Xml.Dom.Text(property.Usage)));
		}
	}
}
