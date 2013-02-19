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
using Kean.Core.Extension;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Gui.OpenGL.Backend.Input
{
    class Pointer : 
		Gui.Backend.Pointer
    {
        Window targetWindow;

        public override Gui.Backend.PointerButtons Buttons
        {
            get { return new PointerButtons(this.targetWindow.InputDriver.Mouse[0]); }
        }
        public override Gui.Backend.PointerWheels Wheels
        {
            get { return new PointerWheels(this.targetWindow.InputDriver.Mouse[0]); }
        }
        public override Geometry2D.Single.Point Position
        {
            get { return new Geometry2D.Single.Point(this.targetWindow.InputDriver.Mouse[0].X, this.targetWindow.InputDriver.Mouse[0].Y); }
        }
        Gui.Input.Cursor cursor;
        public override Gui.Input.Cursor Cursor
        {
            get { return this.cursor; }
            set
            {
                if (this.cursor != value)
                {
                    this.cursor = value;
                    this.UpdateCursor();
                }
            }
        }

        internal Pointer(Window targetWindow)
        {
            this.targetWindow = targetWindow;
            this.targetWindow.InputDriver.Mouse[0].Move += (object sender, OpenTK.Input.MouseMoveEventArgs e) =>
            {
                this.UpdateCursor();
                this.OnMove(new Geometry2D.Single.Point(e.X, e.Y), new Geometry2D.Single.Size(e.XDelta, e.YDelta));
            };
            this.targetWindow.InputDriver.Mouse[0].ButtonDown += (object sender, OpenTK.Input.MouseButtonEventArgs e) => 
            {
                this.UpdateCursor();
                this.OnButton(new Geometry2D.Single.Point(e.X, e.Y), (int)e.Button, e.IsPressed);
            };
            this.targetWindow.InputDriver.Mouse[0].ButtonUp += (object sender, OpenTK.Input.MouseButtonEventArgs e) =>
            {
                this.UpdateCursor();
                this.OnButton(new Geometry2D.Single.Point(e.X, e.Y), (int)e.Button, e.IsPressed);
            };
            this.targetWindow.InputDriver.Mouse[0].WheelChanged += (object sender, OpenTK.Input.MouseWheelEventArgs e) =>
            {
                this.UpdateCursor();
                this.OnWheel(new Geometry2D.Single.Point(e.X, e.Y), 0, e.Delta);
            };
        }

        void UpdateCursor()
        {
            switch (this.Cursor)
            {
                default:
                case Gui.Input.Cursor.Parent:
                case Gui.Input.Cursor.Default:
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    break;
                case Gui.Input.Cursor.Wait:
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                    break;
				case Gui.Input.Cursor.Starting:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.AppStarting;
					break;
				case Gui.Input.Cursor.IBeam:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.IBeam;
					break;
				case Gui.Input.Cursor.Cross:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Cross;
					break;
				case Gui.Input.Cursor.Hand:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
					break;
				case Gui.Input.Cursor.Arrow:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Arrow;
					break;
				case Gui.Input.Cursor.ArrowUp:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.UpArrow;
					break;
				case Gui.Input.Cursor.No:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.No;
					break;
				case Gui.Input.Cursor.NoMoveHorizontal:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.NoMoveHoriz;
					break;
				case Gui.Input.Cursor.NoMoveVertical:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.NoMoveVert;
					break;
				case Gui.Input.Cursor.NoMove:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.NoMove2D;
					break;
				case Gui.Input.Cursor.Help:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Help;
					break;
				case Gui.Input.Cursor.SplitHorizontal:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.HSplit;
					break;
				case Gui.Input.Cursor.SplitVertical:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.VSplit;
					break;
				case Gui.Input.Cursor.SizeAll:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeAll;
					break;
				case Gui.Input.Cursor.SizeWestEast:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeWE;
					break;
				case Gui.Input.Cursor.SizeNorthSouth:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeNS;
					break;
				case Gui.Input.Cursor.SizeNorthEastSouthWest:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeNESW;
					break;
				case Gui.Input.Cursor.SizeNorthWestSouthEast:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeNWSE;
					break;
				case Gui.Input.Cursor.PanWest:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.PanWest;
					break;
				case Gui.Input.Cursor.PanEast:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.PanEast;
					break;
				case Gui.Input.Cursor.PanNorth:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.PanNorth;
					break;
				case Gui.Input.Cursor.PanSouth:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.PanSouth;
					break;
				case Gui.Input.Cursor.PanNorthWest:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.PanNW;
					break;
				case Gui.Input.Cursor.PanNorthEast:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.PanNE;
					break;
				case Gui.Input.Cursor.PanSouthWest:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.PanSW;
					break;
				case Gui.Input.Cursor.PanSouthEast:
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.PanSE;
					break;
			}
        }
    }
}
