// 
//  Keyboard.cs
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

namespace Kean.Gui.OpenGL.Backend.Input
{
    class Keyboard : 
		Gui.Backend.Keyboard
    {
        
        Window targetWindow;
        
        internal Keyboard(Window targetWindow)
        {
            this.targetWindow = targetWindow;
            this.targetWindow.InputDriver.Keyboard[0].KeyDown += (object sender, OpenTK.Input.KeyboardKeyEventArgs e) =>
            {
                this.OnPress(new Gui.Input.Key(Keyboard.Convert(e.Key), 
                    this.targetWindow.InputDriver.Keyboard[0][OpenTK.Input.Key.ShiftLeft] || this.targetWindow.InputDriver.Keyboard[0][OpenTK.Input.Key.ShiftRight],
                    this.targetWindow.InputDriver.Keyboard[0][OpenTK.Input.Key.ControlLeft] || this.targetWindow.InputDriver.Keyboard[0][OpenTK.Input.Key.ControlRight],
                    this.targetWindow.InputDriver.Keyboard[0][OpenTK.Input.Key.AltLeft] || this.targetWindow.InputDriver.Keyboard[0][OpenTK.Input.Key.AltRight]));
            };
            this.targetWindow.KeyPress += (object sender, OpenTK.KeyPressEventArgs e) =>
            {
                if (!char.IsControl(e.KeyChar))
                    this.OnCharacter(e.KeyChar);
            };
        }

        public override Gui.Backend.KeyboardButtons Buttons
        {
            get { return new KeyboardButtons(this.targetWindow.InputDriver.Keyboard[0]); }
        }

        private static Gui.Input.Keys Convert(OpenTK.Input.Key key)
        {
            Gui.Input.Keys result = Gui.Input.Keys.None;
            switch (key)
            {
                default:
                case OpenTK.Input.Key.Unknown: result = Gui.Input.Keys.None; break;
                case OpenTK.Input.Key.ShiftLeft: result = Gui.Input.Keys.ShiftLeft; break;
                case OpenTK.Input.Key.ShiftRight: result = Gui.Input.Keys.ShiftRight; break;
                case OpenTK.Input.Key.ControlLeft: result = Gui.Input.Keys.ControlLeft; break;
                case OpenTK.Input.Key.ControlRight: result = Gui.Input.Keys.ControlRight; break;
                case OpenTK.Input.Key.AltLeft: result = Gui.Input.Keys.AltLeft; break;
                case OpenTK.Input.Key.AltRight: result = Gui.Input.Keys.AltRight; break;
                case OpenTK.Input.Key.WinLeft: result = Gui.Input.Keys.StartLeft; break;
                case OpenTK.Input.Key.WinRight: result = Gui.Input.Keys.StartRight; break;
                case OpenTK.Input.Key.Menu: result = Gui.Input.Keys.Menu; break;
                case OpenTK.Input.Key.F1: result = Gui.Input.Keys.Function01; break;
                case OpenTK.Input.Key.F2: result = Gui.Input.Keys.Function02; break;
                case OpenTK.Input.Key.F3: result = Gui.Input.Keys.Function03; break;
                case OpenTK.Input.Key.F4: result = Gui.Input.Keys.Function04; break;
                case OpenTK.Input.Key.F5: result = Gui.Input.Keys.Function05; break;
                case OpenTK.Input.Key.F6: result = Gui.Input.Keys.Function06; break;
                case OpenTK.Input.Key.F7: result = Gui.Input.Keys.Function07; break;
                case OpenTK.Input.Key.F8: result = Gui.Input.Keys.Function08; break;
                case OpenTK.Input.Key.F9: result = Gui.Input.Keys.Function09; break;
                case OpenTK.Input.Key.F10: result = Gui.Input.Keys.Function10; break;
                case OpenTK.Input.Key.F11: result = Gui.Input.Keys.Function11; break;
                case OpenTK.Input.Key.F12: result = Gui.Input.Keys.Function12; break;
                case OpenTK.Input.Key.F13: result = Gui.Input.Keys.Function13; break;
                case OpenTK.Input.Key.F14: result = Gui.Input.Keys.Function14; break;
                case OpenTK.Input.Key.F15: result = Gui.Input.Keys.Function15; break;
                case OpenTK.Input.Key.F16: result = Gui.Input.Keys.Function16; break;
                case OpenTK.Input.Key.F17: result = Gui.Input.Keys.Function17; break;
                case OpenTK.Input.Key.F18: result = Gui.Input.Keys.Function18; break;
                case OpenTK.Input.Key.F19: result = Gui.Input.Keys.Function19; break;
                case OpenTK.Input.Key.F20: result = Gui.Input.Keys.Function20; break;
                case OpenTK.Input.Key.F21: result = Gui.Input.Keys.Function21; break;
                case OpenTK.Input.Key.F22: result = Gui.Input.Keys.Function22; break;
                case OpenTK.Input.Key.F23: result = Gui.Input.Keys.Function23; break;
                case OpenTK.Input.Key.F24: result = Gui.Input.Keys.Function24; break;
                case OpenTK.Input.Key.F25: result = Gui.Input.Keys.Function25; break;
                case OpenTK.Input.Key.F26: result = Gui.Input.Keys.Function26; break;
                case OpenTK.Input.Key.F27: result = Gui.Input.Keys.Function27; break;
                case OpenTK.Input.Key.F28: result = Gui.Input.Keys.Function28; break;
                case OpenTK.Input.Key.F29: result = Gui.Input.Keys.Function29; break;
                case OpenTK.Input.Key.F30: result = Gui.Input.Keys.Function30; break;
                case OpenTK.Input.Key.F31: result = Gui.Input.Keys.Function31; break;
                case OpenTK.Input.Key.F32: result = Gui.Input.Keys.Function32; break;
                case OpenTK.Input.Key.F33: result = Gui.Input.Keys.Function33; break;
                case OpenTK.Input.Key.F34: result = Gui.Input.Keys.Function34; break;
                case OpenTK.Input.Key.F35: result = Gui.Input.Keys.Function35; break;
                case OpenTK.Input.Key.Up: result = Gui.Input.Keys.MoveUp; break;
                case OpenTK.Input.Key.Down: result = Gui.Input.Keys.MoveDown; break;
                case OpenTK.Input.Key.Left: result = Gui.Input.Keys.MoveLeft; break;
                case OpenTK.Input.Key.Right: result = Gui.Input.Keys.MoveRight; break;
                case OpenTK.Input.Key.Enter: result = Gui.Input.Keys.Enter; break;
                case OpenTK.Input.Key.Escape: result = Gui.Input.Keys.Escape; break;
                case OpenTK.Input.Key.Space: result = Gui.Input.Keys.Space; break;
                case OpenTK.Input.Key.Tab: result = Gui.Input.Keys.Tab; break;
                case OpenTK.Input.Key.BackSpace: result = Gui.Input.Keys.BackSpace; break;
                case OpenTK.Input.Key.Insert: result = Gui.Input.Keys.Insert; break;
                case OpenTK.Input.Key.Delete: result = Gui.Input.Keys.Delete; break;
                case OpenTK.Input.Key.PageUp: result = Gui.Input.Keys.PageUp; break;
                case OpenTK.Input.Key.PageDown: result = Gui.Input.Keys.PageDown; break;
                case OpenTK.Input.Key.Home: result = Gui.Input.Keys.Home; break;
                case OpenTK.Input.Key.End: result = Gui.Input.Keys.End; break;
                case OpenTK.Input.Key.CapsLock: result = Gui.Input.Keys.CapitalsLock; break;
                case OpenTK.Input.Key.ScrollLock: result = Gui.Input.Keys.ScrollLock; break;
                case OpenTK.Input.Key.PrintScreen: result = Gui.Input.Keys.PrintScreen; break;
                case OpenTK.Input.Key.Pause: result = Gui.Input.Keys.Pause; break;
                case OpenTK.Input.Key.NumLock: result = Gui.Input.Keys.NumericLock; break;
                case OpenTK.Input.Key.Clear: result = Gui.Input.Keys.Clear; break;
                case OpenTK.Input.Key.Sleep: result = Gui.Input.Keys.Sleep; break;
                case OpenTK.Input.Key.Keypad0: result = Gui.Input.Keys.Keypad0; break;
                case OpenTK.Input.Key.Keypad1: result = Gui.Input.Keys.Keypad1; break;
                case OpenTK.Input.Key.Keypad2: result = Gui.Input.Keys.Keypad2; break;
                case OpenTK.Input.Key.Keypad3: result = Gui.Input.Keys.Keypad3; break;
                case OpenTK.Input.Key.Keypad4: result = Gui.Input.Keys.Keypad4; break;
                case OpenTK.Input.Key.Keypad5: result = Gui.Input.Keys.Keypad5; break;
                case OpenTK.Input.Key.Keypad6: result = Gui.Input.Keys.Keypad6; break;
                case OpenTK.Input.Key.Keypad7: result = Gui.Input.Keys.Keypad7; break;
                case OpenTK.Input.Key.Keypad8: result = Gui.Input.Keys.Keypad8; break;
                case OpenTK.Input.Key.Keypad9: result = Gui.Input.Keys.Keypad9; break;
                case OpenTK.Input.Key.KeypadDivide: result = Gui.Input.Keys.KeypadDivide; break;
                case OpenTK.Input.Key.KeypadMultiply: result = Gui.Input.Keys.KeypadMultiply; break;
                case OpenTK.Input.Key.KeypadSubtract: result = Gui.Input.Keys.KeypadSubtract; break;
                case OpenTK.Input.Key.KeypadAdd: result = Gui.Input.Keys.KeypadAdd; break;
                case OpenTK.Input.Key.KeypadDecimal: result = Gui.Input.Keys.KeypadDecimal; break;
                case OpenTK.Input.Key.KeypadEnter: result = Gui.Input.Keys.KeypadDivide; break;
                case OpenTK.Input.Key.A: result = Gui.Input.Keys.LetterA; break;
                case OpenTK.Input.Key.B: result = Gui.Input.Keys.LetterB; break;
                case OpenTK.Input.Key.C: result = Gui.Input.Keys.LetterC; break;
                case OpenTK.Input.Key.D: result = Gui.Input.Keys.LetterD; break;
                case OpenTK.Input.Key.E: result = Gui.Input.Keys.LetterE; break;
                case OpenTK.Input.Key.F: result = Gui.Input.Keys.LetterF; break;
                case OpenTK.Input.Key.G: result = Gui.Input.Keys.LetterG; break;
                case OpenTK.Input.Key.H: result = Gui.Input.Keys.LetterH; break;
                case OpenTK.Input.Key.I: result = Gui.Input.Keys.LetterI; break;
                case OpenTK.Input.Key.J: result = Gui.Input.Keys.LetterJ; break;
                case OpenTK.Input.Key.K: result = Gui.Input.Keys.LetterK; break;
                case OpenTK.Input.Key.L: result = Gui.Input.Keys.LetterL; break;
                case OpenTK.Input.Key.M: result = Gui.Input.Keys.LetterM; break;
                case OpenTK.Input.Key.N: result = Gui.Input.Keys.LetterN; break;
                case OpenTK.Input.Key.O: result = Gui.Input.Keys.LetterO; break;
                case OpenTK.Input.Key.P: result = Gui.Input.Keys.LetterP; break;
                case OpenTK.Input.Key.Q: result = Gui.Input.Keys.LetterQ; break;
                case OpenTK.Input.Key.R: result = Gui.Input.Keys.LetterR; break;
                case OpenTK.Input.Key.S: result = Gui.Input.Keys.LetterS; break;
                case OpenTK.Input.Key.T: result = Gui.Input.Keys.LetterT; break;
                case OpenTK.Input.Key.U: result = Gui.Input.Keys.LetterU; break;
                case OpenTK.Input.Key.V: result = Gui.Input.Keys.LetterV; break;
                case OpenTK.Input.Key.W: result = Gui.Input.Keys.LetterW; break;
                case OpenTK.Input.Key.X: result = Gui.Input.Keys.LetterX; break;
                case OpenTK.Input.Key.Y: result = Gui.Input.Keys.LetterY; break;
                case OpenTK.Input.Key.Z: result = Gui.Input.Keys.LetterZ; break;
                case OpenTK.Input.Key.Number0: result = Gui.Input.Keys.Number0; break;
                case OpenTK.Input.Key.Number1: result = Gui.Input.Keys.Number1; break;
                case OpenTK.Input.Key.Number2: result = Gui.Input.Keys.Number2; break;
                case OpenTK.Input.Key.Number3: result = Gui.Input.Keys.Number3; break;
                case OpenTK.Input.Key.Number4: result = Gui.Input.Keys.Number4; break;
                case OpenTK.Input.Key.Number5: result = Gui.Input.Keys.Number5; break;
                case OpenTK.Input.Key.Number6: result = Gui.Input.Keys.Number6; break;
                case OpenTK.Input.Key.Number7: result = Gui.Input.Keys.Number7; break;
                case OpenTK.Input.Key.Number8: result = Gui.Input.Keys.Number8; break;
                case OpenTK.Input.Key.Number9: result = Gui.Input.Keys.Number9; break;
                case OpenTK.Input.Key.Tilde: result = Gui.Input.Keys.Tilde; break;
                case OpenTK.Input.Key.Minus: result = Gui.Input.Keys.Minus; break;
                case OpenTK.Input.Key.Plus: result = Gui.Input.Keys.Plus; break;
                case OpenTK.Input.Key.BracketLeft: result = Gui.Input.Keys.BracketLeft; break;
                case OpenTK.Input.Key.BracketRight: result = Gui.Input.Keys.BracketRight; break;
                case OpenTK.Input.Key.Semicolon: result = Gui.Input.Keys.Semicolon; break;
                case OpenTK.Input.Key.Quote: result = Gui.Input.Keys.Quote; break;
                case OpenTK.Input.Key.Comma: result = Gui.Input.Keys.Comma; break;
                case OpenTK.Input.Key.Period: result = Gui.Input.Keys.Period; break;
                case OpenTK.Input.Key.Slash: result = Gui.Input.Keys.Slash; break;
                case OpenTK.Input.Key.BackSlash: result = Gui.Input.Keys.BackSlash; break;
            }
            return result;
        }
    }
}
