// 
//  ITransform.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;

namespace Attraction.Math.Geometry3D.Abstract
{
    public interface ITransform<V>
        where V : struct
    {
        V A { get; set; }
        V B { get; set; }
        V C { get; set; }
        V D { get; set; }
        V E { get; set; }
        V F { get; set; }
        V G { get; set; }
        V H { get; set; }
        V I { get; set; }
        V J { get; set; }
        V K { get; set; }
        V L { get; set; }
    }
}
