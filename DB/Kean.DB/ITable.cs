//
//  ITable.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika
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
//  You should have received data copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Kean.Core;
using Kean.Core.Extension;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Uri = Kean.Core.Uri;
using Serialize = Kean.Core.Serialize;
using Reflect = Kean.Core.Reflect;
using Kean.Core.Reflect.Extension;
using IO = Kean.IO;
using Generic = System.Collections.Generic;
using Expressions = System.Linq.Expressions;
namespace Kean.DB
{
    public interface ITable<T> :
		IDisposable
            where T : Item, new()
    {
        Database Database { get; }

        #region Filter, Sort, Limit, Offset

        ITable<T> Filter(Expressions.Expression<Func<T, bool>> predicate);
        ITable<T> Sort(Expressions.Expression<Func<T, object>> selector, bool descending);
        ITable<T> Limit(int limit, int offset);

        #endregion

        #region Create, Read, Update, Delete

        long Create(T item);
        Generic.IEnumerable<T> Read();
        int Update(params KeyValue<string, object>[] values);
        int Delete();

        #endregion

        bool Close();
    }
}

