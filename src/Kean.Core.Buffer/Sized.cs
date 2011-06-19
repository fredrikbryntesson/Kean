// 
//  Sized.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009-2011 Simon Mika
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
using Kean.Core.Basis.Extension;

namespace Kean.Core.Buffer
{
    public class Sized :
        Pointer,
        Collection.IVector<byte>
    {
        int size;
        public int Size { get { lock (this.Lock) return this.size; } }
        public Sized(IntPtr data, int size) :
            this(data, size, null) { }
        public Sized(IntPtr data, int size, Action<IntPtr> free) :
            base(data, free)
        {
            this.size = size;
        }
        #region IVector<byte> Members
        byte Collection.IVector<byte>.this[int index]
        {
            get
            {
                lock (this.Lock)
                {
                    if (index < 0 || index >= this.Size)
                        throw new Exception.IndexOutOfRange();
                    unsafe { return ((byte*)this)[index]; }
                }
            }
            set
            {
                lock (this.Lock)
                {
                    if (index < 0 || index >= this.Size)
                        throw new Exception.IndexOutOfRange();
                    unsafe { ((byte*)this)[index] = value; }
                }
            }
        }
        int Collection.IVector<byte>.Count { get { return this.Size; } }
        System.Collections.Generic.IEnumerator<byte> System.Collections.Generic.IEnumerable<byte>.GetEnumerator()
        {
            for (int i = 0; i < this.Size; i++)
                yield return (this as Collection.IVector<byte>)[i];
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < this.Size; i++)
                yield return (this as Collection.IVector<byte>)[i];
        }
        bool IEquatable<Collection.IVector<byte>>.Equals(Collection.IVector<byte> other)
        {
            bool result = other.NotNull() && this.Size == other.Count;
            for (int i = 0; result && i < this.Size; i++)
                result &= (this as Collection.IVector<byte>)[i] == other[i];
            return result;
        }
        #endregion
        public unsafe static implicit operator void*(Sized pointer)
        {
            return ((IntPtr) pointer).ToPointer();
        }
        public static implicit operator IntPtr(Sized pointer)
        {
            return (IntPtr) (Pointer) pointer;
        }

		public Sized Copy()
		{
			Vector<byte> result = new Vector<byte>(this.Size);
			System.Runtime.InteropServices.Marshal.Copy(this, result.Data, 0, this.Size);
			return result;
		}
	}
}