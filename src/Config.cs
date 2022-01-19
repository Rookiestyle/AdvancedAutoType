using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using KeePass;
using PluginTools;
using PluginTranslation;

namespace AlternateAutoType
{
	public static class Config
	{
		public static readonly string SortIcon = "AlternateAutoTypeExtSortIcon";
		public static readonly string DBColumn = "AlternateAutoTypeExtDBColumn";
		public static readonly string PWColumn = "AlternateAutoTypeExtPWColumn";
		public static readonly string PWColumnHeader = KeePass.Resources.KPRes.Password + " (AAT)";
		public static string Placeholder = "{AAT}";

		private static PropertyInfo m_piHotKeyGlobalAutoTypePassword = null;
		public static bool KPAutoTypePWPossible { get; private set; }

		static Config()
		{
			if (KeePassLib.Native.NativeLib.IsUnix())
			{
				KPAutoTypePWPossible = false;
				return;
			}
			if (!Program.Config.CustomConfig.GetBool("AlternateAutoType.UseAutoTypePasswordHotKey", true)) return;
			KPAutoTypePWPossible = Tools.KeePassVersion >= new Version(2, 41);
			if (!KPAutoTypePWPossible) return;
			m_piHotKeyGlobalAutoTypePassword = Program.Config.Integration.GetType().GetProperty("HotKeyGlobalAutoTypePassword");

			if (KPAutoTypePWPossible && (m_piHotKeyGlobalAutoTypePassword != null))
				PluginDebug.AddInfo("Hooking Program.Config.Integration.HotKeyGlobalAutoTypePassword successful");
			else
			{
				PluginDebug.AddError("Hooking Program.Config.Integration.HotKeyGlobalAutoTypePassword failed");
				KPAutoTypePWPossible = false;
			}
		}

		public static bool SearchAsYouType
		{
			get { return Program.Config.CustomConfig.GetBool(m_SearchAsYouType, true); }
			set { Program.Config.CustomConfig.SetBool(m_SearchAsYouType, value); }
		}
		public static bool AddDBColumn
		{
			get { return Program.Config.CustomConfig.GetBool(m_AddDBColumnConfig, false); }
			set { Program.Config.CustomConfig.SetBool(m_AddDBColumnConfig, value); }
		}
		public static bool ColumnsSortable
		{
			get { return Program.Config.CustomConfig.GetBool(m_ColumnsSortableConfig, false); }
			set { Program.Config.CustomConfig.SetBool(m_ColumnsSortableConfig, value); }
		}
		public static bool ColumnsRememberSorting
		{
			get { return Program.Config.CustomConfig.GetBool(m_ColumnsRememberSorting, false); }
			set { Program.Config.CustomConfig.SetBool(m_ColumnsRememberSorting, value); }
		}
		public static long ColumnsSortColumn
		{
			get { return Program.Config.CustomConfig.GetLong(m_ColumnsSortColumn, 0); }
			set { Program.Config.CustomConfig.SetLong(m_ColumnsSortColumn, value); }
		}
		public static bool ColumnsSortGrouping
		{
			get { return Program.Config.CustomConfig.GetBool(m_ColumnsSortGrouping, false); }
			set { Program.Config.CustomConfig.SetBool(m_ColumnsSortGrouping, value); }
		}
		public static bool SpecialColumns
		{
			get
			{
				if (KeePassLib.Native.NativeLib.IsUnix()) return false;
				return Program.Config.CustomConfig.GetBool(m_SpecialColumnsConfig, false);
			}
			set
			{
				if (KeePassLib.Native.NativeLib.IsUnix()) return;
				Program.Config.CustomConfig.SetBool(m_SpecialColumnsConfig, value);
			}
		}
		public static bool KeepATOpen
		{
			get
			{
				if (KeePassLib.Native.NativeLib.IsUnix()) return false;
				return Program.Config.CustomConfig.GetBool(m_KeepATOpenConfig, false);
			}
			set
			{
				if (KeePassLib.Native.NativeLib.IsUnix()) return;
				Program.Config.CustomConfig.SetBool(m_KeepATOpenConfig, value);
			}
		}

		public static bool SpecialColumnsRespectPWEnter
		{
			get
			{
				if (KeePassLib.Native.NativeLib.IsUnix()) return false;
				return Program.Config.CustomConfig.GetBool(m_SpecialColumnsRespectPWEnter, true);
			}
			set
			{
				if (KeePassLib.Native.NativeLib.IsUnix()) return;
				Program.Config.CustomConfig.SetBool(m_SpecialColumnsRespectPWEnter, value);
			}
		}

		public static bool SpecialColumnsRespectUsernameEnter
		{
			get
			{
				if (KeePassLib.Native.NativeLib.IsUnix()) return false;
				return Program.Config.CustomConfig.GetBool(m_SpecialColumnsRespectUsernameEnter, true);
			}
			set
			{
				if (KeePassLib.Native.NativeLib.IsUnix()) return;
				Program.Config.CustomConfig.SetBool(m_SpecialColumnsRespectUsernameEnter, value);
			}
		}

		public static bool PWEnter
		{
			get { return Program.Config.CustomConfig.GetBool(m_PWEnterConfig, false); }
			set { Program.Config.CustomConfig.SetBool(m_PWEnterConfig, value); }
		}

