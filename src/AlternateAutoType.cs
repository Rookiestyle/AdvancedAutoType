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
using System.Windows.Forms;

namespace AlternateAutoType
{
	public sealed class AlternateAutoTypeExt : Plugin
	{
		#region class members
		private IPluginHost m_host = null;
		ToolStripMenuItem m_menuItem = null;

		private int m_sequence = 0;
		private bool m_DBColumnVisible = false;

		private AutoTypeCtxForm m_AT = null;
		private ColumnHeader m_SortColumn = null;
		private SortOrder m_SortOrder = SortOrder.Ascending;

		private bool AATHotkeyPressed
		{
			get { return (m_sequence != 0) && (m_sequence == Config.AATHotkeyID); }
		}

		private bool m_bKPAutoTypePasswordHotkey = false;
		private bool PWOnlyHotkeyPressed
		{
			get { return (m_bKPAutoTypePasswordHotkey || (m_sequence != 0) && (m_sequence == Config.PWOnlyHotkeyID)); }
		}
		#endregion

		public override bool Initialize(IPluginHost host)
		{
			Terminate();
			m_host = host;

			PluginTranslate.Init(this, KeePass.Program.Translation.Properties.Iso6391Code);
			Tools.DefaultCaption = PluginTranslate.PluginName;
			Tools.PluginURL = "https://github.com/rookiestyle/alternateautotype/";

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
			Tools.OptionsFormClosed += ConfigWrite;

			WndProcHook.AddHandler(m_host.MainWindow, WndProcHandler);

			return true;
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
			WndProcHook.RemoveHandler(m_host.MainWindow);
			HotkeysDeactivate();
			SprEngine.FilterPlaceholderHints.Remove(Config.Placeholder);
			AutoType.FilterCompilePre -= this.AutoType_FilterCompilePre;
			m_host.MainWindow.ToolsMenu.DropDownItems.Remove(m_menuItem);

			Tools.OptionsFormShown -= OptionsFormShown;
			Tools.OptionsFormClosed -= ConfigWrite;
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
             * Option 2: No placeholder => nothing to do
			 * Option 3: Placeholder and hotkey for AAT is used => return sequence part AFTER placeholder
             * Option 4: Placeholder and hotkey for AAT is not used => return sequence part BEFORE placeholder
            */
			bool bPWOnly = PWOnlyHotkeyPressed;
			if (bResetPWOnly) CheckKPAutoTypePasswordHotkey(false);
			if (bPWOnly)
				return Config.PWEnter ? "{PASSWORD}{ENTER}" : "{PASSWORD}";
			int pos = sequence.IndexOf(Config.Placeholder);
			if (pos < 0)
				return sequence;
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
			if (!bSet || !Config.KPAutoTypePWPossible) return;
			m_bKPAutoTypePasswordHotkey = bSet;
		}

		private void HotKeyPressed(object sender, HotKeyEventArgs e)
		{
			m_sequence = e.ID;
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
		}

		private void HotkeysDeactivate()
		{
			PTHotKeyManager.HotKeyPressed -= HotKeyPressed;
			if (Config.AATHotkey != Keys.None)
				PTHotKeyManager.UnregisterHotKey(Config.AATHotkeyID);
			if ((Config.PWOnlyHotkey != Keys.None) && !Config.KPAutoTypePWPossible)
				PTHotKeyManager.UnregisterHotKey(Config.PWOnlyHotkeyID);
		}
		#endregion

		#region Autotype form enhancement (DB column, sortable columns, special auto-type)
		private void OnWindowAdded(object sender, GwmWindowEventArgs e)
		{
			if (!(e.Form is AutoTypeCtxForm)) return;
			if (m_host.MainWindow.DocumentManager.GetOpenDatabases().Count < 1) return;
			m_AT = (AutoTypeCtxForm)e.Form;
			m_AT.Shown += OnAutoTypeFormShown;

			PluginDebug.AddInfo("Auto-Type entry selection window added", 0);

			List<AutoTypeCtx> lCtx = (List<AutoTypeCtx>)Tools.GetField("m_lCtxs", m_AT);
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
		}

		private void OnWindowRemoved(object sender, GwmWindowEventArgs e)
		{
			if (m_AT == null) return;
			if (!(e.Form is AutoTypeCtxForm)) return;
			PluginDebug.AddInfo("Auto-Type entry selection window removed", 0);
			m_AT.Shown -= OnAutoTypeFormShown;
			ListView lv = Tools.GetControl("m_lvItems", m_AT) as ListView;
			lv.ColumnWidthChanged -= HandleColumns;
			lv.Columns.RemoveByKey(Config.DBColumn);
			lv.Columns.RemoveByKey(Config.PWColumn);
			UIUtil.ResizeColumns(lv, true);
			string ColumnWidths = UIUtil.GetColumnWidths(lv);
			if (ColumnWidths.Length > 0) KeePass.Program.Config.UI.AutoTypeCtxColumnWidths = ColumnWidths;
			m_AT = null;
			m_SortColumn = null;
			m_SortOrder = SortOrder.Ascending;
			m_DBColumnVisible = false;
		}

