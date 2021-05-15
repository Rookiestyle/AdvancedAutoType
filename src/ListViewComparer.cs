using System;
using System.Collections;
using System.Windows.Forms;

namespace AlternateAutoType
{
	public class ListViewComparer : IComparer
	{
		public int Column;
		public SortOrder SortOrder;
		public bool NoSort = false;

		public ListViewComparer(int column, SortOrder so)
		{
			Column = column;
			SortOrder = so;
		}

		public int Compare(object objA, object objB)
		{
			if (NoSort) return 0;
			ListViewItem liA = objA as ListViewItem;
			ListViewItem liB = objB as ListViewItem;

			int direction = SortOrder == SortOrder.Ascending ? 1 : -1;

			string stringA = string.Empty;
			string stringB = string.Empty;
			if (liA.SubItems.Count > Column)
				stringA = liA.SubItems[Column].Text;

			if (liB.SubItems.Count > Column)
				stringB = liB.SubItems[Column].Text;

			//different comparison logic depending on type of data
			double doubleA, doubleB;
			if (double.TryParse(stringA, out doubleA) && double.TryParse(stringB, out doubleB))
				return direction * doubleA.CompareTo(doubleB);

			DateTime dateA, dateB;
			if (DateTime.TryParse(stringA, out dateA) && DateTime.TryParse(stringB, out dateB))
				return direction * dateA.CompareTo(dateB);

			return direction * string.Compare(stringA, stringB, StringComparison.CurrentCultureIgnoreCase);
		}
		public void UpdateColumnSortingIcons(ListView lv)
		{
			if (lv == null) return;
			if (KeePass.UI.UIUtil.SetSortIcon(lv, Column, SortOrder)) return;

			// if(m_lvEntries.SmallImageList == null) return;

			if (Column < 0) return;

			string strAsc = "  \u2191"; // Must have same length
			string strDsc = "  \u2193"; // Must have same length
			if (KeePass.Util.WinUtil.IsWindows9x || KeePass.Util.WinUtil.IsWindows2000 || KeePass.Util.WinUtil.IsWindowsXP ||
				KeePassLib.Native.NativeLib.IsUnix())
			{
				strAsc = @"  ^";
				strDsc = @"  v";
			}
			else if (KeePass.Util.WinUtil.IsAtLeastWindowsVista)
			{
				strAsc = "  \u25B3";
				strDsc = "  \u25BD";
			}

			foreach (ColumnHeader ch in lv.Columns)
			{
				string strCur = ch.Text, strNew = null;

				if (strCur.EndsWith(strAsc) || strCur.EndsWith(strDsc))
				{
					strNew = strCur.Substring(0, strCur.Length - strAsc.Length);
					strCur = strNew;
				}

				if ((ch.Index == Column) &&
					(SortOrder != SortOrder.None))
				{
					if (SortOrder == SortOrder.Ascending)
						strNew = strCur + strAsc;
					else if (SortOrder == SortOrder.Descending)
						strNew = strCur + strDsc;
				}

				if (strNew != null) ch.Text = strNew;
			}
		}
	}
}
