namespace AdvancedAutoType
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
            this.cbPWEnter = new System.Windows.Forms.CheckBox();
            this.lGATP = new System.Windows.Forms.Label();
            this.tbGAT = new KeePass.UI.HotKeyControlEx();
            this.cbPWHotkey = new System.Windows.Forms.ComboBox();
            this.tbPWOnly = new KeePass.UI.HotKeyControlEx();
            this.lGAT = new System.Windows.Forms.Label();
            this.lAAT = new System.Windows.Forms.Label();
            this.tbAAT = new KeePass.UI.HotKeyControlEx();
            this.cbSpecialColumnsRespectPWEnter = new System.Windows.Forms.CheckBox();
            this.cbColumnsRememberSort = new System.Windows.Forms.CheckBox();
            this.cbExcludeExpiredGroups = new System.Windows.Forms.CheckBox();
            this.cbKeepATOpen = new System.Windows.Forms.CheckBox();
            this.cbSpecialColumns = new System.Windows.Forms.CheckBox();
            this.cbDBColumn = new System.Windows.Forms.CheckBox();
            this.cbColumnsSortable = new System.Windows.Forms.CheckBox();
            this.tcAlternateAutoType = new System.Windows.Forms.TabControl();
            this.tpAlternateAutotypeHotkeys = new System.Windows.Forms.TabPage();
            this.cbUsernameEnter = new System.Windows.Forms.CheckBox();
            this.lGATU = new System.Windows.Forms.Label();
            this.tbUsernameOnly = new KeePass.UI.HotKeyControlEx();
            this.tpAlternateAutotypeIntegration = new System.Windows.Forms.TabPage();
            this.cbSpecialColumnsRespectUsernameEnter = new System.Windows.Forms.CheckBox();
            this.cbSearchAsYouType = new System.Windows.Forms.CheckBox();
            this.cbDontHidePasswordsWithAsterisk = new System.Windows.Forms.CheckBox();
            this.tcAlternateAutoType.SuspendLayout();
            this.tpAlternateAutotypeHotkeys.SuspendLayout();
            this.tpAlternateAutotypeIntegration.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbPWEnter
            // 
            this.cbPWEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPWEnter.AutoSize = true;
            this.cbPWEnter.Location = new System.Drawing.Point(519, 157);
            this.cbPWEnter.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cbPWEnter.Name = "cbPWEnter";
            this.cbPWEnter.Size = new System.Drawing.Size(182, 36);
            this.cbPWEnter.TabIndex = 40;
            this.cbPWEnter.Text = "PW+Enter";
            this.cbPWEnter.UseVisualStyleBackColor = true;
            this.cbPWEnter.CheckedChanged += new System.EventHandler(this.cbPWEnter_CheckedChanged);
            // 
            // lGATP
            // 
            this.lGATP.AutoSize = true;
            this.lGATP.Location = new System.Drawing.Point(27, 107);
            this.lGATP.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lGATP.Name = "lGATP";
            this.lGATP.Size = new System.Drawing.Size(267, 32);
            this.lGATP.TabIndex = 13;
            this.lGATP.Text = "Autotype Password:";
            // 
            // tbGAT
            // 
            this.tbGAT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGAT.Location = new System.Drawing.Point(507, 43);
            this.tbGAT.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tbGAT.Name = "tbGAT";
            this.tbGAT.Size = new System.Drawing.Size(439, 38);
            this.tbGAT.TabIndex = 10;
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
            this.cbPWHotkey.Location = new System.Drawing.Point(27, 107);
            this.cbPWHotkey.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cbPWHotkey.Name = "cbPWHotkey";
            this.cbPWHotkey.Size = new System.Drawing.Size(369, 39);
            this.cbPWHotkey.TabIndex = 20;
            // 
            // tbPWOnly
            // 
            this.tbPWOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPWOnly.Location = new System.Drawing.Point(507, 107);
            this.tbPWOnly.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tbPWOnly.Name = "tbPWOnly";
            this.tbPWOnly.Size = new System.Drawing.Size(439, 38);
            this.tbPWOnly.TabIndex = 30;
            this.tbPWOnly.TextChanged += new System.EventHandler(this.OnPropagateHotKeys);
            // 
            // lGAT
            // 
            this.lGAT.AutoSize = true;
            this.lGAT.Location = new System.Drawing.Point(27, 45);
            this.lGAT.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lGAT.Name = "lGAT";
            this.lGAT.Size = new System.Drawing.Size(331, 32);
            this.lGAT.TabIndex = 11;
            this.lGAT.Text = "Global AutoType &Hotkey:";
            // 
            // lAAT
            // 
            this.lAAT.AutoSize = true;
            this.lAAT.Location = new System.Drawing.Point(32, 334);
            this.lAAT.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lAAT.Name = "lAAT";
            this.lAAT.Size = new System.Drawing.Size(362, 32);
            this.lAAT.TabIndex = 9;
            this.lAAT.Text = "Alternate AutoType Hotkey:";
            // 
            // tbAAT
            // 
            this.tbAAT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAAT.Location = new System.Drawing.Point(507, 331);
            this.tbAAT.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tbAAT.Name = "tbAAT";
            this.tbAAT.Size = new System.Drawing.Size(439, 38);
            this.tbAAT.TabIndex = 70;
            // 
            // cbSpecialColumnsRespectPWEnter
            // 
            this.cbSpecialColumnsRespectPWEnter.AutoSize = true;
            this.cbSpecialColumnsRespectPWEnter.Location = new System.Drawing.Point(56, 351);
            this.cbSpecialColumnsRespectPWEnter.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cbSpecialColumnsRespectPWEnter.Name = "cbSpecialColumnsRespectPWEnter";
            this.cbSpecialColumnsRespectPWEnter.Size = new System.Drawing.Size(507, 36);
            this.cbSpecialColumnsRespectPWEnter.TabIndex = 70;
            this.cbSpecialColumnsRespectPWEnter.Text = "cbSpecialColumnsRespectPWEnter";
            this.cbSpecialColumnsRespectPWEnter.UseVisualStyleBackColor = true;
            // 
            // cbColumnsRememberSort
            // 
            this.cbColumnsRememberSort.AutoSize = true;
            this.cbColumnsRememberSort.Location = new System.Drawing.Point(56, 131);
            this.cbColumnsRememberSort.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cbColumnsRememberSort.Name = "cbColumnsRememberSort";
            this.cbColumnsRememberSort.Size = new System.Drawing.Size(352, 36);
            this.cbColumnsRememberSort.TabIndex = 30;
            this.cbColumnsRememberSort.Text = "Remember sort settings";
            this.cbColumnsRememberSort.UseVisualStyleBackColor = true;
            // 
            // cbExcludeExpiredGroups
            // 
            this.cbExcludeExpiredGroups.AutoSize = true;
            this.cbExcludeExpiredGroups.Location = new System.Drawing.Point(21, 453);
            this.cbExcludeExpiredGroups.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cbExcludeExpiredGroups.Name = "cbExcludeExpiredGroups";
            this.cbExcludeExpiredGroups.Size = new System.Drawing.Size(350, 36);
            this.cbExcludeExpiredGroups.TabIndex = 90;
            this.cbExcludeExpiredGroups.Text = "Exclude expired groups";
            this.cbExcludeExpiredGroups.UseVisualStyleBackColor = true;
            // 
            // cbKeepATOpen
            // 
            this.cbKeepATOpen.AutoSize = true;
            this.cbKeepATOpen.Location = new System.Drawing.Point(56, 305);
            this.cbKeepATOpen.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cbKeepATOpen.Name = "cbKeepATOpen";
            this.cbKeepATOpen.Size = new System.Drawing.Size(293, 36);
            this.cbKeepATOpen.TabIndex = 60;
            this.cbKeepATOpen.Text = "Keep window open";
            this.cbKeepATOpen.UseVisualStyleBackColor = true;
            this.cbKeepATOpen.CheckedChanged += new System.EventHandler(this.cbKeepATOpen_CheckedChanged);
            // 
            // cbSpecialColumns
            // 
            this.cbSpecialColumns.AutoSize = true;
            this.cbSpecialColumns.Location = new System.Drawing.Point(21, 260);
            this.cbSpecialColumns.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cbSpecialColumns.Name = "cbSpecialColumns";
            this.cbSpecialColumns.Size = new System.Drawing.Size(595, 36);
            this.cbSpecialColumns.TabIndex = 50;
            this.cbSpecialColumns.Text = "Special columns (username and password)";
            this.cbSpecialColumns.UseVisualStyleBackColor = true;
            this.cbSpecialColumns.CheckedChanged += new System.EventHandler(this.cbSpecialColumns_CheckedChanged);
            // 
            // cbDBColumn
            // 
            this.cbDBColumn.AutoSize = true;
            this.cbDBColumn.Location = new System.Drawing.Point(21, 191);
            this.cbDBColumn.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cbDBColumn.Name = "cbDBColumn";
            this.cbDBColumn.Size = new System.Drawing.Size(191, 36);
            this.cbDBColumn.TabIndex = 40;
            this.cbDBColumn.Text = "DB column";
            this.cbDBColumn.UseVisualStyleBackColor = true;
            // 
            // cbColumnsSortable
            // 
            this.cbColumnsSortable.AutoSize = true;
            this.cbColumnsSortable.Location = new System.Drawing.Point(21, 86);
            this.cbColumnsSortable.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cbColumnsSortable.Name = "cbColumnsSortable";
            this.cbColumnsSortable.Size = new System.Drawing.Size(274, 36);
            this.cbColumnsSortable.TabIndex = 20;
            this.cbColumnsSortable.Text = "Columns sortable";
            this.cbColumnsSortable.UseVisualStyleBackColor = true;
            // 
            // tcAlternateAutoType
            // 
            this.tcAlternateAutoType.Controls.Add(this.tpAlternateAutotypeHotkeys);
            this.tcAlternateAutoType.Controls.Add(this.tpAlternateAutotypeIntegration);
            this.tcAlternateAutoType.Dock = System.Windows.Forms.DockStyle.Top;
            this.tcAlternateAutoType.Location = new System.Drawing.Point(27, 7);
            this.tcAlternateAutoType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tcAlternateAutoType.Name = "tcAlternateAutoType";
            this.tcAlternateAutoType.SelectedIndex = 0;
            this.tcAlternateAutoType.Size = new System.Drawing.Size(1005, 650);
            this.tcAlternateAutoType.TabIndex = 8;
            // 
            // tpAlternateAutotypeHotkeys
            // 
            this.tpAlternateAutotypeHotkeys.Controls.Add(this.cbUsernameEnter);
            this.tpAlternateAutotypeHotkeys.Controls.Add(this.lGATU);
            this.tpAlternateAutotypeHotkeys.Controls.Add(this.tbUsernameOnly);
            this.tpAlternateAutotypeHotkeys.Controls.Add(this.cbPWEnter);
            this.tpAlternateAutotypeHotkeys.Controls.Add(this.lGATP);
            this.tpAlternateAutotypeHotkeys.Controls.Add(this.tbGAT);
            this.tpAlternateAutotypeHotkeys.Controls.Add(this.cbPWHotkey);
            this.tpAlternateAutotypeHotkeys.Controls.Add(this.tbPWOnly);
            this.tpAlternateAutotypeHotkeys.Controls.Add(this.lGAT);
            this.tpAlternateAutotypeHotkeys.Controls.Add(this.lAAT);
            this.tpAlternateAutotypeHotkeys.Controls.Add(this.tbAAT);
            this.tpAlternateAutotypeHotkeys.Location = new System.Drawing.Point(10, 48);
            this.tpAlternateAutotypeHotkeys.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tpAlternateAutotypeHotkeys.Name = "tpAlternateAutotypeHotkeys";
            this.tpAlternateAutotypeHotkeys.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tpAlternateAutotypeHotkeys.Size = new System.Drawing.Size(985, 541);
            this.tpAlternateAutotypeHotkeys.TabIndex = 0;
            this.tpAlternateAutotypeHotkeys.Text = "tabPage1";
            this.tpAlternateAutotypeHotkeys.UseVisualStyleBackColor = true;
            // 
            // cbUsernameEnter
            // 
            this.cbUsernameEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbUsernameEnter.AutoSize = true;
            this.cbUsernameEnter.Location = new System.Drawing.Point(519, 272);
            this.cbUsernameEnter.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cbUsernameEnter.Name = "cbUsernameEnter";
            this.cbUsernameEnter.Size = new System.Drawing.Size(182, 36);
            this.cbUsernameEnter.TabIndex = 60;
            this.cbUsernameEnter.Text = "PW+Enter";
            this.cbUsernameEnter.UseVisualStyleBackColor = true;
            // 
            // lGATU
            // 
            this.lGATU.AutoSize = true;
            this.lGATU.Location = new System.Drawing.Point(32, 217);
            this.lGATU.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lGATU.Name = "lGATU";
            this.lGATU.Size = new System.Drawing.Size(273, 32);
            this.lGATU.TabIndex = 16;
            this.lGATU.Text = "Autotype Username:";
            // 
            // tbUsernameOnly
            // 
            this.tbUsernameOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUsernameOnly.Location = new System.Drawing.Point(507, 222);
            this.tbUsernameOnly.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tbUsernameOnly.Name = "tbUsernameOnly";
            this.tbUsernameOnly.Size = new System.Drawing.Size(439, 38);
            this.tbUsernameOnly.TabIndex = 50;
            // 
            // tpAlternateAutotypeIntegration
            // 
            this.tpAlternateAutotypeIntegration.Controls.Add(this.cbDontHidePasswordsWithAsterisk);
            this.tpAlternateAutotypeIntegration.Controls.Add(this.cbSpecialColumnsRespectUsernameEnter);
            this.tpAlternateAutotypeIntegration.Controls.Add(this.cbSearchAsYouType);
            this.tpAlternateAutotypeIntegration.Controls.Add(this.cbSpecialColumnsRespectPWEnter);
            this.tpAlternateAutotypeIntegration.Controls.Add(this.cbColumnsRememberSort);
            this.tpAlternateAutotypeIntegration.Controls.Add(this.cbExcludeExpiredGroups);
            this.tpAlternateAutotypeIntegration.Controls.Add(this.cbKeepATOpen);
            this.tpAlternateAutotypeIntegration.Controls.Add(this.cbSpecialColumns);
            this.tpAlternateAutotypeIntegration.Controls.Add(this.cbDBColumn);
            this.tpAlternateAutotypeIntegration.Controls.Add(this.cbColumnsSortable);
            this.tpAlternateAutotypeIntegration.Location = new System.Drawing.Point(10, 48);
            this.tpAlternateAutotypeIntegration.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tpAlternateAutotypeIntegration.Name = "tpAlternateAutotypeIntegration";
            this.tpAlternateAutotypeIntegration.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tpAlternateAutotypeIntegration.Size = new System.Drawing.Size(985, 592);
            this.tpAlternateAutotypeIntegration.TabIndex = 1;
            this.tpAlternateAutotypeIntegration.Text = "tabPage2";
            this.tpAlternateAutotypeIntegration.UseVisualStyleBackColor = true;
            // 
            // cbSpecialColumnsRespectUsernameEnter
            // 
            this.cbSpecialColumnsRespectUsernameEnter.AutoSize = true;
            this.cbSpecialColumnsRespectUsernameEnter.Location = new System.Drawing.Point(56, 396);
            this.cbSpecialColumnsRespectUsernameEnter.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cbSpecialColumnsRespectUsernameEnter.Name = "cbSpecialColumnsRespectUsernameEnter";
            this.cbSpecialColumnsRespectUsernameEnter.Size = new System.Drawing.Size(592, 36);
            this.cbSpecialColumnsRespectUsernameEnter.TabIndex = 80;
            this.cbSpecialColumnsRespectUsernameEnter.Text = "cbSpecialColumnsRespectUsernameEnter";
            this.cbSpecialColumnsRespectUsernameEnter.UseVisualStyleBackColor = true;
            // 
            // cbSearchAsYouType
            // 
            this.cbSearchAsYouType.AutoSize = true;
            this.cbSearchAsYouType.Location = new System.Drawing.Point(21, 24);
            this.cbSearchAsYouType.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cbSearchAsYouType.Name = "cbSearchAsYouType";
            this.cbSearchAsYouType.Size = new System.Drawing.Size(294, 36);
            this.cbSearchAsYouType.TabIndex = 10;
            this.cbSearchAsYouType.Text = "Search as you type";
            this.cbSearchAsYouType.UseVisualStyleBackColor = true;
            // 
            // cbHidePasswordsWithAsterisk
            // 
            this.cbDontHidePasswordsWithAsterisk.AutoSize = true;
            this.cbDontHidePasswordsWithAsterisk.Location = new System.Drawing.Point(21, 509);
            this.cbDontHidePasswordsWithAsterisk.Margin = new System.Windows.Forms.Padding(5);
            this.cbDontHidePasswordsWithAsterisk.Name = "cbHidePasswordsWithAsterisk";
            this.cbDontHidePasswordsWithAsterisk.Size = new System.Drawing.Size(417, 36);
            this.cbDontHidePasswordsWithAsterisk.TabIndex = 91;
            this.cbDontHidePasswordsWithAsterisk.Text = "Hide passwords with asterisk";
            this.cbDontHidePasswordsWithAsterisk.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tcAlternateAutoType);
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "Options";
            this.Padding = new System.Windows.Forms.Padding(27, 7, 27, 7);
            this.Size = new System.Drawing.Size(1059, 699);
            this.Load += new System.EventHandler(this.Options_Load);
            this.tcAlternateAutoType.ResumeLayout(false);
            this.tpAlternateAutotypeHotkeys.ResumeLayout(false);
            this.tpAlternateAutotypeHotkeys.PerformLayout();
            this.tpAlternateAutotypeIntegration.ResumeLayout(false);
            this.tpAlternateAutotypeIntegration.PerformLayout();
            this.ResumeLayout(false);

		}
		internal System.Windows.Forms.ComboBox cbPWHotkey;
		private KeePass.UI.HotKeyControlEx tbPWOnly;
		private System.Windows.Forms.Label lGAT;
		private System.Windows.Forms.Label lAAT;
		private KeePass.UI.HotKeyControlEx tbAAT;
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
        private System.Windows.Forms.TabControl tcAlternateAutoType;
        private System.Windows.Forms.TabPage tpAlternateAutotypeHotkeys;
        private System.Windows.Forms.TabPage tpAlternateAutotypeIntegration;
        internal System.Windows.Forms.CheckBox cbSearchAsYouType;
        private System.Windows.Forms.CheckBox cbUsernameEnter;
        private System.Windows.Forms.Label lGATU;
        private KeePass.UI.HotKeyControlEx tbUsernameOnly;
        internal System.Windows.Forms.CheckBox cbSpecialColumnsRespectUsernameEnter;
        internal System.Windows.Forms.CheckBox cbDontHidePasswordsWithAsterisk;
    }
}