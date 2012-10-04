// 
//  Preprocessor.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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
using Generic = System.Collections.Generic;

namespace Kean.Xml.Serialize
{
	class Preprocessor :
		Dom.Processor
	{
		public Preprocessor()
		{ }
		protected override Generic.IEnumerator<Dom.Node> Process(Dom.ProcessingInstruction instruction)
		{
			switch (instruction.Target)
			{
				case "include":
					return this.Process(this.LoadAll(((Uri.Locator)instruction.Value).Resolve((Uri.Locator)instruction.Region.Resource)));
					break;
				default:
					return base.Process(instruction);
			}
		}
		Generic.IEnumerator<Dom.Node> LoadAll(Uri.Locator locator)
		{
			return (locator.Scheme == "file" && locator.Authority == null) ? this.LoadAll(locator.Path) : this.Load(locator);
		}
		Generic.IEnumerator<Dom.Node> LoadAll(Uri.Path path)
		{
			string plattformPath = path.FolderPath.PlattformPath;
			if (System.IO.Directory.Exists(path))
				foreach (string file in System.IO.Directory.GetFiles(plattformPath, path.Last.Head).Sort())
				{
					Dom.Document document = Dom.Document.Open(Uri.Locator.FromPlattformPath(file));
					if (document.NotNull() && document.Root.NotNull())
						yield return document.Root;
				}
		}
		Generic.IEnumerator<Dom.Node> Load(Uri.Locator locator)
		{
			Dom.Document document = Dom.Document.Open(locator);
			if (document.NotNull() && document.Root.NotNull())
				yield return document.Root;
		}
	}
}