		public static Keys UsernameOnlyHotkey
		{
			get { return ReadKey(m_UserOnlyHotkeyConfig, PluginTranslate.ErrorHotKeyUsernameOnly); }
			set { SetKey(m_UserOnlyHotkeyConfig, value); }
		}
		public static int UsernameOnlyHotkeyID = 0;
		public static bool UsernameOnlyEnter
		{
			get { return Program.Config.CustomConfig.GetBool(m_UserOnlyEnterConfig, false); }
			set { Program.Config.CustomConfig.SetBool(m_UserOnlyEnterConfig, value); }
		}

		public static Keys AATHotkey
		{
			get { return ReadKey(m_AATHotkeyConfig, PluginTranslate.ErrorHotKeyAAT); }
			set { SetKey(m_AATHotkeyConfig, value); }
		}
		public static int AATHotkeyID = 0;

		public static Keys PWOnlyHotkey
		{
			get { return ReadKey(m_PWOnlyHotkeyConfig, PluginTranslate.ErrorHotKeyPWOnly); }
			set { SetKey(m_PWOnlyHotkeyConfig, value); }
		}
		public static int PWOnlyHotkeyID = 0;

		public static bool ExcludeExpiredGroups
		{
			get { return Program.Config.CustomConfig.GetBool(m_ExcludeExpiredGroups, true); }
			set { Program.Config.CustomConfig.SetBool(m_ExcludeExpiredGroups, value); }
		}

		private static string m_AATHotkeyConfig = "AlternateAutoType.AATHotkey";
		private static string m_PWOnlyHotkeyConfig = "AlternateAutoType.PWOnlyHotkey";
		private static string m_PWEnterConfig = "AlternateAutoType.PWEnter";
		private static string m_UserOnlyHotkeyConfig = "AlternateAutoType.UserOnlyHotkey";
		private static string m_UserOnlyEnterConfig = "AlternateAutoType.UserOnlyEnter";
		private static string m_SearchAsYouType = "AlternateAutoType.SearchAsYouType";
		private static string m_AddDBColumnConfig = "AlternateAutoType.AddDBColumn";
		private static string m_ColumnsSortableConfig = "AlternateAutoType.ColumnsSortable";
		private static string m_ColumnsRememberSorting = "AlternateAutoType.ColumnsRememberSorting";
		private static string m_ColumnsSortColumn = "AlternateAutoType.ColumnsSortColumn";
		private static string m_ColumnsSortGrouping = "AlternateAutoType.ColumnsSortGrouping";
		private static string c_PWColumnHeader = KeePass.Resources.KPRes.Password + " (AAT)";
		private static string m_SpecialColumnsConfig = "AlternateAutoType.SpecialColumns";
		private static string m_KeepATOpenConfig = "AlternateAutoType.KeepATOpen";
		private static string m_SpecialColumnsRespectPWEnter = "AlternateAutoType.SpecialColumnsRespectPWEnter";
		private static string m_SpecialColumnsRespectUsernameEnter = "AlternateAutoType.SpecialColumnsRespectUsernameEnter";
		private static string m_ExcludeExpiredGroups = "AlternateAutoType.ExcludeExpiredGroups";

		private static void SetKey(string param, Keys value)
		{
			if (KPAutoTypePWPossible && (param == m_PWOnlyHotkeyConfig))
			{
				//Depending on KeePass version this field is either ulong or long
				try { m_piHotKeyGlobalAutoTypePassword.SetValue(Program.Config.Integration, (ulong)value, null); }
				catch
				{
					try { m_piHotKeyGlobalAutoTypePassword.SetValue(Program.Config.Integration, (long)value, null); }
					catch { }
				}
				KeePass.Util.HotKeyManager.RegisterHotKey(KeePass.App.AppDefs.GlobalHotKeyId.AutoTypePassword, value);
			}
			Program.Config.CustomConfig.SetString(param, value.ToString());
		}

		private static Keys ReadKey(string param, string errormsg)
		{
			Keys kATP = Keys.None;
			if (KPAutoTypePWPossible && (param == m_PWOnlyHotkeyConfig))
			{
				bool bError = false;
				//Depending on KeePass version this field is either ulong or long
				try { kATP = (Keys)((ulong)m_piHotKeyGlobalAutoTypePassword.GetValue(Program.Config.Integration, null)); }
				catch
				{
					try { kATP = (Keys)((long)m_piHotKeyGlobalAutoTypePassword.GetValue(Program.Config.Integration, null)); }
					catch { bError = true; }
				}
				if (!bError) return kATP;
			}
			string helper = Program.Config.CustomConfig.GetString(param, Keys.None.ToString());
			Keys result = Keys.None;
			try
			{
				result = (Keys)Enum.Parse(typeof(Keys), helper);
			}
			catch
			{
				SetKey(m_AATHotkeyConfig, Keys.None);
				string msg = string.Format(errormsg, helper);
				Tools.ShowError(msg);
			}
			if (KPAutoTypePWPossible && (param == m_PWOnlyHotkeyConfig))
			{
				SetKey(param, result);
			}
			return result;
		}
	}
}
