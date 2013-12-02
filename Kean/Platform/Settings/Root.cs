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
using Kean;
using Kean.Extension;
using Uri = Kean.Uri;
using Collection = Kean.Collection;
using Kean.Collection.Extension;

namespace Kean.Platform.Settings
{
	public class Root : 
		Dynamic
	{
		Module module;

		internal Object Object { get; set; }
		internal string Title { get; set; }
		internal Xml.Dom.Fragment Header { get; set; }
		internal Uri.Locator HelpFilename { get; set; }

		[Property("version", "Products version number.", "The version number of the product.")]
		public string Version { get { return this.module.Application.Version; } }

		string errorString;
		[Property("error", "Last error message", "The last error message.")]
		[Notify("OnErrorString")]
		public string ErrorString
		{
			get 
			{
				if (this.errorString.IsEmpty() && this.Error.NotNull())
					this.errorString = string.Format("{0},{1},\"{2}\",\"{3}\",\"{4}\",{5},{6},{7},{8},{9},{10}",
						this.Error.Time,
						this.Error.Level,
						this.Error.Title.Replace("\"", "\"\""),
						this.Error.Message.Replace("\"", "\"\""),
						this.Error.AssemblyName,
						this.Error.AssemblyVersion,
						this.Error.Type,
						this.Error.Method,
						this.Error.Filename,
						this.Error.Line,
						this.Error.Column);
				return this.errorString ?? ""; 
			}
		}
		public event Action<string> OnErrorString;

		public Error.IError Error { get; private set; }
		public event Action<Error.IError> OnError;

		internal Root(Module module)
		{
			this.Title = "Settings Reference Manual";
			this.module = module;
			Kean.Error.Log.OnAppend += this.OnErrorHelper;
		}
		void OnErrorHelper(Error.IError error)
		{
			this.OnError.Call(this.Error = error);
			this.errorString = null;
			if (this.OnErrorString.NotNull())
				this.OnErrorString(this.ErrorString);
		}
		public override void Dispose()
		{
			Kean.Error.Log.OnAppend -= this.OnErrorHelper;
			base.Dispose();
		}
		[Method("close", "Closes application.", "Shuts down the current application instance.")]
		public bool Close()
		{
			return this.module.Application.NotNull() && this.module.Application.Close();
		}

		[Method("help", "Opens help in browser.", "Launches browser viewing help.")]
		public void Help()
		{
			Xml.Dom.Element head = new Xml.Dom.Element("head",
					new Xml.Dom.Element("meta", KeyValue.Create("charset", "UTF-8")),
					new Xml.Dom.Element("title", new Xml.Dom.Text(this.Title ?? this.module.Application.Product + " " + this.module.Application.Version + " Settings Reference Manual")),
					new Xml.Dom.Element("meta", KeyValue.Create("name", "generator"), KeyValue.Create("content", this.module.Application.Product + " " + this.module.Application.Version)),
					new Xml.Dom.Element("meta", KeyValue.Create("name", "date"), KeyValue.Create("content", DateTime.Today.ToShortDateString()))
					);
			if (this.Header.NotNull())
				head.Add(this.Header);
			new Kean.Xml.Dom.Document(new Xml.Dom.Element("html", 
				head, 
				new Xml.Dom.Element("body", this.GetHelp(null, this.Object, true)))).Save(this.HelpFilename);
			System.Diagnostics.Process.Start(this.HelpFilename.PlatformPath);
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
				result.Add(new Xml.Dom.Element("header", new Xml.Dom.Text(this.Title)));
			else if (topLevel && !(topLevel = !(properties.Count > 0 || methods.Count > 0)))
				result.Add(new Xml.Dom.Element("h1", new Xml.Dom.Text(name), KeyValue.Create("id", name), KeyValue.Create("class", "object")));
			if (properties.Count > 0)
			{
				result.Add(new Xml.Dom.Element("h2", new Xml.Dom.Text("Properties"), KeyValue.Create("id", name + "_properties"), KeyValue.Create("class", "properties")));
				Xml.Dom.Element list = new Xml.Dom.Element("dl", KeyValue.Create("class", "properties"));
				properties.Apply(p => list.Add(this.GetHelp(prefix, p)));
				result.Add(list);
			}
			if (methods.Count > 0)
			{
				result.Add(new Xml.Dom.Element("h2", new Xml.Dom.Text("Methods"), KeyValue.Create("id", name + "_methods"), KeyValue.Create("class", "methods")));
				Xml.Dom.Element list = new Xml.Dom.Element("dl", KeyValue.Create("class", "methods"));
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
			result.Add(new Xml.Dom.Element("dt", new Xml.Dom.Text(method.Name), KeyValue.Create("id", name)));
			Xml.Dom.Element parameters = new Xml.Dom.Element("dd", new Xml.Dom.Text(name + " " + string.Join(" ", method.Parameters.Map(p => "<span class='parameter'>" + p.Name + "</span>"))), KeyValue.Create("id", name + "_signature"), KeyValue.Create("class", "signature"));
			if (method.Parameters.NotEmpty())
			{
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
			}
			result.Add(parameters);
			if (method.Usage.NotEmpty())
				result.Add(new Xml.Dom.Element("dd", new Xml.Dom.Text(method.Usage), KeyValue.Create("id", name + "_usage"), KeyValue.Create("class", "usage")));
			if (method.Example.NotEmpty())
				result.Add(new Xml.Dom.Element("dd", new Xml.Dom.Text(name + " " + method.Example), KeyValue.Create("id", name + "_example"), KeyValue.Create("class", "example")));
			return result;
		}
		Collection.IVector<Xml.Dom.Node> GetHelp(string prefix, Property property)
		{
			string name = prefix + property.Name;
			string classes = string.Join(" ", property.Mode.ToString().Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();
			Collection.IList<Xml.Dom.Node> result = new Collection.List<Xml.Dom.Node>();
			result.Add(new Xml.Dom.Element("dt", new Xml.Dom.Text(property.Name), KeyValue.Create("id", name), KeyValue.Create("class", classes)));
			if (property.Usage.NotEmpty())
				result.Add(new Xml.Dom.Element("dd", new Xml.Dom.Text(property.Usage), KeyValue.Create("id", name + "_usage"), KeyValue.Create("class", classes + " usage")));
			if (property.Example.NotEmpty())
				result.Add(new Xml.Dom.Element("dd", new Xml.Dom.Text(name + " " + property.Example), KeyValue.Create("id", name + "_example"), KeyValue.Create("class", classes + " example")));
			return result;
		}
	}
}
