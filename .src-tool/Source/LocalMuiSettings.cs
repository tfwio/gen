using System;
using System.Linq;
using System.Windows.Media;

using FntSize = FirstFloor.ModernUI.Presentation.FontSize;

namespace GeneratorTool
{
	static class LocalMuiSettings
	{
		static public void Assert()
		{
			string localX = LocalMuiSettings.MuiColorTheme;
			if (localX!=null) {
				Color c = ColorUtil.ColorFromHex(localX);
				FirstFloor.ModernUI.Presentation.AppearanceManager.Current.AccentColor = c;
			}
			
			localX = LocalMuiSettings.MuiFontSize;
			if (localX!=null) {
				FirstFloor.ModernUI.Presentation.AppearanceManager.Current.FontSize = localX == "large" ? FntSize.Large : FntSize.Small;
			}
			
			localX = LocalMuiSettings.MuiTheme;
			if (localX!=null) {
				FirstFloor.ModernUI.Presentation.AppearanceManager.Current.ThemeSource = new Uri(localX,UriKind.RelativeOrAbsolute);
			}
		}
		
		
		#region Registry
		// root registry path for your app
		const string regpath = @"Software\tfwio\generator-wpf";

		// a key --- I only use string values FYI
		const string MUI_COLOR = "mui-color"; // such as light or dark
		/// <summary>
		/// Set/Get the MuiTheme setting (Light/Dark)
		/// </summary>
		static public string MuiColorTheme
		{
			get { return Reg.GetKeyValueString(regpath, MUI_COLOR); }
			set { Reg.SetKeyValueString(regpath, MUI_COLOR, value); }
		}
		const string MUI_SIZE = "mui-size";
		/// <summary>
		/// Set/Get the MuiTheme setting
		/// </summary>
		static public string MuiFontSize
		{
			get { return Reg.GetKeyValueString(regpath, MUI_SIZE); }
			set { Reg.SetKeyValueString(regpath, MUI_SIZE, value); }
		}
		const string MUI_THEME = "mui-theme";
		/// <summary>
		/// Set/Get the MuiTheme setting
		/// </summary>
		static public string MuiTheme
		{
			get { return Reg.GetKeyValueString(regpath, MUI_THEME); }
			set { Reg.SetKeyValueString(regpath, MUI_THEME, value); }
		}
		#endregion
		const string LAST_PATH = "selected-path";
		/// <summary>
		/// Set/Get the MuiTheme setting
		/// </summary>
		static public string SelectedPath
		{
			get { return Reg.GetKeyValueString(regpath, LAST_PATH); }
			set { Reg.SetKeyValueString(regpath, LAST_PATH, value); }
		}
	}
	static class ColorUtil
	{
		static public int IntFromHexColor(string color)
		{
			Color c = (Color)ColorConverter.ConvertFromString(color);
			return (c.A << 24) + (c.R << 16) + (c.G << 8) + c.B;
		}
		static public Color ColorFromHex(string hexString)
		{
			if (hexString[0] != '#') return (Color)ColorConverter.ConvertFromString(String.Concat("#",hexString));
			return (Color)ColorConverter.ConvertFromString(hexString);
		}
		
		/// <summary>
		/// For RGBA in hex
		/// </summary>
		/// <param name="c"></param>
		/// <param name="useHash"></param>
		/// <returns></returns>
		static public string ColorToHex8(Color c, bool useHash)
		{
			return string.Format(useHash ? "{0:X8}" : "{0:X8}", (c.A << 24) + (c.R << 16) + (c.G << 8) + c.B);
		}
		static public string ColorToHex8(Color c)
		{
			return ColorToHex8(c,true);
		}
		/// <summary>
		/// Here, the alpha value is ignored.
		/// </summary>
		/// <param name="c"></param>
		/// <param name="useHash"></param>
		/// <returns></returns>
		static public string ColorToHex6(Color c, bool useHash)
		{
			return string.Format(useHash ? "#{0:X6}" : "{0:X6}", (c.R << 16) + (c.G << 8) + c.B);
		}
	}
	static class Reg
	{
		/// <summary>
		/// Current User
		/// </summary>
		/// <param name="path"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		static public string GetKeyValueString(string path, string key)
		{
			string value = null;
			try {
				
				using (Microsoft.Win32.RegistryKey rkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(path, false))
				{
					if (rkey == null) return null;
					value = string.Format("{0}", rkey.GetValue(key));
					rkey.Close();
				}
			} catch {
			}
			return value;
		}
		static public void SetKeyValueString(string path, string key, object value)
		{
			Microsoft.Win32.RegistryKey rkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(path, true);
			if (rkey == null)
			{
				rkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(path, Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);
			}
			if (value != null) rkey.SetValue(key, value, Microsoft.Win32.RegistryValueKind.String);
			rkey.Close();
			rkey = null;
			GC.Collect();
		}
	}
}
