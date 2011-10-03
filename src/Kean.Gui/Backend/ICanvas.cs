// 
//  ICanvas.cs
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
    public interface ICanvas :
		IDrawable,
		IDisposable
    {
		bool TextAntiAlias { get; set; }
		//Svg.Svg.Renderer Measure { get; }
		Geometry2D.Single.Box Clip { get; }
        void Push(Geometry2D.Single.Box clip, Geometry2D.Single.Transform transform);
        void Pop();
        
        void Clear(Geometry2D.Single.Box area);

		ICanvas Create(Geometry2D.Single.Box bounds);
		void Draw(IDrawable viewable);

		//Svg.Svg.Renderer CreateRenderer(Geometry2D.Single.Size size);
		//ICache GetCache(Svg.Svg.Renderer renderer);
		void Draw(ICache cache, Geometry2D.Single.Box source, Geometry2D.Single.Box destination);
	}
}
