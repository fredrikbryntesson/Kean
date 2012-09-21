// 
//  Log.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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

namespace Kean.Core.Error
{
	public static class Log
	{
#if DEBUG
		static bool catchErrors = false;
#else
		static bool catchErrors = true;
#endif
		public static bool CatchErrors
		{ 
			get { return Log.catchErrors; }
			set
			{
				if (Log.catchErrors != value)
					Log.CatchErrorsChanged.Call(Log.catchErrors = value);
			}
		}
		public static event Action<bool> CatchErrorsChanged;
		public static event Action<Error.IError> OnAppend;
		public static void Append(Error.IError entry)
		{
			Log.OnAppend.Call(entry);
		}
		public static void Append(Error.Level level, string title, System.Exception exception)
		{
			Log.Append(Error.Entry.Create(level, title, exception));
		}
		public static void Append(Error.Level level, string title, string message)
		{
			Log.Append(Error.Entry.Create(level, title, message));
		}
		#region Wrap
		public static Action Wrap(Action task)
		{
			return Log.Wrap("Task failed", task);
		}
		public static Action Wrap(string title, Action task)
		{
			return Log.CatchErrors ? () =>
			{
				try { task.Call(); }
				catch (System.Threading.ThreadInterruptedException) { throw; }
				catch (Error.Exception e)
				{
					Log.Append(e);
				}
				catch (System.Exception e)
				{
					Log.Append(Error.Entry.Create(Error.Level.Recoverable, title, e));
				}
			} : (Action)task.Call;
		}
		public static Action<T> Wrap<T>(Action<T> task)
		{
			return Log.Wrap("Task failed", task);
		}
		public static Action<T> Wrap<T>(string title, Action<T> task)
		{
			return Log.CatchErrors ? (argument) =>
			{
				try { task.Call(argument); }
				catch (System.Threading.ThreadInterruptedException) { throw; }
				catch (Error.Exception e)
				{
					Log.Append(e);
				}
				catch (System.Exception e)
				{
					Log.Append(Error.Entry.Create(Error.Level.Recoverable, title, e));
				}
			} : (Action<T>)task.Call;
		}
		public static Action<T1, T2> Wrap<T1, T2>(Action<T1, T2> task)
		{
			return Log.Wrap("Task failed", task);
		}
		public static Action<T1, T2> Wrap<T1, T2>(string title, Action<T1, T2> task)
		{
			return Log.CatchErrors ? (argument1, argument2) =>
			{
				try { task.Call(argument1, argument2); }
				catch (System.Threading.ThreadInterruptedException) { throw; }
				catch (Error.Exception e)
				{
					Log.Append(e);
				}
				catch (System.Exception e)
				{
					Log.Append(Error.Entry.Create(Error.Level.Recoverable, title, e));
				}
			} : (Action<T1, T2>)task.Call;
		}
		#endregion
	}
}
