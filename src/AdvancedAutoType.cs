using KeePass.Forms;
using KeePass.Plugins;
using KeePass.UI;
using KeePass.Util;
using KeePass.Util.Spr;
using PluginTools;
using PluginTranslation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AdvancedAutoType
{
	public sealed class AdvancedAutoTypeExt : Plugin
	{
		#region class members
		private IPluginHost m_host = null;
		ToolStripMenuItem m_menuItem = null;

		private int m_sequence = 0;
		private bool m_DBColumnVisible = false;

		private ColumnHeader m_SortColumn = null;
		private SortOrder m_SortOrder = SortOrder.Ascending;

		MethodInfo m_miProcessItemSelection = null;

		private AutotypeWindowWatcher m_aww = null;

		private bool AATHotkeyPressed
		{
			get { return (m_sequence != 0) && (m_sequence == Config.AATHotkeyID); }
		}

		private bool m_bKPAutoTypePasswordHotkey = false;
		private bool PWOnlyHotkeyPressed
		{
			get { return (m_bKPAutoTypePasswordHotkey || (m_sequence != 0) && (m_sequence == Config.PWOnlyHotkeyID)); }
		}

		private bool UsernameOnlyHotkeyPressed
		{
			get { return (m_sequence != 0) && (m_sequence == Config.UsernameOnlyHotkeyID); }
		}
		#endregion

		public override bool Initialize(IPluginHost host)
		{
			Terminate();
			m_host = host;

			PluginTranslate.Init(this, KeePass.Program.Translation.Properties.Iso6391Code);
			Tools.DefaultCaption = PluginTranslate.PluginName;
			Tools.PluginURL = "https://github.com/rookiestyle/advancedautotype/";

			SprEngine.FilterPlaceholderHints.Add(Config.Placeholder);
			AutoType.FilterCompilePre += AutoType_FilterCompilePre;

			HotkeysActivate();

			ToolStripItemCollection tsMenu = m_host.MainWindow.ToolsMenu.DropDownItems;
			m_menuItem = new ToolStripMenuItem();
			m_menuItem.Image = SmallIcon;
			m_menuItem.Text = PluginTranslate.PluginName + "...";
			m_menuItem.Click += (o, e) => Tools.ShowOptions();
			tsMenu.Add(m_menuItem);

			GlobalWindowManager.WindowAdded += OnWindowAdded;
			GlobalWindowManager.WindowRemoved += OnWindowRemoved;

			Tools.OptionsFormShown += OptionsFormShown;
			Tools.OptionsFormClosed += OptionsForm_Closed;

			if (Config.PWEnter) WndProcHook.AddHandler(m_host.MainWindow, WndProcHandler);

			m_miProcessItemSelection = typeof(AutoTypeCtxForm).GetMethod("ProcessItemSelection", BindingFlags.NonPublic | BindingFlags.Instance);

			m_aww = new AutotypeWindowWatcher(SmallIcon);
			m_aww.Enable();

            m_host.MainWindow.FormLoadPost += MainWindow_FormLoadPost;

			return true;
		}

        private void MainWindow_FormLoadPost(object sender, EventArgs e)
        {
			var p = Tools.GetPluginInstance("AlternateAutoType") as Plugin;
			if (p == null) return;
			p.Terminate();
			Tools.ShowInfo(@"AlternateAutoType has been replaced by Advanced Auto-Type.

Please ignore any error messages related to AlternateAutotype, close KeePass and remove AlternateAutoType.plgx");
        }

        private void WndProcHandler(object sender, WndProcEventArgs e)
		{
			e.SkipBase = false;
			if (e.m.Msg != 0x0312) return; // 0x0312 = WM_HOTKEY
			int hk = (int)e.m.WParam;
			CheckKPAutoTypePasswordHotkey(hk == KeePass.App.AppDefs.GlobalHotKeyId.AutoTypePassword);
		}

		public override void Terminate()
		{
			if (m_host == null) return;
			m_aww.Disable();
			WndProcHook.RemoveHandler(m_host.MainWindow);
			HotkeysDeactivate();
			SprEngine.FilterPlaceholderHints.Remove(Config.Placeholder);
			AutoType.FilterCompilePre -= this.AutoType_FilterCompilePre;
			m_host.MainWindow.ToolsMenu.DropDownItems.Remove(m_menuItem);

			Tools.OptionsFormShown -= OptionsFormShown;
			Tools.OptionsFormClosed -= OptionsForm_Closed;
			GlobalWindowManager.WindowAdded -= OnWindowAdded;
			GlobalWindowManager.WindowRemoved -= OnWindowRemoved;

			PluginDebug.SaveOrShow();

			m_host = null;
		}

		#region Additional hotkeys
		private string AdjustSequence(string sequence, bool bResetPWOnly)
		{
			/*
			 * Option 1: Hotkey for password only is used => return {PASSWORD}
			 * Option 2: Hotkey for username only is used => return {USERNAME}
			 * Option 3: No placeholder => nothing to do
			 * Option 4: Placeholder and hotkey for AAT is used => return sequence part AFTER placeholder
			 * Option 5: Placeholder and hotkey for AAT is not used => return sequence part BEFORE placeholder
			 */
			bool bPWOnly = PWOnlyHotkeyPressed;
			if (bResetPWOnly) CheckKPAutoTypePasswordHotkey(false);
			if (bPWOnly) return "{PASSWORD}" + (Config.PWEnter ? "{ENTER}" : string.Empty);
			if (UsernameOnlyHotkeyPressed) return "{USERNAME}" + (Config.UsernameOnlyEnter ? "{ENTER}" : string.Empty);

			int pos = sequence.IndexOf(Config.Placeholder);
			if (pos < 0) return sequence;
			if (AATHotkeyPressed)
				return sequence.Substring(pos + Config.Placeholder.Length);
			else
				return sequence.Substring(0, pos);
		}

		private void AutoType_FilterCompilePre(object sender, AutoTypeEventArgs e)
		{
			/* This is only required if the Auto-Type Entry Selection window is NOT shown
			 * If the window is shown, the auto-type sequences are already adjusted
			*/
			e.Sequence = AdjustSequence(e.Sequence, true);
		}

		private void CheckKPAutoTypePasswordHotkey(bool bSet)
		{
			m_bKPAutoTypePasswordHotkey = false;
			if (!Config.PWEnter || !bSet || !Config.KPAutoTypePWPossible) return;
			m_bKPAutoTypePasswordHotkey = bSet;
		}

		private void HotKeyPressed(object sender, HotKeyEventArgs e)
		{
			m_sequence = e.ID;
			List<string> lMsg = new List<string>();
			lMsg.Add("Hotkey id: " + m_sequence.ToString());
			lMsg.Add("PW only hotkey: " + Config.PWOnlyHotkeyID.ToString() + " - " + (Config.PWOnlyHotkeyID == e.ID).ToString());
			lMsg.Add("AAT only hotkey: " + Config.AATHotkeyID + " - " + (Config.AATHotkeyID == e.ID).ToString());
			lMsg.Add("Username only hotkey: " + Config.UsernameOnlyHotkeyID+ " - " + (Config.UsernameOnlyHotkeyID == e.ID).ToString()); 
			PluginDebug.AddInfo("Alternate Auto-Type hotkey detected", 0, lMsg.ToArray());
			m_host.MainWindow.ExecuteGlobalAutoType();
			m_sequence = 0;
		}

		private void HotkeysActivate()
		{
			if (KeePassLib.Native.NativeLib.IsUnix()) return;
			PTHotKeyManager.HotKeyPressed += HotKeyPressed;
			if (Config.AATHotkey != Keys.None)
			{
				Config.AATHotkeyID = PTHotKeyManager.RegisterHotKey(Config.AATHotkey);
				if (Config.AATHotkeyID == 0)
					Tools.ShowError(string.Format(PluginTranslate.ErrorHotKeyAAT, Config.AATHotkey.ToString()));
			}
			if ((Config.PWOnlyHotkey != Keys.None) && !Config.KPAutoTypePWPossible)
			{
				Config.PWOnlyHotkeyID = PTHotKeyManager.RegisterHotKey(Config.PWOnlyHotkey);
				if (Config.PWOnlyHotkeyID == 0)
					Tools.ShowError(string.Format(PluginTranslate.ErrorHotKeyPWOnly, Config.PWOnlyHotkey.ToString()));
			}
			if (Config.UsernameOnlyHotkey != Keys.None)
			{
				Config.UsernameOnlyHotkeyID = PTHotKeyManager.RegisterHotKey(Config.UsernameOnlyHotkey);
				if (Config.UsernameOnlyHotkeyID == 0)
					Tools.ShowError(string.Format(PluginTranslate.ErrorHotKeyUsernameOnly, Config.UsernameOnlyHotkey.ToString()));
			}
		}

		private void HotkeysDeactivate()
		{
			PTHotKeyManager.HotKeyPressed -= HotKeyPressed;
			if (Config.AATHotkey != Keys.None)
				PTHotKeyManager.UnregisterHotKey(Config.AATHotkeyID);
			if ((Config.PWOnlyHotkey != Keys.None) && !Config.KPAutoTypePWPossible)
				PTHotKeyManager.UnregisterHotKey(Config.PWOnlyHotkeyID);
			if (Config.UsernameOnlyHotkey != Keys.None)
				PTHotKeyManager.UnregisterHotKey(Config.UsernameOnlyHotkeyID);
		}
		#endregion

		#region Autotype form enhancement (DB column, sortable columns, special auto-type)
		private void OnWindowAdded(object sender, GwmWindowEventArgs e)
		{
			if (!(e.Form is AutoTypeCtxForm)) return;
			int iOpenedDatabases = m_host.MainWindow.DocumentManager.GetOpenDatabases().Count;
			if (!Config.SearchAsYouType && iOpenedDatabases < 1) return;
			e.Form.Shown += OnAutoTypeFormShown;

			if (iOpenedDatabases < 1) return;
			PluginDebug.AddInfo("Auto-Type entry selection window added", 0);


			List<AutoTypeCtx> lCtx = (List<AutoTypeCtx>)Tools.GetField("m_lCtxs", e.Form);
			if (lCtx == null) return;
			// Adjust content

			// - Remove entries in expired groups
			if (Config.ExcludeExpiredGroups)
			{
				int PrevCount = lCtx.Count;
				lCtx.RemoveAll(x => IsGroupExpired(x.Entry.ParentGroup));
				PluginDebug.AddInfo("Removed entries in expired groups", 0,
					"Before: " + PrevCount.ToString(),
					"After: " + lCtx.Count.ToString());
			}

			// - Adjust sequence to show correct auto-type sequence
			// - Remove lines that don't contain AAT placeholder if AAT hotkey is used
			if (AATHotkeyPressed)
			{
				int PrevCount = lCtx.Count;
				lCtx.RemoveAll(x => x.Sequence.IndexOf(Config.Placeholder) < 0);
				PluginDebug.AddInfo("Removed sequences without AAT placeholder", 0,
					"Before: " + PrevCount.ToString(),
					"After: " + lCtx.Count.ToString());
			}
			List<AutoTypeCtx> lUnique = new List<AutoTypeCtx>();
			for (int i = lCtx.Count - 1; i >= 0; i--)
			{
				lCtx[i].Sequence = AdjustSequence(lCtx[i].Sequence, i == 0);
				if (lUnique.Find(x => (x.Entry == lCtx[i].Entry) && (x.Sequence == lCtx[i].Sequence)) == null)
					lUnique.Add(lCtx[i]);
				else
				{
					PluginDebug.AddInfo("Remove sequence", 0,
						"Reason: Duplicate after adjusting auto-type sequences",
						"Entry: " + lCtx[i].Entry.Uuid.ToString(),
						"Sequence: " + lCtx[i].Sequence);
					lCtx.RemoveAt(i);
				}
			}
			//CheckKPAutoTypePasswordHotkey(false);
		}

		private void OnWindowRemoved(object sender, GwmWindowEventArgs e)
		{
			if (!(e.Form is AutoTypeCtxForm)) return;
			PluginDebug.AddInfo("Auto-Type entry selection window removed", 0);
			e.Form.Shown -= OnAutoTypeFormShown;
			ListView lv = Tools.GetControl("m_lvItems", e.Form) as ListView;
			lv.ColumnWidthChanged -= HandleColumns;
			lv.Columns.RemoveByKey(Config.DBColumn);
			
			UIUtil.ResizeColumns(lv, true);
			string ColumnWidths = UIUtil.GetColumnWidths(lv);
			if (ColumnWidths.Length > 0) KeePass.Program.Config.UI.AutoTypeCtxColumnWidths = ColumnWidths;
			
			m_SortColumn = null;
			m_SortOrder = SortOrder.Ascending;
			m_DBColumnVisible = false;
		}

		private void OnAutoTypeFormShown(object sender, EventArgs e)
		{
			if (m_host.MainWindow.DocumentManager.GetOpenDatabases().Count >= 1) InitializeAutoTypeListView(sender, e);

			if (Config.SearchAsYouType) AddSearchfield(sender as AutoTypeCtxForm);
		}

		private void InitializeAutoTypeListView(object sender, EventArgs e)
		{
			AutoTypeCtxForm f = sender as AutoTypeCtxForm;
			if (f == null) return;
			//AddSearchfield(f);
			if (m_host.MainWindow.DocumentManager.GetOpenDatabases().Count < 1) return;
			ListView lv = Tools.GetControl("m_lvItems", f) as ListView;
			PluginDebug.AddInfo("Auto-Type entry selection window shown", 0);
			if ((lv != null) && (lv.Items.Count == 0) && !KeePass.Program.Config.Integration.AutoTypeAlwaysShowSelDialog)
			{
				PluginDebug.AddInfo("Auto-Type Entry Selection window closed", 0, "Reason: No entries to display");
				f.Close();
				return;
			}
			if ((lv != null) && (lv.Items.Count == 1) && !KeePass.Program.Config.Integration.AutoTypeAlwaysShowSelDialog)
			{
				lv.Items[0].Selected = true;
				try
				{
					m_miProcessItemSelection.Invoke(f, null);
					PluginDebug.AddInfo("Auto-Type Entry Selection window closed", 0, "Reason: Only one entry to be shown");
				}
				catch (Exception ex)
				{
					PluginDebug.AddError("Auto-Type Entry Selection window NOT closed", 0, "Reason: Could not process entry", "Details: " + ex.Message);
				}
				return;
			}
			if (!Config.AddDBColumn && !Config.ColumnsSortable && !Config.SpecialColumns) return;
			try
			{
				if (Config.ColumnsSortable)
				{
					lv.HeaderStyle = ColumnHeaderStyle.Clickable;
					//Recreate groups to ensure the first group is shown correct in case KeeTheme is installed
					List<ListViewGroup> lvg = new List<ListViewGroup>();
					foreach (ListViewGroup g in lv.Groups)
						lvg.Add(g);
					lv.Groups.Clear();
					foreach (ListViewGroup g in lvg)
						lv.Groups.Add(g);
					lv.ColumnClick += SortColumns;
					Button bTools = (Button)Tools.GetControl("m_btnTools", f);
					if (bTools != null)
					{
						CheckBox cbShowGroups = new CheckBox();
						cbShowGroups.Name = "cbAAT_ShowGroups";
						cbShowGroups.Checked = Config.ColumnsRememberSorting && Config.ColumnsSortGrouping;
						cbShowGroups.Text = PluginTranslate.AATFormShowGroups;
						cbShowGroups.AutoSize = true;
						f.Controls.Add(cbShowGroups);
						bTools.Parent.Controls.Add(cbShowGroups);
						cbShowGroups.Top = bTools.Bottom - cbShowGroups.Height;
						cbShowGroups.Left = bTools.Left + bTools.Width + 5;
						cbShowGroups.CheckedChanged += AutoTypeForm_ShowGroups_CheckedChanged;
					}
					int s = (int)Config.ColumnsSortColumn;
					if (Config.ColumnsRememberSorting && (s != 0) && (Math.Abs(s) <= lv.Columns.Count))
					{
						SortColumns(lv, new ColumnClickEventArgs(Math.Abs(s) - 1));
						if (((s > 0) && (m_SortOrder != SortOrder.Ascending)) || ((s < 0) && (m_SortOrder != SortOrder.Descending)))
							SortColumns(lv, new ColumnClickEventArgs(Math.Abs(s) - 1));
					}
				}

				if (Config.SpecialColumns && !KeePassLib.Native.NativeLib.IsUnix())
				{
					lv.MouseClick += CellClick;
					lv.ColumnWidthChanged += HandleColumns;
					HandleColumns(lv, null);
				}

				if (Config.AddDBColumn)
				{
					List<AutoTypeCtx> lEntries = (List<AutoTypeCtx>)Tools.GetField("m_lCtxs", f);
					string db1 = lEntries[0].Database.IOConnectionInfo.Path;
					if (lEntries.Any(x => x.Database.IOConnectionInfo.Path != db1))
					//foreach (AutoTypeCtx entry in lEntries)
					//{
						//if (entry.Database.IOConnectionInfo.Path != db1)
						{
							m_DBColumnVisible = true;
							if (!Config.SpecialColumns) lv.ColumnWidthChanged += HandleColumns;
							HandleColumns(lv, null);
							Button btnTools = (Button)Tools.GetField("m_btnTools", f);
							btnTools.Click += OnColumnMenuOpening;
							//break;
						}
					//}
				}
			}
			catch (Exception) { }
		}

		private class _SearchAsYouTypeData
        {
			internal ListView ShownEntries;
			internal List<ListViewItem> AllEntries;
        }
        private void AddSearchfield(AutoTypeCtxForm f)
        {
			var lvShownEntries = Tools.GetControl("m_lvItems", f) as ListView;
			if (lvShownEntries == null)
			{
				PluginDebug.AddError("Could not locate m_lvItems, search-as-you-type field not added");
				return;
			}
			var c = lvShownEntries.Parent;
			Label lSearch = new Label();
			TextBox tbSearch = new TextBox();
			c.Controls.Add(lSearch);
			c.Controls.Add(tbSearch);
			lSearch.Text = KeePass.Resources.KPRes.FindEntries;
			lSearch.AutoSize = true;
			tbSearch.Left = lvShownEntries.Left + lSearch.Width + DpiUtil.ScaleIntX(10);
			lSearch.Left = lvShownEntries.Left;
			tbSearch.Top = lvShownEntries.Top;
			lSearch.Top = lvShownEntries.Top + tbSearch.Height / 2 - lSearch.Height / 2;
			tbSearch.Width = lvShownEntries.Width / 2;
			List<ListViewItem> lvAllEntries = new List<ListViewItem>(lvShownEntries.Items.Cast< ListViewItem>());
			tbSearch.Tag = new _SearchAsYouTypeData() { AllEntries = lvAllEntries, ShownEntries = lvShownEntries };
			tbSearch.TextChanged += OnFilterSearchResults;
			int iGap = DpiUtil.ScaleIntY(10);
			int iHeight = lvShownEntries.Height;
			tbSearch.Dock = lvShownEntries.Dock = DockStyle.None;
			lvShownEntries.Top += tbSearch.Height + iGap -1;
			lvShownEntries.Height = iHeight - tbSearch.Height - iGap;
			lvShownEntries.Width = lvShownEntries.Parent.ClientSize.Width - lvShownEntries.Parent.Padding.Left - lvShownEntries.Parent.Padding.Right;
		}

		private void OnFilterSearchResults(object sender, EventArgs e)
        {
			TextBox tbSearch = sender as TextBox;
			if (tbSearch == null) return;
			_SearchAsYouTypeData st = tbSearch.Tag as _SearchAsYouTypeData;
			if (st == null) return;
			string s = tbSearch.Text.ToLowerInvariant();
			st.ShownEntries.BeginUpdate();
			st.ShownEntries.Items.Clear();
			st.ShownEntries.Items.AddRange(st.AllEntries.Where(x => 
				x.Text.ToLowerInvariant().Contains(s) // Entry title
				||
				x.SubItems[1].Text.ToLowerInvariant().Contains(s) // User name
				).ToArray());
			st.ShownEntries.EndUpdate();
			if (!st.ShownEntries.ShowGroups) return;
			int column = st.ShownEntries.Columns.IndexOf(m_SortColumn);
			AdjustGroups(st.ShownEntries, column);
        }

        private void AutoTypeForm_ShowGroups_CheckedChanged(object sender, EventArgs e)
		{
			Form f = (sender as Control).FindForm();
			ListView lv = Tools.GetControl("m_lvItems", f) as ListView;
			if (lv == null) return;
			CheckBox cbShowGroups = (CheckBox)Tools.GetControl("cbAAT_ShowGroups", f);
			if (cbShowGroups == null) return;
			if (m_SortColumn == null) return;
			lv.ShowGroups = cbShowGroups.Checked;
			int column = lv.Columns.IndexOf(m_SortColumn);
			AdjustGroups(lv, column);
		}

		private void SortColumns(object sender, ColumnClickEventArgs e)
		{
			ListView lv = (ListView)sender;
			ColumnHeader sortColumn = lv.Columns[e.Column];

			if (m_SortColumn != null)
			{
				m_SortColumn.ImageKey = string.Empty;
				m_SortColumn.TextAlign = m_SortColumn.TextAlign; //required as otherwise the first image of the imagelist is shown
			}

			//Don't sort password if password's are hidden - it's asterisks only
			if (Config.HidePasswordInAutoTypeForm && sortColumn.Text == KeePass.Resources.KPRes.Password) return;

			if (m_SortColumn == sortColumn)
			{
				if (m_SortOrder == SortOrder.Ascending)
					m_SortOrder = SortOrder.Descending;
				else
					m_SortOrder = SortOrder.Ascending;
			}
			else
				m_SortOrder = SortOrder.Ascending;

			if (Config.ColumnsRememberSorting)
			{
				if (m_SortOrder == SortOrder.Ascending)
					Config.ColumnsSortColumn = e.Column + 1;
				else
					Config.ColumnsSortColumn = -1 * (e.Column + 1);
			}
			m_SortColumn = sortColumn;

			ListViewComparer lvc = lv.ListViewItemSorter as ListViewComparer;
			if (lvc == null)
			{
				lvc = new ListViewComparer(e.Column, m_SortOrder);
				lv.ListViewItemSorter = lvc;
			}
			lvc.Column = e.Column;
			lvc.SortOrder = m_SortOrder;
			lv.Sort();
			
			lv.ListViewItemSorter = lvc;
			lvc.UpdateColumnSortingIcons(lv);


			AdjustGroups(lv, e.Column);
		}

		private void AdjustGroups(ListView lv, int column)
		{
			Form f = lv.FindForm();
			lv.Groups.Clear();
			List<string> groups = new List<string>();
			ListViewGroup lvg = null;
			CheckBox cbShowGroups = (CheckBox)Tools.GetControl("cbAAT_ShowGroups", f);
			if ((cbShowGroups != null) && cbShowGroups.Checked)
			{
				if (Config.ColumnsRememberSorting) Config.ColumnsSortGrouping = true;
				foreach (ListViewItem i in lv.Items)
				{
					string value = i.SubItems[column].Text;
					if (!groups.Contains(value))
					{
						groups.Add(value);
						lvg = new ListViewGroup(value, HorizontalAlignment.Left);
						lv.Groups.Add(lvg);
					}
					i.Group = lvg;
				}
			}
			else if (Config.ColumnsRememberSorting) Config.ColumnsSortGrouping = false;
		}

		private void HandleColumns(object sender, ColumnWidthChangedEventArgs e)
		{
			ListView lv = sender as ListView;

			//ColumnWidthChanged event is triggered pretty often
			//Only do something if list contains items and if the event is triggered for the last visible column
			if (lv.Items.Count == 0) return;
			if ((e != null) && (e.ColumnIndex != lv.Columns.Count - 1)) return;

			lv.ColumnWidthChanged -= HandleColumns;

			if (Config.HidePasswordInAutoTypeForm) HidePasswordInAutoTypeForm(lv);
			
			if (Config.SpecialColumns && !KeePassLib.Native.NativeLib.IsUnix()) ColorSpecialColumns(lv);

			if (Config.AddDBColumn) HandleDBColumn(lv);

			lv.ColumnWidthChanged += HandleColumns;
		}

        private void HidePasswordInAutoTypeForm(ListView lv)
        {
			if (lv.Items.Count < 1) return;
			int colPassword = -1;
			foreach (ColumnHeader col in lv.Columns)
			{
				if (col.Text == KeePass.Resources.KPRes.Password)
				{
					colPassword = col.Index;
					break;
				}
			}
			if (colPassword < 0) return;
			for (int i = 0; i < lv.Items.Count; i++)
				lv.Items[i].SubItems[colPassword].Text = KeePassLib.PwDefs.HiddenPassword;
		}

		private void ColorSpecialColumns(ListView lv)
		{
			if (lv.Items.Count < 1) return;
			int colUsername = -1;
			int colPassword = -1;
			foreach (ColumnHeader col in lv.Columns)
			{
				if (col.Text == KeePass.Resources.KPRes.UserName)
					colUsername = col.Index;
				if (col.Text == KeePass.Resources.KPRes.Password)
					colPassword = col.Index;
			}
			
			Color b = lv.Items[0].BackColor;
			Color f = lv.Items[0].ForeColor;
			if (KeePass.Program.Config.MainWindow.EntryListAlternatingBgColors)	b = UIUtil.GetAlternateColorEx(b);
			else
			{
				b = UIUtil.ColorMiddle(b, UIUtil.ColorMiddle(b, f));
				if (UIUtil.IsDarkColor(b)) f = UIUtil.LightenColor(b, 1);
				else f = UIUtil.DarkenColor(b, 1);
			}
			foreach (ListViewItem li in lv.Items)
			{
				li.UseItemStyleForSubItems = false;
				AdjustColors(li, colUsername, b, f);
				AdjustColors(li, colPassword, b, f);
			}
		}

		private void AdjustColors(ListViewItem li, int i, Color b, Color f)
		{
			if (i < 0) return;
			li.SubItems[i].BackColor = b;
			li.SubItems[i].ForeColor = f;
		}

		private void HandleDBColumn(ListView lv)
		{
			Form f = lv.FindForm();
			bool DBColumnVisible = (lv.Columns.IndexOfKey(Config.DBColumn) >= 0);
			if (!m_DBColumnVisible) //column shall not be visible
			{
				if (DBColumnVisible) //column is visible => remove and resize
				{
					lv.Columns.RemoveByKey(Config.DBColumn);
					UIUtil.ResizeColumns(lv, true);
				}
				return;
			}
			if (DBColumnVisible)
			{
				UIUtil.ResizeColumns(lv, true);
				return;
			}
			List<AutoTypeCtx> lEntries = (List<AutoTypeCtx>)Tools.GetField("m_lCtxs", f);
			ColumnHeader h = new ColumnHeader();
			h.Text = KeePass.Resources.KPRes.Database;
			h.Name = Config.DBColumn;
			lv.Columns.Add(h);
			for (int i = 0; i < lv.Items.Count; i++)
			{
				string db = lEntries[i].Database.Name;
				if (string.IsNullOrEmpty(db))
					db = KeePassLib.Utility.UrlUtil.GetFileName(lEntries[i].Database.IOConnectionInfo.Path);
				lv.Items[i].SubItems.Add(db);
			}
			UIUtil.ResizeColumns(lv, true);
		}

		private void CellClick(object sender, MouseEventArgs e)
		{
			Form f = (sender as Control).FindForm();
			ListViewHitTestInfo info = (sender as ListView).HitTest(e.X, e.Y);
			int col = info.Item.SubItems.IndexOf(info.SubItem);
			if ((col < 0) || ((sender as ListView).Columns.Count <= col)) return;
			string column = (sender as ListView).Columns[col].Text;
			string sequence = string.Empty;
			if (column == KeePass.Resources.KPRes.UserName)
			{
				sequence = "{USERNAME}";
				if (Config.SpecialColumnsRespectUsernameEnter) sequence += "{ENTER}";
			}
			else if (column == KeePass.Resources.KPRes.Password)
			{
				sequence = "{PASSWORD}";
				if (Config.SpecialColumnsRespectPWEnter) sequence += "{ENTER}";
			}
			else return;
			AutoTypeCtx ctx = info.Item.Tag as AutoTypeCtx;
			if (ctx == null) return;
			if (Config.KeepATOpen)
			{
				(sender as ListView).SelectedItems.Clear(); //ugly hack to avoid KeePass performing the standard behaviour
				f.DialogResult = DialogResult.None;
				AutoType.PerformIntoPreviousWindow(f, ctx.Entry, m_host.MainWindow.DocumentManager.SafeFindContainerOf(ctx.Entry), sequence);
				f.Activate();
			}
			else
			{
				ctx.Sequence = sequence;
			}
		}

		private void OnColumnMenuOpening(object sender, EventArgs e)
		{
			Form f = (sender as Control).FindForm();
			ToolStripMenuItem tsmi = (ToolStripMenuItem)Tools.GetField("m_tsmiColumns", f);

			ToolStripMenuItem db = new ToolStripMenuItem(KeePass.Resources.KPRes.Database);
			ListView lv = Tools.GetControl("m_lvItems", f) as ListView;
			db.Checked = lv.Columns.ContainsKey(Config.DBColumn);
			db.Name = Config.DBColumn + "Toggle";
			db.Click += (o, x) => { m_DBColumnVisible = !db.Checked; HandleColumns(lv, null); };
			tsmi.DropDownItems.Add(db);
		}
		#endregion

		#region Configuration
		private void OptionsFormShown(object sender, Tools.OptionsFormsEventArgs e)
		{
			Options options = new Options();
			options.AATHotkey = Config.AATHotkey;
			options.PWOnlyHotkey = Config.PWOnlyHotkey;
			options.UsernameOnlyHotkey = Config.UsernameOnlyHotkey;
			options.UsernameOnlyEnter = Config.UsernameOnlyEnter;
			options.cbPWHotkey.SelectedIndex = Config.PWEnter ? 1 : 0;
			options.cbColumnsSortable.Checked = Config.ColumnsSortable;
			options.cbColumnsRememberSort.Checked = Config.ColumnsRememberSorting;
			options.cbDBColumn.Checked = Config.AddDBColumn;
			options.cbSpecialColumns.Checked = Config.SpecialColumns;
			options.cbSpecialColumnsRespectPWEnter.Checked = Config.SpecialColumnsRespectPWEnter;
			options.cbSpecialColumnsRespectUsernameEnter.Checked = Config.SpecialColumnsRespectUsernameEnter; 
			options.cbKeepATOpen.Checked = Config.KeepATOpen;
			options.cbExcludeExpiredGroups.Checked = Config.ExcludeExpiredGroups;
			options.cbSearchAsYouType.Checked = Config.SearchAsYouType;
			options.cbDontHidePasswordsWithAsterisk.Checked = !Config.HidePasswordInAutoTypeForm;
			e.form.Shown += options.OptionsForm_Shown;
			Tools.AddPluginToOptionsForm(this, options);
		}

		private void OptionsForm_Closed(object sender, Tools.OptionsFormsEventArgs e)
		{
			if (e.form.DialogResult != DialogResult.OK) return;
			bool shown;
			Options options = (Options)Tools.GetPluginFromOptions(this, out shown);
			if (!shown) return;

			bool bPWEnter = options.cbPWHotkey.SelectedIndex == 1;
			if (Config.PWEnter &&(bPWEnter != Config.PWEnter)) WndProcHook.RemoveHandler(m_host.MainWindow);
			Config.PWEnter = bPWEnter;
			if (Config.PWEnter) WndProcHook.AddHandler(m_host.MainWindow, WndProcHandler);

			HotkeysDeactivate();
			Config.AATHotkey = options.AATHotkey;
			Config.PWOnlyHotkey = options.PWOnlyHotkey;
			Config.UsernameOnlyHotkey = options.UsernameOnlyHotkey;
			Config.UsernameOnlyEnter = options.UsernameOnlyEnter;
			Config.ColumnsSortable = options.cbColumnsSortable.Checked;
			Config.ColumnsRememberSorting = options.cbColumnsRememberSort.Checked;
			Config.AddDBColumn = options.cbDBColumn.Checked;
			Config.SpecialColumns = options.cbSpecialColumns.Checked;
			Config.SpecialColumnsRespectPWEnter = options.cbSpecialColumnsRespectPWEnter.Checked;
			Config.SpecialColumnsRespectUsernameEnter = options.cbSpecialColumnsRespectUsernameEnter.Checked;
			Config.KeepATOpen = options.cbKeepATOpen.Checked;
			Config.ExcludeExpiredGroups = options.cbExcludeExpiredGroups.Checked;
			Config.SearchAsYouType = options.cbSearchAsYouType.Checked;
			Config.HidePasswordInAutoTypeForm = !options.cbDontHidePasswordsWithAsterisk.Checked;

			if ((Config.AATHotkey != Keys.None) || (Config.PWOnlyHotkey != Keys.None) || (Config.UsernameOnlyHotkey != Keys.None))
				HotkeysActivate();
		}
		#endregion

		private bool IsGroupExpired(KeePassLib.PwGroup pg)
		{
			if (pg == null)
				return false;
			if (pg.Expires && (pg.ExpiryTime < DateTime.UtcNow))
				return true;
			return IsGroupExpired(pg.ParentGroup);
		}

		public override string UpdateUrl
		{
			get { return @"https://raw.githubusercontent.com/rookiestyle/advancedautotype/master/version.info"; }
		}

		public override Image SmallIcon
		{
			get { return KeePassLib.Utility.GfxUtil.ScaleImage(Resources.smallicon, 16, 16); }
		}
	}
}