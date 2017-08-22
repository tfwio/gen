/* oio : 1/21/2014 9:33 AM */
using System;
using System.Linq;
using System.Windows.Data;

namespace GeneratorTool.Views
{
	public class IsEnabledToColorConverter : IValueConverter
	{
		#region IValueConverter Members
		static readonly System.Windows.Media.Color ForegroundActive = System.Windows.Media.Color.FromRgb(0,127,255);
		static readonly System.Windows.Media.Color ForegroundInactive = System.Windows.Media.Color.FromRgb(127,127,127);
		static readonly System.Windows.Media.SolidColorBrush ForegroundActiveBrush = new System.Windows.Media.SolidColorBrush(ForegroundActive);
		static readonly System.Windows.Media.SolidColorBrush ForegroundInactiveBrush = new System.Windows.Media.SolidColorBrush(ForegroundInactive);
		
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if ((bool)value) return ForegroundActiveBrush;
			return ((bool) value) ? ForegroundActiveBrush : ForegroundInactiveBrush;
		}
	
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	
		#endregion
	}
}



