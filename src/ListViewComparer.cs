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
	}
}
