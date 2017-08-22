﻿#define LOCALVLC1
// delete the 1
using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
namespace GeneratorTool
{

	public class RectConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			double width = (double)values[0];
			double height = (double)values[1];
			return new Rect(0, 0, width, height);
		}
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
