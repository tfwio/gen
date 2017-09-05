/*
 * User: oIo
 * Date: 2/2/2011
 * Time: 2:33 AM
 */
using System;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using Generator.Extensions;
using ICSharpCode.SharpDevelop;

namespace GeneratorTool.Views.Controls
{
	/// <summary>
	/// Interaction logic for UcPanelSpec.xaml
	/// </summary>
	public partial class DataEditorContent : UserControl, IContent
	{
		
		public void OnFragmentNavigation(FragmentNavigationEventArgs e){
			System.Diagnostics.Debug.WriteLine("OnFragmentNavigation: {0}, {1}", e.Fragment);
		}

		public void OnNavigatedFrom(NavigationEventArgs e){
			System.Diagnostics.Debug.WriteLine("OnNavigatedFrom: {0}, {1}",e.Frame.Content, e.Content);
		}

		public void OnNavigatedTo(NavigationEventArgs e){
			System.Diagnostics.Debug.WriteLine("OnNavigatedTo: {0}, {1}",e.Frame.Content, e.Content);
		}

		public void OnNavigatingFrom(NavigatingCancelEventArgs e){
			System.Diagnostics.Debug.WriteLine("OnNavigatingFrom: {0}",e.Frame.Content);
		}
		
		public DataEditorContent()
		{
			InitializeComponent();
			InitializeDataSources();
//			customItems.Add("custom item",new TextBox());
//			customItems.Add("custom item",new TextBox());
//			customItems.Add("custom item",new ComboBox());
//			customItems.Add("custom item",new TextBox());
//			customItems.Add("custom item",new TextBox());
		}
		
		/// <summary>
		/// needs to check on the Database Type of the collection or DataTable
		/// </summary>
		public void InitializeDataSources()
		{
			comboWebFormType.ItemsSource = System.TypeCode.GetValues(typeof(Generator.Elements.Types.WebFormTypes));
			// we would like to get access database types, but can wait for this.
			//FIXME: we need this to update via an event-handler
			viewField.comboDataType.ItemsSource = System.TypeCode.GetValues(typeof(System.Data.SqlDbType));
			viewField.comboDataNativeType.ItemsSource = System.TypeCode.GetValues(typeof(System.TypeCode));
			viewTable.comboDatabaseType.ItemsSource = Enum.GetValues(typeof(Generator.Elements.Types.DatabaseType));
			viewTable.comboTableType.ItemsSource = Enum.GetValues(typeof(Generator.Elements.Types.DatabaseType));
		}
		
		#region methods PrimaryKey
		#if NO
		
		public UITemplateManager Data;
		
		bool SelectionIsPrimaryKey { get { return !Data.HasSelectionForField ? false : Data.SelectedTable.PrimaryKey == Data.SelectedField.DataName; } }
		
		void PrimaryKeyUnChecked(Object sender, RoutedEventArgs args)
		{
			if (!Data.HasSelectionForField) return;
			Data.SelectedTable.PrimaryKey = null;
		}
		void PrimaryKeyChecked(Object sender, RoutedEventArgs args)
		{
			if (!Data.HasSelectionForField) return;
			Data.SelectedTable.PrimaryKey = Data.SelectedField.DataName;
		}
		
		public void BindToData(UITemplateManager ifacto)
		{
			Logger.LogG("field-editor","bind-to-data");
			
			// reset WPF Binding
			Data = null;
			Data = ifacto;
			
			viewField.checkIsPrimaryKey.Checked -= PrimaryKeyChecked;
			viewField.checkIsPrimaryKey.Unchecked -= PrimaryKeyUnChecked;
			
			// FIXME: for third-party support we should handle this in a external function
			// that may in the future out-source.
			
			viewField.comboDataType.ItemsSource = null;
			
			if (Data.SelectedTable.DbType == "OleAccess") {
				viewField.comboDataType.ItemsSource = AccessDataExtension.AceStringNames(0);
			} else if (Data.SelectedTable.DbType == "SQLite") {
				viewField.comboDataType.ItemsSource = Enum.GetNames(typeof(System.Data.SQLite.TypeAffinity));
			} else {
				viewField.comboDataType.ItemsSource = System.TypeCode.GetValues(typeof(System.Data.SqlDbType));
			}
			
			// begin:WPF Bindings
			DataContext = null;
			DataContext = this.Data; // changed from 'ifacto' to 'Data'
			// end:WPF Bindings
			
			viewField.checkIsPrimaryKey.IsChecked = SelectionIsPrimaryKey;
//			viewField.checkIsArray.IsChecked = SelectionIsArray;
			
			viewField.checkIsPrimaryKey.Checked += PrimaryKeyChecked;
			viewField.checkIsPrimaryKey.Unchecked += PrimaryKeyUnChecked;
			
			InvalidateProperty(UserControl.DataContextProperty);
			
			RefreshCommands();
		}
		#endif
		#endregion
		
		public void RefreshCommands()
		{
			CommandBindings.Clear();
		}
		
		
	}
}