// 
//  Media.cs
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

namespace Kean.Cli.Processor.Test.Command
{
	public class Media
	{
		MediaState state;
		[Property("state")]
		[Notify("StateChanged")]
		public MediaState State 
		{
			get { return this.state; }
			private set
			{
				this.state = value;
				switch (this.state)
				{
					case MediaState.Closed:
						this.Position = TimeSpan.Zero;
						break;
				}
				this.StateChanged.Call(this.state);
			}
		}
		public event Action<MediaState> StateChanged;

		TimeSpan position;
		[Property("position")]
		[Notify("StateChanged")]
		public TimeSpan Position 
		{
			get { return this.position; }
			set
			{
				this.position = value;
				this.PositionChanged.Call(this.position);
			}
		}
		public event Action<TimeSpan> PositionChanged;

		public Media()
		{
		}

		[Method("play")]
		public bool Play()
		{
			bool result;
			if (result = this.State == MediaState.Paused)
				this.State = MediaState.Playing;
			return result;
		}
		[Method("pause")]
		public bool Pause()
		{
			bool result;
			if (result = this.State == MediaState.Playing)
				this.State = MediaState.Paused;
			return result;
		}
		[Method("eject")]
		public bool Eject()
		{
			bool result;
			if (result = this.State != MediaState.Closed)
				this.State = MediaState.Closed;
			return result;
		}
		[Method("open", "Open a resource.", "Opens a resource and pauses.")]
		public bool Open(string resource)
		{
			bool result;
			if (result = this.State == MediaState.Closed)
				this.State = MediaState.Paused;
			return result;
		}
		[Method("changeState")]
		public void ChangeState(MediaState state)
		{
			this.State = state;
		}
	}
}
