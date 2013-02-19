// 
//  Dynamic.cs
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

namespace Kean.Platform.Settings
{
	public class Dynamic :
		IDynamic
	{
		Collection.Dictionary<string, Tuple<string, string, object>> data = new Collection.Dictionary<string, Tuple<string, string, object>>();
		Action<string, object> loaded;

		public object this[string name]
		{
			get
			{
				object result;
				if (name.NotEmpty())
				{
					string[] splitted = (name ?? "").Split(new char[] { '.' }, 2);
					Tuple<string, string, object> member = this.data[splitted[0]];
					result = member.NotNull() ? member.Item3 : null;
					if (result is Dynamic && splitted.Length > 1)
						result = (result as Dynamic)[splitted[1]];
				}
				else
					result = this;
				return result;
			}
			set 
			{
				if (value.NotNull())
					this.Load(name, value);
				else
					this.Unload(name);
			}
		}

		public Dynamic()
		{ }

		public void Load(string name, object value)
		{
			this.Load(name, null, value);
		}
		public void Load(string name, string description, object value)
		{
			this.Load(name, description, null, value);
		}
		public void Load(string name, string description, string usage, object value)
		{
            string[] path = name.Split(new char[] { '.' }, 2);
            if (path.Length > 1)
            {
                Dynamic next = this[path[0]] as Dynamic;
                if (next.IsNull())
                    this.Load(path[0], next = new Dynamic());
                next.Load(name.Substring(path[0].Length + 1), description, usage, value);
            }
            else
            {
                this.data[name] = Tuple.Create(description, usage, value);
                this.loaded.Call(name, value);
                this.Reload();
            }
		}
		public void Unload(string name)
		{
			this.data.Remove(name);
			this.Reload();
		}
		public void WhenLoaded<T>(string name, Action<T> loaded)
		{
			object member = this[name];
			if (member is T)
				loaded.Call((T)member);
			this.loaded += (n, m) =>
			{
				if (n == name && m is T)
					loaded.Call((T) m);
			};
		}

		void Reload()
		{
			this.reload.Call();
		}

		#region IDynamic Members
		System.Collections.Generic.IEnumerable<Tuple<string, string, string, object>> IDynamic.GetDynamic()
		{
			foreach (KeyValue<string, Tuple<string, string, object>> member in this.data)
				yield return Tuple.Create(member.Key, member.Value.Item1, member.Value.Item2, member.Value.Item3);
		}
		#endregion
		#region IReload Members
		event Action reload;
		event Action IReload.Reload
		{
			add { this.reload += value; }
			remove { this.reload -= value; }
		}
		#endregion

        public virtual void Dispose()
        {
            if (this.data.NotNull())
            {
                foreach (KeyValue<string, Tuple<string, string, object>> item in this.data)
                    if (item.Value.Item3 is IDisposable)
                        (item.Value.Item3 as IDisposable).Dispose();
                this.data = null;
                this.loaded = null;
            }
        }
    }
}
