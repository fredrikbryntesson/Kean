﻿// 
//  Server.cs
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

namespace Kean.IO.Net.Telnet
{
	enum Option :
		byte
	{
		SuppressGoAhead = 3,
		Status = 5,
		Echo = 1,
		TimingMark = 6,
		TerminalType = 24,
		WindowSize = 31,
		TerminalSpeed = 32,
		RemoteFlowControl = 33,
		LineMode = 34,
		EnvironmentVariables = 36,
	}
}
