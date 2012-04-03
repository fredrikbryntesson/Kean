// 
//  Root.cs
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

		public Root(Module module)
		{
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
				new Xml.Dom.Element("body", this.GetHelp(this.Object, 1)))).Save(filename);
			System.Diagnostics.Process.Start(filename);
		}
		Collection.IVector<Xml.Dom.Node> GetHelp(Object @object, int headerLevel)
		{
			Collection.IList<Xml.Dom.Node> properties = new Collection.List<Xml.Dom.Node>();
			Collection.IList<Xml.Dom.Node> methods = new Collection.List<Xml.Dom.Node>();
			Collection.IList<Xml.Dom.Node> objects = new Collection.List<Xml.Dom.Node>();
			@object.Members.Apply(m =>
			{
				if (m is Property)
					properties.Add(this.GetHelp(m as Property));
				else if (m is Method)
					methods.Add(this.GetHelp(m as Method));
				else if (m is Object)
					objects.Add(this.GetHelp(m as Object, headerLevel++));
			});

			return new Collection.Merge<Xml.Dom.Node>(new Collection.Vector<Xml.Dom.Node>(
			new Xml.Dom.Element("h" + headerLevel, new Xml.Dom.Text(@object.Name)),
			new Xml.Dom.Element("p", new Xml.Dom.Text(@object.Usage))),
			new Collection.Merge<Xml.Dom.Node>(properties, new Collection.Merge<Xml.Dom.Node>(methods, objects)));
		}
		Collection.IVector<Xml.Dom.Node> GetHelp(Method method)
		{
			return new Collection.Vector<Xml.Dom.Node>(
			new Xml.Dom.Element("h5", new Xml.Dom.Text(method.Name)),
			new Xml.Dom.Element("p", new Xml.Dom.Text(method.Usage)));
		}
		Collection.IVector<Xml.Dom.Node> GetHelp(Property property)
		{
			return new Collection.Vector<Xml.Dom.Node>(
			new Xml.Dom.Element("h5", new Xml.Dom.Text(property.Name)),
			new Xml.Dom.Element("p", new Xml.Dom.Text(property.Usage)));
		}
	}
}
