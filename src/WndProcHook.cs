using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace PluginTools
{
	#region WndProc for other forms
	public class WndProcEventArgs : EventArgs
	{
		public Form form;
		public Message m;
		public bool SkipBase;

		public WndProcEventArgs(Form form, Message m)
		{
			this.form = form;
			this.m = m;
			SkipBase = false;
		}
	}

	public static class WndProcHook
	{
		private static Dictionary<Form, WndProcHookForm> m_forms = new Dictionary<Form, WndProcHookForm>();

		public static bool AddHandler(Form form, EventHandler<WndProcEventArgs> handler)
		{
			if (form == null) return false;
			WndProcHookForm f = null;
			m_forms.TryGetValue(form, out f);
			if (f == null)
			{
				f = new WndProcHookForm(form);
				m_forms[form] = f;
			}
			f.WndProcEvent += handler;
			return true;
		}

		public static void RemoveHandler(Form form)
		{
			if (form == null) return;
			WndProcHookForm f = null;
			m_forms.TryGetValue(form, out f);
			if (f != null) f.Cleanup();
			m_forms.Remove(form);
		}

		public static void RemoveHandler(Form form, EventHandler<WndProcEventArgs> handler)
		{
			if (form == null) return;
			WndProcHookForm f = null;
			bool found = m_forms.TryGetValue(form, out f);
			if (!found) return;
			f.WndProcEvent -= handler;
		}

		private class WndProcHookForm : NativeWindow
		{
			private Form m_form = null;
			public event EventHandler<WndProcEventArgs> WndProcEvent;

			public WndProcHookForm(Form form)
			{
				if (form == null) return;
				this.m_form = form;
				if (!form.IsHandleCreated)
					form.HandleCreated += OnHandleCreated;
				else
					OnHandleCreated(form, null);
				form.HandleDestroyed += OnHandleDestroyed;
			}

			public void Cleanup()
			{
				m_form.HandleDestroyed -= OnHandleDestroyed;
				try
				{
					ReleaseHandle();
				}
				catch (Exception) { }
			}

			internal void OnHandleDestroyed(object sender, EventArgs e)
			{
				ReleaseHandle();
			}

			protected override void WndProc(ref Message m)
			{
				WndProcEventArgs e = new WndProcEventArgs(m_form, m);
				if (WndProcEvent != null)
					WndProcEvent(m_form, e);
				if (e.SkipBase) return;
				base.WndProc(ref m);
			}

			private void OnHandleCreated(object sender, EventArgs args)
			{
				AssignHandle(((Form)sender).Handle);
			}
		}
	}
	#endregion
}