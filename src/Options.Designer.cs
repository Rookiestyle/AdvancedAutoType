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
            this.cbSpecialColumnsRespectPWEnter = new System.Windows.Forms.CheckBox();
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
            this.gHotkeys.Location = new System.Drawing.Point(27, 8);
            this.gHotkeys.Margin = new System.Windows.Forms.Padding(5);
            this.gHotkeys.Name = "gHotkeys";
            this.gHotkeys.Padding = new System.Windows.Forms.Padding(32, 28, 32, 28);
            this.gHotkeys.Size = new System.Drawing.Size(933, 279);
            this.gHotkeys.TabIndex = 6;
            this.gHotkeys.TabStop = false;
            this.gHotkeys.Text = "Hotkeys";
            // 
            // cbPWEnter
            // 
            this.cbPWEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPWEnter.AutoSize = true;
            this.cbPWEnter.Location = new System.Drawing.Point(473, 158);
            this.cbPWEnter.Margin = new System.Windows.Forms.Padding(5);
            this.cbPWEnter.Name = "cbPWEnter";
            this.cbPWEnter.Size = new System.Drawing.Size(182, 36);
            this.cbPWEnter.TabIndex = 4;
            this.cbPWEnter.Text = "PW+Enter";
            this.cbPWEnter.UseVisualStyleBackColor = true;
            this.cbPWEnter.CheckedChanged += new System.EventHandler(this.cbPWEnter_CheckedChanged);
            // 
            // lGATP
            // 
            this.lGATP.AutoSize = true;
            this.lGATP.Location = new System.Drawing.Point(27, 108);
            this.lGATP.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lGATP.Name = "lGATP";
            this.lGATP.Size = new System.Drawing.Size(267, 32);
            this.lGATP.TabIndex = 13;
            this.lGATP.Text = "Autotype Password:";
            // 
            // tbGAT
            // 
            this.tbGAT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGAT.Location = new System.Drawing.Point(462, 43);
            this.tbGAT.Margin = new System.Windows.Forms.Padding(5);
            this.tbGAT.Name = "tbGAT";
            this.tbGAT.Size = new System.Drawing.Size(438, 38);
            this.tbGAT.TabIndex = 1;
            this.tbGAT.TabStop = false;
            this.tbGAT.TextChanged += new System.EventHandler(this.OnPropagateHotKeys);
            // 
            // cbPWHotkey
            // 
            this.cbPWHotkey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPWHotkey.FormattingEnabled = true;
            this.cbPWHotkey.Items.AddRange(new object[] {
            "Password Only Hotkey",
            "Password+Enter Hotkey"});
            this.cbPWHotkey.Location = new System.Drawing.Point(27, 108);
            this.cbPWHotkey.Margin = new System.Windows.Forms.Padding(5);
            this.cbPWHotkey.Name = "cbPWHotkey";
            this.cbPWHotkey.Size = new System.Drawing.Size(370, 39);
            this.cbPWHotkey.TabIndex = 1;
            // 
            // tbPWOnly
            // 
            this.tbPWOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPWOnly.Location = new System.Drawing.Point(462, 108);
            this.tbPWOnly.Margin = new System.Windows.Forms.Padding(5);
            this.tbPWOnly.Name = "tbPWOnly";
            this.tbPWOnly.Size = new System.Drawing.Size(438, 38);
            this.tbPWOnly.TabIndex = 3;
            this.tbPWOnly.TextChanged += new System.EventHandler(this.OnPropagateHotKeys);
            // 
            // lGAT
            // 
            this.lGAT.AutoSize = true;
            this.lGAT.Location = new System.Drawing.Point(27, 46);
            this.lGAT.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lGAT.Name = "lGAT";
            this.lGAT.Size = new System.Drawing.Size(331, 32);
            this.lGAT.TabIndex = 11;
            this.lGAT.Text = "Global AutoType &Hotkey:";
            // 
            // lAAT
            // 
            this.lAAT.AutoSize = true;
            this.lAAT.Location = new System.Drawing.Point(27, 217);
            this.lAAT.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lAAT.Name = "lAAT";
            this.lAAT.Size = new System.Drawing.Size(362, 32);
            this.lAAT.TabIndex = 9;
            this.lAAT.Text = "Alternate AutoType Hotkey:";
            // 
            // tbAAT
            // 
            this.tbAAT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAAT.Location = new System.Drawing.Point(462, 217);
            this.tbAAT.Margin = new System.Windows.Forms.Padding(5);
            this.tbAAT.Name = "tbAAT";
            this.tbAAT.Size = new System.Drawing.Size(438, 38);
            this.tbAAT.TabIndex = 5;
            // 
            // gIntegration
            // 
            this.gIntegration.AutoSize = true;
            this.gIntegration.Controls.Add(this.cbSpecialColumnsRespectPWEnter);
            this.gIntegration.Controls.Add(this.cbColumnsRememberSort);
            this.gIntegration.Controls.Add(this.cbExcludeExpiredGroups);
            this.gIntegration.Controls.Add(this.cbKeepATOpen);
            this.gIntegration.Controls.Add(this.cbSpecialColumns);
            this.gIntegration.Controls.Add(this.cbDBColumn);
            this.gIntegration.Controls.Add(this.cbColumnsSortable);
            this.gIntegration.Dock = System.Windows.Forms.DockStyle.Top;
            this.gIntegration.Location = new System.Drawing.Point(27, 287);
            this.gIntegration.Margin = new System.Windows.Forms.Padding(5);
            this.gIntegration.Name = "gIntegration";
            this.gIntegration.Padding = new System.Windows.Forms.Padding(5);
            this.gIntegration.Size = new System.Drawing.Size(933, 403);
            this.gIntegration.TabIndex = 7;
            this.gIntegration.TabStop = false;
            this.gIntegration.Text = "Integration";
            // 
            // cbSpecialColumnsRespectPWEnter
            // 
            this.cbSpecialColumnsRespectPWEnter.AutoSize = true;
            this.cbSpecialColumnsRespectPWEnter.Location = new System.Drawing.Point(62, 279);
            this.cbSpecialColumnsRespectPWEnter.Margin = new System.Windows.Forms.Padding(5);
            this.cbSpecialColumnsRespectPWEnter.Name = "cbSpecialColumnsRespectPWEnter";
            this.cbSpecialColumnsRespectPWEnter.Size = new System.Drawing.Size(507, 36);
            this.cbSpecialColumnsRespectPWEnter.TabIndex = 9;
            this.cbSpecialColumnsRespectPWEnter.Text = "cbSpecialColumnsRespectPWEnter";
            this.cbSpecialColumnsRespectPWEnter.UseVisualStyleBackColor = true;
            // 
            // cbColumnsRememberSort
            // 
            this.cbColumnsRememberSort.AutoSize = true;
            this.cbColumnsRememberSort.Location = new System.Drawing.Point(62, 93);
            this.cbColumnsRememberSort.Margin = new System.Windows.Forms.Padding(5);
            this.cbColumnsRememberSort.Name = "cbColumnsRememberSort";
            this.cbColumnsRememberSort.Size = new System.Drawing.Size(352, 36);
            this.cbColumnsRememberSort.TabIndex = 95;
            this.cbColumnsRememberSort.Text = "Remember sort settings";
            this.cbColumnsRememberSort.UseVisualStyleBackColor = true;
            // 
            // cbExcludeExpiredGroups
            // 
            this.cbExcludeExpiredGroups.AutoSize = true;
            this.cbExcludeExpiredGroups.Location = new System.Drawing.Point(27, 326);
            this.cbExcludeExpiredGroups.Margin = new System.Windows.Forms.Padding(5);
            this.cbExcludeExpiredGroups.Name = "cbExcludeExpiredGroups";
            this.cbExcludeExpiredGroups.Size = new System.Drawing.Size(350, 36);
            this.cbExcludeExpiredGroups.TabIndex = 10;
            this.cbExcludeExpiredGroups.Text = "Exclude expired groups";
            this.cbExcludeExpiredGroups.UseVisualStyleBackColor = true;
            // 
            // cbKeepATOpen
            // 
            this.cbKeepATOpen.AutoSize = true;
            this.cbKeepATOpen.Location = new System.Drawing.Point(62, 232);
            this.cbKeepATOpen.Margin = new System.Windows.Forms.Padding(5);
            this.cbKeepATOpen.Name = "cbKeepATOpen";
            this.cbKeepATOpen.Size = new System.Drawing.Size(293, 36);
            this.cbKeepATOpen.TabIndex = 8;
            this.cbKeepATOpen.Text = "Keep window open";
            this.cbKeepATOpen.UseVisualStyleBackColor = true;
            this.cbKeepATOpen.CheckedChanged += new System.EventHandler(this.cbKeepATOpen_CheckedChanged);
            // 
            // cbSpecialColumns
            // 
            this.cbSpecialColumns.AutoSize = true;
            this.cbSpecialColumns.Location = new System.Drawing.Point(27, 186);
            this.cbSpecialColumns.Margin = new System.Windows.Forms.Padding(5);
            this.cbSpecialColumns.Name = "cbSpecialColumns";
            this.cbSpecialColumns.Size = new System.Drawing.Size(595, 36);
            this.cbSpecialColumns.TabIndex = 7;
            this.cbSpecialColumns.Text = "Special columns (username and password)";
            this.cbSpecialColumns.UseVisualStyleBackColor = true;
            this.cbSpecialColumns.CheckedChanged += new System.EventHandler(this.cbSpecialColumns_CheckedChanged);
            // 
            // cbDBColumn
            // 
            this.cbDBColumn.AutoSize = true;
            this.cbDBColumn.Location = new System.Drawing.Point(27, 139);
            this.cbDBColumn.Margin = new System.Windows.Forms.Padding(5);
            this.cbDBColumn.Name = "cbDBColumn";
            this.cbDBColumn.Size = new System.Drawing.Size(191, 36);
            this.cbDBColumn.TabIndex = 6;
            this.cbDBColumn.Text = "DB column";
            this.cbDBColumn.UseVisualStyleBackColor = true;
            // 
            // cbColumnsSortable
            // 
            this.cbColumnsSortable.AutoSize = true;
            this.cbColumnsSortable.Location = new System.Drawing.Point(27, 46);
            this.cbColumnsSortable.Margin = new System.Windows.Forms.Padding(5);
            this.cbColumnsSortable.Name = "cbColumnsSortable";
            this.cbColumnsSortable.Size = new System.Drawing.Size(274, 36);
            this.cbColumnsSortable.TabIndex = 4;
            this.cbColumnsSortable.Text = "Columns sortable";
            this.cbColumnsSortable.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.gIntegration);
            this.Controls.Add(this.gHotkeys);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Options";
            this.Padding = new System.Windows.Forms.Padding(27, 8, 27, 8);
            this.Size = new System.Drawing.Size(987, 698);
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
        internal System.Windows.Forms.CheckBox cbSpecialColumnsRespectPWEnter;
    }
}