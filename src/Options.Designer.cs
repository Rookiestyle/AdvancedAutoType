namespace AlternateAutoType
{
	partial class Options
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.gHotkeys = new System.Windows.Forms.GroupBox();
			this.cbPWEnter = new System.Windows.Forms.CheckBox();
			this.lGATP = new System.Windows.Forms.Label();
			this.tbGAT = new KeePass.UI.HotKeyControlEx();
			this.cbPWHotkey = new System.Windows.Forms.ComboBox();
			this.tbPWOnly = new KeePass.UI.HotKeyControlEx();
			this.lGAT = new System.Windows.Forms.Label();
			this.lAAT = new System.Windows.Forms.Label();
			this.tbAAT = new KeePass.UI.HotKeyControlEx();
			this.gIntegration = new System.Windows.Forms.GroupBox();
			this.cbColumnsRememberSort = new System.Windows.Forms.CheckBox();
			this.cbExcludeExpiredGroups = new System.Windows.Forms.CheckBox();
			this.cbKeepATOpen = new System.Windows.Forms.CheckBox();
			this.cbSpecialColumns = new System.Windows.Forms.CheckBox();
			this.cbDBColumn = new System.Windows.Forms.CheckBox();
			this.cbColumnsSortable = new System.Windows.Forms.CheckBox();
			this.gHotkeys.SuspendLayout();
			this.gIntegration.SuspendLayout();
			this.SuspendLayout();
			// 
			// gHotkeys
			// 
			this.gHotkeys.Controls.Add(this.cbPWEnter);
			this.gHotkeys.Controls.Add(this.lGATP);
			this.gHotkeys.Controls.Add(this.tbGAT);
			this.gHotkeys.Controls.Add(this.cbPWHotkey);
			this.gHotkeys.Controls.Add(this.tbPWOnly);
			this.gHotkeys.Controls.Add(this.lGAT);
			this.gHotkeys.Controls.Add(this.lAAT);
			this.gHotkeys.Controls.Add(this.tbAAT);
			this.gHotkeys.Dock = System.Windows.Forms.DockStyle.Top;
			this.gHotkeys.Location = new System.Drawing.Point(15, 5);
			this.gHotkeys.Name = "gHotkeys";
			this.gHotkeys.Padding = new System.Windows.Forms.Padding(18);
			this.gHotkeys.Size = new System.Drawing.Size(525, 180);
			this.gHotkeys.TabIndex = 6;
			this.gHotkeys.TabStop = false;
			this.gHotkeys.Text = "Hotkeys";
			// 
			// cbPWEnter
			// 
			this.cbPWEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbPWEnter.AutoSize = true;
			this.cbPWEnter.Location = new System.Drawing.Point(260, 102);
			this.cbPWEnter.Name = "cbPWEnter";
			this.cbPWEnter.Size = new System.Drawing.Size(108, 24);
			this.cbPWEnter.TabIndex = 4;
			this.cbPWEnter.Text = "PW+Enter";
			this.cbPWEnter.UseVisualStyleBackColor = true;
			this.cbPWEnter.CheckedChanged += new System.EventHandler(this.cbPWEnter_CheckedChanged);
			// 
			// lGATP
			// 
			this.lGATP.AutoSize = true;
			this.lGATP.Location = new System.Drawing.Point(15, 70);
			this.lGATP.Name = "lGATP";
			this.lGATP.Size = new System.Drawing.Size(150, 20);
			this.lGATP.TabIndex = 13;
			this.lGATP.Text = "Autotype Password:";
			// 
			// tbGAT
			// 
			this.tbGAT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbGAT.Location = new System.Drawing.Point(260, 28);
			this.tbGAT.Name = "tbGAT";
			this.tbGAT.ReadOnly = true;
			this.tbGAT.Size = new System.Drawing.Size(248, 26);
			this.tbGAT.TabIndex = 1;
			this.tbGAT.TabStop = false;
			// 
			// cbPWHotkey
			// 
			this.cbPWHotkey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPWHotkey.FormattingEnabled = true;
			this.cbPWHotkey.Items.AddRange(new object[] {
            "Password Only Hotkey",
            "Password+Enter Hotkey"});
			this.cbPWHotkey.Location = new System.Drawing.Point(15, 70);
			this.cbPWHotkey.Name = "cbPWHotkey";
			this.cbPWHotkey.Size = new System.Drawing.Size(210, 28);
			this.cbPWHotkey.TabIndex = 1;
			// 
			// tbPWOnly
			// 
			this.tbPWOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbPWOnly.Location = new System.Drawing.Point(260, 70);
			this.tbPWOnly.Name = "tbPWOnly";
			this.tbPWOnly.Size = new System.Drawing.Size(248, 26);
			this.tbPWOnly.TabIndex = 3;
			// 
			// lGAT
			// 
			this.lGAT.AutoSize = true;
			this.lGAT.Location = new System.Drawing.Point(15, 30);
			this.lGAT.Name = "lGAT";
			this.lGAT.Size = new System.Drawing.Size(185, 20);
			this.lGAT.TabIndex = 11;
			this.lGAT.Text = "Global AutoType &Hotkey:";
			// 
			// lAAT
			// 
			this.lAAT.AutoSize = true;
			this.lAAT.Location = new System.Drawing.Point(15, 140);
			this.lAAT.Name = "lAAT";
			this.lAAT.Size = new System.Drawing.Size(204, 20);
			this.lAAT.TabIndex = 9;
			this.lAAT.Text = "Alternate AutoType Hotkey:";
			// 
			// tbAAT
			// 
			this.tbAAT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbAAT.Location = new System.Drawing.Point(260, 140);
			this.tbAAT.Name = "tbAAT";
			this.tbAAT.Size = new System.Drawing.Size(248, 26);
			this.tbAAT.TabIndex = 5;
			// 
			// gIntegration
			// 
			this.gIntegration.AutoSize = true;
			this.gIntegration.Controls.Add(this.cbColumnsRememberSort);
			this.gIntegration.Controls.Add(this.cbExcludeExpiredGroups);
			this.gIntegration.Controls.Add(this.cbKeepATOpen);
			this.gIntegration.Controls.Add(this.cbSpecialColumns);
			this.gIntegration.Controls.Add(this.cbDBColumn);
			this.gIntegration.Controls.Add(this.cbColumnsSortable);
			this.gIntegration.Dock = System.Windows.Forms.DockStyle.Top;
			this.gIntegration.Location = new System.Drawing.Point(15, 185);
			this.gIntegration.Name = "gIntegration";
			this.gIntegration.Size = new System.Drawing.Size(525, 229);
			this.gIntegration.TabIndex = 7;
			this.gIntegration.TabStop = false;
			this.gIntegration.Text = "Integration";
			// 
			// cbColumnsRememberSort
			// 
			this.cbColumnsRememberSort.AutoSize = true;
			this.cbColumnsRememberSort.Location = new System.Drawing.Point(35, 60);
			this.cbColumnsRememberSort.Name = "cbColumnsRememberSort";
			this.cbColumnsRememberSort.Size = new System.Drawing.Size(205, 24);
			this.cbColumnsRememberSort.TabIndex = 95;
			this.cbColumnsRememberSort.Text = "Remember sort settings";
			this.cbColumnsRememberSort.UseVisualStyleBackColor = true;
			// 
			// cbExcludeExpiredGroups
			// 
			this.cbExcludeExpiredGroups.AutoSize = true;
			this.cbExcludeExpiredGroups.Location = new System.Drawing.Point(15, 180);
			this.cbExcludeExpiredGroups.Name = "cbExcludeExpiredGroups";
			this.cbExcludeExpiredGroups.Size = new System.Drawing.Size(199, 24);
			this.cbExcludeExpiredGroups.TabIndex = 9;
			this.cbExcludeExpiredGroups.Text = "Exclude expired groups";
			this.cbExcludeExpiredGroups.UseVisualStyleBackColor = true;
			// 
			// cbKeepATOpen
			// 
			this.cbKeepATOpen.AutoSize = true;
			this.cbKeepATOpen.Location = new System.Drawing.Point(35, 150);
			this.cbKeepATOpen.Name = "cbKeepATOpen";
			this.cbKeepATOpen.Size = new System.Drawing.Size(168, 24);
			this.cbKeepATOpen.TabIndex = 8;
			this.cbKeepATOpen.Text = "Keep window open";
			this.cbKeepATOpen.UseVisualStyleBackColor = true;
			this.cbKeepATOpen.CheckedChanged += new System.EventHandler(this.cbKeepATOpen_CheckedChanged);
			// 
			// cbSpecialColumns
			// 
			this.cbSpecialColumns.AutoSize = true;
			this.cbSpecialColumns.Location = new System.Drawing.Point(15, 120);
			this.cbSpecialColumns.Name = "cbSpecialColumns";
			this.cbSpecialColumns.Size = new System.Drawing.Size(338, 24);
			this.cbSpecialColumns.TabIndex = 7;
			this.cbSpecialColumns.Text = "Special columns (username and password)";
			this.cbSpecialColumns.UseVisualStyleBackColor = true;
			this.cbSpecialColumns.CheckedChanged += new System.EventHandler(this.cbSpecialColumns_CheckedChanged);
			// 
			// cbDBColumn
			// 
			this.cbDBColumn.AutoSize = true;
			this.cbDBColumn.Location = new System.Drawing.Point(15, 90);
			this.cbDBColumn.Name = "cbDBColumn";
			this.cbDBColumn.Size = new System.Drawing.Size(113, 24);
			this.cbDBColumn.TabIndex = 6;
			this.cbDBColumn.Text = "DB column";
			this.cbDBColumn.UseVisualStyleBackColor = true;
			// 
			// cbColumnsSortable
			// 
			this.cbColumnsSortable.AutoSize = true;
			this.cbColumnsSortable.Location = new System.Drawing.Point(15, 30);
			this.cbColumnsSortable.Name = "cbColumnsSortable";
			this.cbColumnsSortable.Size = new System.Drawing.Size(158, 24);
			this.cbColumnsSortable.TabIndex = 4;
			this.cbColumnsSortable.Text = "Columns sortable";
			this.cbColumnsSortable.UseVisualStyleBackColor = true;
			// 
			// Options
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.gIntegration);
			this.Controls.Add(this.gHotkeys);
			this.Name = "Options";
			this.Padding = new System.Windows.Forms.Padding(15, 5, 15, 5);
			this.Size = new System.Drawing.Size(555, 419);
			this.Load += new System.EventHandler(this.Options_Load);
			this.gHotkeys.ResumeLayout(false);
			this.gHotkeys.PerformLayout();
			this.gIntegration.ResumeLayout(false);
			this.gIntegration.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private System.Windows.Forms.GroupBox gHotkeys;
		internal System.Windows.Forms.ComboBox cbPWHotkey;
		private KeePass.UI.HotKeyControlEx tbPWOnly;
		private System.Windows.Forms.Label lGAT;
		private System.Windows.Forms.Label lAAT;
		private KeePass.UI.HotKeyControlEx tbAAT;
		private System.Windows.Forms.GroupBox gIntegration;
		private KeePass.UI.HotKeyControlEx tbGAT;
		internal System.Windows.Forms.CheckBox cbDBColumn;
		internal System.Windows.Forms.CheckBox cbSpecialColumns;
		internal System.Windows.Forms.CheckBox cbKeepATOpen;
		internal System.Windows.Forms.CheckBox cbExcludeExpiredGroups;
		internal System.Windows.Forms.CheckBox cbColumnsSortable;
		private System.Windows.Forms.Label lGATP;
		private System.Windows.Forms.CheckBox cbPWEnter;
		internal System.Windows.Forms.CheckBox cbColumnsRememberSort;
	}
}