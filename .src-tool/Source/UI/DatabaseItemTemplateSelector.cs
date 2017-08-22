/* oio : 1/21/2014 9:33 AM */
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Generator.Elements;
using Generator.Elements.Basic;

namespace GeneratorTool.Views
{
	public class DatabaseItemTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			var element = container as FrameworkElement;
      if (element != null && item != null && item is DataMapElement)
			{
				if (item is TableElement) return element.FindResource("TableTemplate") as DataTemplate;
				return element.FindResource("DataViewTemplate") as DataTemplate;
			}
			return null;
		}
	}
}



