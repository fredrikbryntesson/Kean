// 
//  Settings.cs
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
using Kean.Core.Basis.Extension;

namespace Kean.IO.Serial
{
	public struct Settings :
		IEquatable<Settings>
	{
		public int BaudRate { get; set; }
		public int DataBits { get; set; }
		public Parity Parity { get; set; }
		public int StopBits { get; set; }

		string ParityString
		{
			get
			{
				string result;
				switch (this.Parity)
				{
				default:
				case Parity.None: result = "N"; break;
				case Parity.Odd: result = "O"; break;
				case Parity.Even: result = "E"; break;
				}
				return result;
			}
		}

		#region Object Overrides
		public override string ToString ()
		{
			return this;
		}
		public override int GetHashCode ()
		{
			return this.BaudRate.GetHashCode() ^
					this.DataBits.GetHashCode() ^
					this.Parity.GetHashCode() ^
					this.StopBits.GetHashCode();
		}
		public override bool Equals(object other)
		{
			return other is Settings && this.Equals((Settings) other);
		}
		#endregion
		#region IEquatable<Settings>
		public bool Equals(Settings other)
		{
			return this.BaudRate == other.BaudRate &&
					this.DataBits == other.DataBits &&
					this.Parity == other.Parity &&
					this.StopBits == other.StopBits;
		}
		#endregion
		#region Operators
		public static bool operator ==(Settings left, Settings right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(Settings left, Settings right)
		{
			return !left.Equals(right);
		}
		#endregion
		#region Casts
		public static implicit operator string(Settings settings)
		{
			return settings.BaudRate + " " + settings.DataBits + settings.ParityString + settings.StopBits;
		}
		System.Collections.Generic.IEnumerable<string> SplitSettings(string settings)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			int index = 0;
			while (settings.Length > index)
			{
				if (char.IsDigit(settings[index]))
				{
					while (settings.Length > index && char.IsDigit(settings[index]))
						result.Append(settings[index++]);
					yield return result.ToString();
					result = new System.Text.StringBuilder();
				}
				else if (char.IsLetter(settings[index]))
				{
					while (settings.Length > index && char.IsLetter(settings[index]))
						result.Append(settings[index++]);
					yield return result.ToString();
					result = new System.Text.StringBuilder();
				}
				else
					index++;
			}
		}
		public static implicit operator Settings(string settings)
		{
			Settings result = new Settings()
			{
				BaudRate = 9600,
				DataBits = 8,
				Parity = Parity.None,
			};
			if (settings.NotEmpty())
			{
				foreach (string setting in result.SplitSettings(settings))
				{
					int number;
					if (int.TryParse(setting, out number) && number > 0)
					{
						if (number < 3)
							result.StopBits = number;
						else if (number < 100)
							result.DataBits = number;
						else
							result.BaudRate = number;
					}
					else
					{
						switch (setting.ToLower())
						{
						case "n":
						case "none":
							result.Parity = Parity.None;
							break;
						case "o":
						case "odd":
							result.Parity = Parity.Odd;
							break;
						case "e":
						case "even":
							result.Parity = Parity.Even;
							break;
						}
					}
				}
			}
			return result;
		}
		public static implicit operator Settings(int baudRate)
		{
			return new Settings()
			{
				BaudRate = baudRate,
				DataBits = 8,
				Parity = Parity.None,
			};
		}
		#endregion
	}
}

