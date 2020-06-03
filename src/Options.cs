using System.Windows.Forms;
using KeePass.UI;

using PluginTranslation;
using PluginTools;
using System;

namespace AlternateAutoType
{
	public partial class Options : UserControl
	{
		public Options()
		{
			InitializeComponent();

			Text = PluginTranslate.Options;
			gHotkeys.Text = PluginTranslate.Hotkeys;
			lGAT.Text = PluginTranslate.GlobalAutotypeHotKey;
			lAAT.Text = PluginTranslate.AATHotKey;
			cbPWEnter.Text = PluginTranslate.PasswordEnterHotKey;
			cbPWHotkey.Items.Clear();
			cbPWHotkey.Items.Add(PluginTranslate.PasswordOnlyHotKey);
			cbPWHotkey.Items.Add(PluginTranslate.PasswordEnterHotKey);
			gIntegration.Text = PluginTranslate.Integration;
			cbColumnsSortable.Text = PluginTranslate.ColumnsSortable;
			cbDBColumn.Text = PluginTranslate.AddDBColumn;
			cbSpecialColumns.Text = PluginTranslate.SpecialColumns;
			cbKeepATOpen.Text = PluginTranslate.KeepATOpen;
			cbExcludeExpiredGroups.Text = PluginTranslate.ExcludeExpiredGroups;
			cbColumnsRememberSort.Text = PluginTranslate.ColumnsSortRemember;

			SetHotKey(tbGAT, (Keys)KeePass.Program.Config.Integration.HotKeyGlobalAutoType);
			tbGAT.Enabled = false;

			lAAT.Enabled = tbAAT.Enabled = !KeePassLib.Native.NativeLib.IsUnix();
			cbSpecialColumns.Enabled = cbKeepATOpen.Enabled = !KeePassLib.Native.NativeLib.IsUnix();
		}

		public Keys AATHotkey
		{
			get { return GetHotKey(tbAAT); }
			set { SetHotKey(tbAAT, value); }
		}

		public Keys PWOnlyHotkey
		{
			get { return GetHotKey(tbPWOnly); }
			set { SetHotKey(tbPWOnly, value); }
		}

		private void SetHotKey(HotKeyControlEx hkBox, Keys hk)
		{
			if ((hkBox == tbPWOnly) && Config.KPAutoTypePWPossible)
			{
				hkBox.TabStop = hkBox.Enabled = false;
				hkBox.ReadOnly = true;
			}
			try
			{
				var check = hkBox.GetType().GetProperty("HotKeyModifiers");
				if (check == null)
				{
					// only available with KeePass versions > 2.41
					hkBox.HotKey = hk;
					return;
				}
				hkBox.HotKey = hk & Keys.KeyCode;
				check.SetValue(hkBox, hk & Keys.Modifiers, null);
				var render = hkBox.GetType().GetMethod("RenderHotKey");
				if (render == null) return;
				render.Invoke(hkBox, null);
			}
			catch { }
		}

		private Keys GetHotKey(HotKeyControlEx hkBox)
		{
			try
			{
				var check = hkBox.GetType().GetProperty("HotKeyModifiers");
				if (check == null)
				{
					// only available with KeePass versions > 2.41
					return hkBox.HotKey;
				}
				return hkBox.HotKey | (Keys)check.GetValue(hkBox, null);
			}
			catch { }
			return Keys.None;
		}

		private void cbSpecialColumns_CheckedChanged(object sender, System.EventArgs e)
		{
			cbKeepATOpen.Enabled = cbSpecialColumns.Checked;
		}

		private void Options_Load(object sender, System.EventArgs e)
		{
			gHotkeys.Enabled = !KeePassLib.Native.NativeLib.IsUnix();
		}

		internal void OptionsForm_Shown(object sender, EventArgs e)
		{
			lGATP.Visible = cbPWEnter.Visible = Config.KPAutoTypePWPossible;
			cbPWHotkey.Visible = cbPWHotkey.TabStop = !Config.KPAutoTypePWPossible;
			cbPWEnter.Checked = Config.PWEnter;
			cbPWEnter.Left = tbPWOnly.Left;
			if (!Config.KPAutoTypePWPossible) return;
			Control c = Tools.GetControl("m_lblAutotype", sender as Form);
			if (c != null) lGAT.Text = c.Text;
			c = Tools.GetControl("m_lblAutotypePassword", sender as Form);
			if (c != null) lGATP.Text = c.Text;
			c = Tools.GetControl("m_tabIntegration", sender as Form);
			if (c == null) return;
			(c as TabPage).Leave += IntegrationTab_Leave;
		}

		private void IntegrationTab_Leave(object sender, EventArgs e)
		{
			Control c = Tools.GetControl("m_hkAutotype", ParentForm);
			if (c != null) tbGAT.Text = c.Text;
			c = Tools.GetControl("m_hkAutotypePassword", ParentForm);
			if (c != null)
			{
				tbPWOnly.Text = c.Text;
				SetHotKey(tbPWOnly, GetHotKey(c as HotKeyControlEx));
			}
		}

		private void cbPWEnter_CheckedChanged(object sender, EventArgs e)
		{
			cbPWHotkey.SelectedIndex = cbPWEnter.Checked ? 1 : 0;
		}

		private void cbKeepATOpen_CheckedChanged(object sender, EventArgs e)
		{
			if (cbKeepATOpen.Checked) cbSpecialColumns.Checked = true;
		}
	}
}