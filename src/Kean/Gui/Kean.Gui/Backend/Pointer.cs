// 
//  Pointer.cs
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
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Core.Extension;

namespace Kean.Gui.Backend
{
    public abstract class Pointer
    {
        public abstract Geometry2D.Single.Point Position { get; }

        public abstract PointerButtons Buttons { get; }
        public abstract PointerWheels Wheels { get; }

        public abstract Input.Cursor Cursor { get; set; }

        internal event Action<Geometry2D.Single.Point, Geometry2D.Single.Size> Move;
        internal event Action<Geometry2D.Single.Point, int, bool> Button;
        internal event Action<Geometry2D.Single.Point, int, int> Wheel;

        protected void OnMove(Geometry2D.Single.Point position, Geometry2D.Single.Size delta)
        {
            if (this.Move.NotNull())
                this.Move(position, delta);
        }
        protected void OnButton(Geometry2D.Single.Point position, int button, bool pressed)
        {
            if (this.Button.NotNull())
                this.Button(position, button, pressed);
        }
        protected void OnWheel(Geometry2D.Single.Point position, int wheel, int delta)
        {
            if (this.Wheel.NotNull())
                this.Wheel(position, wheel, delta);
        }
    }
}
