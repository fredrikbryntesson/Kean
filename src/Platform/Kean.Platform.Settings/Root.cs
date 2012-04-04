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
		public string Style { get; set; }
		public Root(Module module)
		{
			this.Title = "Settings Reference Manual";
			this.module = module;
			this.Style = @"";
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
					new Xml.Dom.Element("title", new Xml.Dom.Text(this.Title)),
					new Xml.Dom.Element("style", new Xml.Dom.Text(this.Style))
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
			string name = (prefix.NotEmpty() ? prefix : "") + @object.Name ;
			if (name.NotEmpty())
				prefix = name + ".";
			if (name.IsEmpty())
				result.Add(new Xml.Dom.Element("h1", new Xml.Dom.Text(this.Title)));
			else if (topLevel && !(topLevel = !(properties.Count > 0 || methods.Count > 0)))
				result.Add(new Xml.Dom.Element("h2", new Xml.Dom.Text(name), KeyValue.Create("id", name), KeyValue.Create("class", "object")));
			if (properties.Count > 0)
			{
				result.Add(new Xml.Dom.Element("h3", new Xml.Dom.Text("Properties"), KeyValue.Create("id", name + "_properties"), KeyValue.Create("class", "properties")));
				Xml.Dom.Element list = new Xml.Dom.Element("dl");
				properties.Apply(p => list.Add(this.GetHelp(prefix, p)));
				result.Add(list);
			}
			if (methods.Count > 0)
			{
				result.Add(new Xml.Dom.Element("h3", new Xml.Dom.Text("Methods"), KeyValue.Create("id", name + "_methods"), KeyValue.Create("class", "methods")));
				Xml.Dom.Element list = new Xml.Dom.Element("dl");
				methods.Apply(m => list.Add(this.GetHelp(prefix, m)));
				result.Add(list);
			}
			objects.Apply(o => result.Add(this.GetHelp(prefix, o, topLevel)));
			return result;
		}
		Collection.IVector<Xml.Dom.Node> GetHelp(string prefix, Method method)
		{
			string name = prefix + method.Name;
			Collection.IList<Xml.Dom.Node> result = new Collection.List<Xml.Dom.Node>();
			result.Add(new Xml.Dom.Element("dt", new Xml.Dom.Text(name), KeyValue.Create("id", name)));
			Xml.Dom.Element parameters = new Xml.Dom.Element("dd", new Xml.Dom.Text(name + " " + string.Join(" ", method.Parameters.Map(p => "<" + p.Name + ">"))), KeyValue.Create("class", "signature"));
			Xml.Dom.Element parametersList = new Xml.Dom.Element("dl");
			foreach (Parameter.Abstract parameter in method.Parameters)
			{
				parametersList.Add(new Xml.Dom.Element("dt", parameter.Name));
				if (parameter.Usage.NotEmpty())
					parametersList.Add(new Xml.Dom.Element("dd", parameter.Usage, KeyValue.Create("class", "usage")));
				if (parameter.Description.NotEmpty())
					parametersList.Add(new Xml.Dom.Element("dd", parameter.Description, KeyValue.Create("class", "description")));
			}
			parameters.Add(parametersList);
			result.Add(parameters);
			result.Add(new Xml.Dom.Element("dd", new Xml.Dom.Text(name + " " + string.Join(" ", method.Parameters.Map(p => "<" + p.Name + ">"))), KeyValue.Create("class", "signature")));
			if (method.Usage.NotEmpty())
				result.Add(new Xml.Dom.Element("dd", new Xml.Dom.Text(method.Usage), KeyValue.Create("class", "usage")));
			if (method.Example.NotEmpty())
				result.Add(new Xml.Dom.Element("dd", new Xml.Dom.Text(method.Example), KeyValue.Create("class", "example")));
			return result;
		}
		Collection.IVector<Xml.Dom.Node> GetHelp(string prefix, Property property)
		{
			string name = prefix + property.Name;
			string classes = string.Join(" ", property.Mode.ToString().Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();
			Collection.IList<Xml.Dom.Node> result = new Collection.List<Xml.Dom.Node>();
			result.Add(new Xml.Dom.Element("dt", new Xml.Dom.Text(name), KeyValue.Create("id", name), KeyValue.Create("class", classes)));
			if (property.Usage.NotEmpty())
				result.Add(new Xml.Dom.Element("dd", new Xml.Dom.Text(property.Usage), KeyValue.Create("class", classes + " usage")));
			if (property.Example.NotEmpty())
				result.Add(new Xml.Dom.Element("dd", new Xml.Dom.Text(property.Example), KeyValue.Create("class", classes + " example")));
			return result;
		}
	}
}