		private void OnAutoTypeFormShown(object sender, EventArgs e)
		{
			ListView lv = Tools.GetControl("m_lvItems", m_AT) as ListView;
			PluginDebug.AddInfo("Auto-Type entry selection window shown", 0);
			if ((lv != null) && (lv.Items.Count == 0) && !KeePass.Program.Config.Integration.AutoTypeAlwaysShowSelDialog)
			{
				PluginDebug.AddInfo("Auto-Type Entry Selection window closed", 0, "Reason: No entries to display");
				m_AT.Close();
				return;
			}
			if ((lv != null) && (lv.Items.Count == 1) && !KeePass.Program.Config.Integration.AutoTypeAlwaysShowSelDialog)
			{
				lv.Items[0].Selected = true;
				System.Reflection.MethodInfo miPIS = m_AT.GetType().GetMethod("ProcessItemSelection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
				try
				{
					miPIS.Invoke(m_AT, null);
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
					Button bTools = (Button)Tools.GetControl("m_btnTools", m_AT);
					if (bTools != null)
					{
						CheckBox cbShowGroups = new CheckBox();
						cbShowGroups.Name = "cbAAT_ShowGroups";
						cbShowGroups.Checked = Config.ColumnsRememberSorting && Config.ColumnsSortGrouping;
						cbShowGroups.Text = PluginTranslate.AATFormShowGroups;
						cbShowGroups.AutoSize = true;
						m_AT.Controls.Add(cbShowGroups);
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
					List<AutoTypeCtx> lEntries = (List<AutoTypeCtx>)Tools.GetField("m_lCtxs", m_AT);
					string db1 = lEntries[0].Database.IOConnectionInfo.Path;
					foreach (AutoTypeCtx entry in lEntries)
					{
						if (entry.Database.IOConnectionInfo.Path != db1)
						{
							m_DBColumnVisible = true;
							if (!Config.SpecialColumns) lv.ColumnWidthChanged += HandleColumns;
							HandleColumns(lv, null);
							Button btnTools = (Button)Tools.GetField("m_btnTools", m_AT);
							btnTools.Click += OnColumnMenuOpening;
							break;
						}
					}
				}
			}
			catch (Exception) { }
		}

		private void AutoTypeForm_ShowGroups_CheckedChanged(object sender, EventArgs e)
		{
			ListView lv = Tools.GetControl("m_lvItems", m_AT) as ListView;
			if (lv == null) return;
			CheckBox cbShowGroups = (CheckBox)Tools.GetControl("cbAAT_ShowGroups", m_AT);
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

			//Don't sort our own password column as it's asterisks only
			if (sortColumn.Text == Config.PWColumnHeader) return;

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

			lv.SmallImageList.Images.RemoveByKey(Config.SortIcon);
			if (m_SortOrder == SortOrder.Ascending)
				lv.SmallImageList.Images.Add(Config.SortIcon, Resources.sort_asc);
			else
				lv.SmallImageList.Images.Add(Config.SortIcon, Resources.sort_desc);
			m_SortColumn = sortColumn;
			m_SortColumn.ImageKey = Config.SortIcon;

			lv.ListViewItemSorter = new ListViewComparer(e.Column, m_SortOrder);

			AdjustGroups(lv, e.Column);
		}

		private void AdjustGroups(ListView lv, int column)
		{
			lv.Groups.Clear();
			List<string> groups = new List<string>();
			ListViewGroup lvg = null;
			CheckBox cbShowGroups = (CheckBox)Tools.GetControl("cbAAT_ShowGroups", m_AT);
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
			else
				if (Config.ColumnsRememberSorting) Config.ColumnsSortGrouping = false;
		}

		private void HandleColumns(object sender, ColumnWidthChangedEventArgs e)
		{
			ListView lv = sender as ListView;

			//ColumnWidthChanged event is triggered pretty often
			//Only do something if list contains items and if the event is triggered for the last visible column
			if (lv.Items.Count == 0) return;
			if ((e != null) && (e.ColumnIndex != lv.Columns.Count - 1)) return;

			lv.ColumnWidthChanged -= HandleColumns;

			if (Config.SpecialColumns && !KeePassLib.Native.NativeLib.IsUnix()) ColorSpecialColumns(lv);

			if (Config.AddDBColumn) HandleDBColumn(lv);

			lv.ColumnWidthChanged += HandleColumns;
		}

		private void ColorSpecialColumns(ListView lv)
		{
			int colUsername = -1;
			int colPassword = -1;
			int colAATPassword = -1;
			foreach (ColumnHeader col in lv.Columns)
			{
				if (col.Text == KeePass.Resources.KPRes.UserName)
					colUsername = col.Index;
				if (col.Text == KeePass.Resources.KPRes.Password)
					colPassword = col.Index;
				if (col.Text == Config.PWColumnHeader)
					colAATPassword = col.Index;
			}
			if (colAATPassword < 0)
			{
				ColumnHeader h = new ColumnHeader();
				h.Text = Config.PWColumnHeader;
				h.Name = Config.PWColumn;
				if (colUsername >= 0)
				{
					colAATPassword = colUsername + 1;
					if (colPassword >= colAATPassword) colPassword++;
				}
				else
					colAATPassword = lv.Columns.Count;
				lv.Columns.Insert(colAATPassword, h);
				for (int i = 0; i < lv.Items.Count; i++)
					lv.Items[i].SubItems.Insert(colAATPassword, new ListViewItem.ListViewSubItem(lv.Items[i], KeePassLib.PwDefs.HiddenPassword));
				UIUtil.ResizeColumns(lv, true);
			}
			foreach (ListViewItem li in lv.Items)
			{
				li.UseItemStyleForSubItems = false;
				if (colUsername > -1)
				{
					li.SubItems[colUsername].BackColor = SystemColors.Info;
					li.SubItems[colUsername].ForeColor = SystemColors.InfoText;
				}
				if (colPassword > -1)
				{
					li.SubItems[colPassword].BackColor = SystemColors.Info;
					li.SubItems[colPassword].ForeColor = SystemColors.InfoText;
				}
				if (colAATPassword > -1)
				{
					li.SubItems[colAATPassword].BackColor = SystemColors.Info;
					li.SubItems[colAATPassword].ForeColor = SystemColors.InfoText;
				}
			}
		}

		private void HandleDBColumn(ListView lv)
		{
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
			List<AutoTypeCtx> lEntries = (List<AutoTypeCtx>)Tools.GetField("m_lCtxs", m_AT);
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
			ListViewHitTestInfo info = (sender as ListView).HitTest(e.X, e.Y);
			int col = info.Item.SubItems.IndexOf(info.SubItem);
			if ((col < 0) || ((sender as ListView).Columns.Count <= col)) return;
			string column = (sender as ListView).Columns[col].Text;
			string sequence = string.Empty;
			if (column == KeePass.Resources.KPRes.UserName) sequence = "{USERNAME}";
			else if (column == KeePass.Resources.KPRes.Password) sequence = "{PASSWORD}";
			else if (column == Config.PWColumnHeader) sequence = "{PASSWORD}";
			else return;
			AutoTypeCtx ctx = info.Item.Tag as AutoTypeCtx;
			if (ctx == null) return;
			if (Config.KeepATOpen)
			{
				(sender as ListView).SelectedItems.Clear(); //ugly hack to avoid KeePass performing the standard behaviour
				m_AT.DialogResult = DialogResult.None;
				AutoType.PerformIntoPreviousWindow(m_AT, ctx.Entry, m_host.MainWindow.DocumentManager.SafeFindContainerOf(ctx.Entry), sequence);
				m_AT.Activate();
			}
			else
			{
				ctx.Sequence = sequence;
			}
		}

		private void OnColumnMenuOpening(object sender, EventArgs e)
		{
			ToolStripMenuItem tsmi = (ToolStripMenuItem)Tools.GetField("m_tsmiColumns", m_AT);

			ToolStripMenuItem db = new ToolStripMenuItem(KeePass.Resources.KPRes.Database);
			ListView lv = Tools.GetControl("m_lvItems", m_AT) as ListView;
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
			options.cbPWHotkey.SelectedIndex = Config.PWEnter ? 1 : 0;
			options.cbColumnsSortable.Checked = Config.ColumnsSortable;
			options.cbColumnsRememberSort.Checked = Config.ColumnsRememberSorting;
			options.cbDBColumn.Checked = Config.AddDBColumn;
			options.cbSpecialColumns.Checked = Config.SpecialColumns;
			options.cbKeepATOpen.Checked = Config.KeepATOpen;
			options.cbExcludeExpiredGroups.Checked = Config.ExcludeExpiredGroups;
			e.form.Shown += options.OptionsForm_Shown;
			Tools.AddPluginToOptionsForm(this, options);
		}

		private void ConfigWrite(object sender, Tools.OptionsFormsEventArgs e)
		{
			if (e.form.DialogResult != DialogResult.OK) return;
			bool shown;
			Options options = (Options)Tools.GetPluginFromOptions(this, out shown);
			if (!shown) return;
			Config.PWEnter = options.cbPWHotkey.SelectedIndex == 1;
			HotkeysDeactivate();
			Config.AATHotkey = options.AATHotkey;
			Config.PWOnlyHotkey = options.PWOnlyHotkey;
			Config.ColumnsSortable = options.cbColumnsSortable.Checked;
			Config.ColumnsRememberSorting = options.cbColumnsRememberSort.Checked;
			Config.AddDBColumn = options.cbDBColumn.Checked;
			Config.SpecialColumns = options.cbSpecialColumns.Checked;
			Config.KeepATOpen = options.cbKeepATOpen.Checked;
			Config.ExcludeExpiredGroups = options.cbExcludeExpiredGroups.Checked;

			if ((Config.AATHotkey != Keys.None) || (Config.PWOnlyHotkey != Keys.None))
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
			get { return "https://raw.githubusercontent.com/rookiestyle/alternateautotype/master/version.info"; }
		}

		public override Image SmallIcon
		{
			get { return KeePassLib.Utility.GfxUtil.ScaleImage(Resources.smallicon, 16, 16); }
		}
	}
}