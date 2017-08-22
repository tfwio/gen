#region User/License
// oio * 8/22/2012 * 12:59 PM

// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
#endregion
using System.Collections.Generic;
using System.IO;
//using Microsoft.Win32;

namespace System
{

	// mscorlib32 (v2.0)
	class Reg
	{
		static public int[] TranslateString(string input, string delimiter = ",")
		{
			if (string.IsNullOrEmpty(input)) return null;
			if (!input.Contains(delimiter)) return null;
			string[] a = input.Split(',');
			int[] l = new int[a.Length];
			for (int i=0; i< a.Length; i++) l[i] = int.Parse(a[i]);
			a = null;
			GC.Collect();
			return l;
		}
		static public string TranslateString(int[] input, string delimiter = ",")
		{
			if (input==null) return null;
			List<string> list = new List<string>();
			foreach (int i in input) list.Add(i.ToString());
			string[] l = list.ToArray();
			string value = string.Join(delimiter,l);
			l = null;
			list = null;
			GC.Collect();
			return value;
		}

		static public void SetControlFontSize(System.Windows.Forms.Control listview, float size)
		{
			listview.Font = new System.Drawing.Font(listview.Font.FontFamily,size,System.Drawing.GraphicsUnit.Point);
		}
		/// <summary>
		/// Current User
		/// </summary>
		/// <param name="path"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		static public string GetKeyValueString(string path, string key)
		{
			string value = null;
			using(Microsoft.Win32.RegistryKey rkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(path,false))
			{
				if (rkey==null) return null;
				value = string.Format("{0}",rkey.GetValue(key));
				rkey.Close();
			}
			return value;
		}
		static public void SetKeyValueString(string path, string key, object value)
		{
			Microsoft.Win32.RegistryKey rkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(path,true);
			if (rkey==null) {
				rkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(path,Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);
			}
			if (value!=null) rkey.SetValue(key,value,Microsoft.Win32.RegistryValueKind.String);
			rkey.Close();
			rkey = null;
			GC.Collect();
		}
	}
	
	/**
	  * I was trying to get SaveFileDialog and OpenFileDialog working in the
	  * using 'system.windows' for WPF apps and
	  * using System.Windows.Forms for the WinForms apps.
	  * This in many cases broke down with the intellisense in SharpDevelop
	  * and perhaps also VCSharp, so we're using #if pragmas.
	  **/
	/// <summary>
	/// Use me as an include.
	/// </summary>
	static class Helper
	{
		static public bool CheckFile(
			System.Windows.Forms.SaveFileDialog sfd,
			string filter,
			string filterExceptionMsg = "The file ‘{0}’ didn't exist"
		)
		{
			if (!DialogResultIsOK(sfd,filter)) return false;
			try {
				CheckFile(sfd.FileName,filterExceptionMsg);
			} catch {
				return false;
			}
			return true;
		}
		static public bool CheckFile(
			System.Windows.Forms.OpenFileDialog ofd,
			string filter,
			string filterExceptionMsg = "The file ‘{0}’ didn't exist"
		)
		{
			if (!DialogResultIsOK(ofd,filter)) return false;
			try {
				CheckFile(ofd.FileName,filterExceptionMsg);
			} catch {
				return false;
			}
			return true;
		}
		/// <summary>
		/// This method requires use of msbuild >= 4.0
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="filterExceptionMsg"></param>
		/// <returns></returns>
		static public void CheckFile(
			string fileName,
			string filterExceptionMsg = "The file ‘{0}’ didn't exist")
		{
			if (!File.Exists(fileName))
				throw new FileLoadException(
					string.Format(
						filterExceptionMsg,
						Path.GetFileName(fileName)
					));
		}
		
		static public bool DialogResultIsOK(System.Windows.Forms.FileDialog ofd, string filter)
		{
			#if WPF4
			throw new NotImplementedException("hehaw (not implemented).");
			#else
			if (filter!=null) ofd.Filter = filter;
			return ofd.ShowDialog()== System.Windows.Forms.DialogResult.OK;
			#endif
		}
		static public bool DialogResultIsOK(System.Windows.Forms.OpenFileDialog ofd, string filter)
		{
			#if WPF4
			throw new NotImplementedException("hehaw (not implemented).");
			#else
			if (filter!=null) ofd.Filter = filter;
			return ofd.ShowDialog()== System.Windows.Forms.DialogResult.OK;
			#endif
		}
		static public bool DialogResultIsOK(System.Windows.Forms.SaveFileDialog ofd, string filter)
		{
			#if WPF4
			throw new NotImplementedException("hehaw (not implemented).");
			#else
			if (filter!=null) ofd.Filter = filter;
			return ofd.ShowDialog()== System.Windows.Forms.DialogResult.OK;
			#endif
		}
		
		#if WPF4
		static public bool MessageBoxIsOK(string message, string caption, System.Windows.MessageBoxButton buttons, System.Windows.MessageBoxImage icon)
		{
			return System.Windows.MessageBox.Show(message,caption,buttons,icon) == System.Windows.MessageBoxResult.OK;
		}
		static public bool MessageBoxIsOK(string message,string caption)
		{
			return MessageBoxIsOK(message, caption, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
		}
		#else
		static public bool MessageBoxIsOK(string message, string caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon)
		{
			return System.Windows.Forms.MessageBox.Show(message,caption,buttons,icon) == System.Windows.Forms.DialogResult.OK;
		}
		static public bool MessageBoxIsOK(string message,string caption)
		{
			return MessageBoxIsOK(message,caption,System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Information);
		}
		#endif
	}
}
