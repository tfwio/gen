/* oio : 1/21/2014 9:33 AM */
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
namespace GeneratorTool.Views
{
	public class GroupOnMultiBindingConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture){
			
			var param = (parameter==null) ? (string)null : parameter as string;
			Debug.WriteLine("{0}: {1}", param, values);
			return values;
			// we want a template item?
		}
		
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture){
			throw new NotImplementedException();
		}
	}
}





