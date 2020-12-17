using KeePass.Forms;
using KeePass.Resources;
using KeePass.UI;
using KeePassLib;
using PluginTools;
using PluginTranslation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AlternateAutoType
{
	internal class AutotypeWindowWatcher
	{
		private bool m_bInitialized = false;
		private MethodInfo m_miEditSelectedEntry;

		private ToolStripMenuItem m_tsmiCtxContainer = null;
		private ToolStripMenuItem m_tsmiCtxLastWindow = null;
		private ToolStripComboBox m_tscbCtxWindows = null;

		private ToolStripMenuItem m_tsmiMenuContainer = null;
		private ToolStripMenuItem m_tsmiMenuLastWindow = null;
		private ToolStripComboBox m_tscbMenuWindows = null;

		private Image m_imgDefaultIcon = null;
		private string m_sAddedWindowText;

		internal AutotypeWindowWatcher(Image imgDefaultIcon)
		{
			m_imgDefaultIcon = imgDefaultIcon;

			m_miEditSelectedEntry = KeePass.Program.MainForm.GetType().GetMethod("EditSelectedEntry", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

			m_bInitialized = EditAutoType.Valid;
		}

		internal void Enable()
		{
			if (!m_bInitialized) return;
			EditAutoType.OnAddWindow += OnMeasureItem;
			AddMenu(ref m_tsmiCtxContainer, ref m_tsmiCtxLastWindow, ref m_tscbCtxWindows);
			KeePass.Program.MainForm.EntryContextMenu.Opening += EntryContextMenu_Opening;
			KeePass.Program.MainForm.EntryContextMenu.Items.Add(m_tsmiCtxContainer);

			GlobalWindowManager.WindowAdded += OnWindowAdded;

			try
			{
				ToolStripMenuItem last = KeePass.Program.MainForm.MainMenu.Items["m_menuEntry"] as ToolStripMenuItem;
				last.DropDownOpening += EntryMainMenu_Opening;
				AddMenu(ref m_tsmiMenuContainer, ref m_tsmiMenuLastWindow, ref m_tscbMenuWindows);
				last.DropDownItems.Add(m_tsmiMenuContainer);
			}
			catch { }
		}

		private void OnWindowAdded(object sender, GwmWindowEventArgs e)
		{
			if (string.IsNullOrEmpty(m_sAddedWindowText)) return;
			if (e.Form is PwEntryForm) e.Form.Shown += OnEntryFormShown;
		}

		private void OnEntryFormShown(object sender, EventArgs e)
		{
			string s = m_sAddedWindowText;
			m_sAddedWindowText = string.Empty;
			TabPage tpAutoType = Tools.GetControl("m_tabAutoType", sender as Form) as TabPage;
			if (tpAutoType == null) return;
			ListView lvAutoType = Tools.GetControl("m_lvAutoType", sender as Form) as ListView;
			if (lvAutoType == null) return;
			(tpAutoType.Parent as TabControl).SelectedTab = tpAutoType;
			lvAutoType.Select();
			for (int i = lvAutoType.Items.Count - 1; i >= 0; i--)
			{
				ListViewItem lvi = lvAutoType.Items[i];
				lvi.Selected = lvi.Text == s;
				if (lvi.Selected) break;
			}
			Button btnAutoTypeEdit = Tools.GetControl("m_btnAutoTypeEdit", sender as Form) as Button;
			if (btnAutoTypeEdit == null || !btnAutoTypeEdit.Enabled) return;
			btnAutoTypeEdit.PerformClick();
		}

		internal void Disable()
		{
			GlobalWindowManager.WindowAdded -= OnWindowAdded;

			EditAutoType.OnAddWindow -= OnMeasureItem;
			KeePass.Program.MainForm.EntryContextMenu.Opening -= EntryContextMenu_Opening;
			if (m_tsmiCtxContainer != null)
			{
				KeePass.Program.MainForm.EntryContextMenu.Items.Remove(m_tsmiCtxContainer);
				m_tsmiCtxContainer.Dispose();
				m_tsmiCtxContainer = null;
			}

			try
			{
				ToolStripMenuItem last = KeePass.Program.MainForm.MainMenu.Items["m_menuEntry"] as ToolStripMenuItem;
				last.DropDownOpening -= EntryMainMenu_Opening;
				if (m_tsmiMenuContainer != null)
				{
					last.DropDownItems.Remove(m_tsmiMenuContainer);
					m_tsmiMenuContainer.Dispose();
					m_tsmiMenuContainer = null;
				}
			}
			catch { }
		}

		private void AddMenu(ref ToolStripMenuItem tsmiContainer, ref ToolStripMenuItem tsmiLastWindow, ref ToolStripComboBox tscbWindows)
		{
			tsmiContainer = new ToolStripMenuItem(KPRes.AutoType + ": ", m_imgDefaultIcon);
			string s = "Add";
			var f = KeePass.Program.Translation.Forms.FirstOrDefault(x => x.FullName == "KeePass.Forms.PwEntryForm");
			if (f != null)
			{
				var c = f.Controls.FirstOrDefault(x => x.Name == "m_btnAutoTypeAdd");
				if (c != null) s = c.Text;
			}
			tsmiContainer.Text += s;
			tsmiContainer.DropDown = new ToolStripDropDown();
			tsmiContainer.DropDown.LayoutStyle = ToolStripLayoutStyle.Table;
			var settings = tsmiContainer.DropDown.LayoutSettings as TableLayoutSettings;
			settings.ColumnCount = 2;

			tsmiLastWindow = new ToolStripMenuItem(m_imgDefaultIcon);
			tsmiLastWindow.Click += OnAddPreviousWindow;
			tsmiLastWindow.Dock = DockStyle.Fill;
			tsmiLastWindow.ImageAlign = ContentAlignment.MiddleLeft;
			tsmiLastWindow.TextAlign = ContentAlignment.MiddleLeft;
			tsmiContainer.DropDown.Items.Add(tsmiLastWindow);
			settings.SetColumnSpan(tsmiLastWindow, 2);

			tscbWindows = new ToolStripComboBox();
			tscbWindows.Tag = tsmiContainer;
			tscbWindows.AutoSize = false;
			tscbWindows.ComboBox.DrawMode = DrawMode.OwnerDrawVariable;
			tscbWindows.ComboBox.DrawItem += EditAutoType.DrawItem;
			tsmiContainer.DropDown.Items.Add(tscbWindows);
			tsmiContainer.DropDown.Items.Add(new ToolStripButton() { Text = KPRes.Ok, Tag = tscbWindows });
			(tsmiContainer.DropDown.Items[2] as ToolStripButton).Click += OnAddSelectedWindow;
			tscbWindows.KeyDown += OnKeyDown;
			settings.SetColumnSpan(tscbWindows, 1);
			settings.SetColumnSpan(tsmiContainer.DropDown.Items[2], 1);
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				var t = (sender as ToolStripComboBox).GetCurrentParent() as ToolStripDropDown;
				t.Items[2].PerformClick();
			}
		}

		private void OnAddPreviousWindow(object sender, EventArgs e)
		{
			string s = (sender as ToolStripMenuItem).Tag as string;
			AddToAutotype(s);
		}

		private void OnAddSelectedWindow(object sender, EventArgs e)
		{
			ToolStripButton tsb = sender as ToolStripButton;
			ToolStripComboBox tscb = tsb.Tag as ToolStripComboBox;
			AddToAutotype(tscb.Text);
		}

		private void AddToAutotype(string s)
		{
			if (string.IsNullOrEmpty(s)) return;
			int i = 0;
			AWMDuplicateHandling dup = AWMDuplicateHandling.Undefined;
			bool bShift = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
			foreach (var pe in KeePass.Program.MainForm.GetSelectedEntries())
			{
				if (AddToAutotype(pe, s, ref dup)) i++;
			}
			if (i > 0) Tools.RefreshEntriesList(true);
			if ((dup == AWMDuplicateHandling.Edit || bShift) && m_miEditSelectedEntry != null)
			{
				m_sAddedWindowText = s;
				m_miEditSelectedEntry.Invoke(KeePass.Program.MainForm, new object[] { false });
			}
		}

		private bool AddToAutotype(PwEntry pe, string s, ref AWMDuplicateHandling dup)
		{
			if (pe.AutoType.Associations.FirstOrDefault(x => string.Compare(x.WindowName, s, true) == 0) != null)
			{
				if (dup == AWMDuplicateHandling.Undefined) dup = GetDuplicateHandling(s);
				if (dup != AWMDuplicateHandling.Add) return false;
			}
			pe.CreateBackup(KeePass.Program.MainForm.DocumentManager.SafeFindContainerOf(pe));
			pe.AutoType.Add(new KeePassLib.Collections.AutoTypeAssociation(s, string.Empty));
			pe.Touch(true, false);
			return true;
		}

		private AWMDuplicateHandling GetDuplicateHandling(string s)
		{
			VistaTaskDialog vtd = new VistaTaskDialog();
			vtd.AddButton(1, KPRes.EditEntry, null);
			vtd.AddButton(2, KPRes.AddEntry, null);
			vtd.AddButton(3, KPRes.Skip, null);
			vtd.MainInstruction = s;
			vtd.ExpandedInformation = string.Format(PluginTranslate.AWMCheckDuplicateDetails,
				KPRes.EditEntry,
				KPRes.AddEntry,
				KPRes.Skip);
			vtd.Content = PluginTranslate.AWMCheckDuplicateInfo;
			vtd.WindowTitle = PluginTranslate.PluginName + ": " + m_tsmiCtxContainer.Text.Replace("&", string.Empty);
			vtd.SetIcon(VtdIcon.Information);
			if (vtd.ShowDialog(KeePass.Program.MainForm))
			{
				if (vtd.Result == 1) return AWMDuplicateHandling.Edit;
				if (vtd.Result == 2) return AWMDuplicateHandling.Add;
				if (vtd.Result == 3) return AWMDuplicateHandling.Skip;
			}
			else
			{
				string text = PluginTranslate.AWMCheckDuplicateInfo+"\n\n"+string.Format(PluginTranslate.AWMCheckDuplicateDetails,
				DialogResult.Yes.ToString(),
				DialogResult.No.ToString(),
				DialogResult.Cancel.ToString());
				DialogResult r = MessageBox.Show(text, PluginTranslate.PluginName + ": " + m_tsmiCtxContainer.Text.Replace("&", string.Empty),
					MessageBoxButtons.YesNoCancel);
				if (r == DialogResult.Yes) return AWMDuplicateHandling.Edit;
				if (r == DialogResult.No) return AWMDuplicateHandling.Add;
				if (r == DialogResult.Cancel) return AWMDuplicateHandling.Skip;
			}
			return AWMDuplicateHandling.Skip;
		}

		private void EntryContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (e.Cancel) return;
			HandleMenu(m_tsmiCtxContainer, m_tsmiCtxLastWindow, m_tscbCtxWindows);
		}

		private void EntryMainMenu_Opening(object sender, EventArgs e)
		{
			HandleMenu(m_tsmiMenuContainer, m_tsmiMenuLastWindow, m_tscbMenuWindows);
		}

		private void HandleMenu(ToolStripMenuItem tsmiContainer, ToolStripMenuItem tsmiLastWindow, ToolStripComboBox tscbWindows)
		{
			if (!EditAutoType.Valid)
			{
				Disable();
				return;
			}
			tsmiContainer.Enabled = false;
			tsmiLastWindow.Visible = false;
			tscbWindows.Items.Clear();
			tscbWindows.Text = string.Empty;
			tscbWindows.Width = 0;
			if (KeePass.Program.MainForm.GetSelectedEntriesCount() < 1) return;
			EditAutoType.Refresh();
		}

		private void OnMeasureItem(object sender, MeasureItemEventArgs e)
		{
			string s = (sender as ComboBox).Items[e.Index] as string;
			int w = (int)e.Graphics.MeasureString(s, m_tscbCtxWindows.Font).Width;
			w = Math.Min(w, (sender as ComboBox).Width);
			AddWindowText(s, m_tscbCtxWindows, w);
			AddWindowText(s, m_tscbMenuWindows, w);
		}

		private void AddWindowText(string s, ToolStripComboBox tscb, int w)
		{
			if (tscb == null) return;
			tscb.Items.Add(s);
			tscb.Width = Math.Max(tscb.Width, w);

			var t = tscb.Tag as ToolStripMenuItem;
			if (t == null) return;
			if (tscb.Items.Count == 1)
			{
				t.Enabled = true;
				t.DropDown.Items[0].Visible = true;
				t.DropDown.Items[0].Tag = EditAutoType.FirstWindowText;
				int iMaxLength = 70;
				if (EditAutoType.FirstWindowText.Length > iMaxLength)
					t.DropDown.Items[0].Text = EditAutoType.FirstWindowText.Substring(0, iMaxLength - 5) + "...";
				else
					t.DropDown.Items[0].Text = EditAutoType.FirstWindowText;
				t.DropDown.Items[0].Image = EditAutoType.FirstWindowImage;
				if (t.DropDown.Items[0].Image == null) t.DropDown.Items[0].Image = m_imgDefaultIcon;
			}
		}

		private enum AWMDuplicateHandling
		{
			Undefined,
			Edit,
			Add,
			Skip
		}

		private class EditAutoType
		{
			private static Form f = null;
			private static ComboBox WindowTexts = null;
			private static List<Image> Images = null;
			private static MethodInfo PopulateWindowsList = null;
			private static MethodInfo DoDrawItem = null;

			internal static EventHandler<MeasureItemEventArgs> OnAddWindow = null;

			internal static string FirstWindowText
			{
				get
				{
					if (!Valid) return string.Empty;
					if (WindowTexts.Items.Count == 0) return string.Empty;
					return WindowTexts.Items[0] as string;
				}
			}

			internal static Image FirstWindowImage
			{
				get
				{
					if (!Valid) return null;
					if (Images.Count == 0) return null;
					return Images[0];
				}
			}

			private static bool m_FirstTime = true;
			internal static bool Valid
			{
				get
				{
					if (m_FirstTime) Init();
					return f != null;
				}
			}

			internal static void Refresh()
			{
				if (!Valid) return;
				WindowTexts.Items.Clear();
				if (Images != null)
				{
					foreach (var img in Images)
					{
						if (img != null) img.Dispose();
					}
					Images.Clear();
				}
				PopulateWindowsList.Invoke(f, null);


				//On Windows the window names are added one by one using a new thread
				//OnMeasureItem is invoked and we don't need to call it manually
				if (!KeePassLib.Native.NativeLib.IsUnix()) return;

				using (var g = WindowTexts.CreateGraphics())
				{
					for (int i = 0; i < WindowTexts.Items.Count; i++)
					{
						MeasureItemEventArgs e = new MeasureItemEventArgs(g, i);
						OnMeasureItem(WindowTexts, e);
					}
				}
			}

			private static void Init()
			{
				m_FirstTime = false;
				f = new EditAutoTypeItemForm();
				//Ensure handle is created
				//PopulateWindowsListWin uses IfInvokeRequired which won't work otherwise
				List<string> lMsg = new List<string>();
				lMsg.Add("Created EditAutoTypeItemForm, handle: " + f.Handle.ToString());

				WindowTexts = (ComboBox)Tools.GetControl("m_cmbWindow", f);
				Images = (List<Image>)Tools.GetField("m_vWndImages", f);

				lMsg.Add("m_cmbWindow: " + WindowTexts == null ? "Not found" : "Found");
				lMsg.Add("m_vWndImages: " + Images == null ? "Not found" : "Found");

				if (KeePassLib.Native.NativeLib.IsUnix())
					PopulateWindowsList = f.GetType().GetMethod("PopulateWindowsListUnix", BindingFlags.Instance | BindingFlags.NonPublic);
				else
					PopulateWindowsList = f.GetType().GetMethod("PopulateWindowsListWin", BindingFlags.Instance | BindingFlags.NonPublic);
				lMsg.Add("PopulateWindowsList: " + PopulateWindowsList == null ? "Not found" : PopulateWindowsList.Name);

				DoDrawItem = WindowTexts.GetType().GetMethod("OnDrawItem", BindingFlags.Instance | BindingFlags.NonPublic);
				lMsg.Add("DoDrawItem: " + DoDrawItem == null ? "Not found" : DoDrawItem.Name);

				PluginDebug.AddInfo("Init AutoTypeWindowwatcher", 0, lMsg.ToArray());

				if (WindowTexts == null || PopulateWindowsList == null || DoDrawItem == null)
				{
					f.Dispose();
					f = null;
					return;
				}
				WindowTexts.MeasureItem += OnMeasureItem;
			}

			private static void OnMeasureItem(object sender, MeasureItemEventArgs e)
			{
				if (!Valid) return;
				if (OnAddWindow != null) OnAddWindow(sender, e);
			}

			internal static void DrawItem(object sender, DrawItemEventArgs e)
			{
				if (!Valid) return;
				DoDrawItem.Invoke(WindowTexts, new object[] { e });
			}
		}
	}
}
