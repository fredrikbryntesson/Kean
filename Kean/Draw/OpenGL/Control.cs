// 
//  Control.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2013 Simon Mika
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
using Kean.Extension;
using Parallel = Kean.Parallel;
using Forms = System.Windows.Forms;

namespace Kean.Draw.OpenGL
{
	public class Control :
		Forms.UserControl
	{
		public event Action Initialized;
		public event Action Draw;

		public Parallel.ThreadPool ThreadPool { get { return this.backend.NotNull() ? this.backend.ThreadPool : null; } }

		bool designMode;
		protected new bool DesignMode
		{
			get { return this.designMode || base.DesignMode || System.Diagnostics.Process.GetCurrentProcess().ProcessName.StartsWith("devenv") || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime; }
			set { this.designMode = value; }
		}
		Backend.Control backend;
		public Control()
		{
			this.DesignMode = this.DesignMode;
			this.SuspendLayout();
			// 
			// backend
			// 
			if (!this.DesignMode)
			{
				this.backend = Backend.Control.Create();
				this.backend.Initialized += () => this.Initialized.Call();
				this.backend.Draw += () => this.Draw.Call();
				this.backend.AutoSize = true;
				this.backend.BackColor = System.Drawing.Color.Transparent;
				this.backend.Dock = System.Windows.Forms.DockStyle.Fill;
				this.backend.Location = new System.Drawing.Point(0, 0);
				this.backend.Name = "backend";
				this.backend.Size = new System.Drawing.Size(150, 150);
				this.backend.TabIndex = 0;
				this.backend.VSync = false;
				this.Controls.Add(this.backend);
			}
			// 
			// Control
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "Viewer";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		public void Redraw()
		{
			this.backend.Invalidate();
		}
		protected override void Dispose(bool disposing)
		{
			if (this.backend.NotNull())
			{
				this.backend.Dispose();
				this.backend = null;
			}
		}
	}
}
