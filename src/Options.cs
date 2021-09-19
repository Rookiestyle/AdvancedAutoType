using System.Windows.Forms;
using KeePass.UI;

using PluginTranslation;
using PluginTools;
using System;
using System.Linq;

namespace AlternateAutoType
{
	public partial class Options : UserControl
	{
		private Form m_ParentForm = null;
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

			lAAT.Enabled = tbAAT.Enabled = !KeePassLib.Native.NativeLib.IsUnix();
			cbSpecialColumns.Enabled = cbKeepATOpen.Enabled = !KeePassLib.Native.NativeLib.IsUnix();
		}

        private void OnPropagateHotKeys(object sender, EventArgs e)
        {
			string sControl = sender == tbGAT ? "m_hkAutotype" : "m_hkAutotypePassword";
			var c = Tools.GetControl(sControl, m_ParentForm) as HotKeyControlEx;
			if (c == null) return;
			c.Text = (sender as HotKeyControlEx).Text;
			var st = new System.Diagnostics.StackTrace().GetFrames();
			var sfPluginOptionsEntered = st.Where(x => x.GetMethod().Name.ToLowerInvariant().Contains("pluginoptionsenter")).FirstOrDefault();
			if (sfPluginOptionsEntered == null) SetHotKey(c, GetHotKey(sender as HotKeyControlEx));
			else SetHotKey(sender as HotKeyControlEx, GetHotKey(c));
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
			hkBox.HotKey = hk;
		}

		private Keys GetHotKey(HotKeyControlEx hkBox)
		{
			return hkBox.HotKey;
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
			//HotKey cannot be set in our controls if they are place inside the UserControl
			//Reason: HotKeyManager.HandleHotKeyIntoSelf
			//  This checks for OptionsForm.ActiveControl which will be the UserControl
			//  but neds to be HotKeyControlEx
			//
			//Move all controls from UserControl in TabPage to TabPage
			//Make UserControl invisible (do NOT remove: OptionsForm_Closed won't work otherwise)
			m_ParentForm = ParentForm;
			while (Controls.Count > 0) Parent.Controls.Add(Controls[0]);
			Height = Width = 0;
			Visible = false;
			lGATP.Visible = cbPWEnter.Visible = Config.KPAutoTypePWPossible;
			cbPWHotkey.Visible = cbPWHotkey.TabStop = !Config.KPAutoTypePWPossible;
			cbPWEnter.Checked = Config.PWEnter;
			cbPWEnter.Left = tbPWOnly.Left;
			if (!Config.KPAutoTypePWPossible) return;
			Control c = Tools.GetControl("m_lblAutotype", sender as Form);
			if (c != null) lGAT.Text = c.Text;
			c = Tools.GetControl("m_lblAutotypePassword", sender as Form);
			if (c != null) lGATP.Text = c.Text;
			TabPage tpPluginOptions = Parent.Parent.Parent as TabPage; //TabPage AlternateAutoType - TabControl - TabPage Plugin Options
			if (tpPluginOptions != null) tpPluginOptions.Enter += PluginOptionsEnter;
		}

		private void PluginOptionsEnter(object sender, EventArgs e)
		{
			Control c = Tools.GetControl("m_hkAutotype", m_ParentForm);
			if (c != null)
			{
				tbGAT.Text = c.Text;
				SetHotKey(tbGAT, GetHotKey(c as HotKeyControlEx));
			}
			c = Tools.GetControl("m_hkAutotypePassword", m_ParentForm);
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