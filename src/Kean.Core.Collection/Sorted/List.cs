// 
//  List.cs
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
using System;
using Kean.Core.Basis.Extension;

namespace Kean.Core.Collection.Sorted
{
    public class List<T> :
        IList<T>
    {
        IList<T> data;
        Basis.Comparer<T> comparer;
        #region Constructors
        public List() :
            this(new Collection.List<T>())
        { }
		public List(IList<T> data) :
			this(List<T>.Compare, data)
		{ }
		public List(Basis.Comparer<T> comparer) :
            this(comparer, new Collection.List<T>())
        { }
        public List(Basis.Comparer<T> comparer, IList<T> data)
        {
            this.comparer = comparer;
            this.data = data;
        }
        #endregion
        #region IList<T> Members
        public void Add(T item)
        {
            if (this.data.Count != 0)
            {
                for (int i = 0; i < this.data.Count; i++)
                    switch (this.comparer(this.data[i], item))
                    {
                        case Basis.Order.LessThan:
                            break;
                        case Basis.Order.Equal:
                            this.data[i] = item;
                            goto Done;
                        case Basis.Order.GreaterThan:
                            this.data.Insert(i, item);
                            goto Done;
                    }
                this.data.Insert(this.data.Count, item);
            Done:
                ;
            }
            else
                this.data.Add(item);
        }
        public T Remove()
        {
            return this.data.Remove();
        }
        public void Insert(int index, T item)
        {
            this.Add(item);
        }
        public T Remove(int index)
        {
            return this.data.Remove(index);
        }
        #endregion

        #region IVector<T> Members
        public int Count
        {
            get { return this.data.Count; }
        }
        public T this[int index]
        {
            get { return this.data[index]; }
            set
            {
                if (this.comparer(this.data[index], value) == Basis.Order.Equal)
                    this.data[index] = value;
                else
                {
                    this.Remove(index);
                    this.Add(value);
                }
            }
        }
        #endregion

        #region IEnumerable<T> Members
        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            return this.data.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region IEquatable<IVector<T>> Members
        public bool Equals(IVector<T> other)
        {
            return this.data.Equals(other);
        }
        #endregion
        #region Static
        static Basis.Order Compare(T left, T right)
        {
            Basis.Order result;
            if (left is Basis.IComparable<T>)
                result = List<T>.Compare(left as Basis.IComparable<T>, right);
            else if (left is IComparable<T>)
                result = List<T>.Compare(left as IComparable<T>, right);
            else if (left is IComparable)
                result = List<T>.Compare(left as IComparable, right);
            else
                throw new Exception.Unsortable(typeof(T));
            return result;
        }
        static Basis.Order Compare(Basis.IComparable<T> left, T right)
        {
            return left.Compare(right);
        }
        static Basis.Order Compare(IComparable<T> left, T right)
        {
            int result = left.CompareTo(right);
            return result > 0 ? Basis.Order.GreaterThan : result == 0 ? Basis.Order.Equal : Kean.Core.Basis.Order.LessThan;
        }
        static Basis.Order Compare(IComparable left, T right)
        {
            int result = left.CompareTo(right);
            return result > 0 ? Basis.Order.GreaterThan : result == 0 ? Basis.Order.Equal : Kean.Core.Basis.Order.LessThan;
        }
        #endregion
    }
}
