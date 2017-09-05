/* oio : 1/21/2014 9:33 AM */
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using Generator;
using Generator.Elements;
using Generator.Core.Markup;
using GeneratorTool.Views.Content;

namespace GeneratorTool.Views
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class MoxiView : UserControl, INotifyPropertyChanged
	{
		// http://stackoverflow.com/questions/11986840/wpf-treeview-refreshing
		static TreeViewItem FindTreeViewSelectedItemContainer(ItemsControl root, object selection)
		{
			var item = root.ItemContainerGenerator.ContainerFromItem(selection) as TreeViewItem;
			if (item == null)
			{
				foreach (var subItem in root.Items)
				{
					item = FindTreeViewSelectedItemContainer((TreeViewItem)root.ItemContainerGenerator.ContainerFromItem(subItem), selection);
					if (item != null)
					{
						break;
					}
				}
			}
			return item;
		}
		
		public void RefreshDataTree(DatabaseElement parent)
		{
			var selectedparent = FindTreeViewSelectedItemContainer(tvModel,parent);
			selectedparent.ItemsSource = parent.Children;
			selectedparent.Items.Refresh();
			selectedparent.UpdateLayout();
		}
		public void RefreshTree(object parent)
		{
			var selectedparent = FindTreeViewSelectedItemContainer(tvModel,parent);
			//			selectedparent.ItemsSource = parent.Children;
			selectedparent.Items.Refresh();
			selectedparent.UpdateLayout();
		}
		
		
		#region Commands
		static public readonly ICommand MyCommandCommand = new RoutedUICommand(){ Text="MyCommandCommand Command." };
		
		static public readonly ICommand TemplateViewCommand = new RoutedUICommand(){ Text="TemplateView Command.", InputGestures={ new KeyGesture(Key.F6) } };
		static public readonly ICommand DatabaseViewCommand = new RoutedUICommand(){ Text="DatabaseView Command.", InputGestures={ new KeyGesture(Key.F7) } };
		// for $(DbType), $(ConnectionT) $(AdapterT)
		static public readonly ICommand TemplateSaveCommand = new RoutedUICommand(){ Text="Template Save Command.", InputGestures={ new KeyGesture(Key.S, ModifierKeys.Control) } };
		static public readonly ICommand TemplateLoadCommand = new RoutedUICommand(){ Text="Template Load Command.", InputGestures={ new KeyGesture(Key.O, ModifierKeys.Shift|ModifierKeys.Control) } };
		
		static public readonly ICommand ShowContextCommamd = new RoutedUICommand(){ Text="Button Show ContextMenu Command." };
		// via treeview
		static public readonly ICommand StatePushCommand = new RoutedUICommand(){ Text="TreeItem Changed Command." };
		// via combobox
		static public readonly ICommand TableTypeCommand = new RoutedUICommand(){ Text="Table Type Command." };
		static public readonly ICommand ToggleTemplateGroupCommand = new RoutedUICommand(){ Text="Template Group Command." };
		static public readonly ICommand ToggleTemplateRowCommand = new RoutedUICommand(){ Text="Template Row Command." };
		
		static public readonly ICommand ToggleViewCommand = new RoutedUICommand(){ Text="Select View tab-item.", InputGestures={ new KeyGesture(Key.F2) } };
		static public readonly ICommand ToggleTableCommand = new RoutedUICommand(){ Text="Template button toggled.", InputGestures={ new KeyGesture(Key.F3) } };
		static public readonly ICommand ToggleFieldCommand = new RoutedUICommand(){ Text="Field button toggled.", InputGestures={ new KeyGesture(Key.F4) } };
		static public readonly ICommand TogglePreviewCommand = new RoutedUICommand(){ Text="Preview button toggled.", InputGestures={ new KeyGesture(Key.F5) } };
		static public readonly ICommand ToggleDataCommand = new RoutedUICommand(){ Text="Select the data tab-item.", InputGestures={ new KeyGesture(Key.F5) } };
		
		
		
		static public readonly FieldCutCmd FieldCutCommand = new FieldCutCmd();
		static public readonly FieldCopyCmd FieldCopyCommand = new FieldCopyCmd();
		static public readonly FieldPasteAboveCmd FieldPasteAboveCommand = new FieldPasteAboveCmd();
		static public readonly FieldPasteBelowCmd FieldPasteBelowCommand = new FieldPasteBelowCmd();
		static public readonly TableCutCmd TableCutCommand = new TableCutCmd();
		static public readonly TableCopyCmd TableCopyCommand = new TableCopyCmd();
		static public readonly TableCreateCmd TableCreateCommand = new TableCreateCmd();
		static public readonly TablePasteAboveCmd TablePasteAboveCommand = new TablePasteAboveCmd();
		static public readonly TablePasteBelowCmd TablePasteBelowCommand = new TablePasteBelowCmd();
		#endregion
		
		public GeneratorUIModel Model = new GeneratorUIModel();
		
    void Event_ButtonContextMenu(object sender, RoutedEventArgs e) { (sender as Button).ContextMenu.IsOpen = true; }
    //void Event_ButtonContextMenu(object sender, System.Windows.RoutedEventArgs e) { (e.OriginalSource as Button).ContextMenu.IsOpen = true; }
		
		
		#region Little Helpers
		
		void SetCombos(TableElement table) { cbDatabase.SelectedItem = table.Parent; cbTable.SelectedItem = table; cbField.SelectedItem = null; }
		void SetCombos(FieldElement field) { cbDatabase.SelectedItem = field.Parent.Parent; cbTable.SelectedItem = field.Parent; cbField.SelectedItem = field; }
		void SetCombos(DatabaseElement db) { cbDatabase.SelectedItem = db; cbTable.SelectedItem = null; cbField.SelectedItem = null; }
		
		#endregion
		#region TemplateManager Methods
		/// <summary>
		/// Nothing to do here really.
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		public TemplateManager CreateTemplate(DatabaseElement field)
		{
			return new TemplateManager(){
				SelectedDatabase = field
			};
		}
		public TemplateManager CreateTemplate(DataViewElement view)
		{
			return new TemplateManager(){
				SelectedView = view
			};
		}
		public TemplateManager CreateTemplate(TableElement field)
		{
			return new TemplateManager(){
				SelectedCollection = Model.Reader.Model.Databases,
				SelectedDatabase = field.Parent,
				SelectedTable = field,
				SelectedField = null,
				SelectedTemplate = (TableTemplate)cbTemplateGroups.SelectedItem
			};
		}
		public TemplateManager CreateTemplate(FieldElement field)
		{
			return new TemplateManager(){
				SelectedCollection = Model.Reader.Model.Databases,
				SelectedDatabase = field.Parent.Parent,
				SelectedTable = field.Parent,
				SelectedField = field,
				SelectedTemplate = (TableTemplate)cbTemplateGroups.SelectedItem,
			};
		}
		#endregion
		#region State
		void PushState(DataViewElement dataview)
		{
			Model.LastViewMode = ViewMode.DataView;
			Model.LastSelectedObject = dataview;
			Model.LastFactory = CreateTemplate(dataview);
//			SetCombos(database);
		}
		void PushState(DatabaseElement database)
		{
			Model.LastViewMode = ViewMode.Database;
			Model.LastSelectedObject = database;
			Model.LastFactory = CreateTemplate(database);
			SetCombos(database);
		}
		void PushState(TableElement table)
		{
			Model.LastViewMode = ViewMode.Table;
			Model.LastSelectedObject = table;
			Model.LastFactory = CreateTemplate(table);
			
			SetCombos(table);
		}
		void PushState(FieldElement field)
		{
			Model.LastViewMode = ViewMode.Field;
			Model.LastSelectedObject = field;
			Model.LastFactory = CreateTemplate(field);
			
			SetCombos(field);
		}
		void PushState(TableTemplate template)
		{
			Model.LastViewMode = ViewMode.TemplatePreview;
			Model.LastTemplate = template;
			cbTemplateGroups.Text = (template).Group;
			cbTemplateRow.SelectedValue = template;
		}
		/// <summary>Through this method, the LastTemplate is set.</summary>
		void StatePush()
		{
			var treeSelection = tvModel.SelectedValue;
			if (treeSelection is DatabaseElement) PushState(treeSelection as DatabaseElement);
			else if (treeSelection is TableElement) PushState(treeSelection as TableElement);
			else if (treeSelection is FieldElement) PushState(treeSelection as FieldElement);
			else if (treeSelection is TableTemplate) PushState(treeSelection as TableTemplate);
			else if (treeSelection is DataViewElement) PushState(treeSelection as DataViewElement);
			else if (treeSelection is string) MessageBox.Show("Look a string.");
			dataEditor.DataContext = Model.LastFactory;
		}
		void StatePushAction(object sender, RoutedEventArgs e)
		{
			StatePush();
		}
		#endregion
		
		public MoxiView()
		{
			InitializeComponent();
			FieldCutCommand.View = this;
			FieldCopyCommand.View = this;
			FieldPasteAboveCommand.View = this;
			FieldPasteBelowCommand.View = this;
			TableCutCommand.View = this;
			TableCreateCommand.View = this;
			TableCopyCommand.View = this;
			TablePasteAboveCommand.View = this;
			TablePasteBelowCommand.View = this;
			
			tabs.ItemContainerStyle = new Style(){
			  Setters={
			    new Setter(UIElement.VisibilityProperty, Visibility.Collapsed)
			  }
			};
			InitializeReader();
			Model.LastSelectedView = pane;
		}
		
		/// <summary>
		/// The active-template was changed.
		/// We might want to call StatePush.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ToggleTemplateRowAction(object sender, RoutedEventArgs e)
		{
			// check if a group has been selected.
			if (string.IsNullOrEmpty(cbTemplateRow.Text)) {
				editor.Text = string.Empty; return;
			}
			// check LastTemplate
			// check if a row has been selected.
			if (cbTemplateRow.SelectedValue == null) {
				editor.Text = string.Empty; return;
			}
			// 
			switch (Model.LastViewMode) {
				case ViewMode.Undefined:
				case ViewMode.TemplateTable:   if (cbTemplateRow.SelectedValue!=null) editor.Text = (cbTemplateRow.SelectedValue as TableTemplate).ElementTemplate; break;
				case ViewMode.TemplateField:   if (cbTemplateRow.SelectedValue!=null) editor.Text = (cbTemplateRow.SelectedValue as TableTemplate).ItemsTemplate; break;
				case ViewMode.TemplatePreview: if (cbTemplateRow.SelectedValue!=null) editor.Text = Model.Reader.Generate(cbTable.SelectedValue as TableElement, cbTemplateRow.SelectedValue as TableTemplate); break;
				default: break;
			}
		}
		
		void ToggleTemplateGroupAction(object sender, RoutedEventArgs e)
		{
			if (cbTemplateGroups.SelectedValue == null) return;
			string groupname = (cbTemplateGroups.SelectedValue as TableTemplate) . Group;
			System.Diagnostics.Debug.WriteLine("Text: {0}",groupname);
			cbTemplateRow.ItemsSource = Model.Reader.Model.Templates.Templates.Where( p => p.Group == groupname);
			editor.Text = null;
		}
		
		#region Show in editor
		
		void ToggleEditorTemplateAction(object sender, RoutedEventArgs e) { Model.LastTemplate=cbTemplateRow.SelectedValue; Model.LastViewMode = ViewMode.TemplateTable; try { Model.ViewText = editor.Text = (cbTemplateRow.SelectedValue as TableTemplate).ElementTemplate; } catch(Exception err) { if (cbTemplateRow.SelectedValue != null) { throw err; } } tabs.SelectedItem = tabEdit; }
		void ToggleEditorFieldAction(object sender, RoutedEventArgs e) { Model.LastTemplate=cbTemplateRow.SelectedValue; Model.LastViewMode = ViewMode.TemplateField; try { Model.ViewText = editor.Text = (cbTemplateRow.SelectedValue as TableTemplate).ItemsTemplate; }  catch(Exception err) { if (cbTemplateRow.SelectedValue != null) { throw err; } } tabs.SelectedItem = tabEdit; }
		void ToggleEditorPreviewAction(object sender, RoutedEventArgs e) { Model.LastTemplate=cbTemplateRow.SelectedValue; Model.LastViewMode = ViewMode.TemplatePreview; try { Model.ViewText = Model.Reader.Generate(cbTable.SelectedValue as TableElement,cbTemplateRow.SelectedValue as TableTemplate); } catch {} tabs.SelectedItem = tabEdit; }
		void ToggleDataAction(object sender, RoutedEventArgs e) { tabs.SelectedItem = tabData; }
		void ToggleViewAction(object sender, RoutedEventArgs e) { tabs.SelectedItem = tabView; }
		
		
		#endregion
		
		// ComboBox Trigger
		void TableTypeAction(object sender, RoutedEventArgs e)
		{
			try {
				string val = string.Format("{0}",dataEditor.viewTable.comboTableType.SelectedValue);
				Debug.WriteLine(val);
				dataEditor.viewField.comboDataType.ItemsSource = null;
				//				dataEditor.viewTable.comboTableType
				switch (val) {
					case "SQLite":
						dataEditor.viewField.comboDataType.ItemsSource = Enum.GetValues(typeof(System.Data.SQLite.TypeAffinity));
						break;
					case "SqlServer":
						dataEditor.viewField.comboDataType.ItemsSource = Enum.GetValues(typeof(System.Data.SqlDbType));
						break;
					case "OleDb":
					case "OleAccess":
						dataEditor.viewField.comboDataType.ItemsSource = Enum.GetValues(typeof(System.Data.OleDb.OleDbType));
						break;
				}
			} finally {
			}
		}
		
		void InitializeReader()
		{
			Model.Reader = new GeneratorReader { LoadCompleteAction=InitializeDataSource, };
			Model.Reader.BindUIElement(this);
			Model.Reader.TemplateGeneratedAction = a => editor.Text = a;
			//			CommandBindings.Add(new CommandBinding(DatabaseViewCommand,(e,r) => { dataEditor.Visibility = Visibility.Visible; pane.Visibility = Visibility.Collapsed; }));
			//			CommandBindings.Add(new CommandBinding(TemplateViewCommand,(e,r) => { dataEditor.Visibility = Visibility.Collapsed; pane.Visibility = Visibility.Visible; }));
			editor.CommandBindings.Add(
				new CommandBinding(
					TemplateSaveCommand,
					(e,r) => {
						if (Model.LastTemplate==null) {
							editor.Text += "\n";
							editor.Text += "No template to save.";
							editor.Text += "\n";
							return;
						}
						switch (Model.LastViewMode) {
							case ViewMode.Undefined:
								editor.Text += "ViewMode.Undefined.";
								editor.Text += "\n";
								editor.Text = string.Format("Error[{0}]:", (int)Model.LastViewMode);
								editor.Text += "\n";
								break;
							case ViewMode.TemplateField:
								(Model.LastTemplate as TableTemplate).ItemsTemplate = editor.Text;
								break;
							case ViewMode.TemplateTable:
								(Model.LastTemplate as TableTemplate).ElementTemplate = editor.Text;
								break;
							case ViewMode.TemplatePreview:
								editor.Text += "PREVIEW";
								editor.Text += "\n";
								break;
						}
					}));
			editor.CommandBindings.Add(
				new CommandBinding(
					TemplateLoadCommand,
					(e,r) => {
						if (Model.LastTemplate==null) return;
						if (Model.LastTemplate==null) {
							editor.Text = "No template to save.";
							return;
						}
						editor.Text = "Testing the Load Template Command.";
						editor.Text += "\n";
						editor.Text += "Compare SelectedTemplate to Model.";
					}));
			
			CommandBindings.Add(new CommandBinding(StatePushCommand,StatePushAction));
			// cbTemplateRow
			CommandBindings.Add(new CommandBinding(ToggleTemplateRowCommand,ToggleTemplateRowAction));
			// cbTemplateGroups
			CommandBindings.Add(new CommandBinding(ToggleTemplateGroupCommand,ToggleTemplateGroupAction));
			// comboTableType
			CommandBindings.Add(new CommandBinding(TableTypeCommand,TableTypeAction));
			//
			CommandBindings.Add(new CommandBinding(ToggleTableCommand,ToggleEditorTemplateAction));
			CommandBindings.Add(new CommandBinding(ToggleFieldCommand,ToggleEditorFieldAction));
			CommandBindings.Add(new CommandBinding(TogglePreviewCommand,ToggleEditorPreviewAction));
			CommandBindings.Add(new CommandBinding(ToggleDataCommand,ToggleDataAction));
			CommandBindings.Add(new CommandBinding(ToggleViewCommand,ToggleViewAction));
		}
		
		void InitializeDataSource()
		{
			cbDatabase.ItemsSource = null;
			cbDatabase.ItemsSource = Model.Reader.Model.Databases.Databases;
			cbDatabase.DisplayMemberPath = "Name";
			
			RefreshTemplates();
			DataContext = Model.Reader.Model;
		}
		
		void RefreshTemplates()
		{
			cbTemplateRow.ItemsSource = null;
			Model.TemplateGroups = new ObservableCollection<TableTemplate>();
			foreach (var tpl in Model.Reader.Model.Templates.Templates) {
				if ((Model.TemplateGroups.Where(p => p.Group == tpl.Group)).Any()) continue;
				Model.TemplateGroups.Add(tpl);
			}
			cbTemplateGroups.ItemsSource = Model.TemplateGroups.OrderBy(m=>m.Group);
			tnTemplates.ItemsSource = Model.Reader.Model.Templates.GetGrouping();
		}
		
		CreateTemplateDialog control { get; set; }
		void Event_CreateTemplate(object sender, RoutedEventArgs e)
		{
			control = new CreateTemplateDialog(){ Title="New Template", ResizeMode=ResizeMode.CanResizeWithGrip, };
			control.cbGroup.ItemsSource = Model.TemplateGroups;
			control.cbGroup.DisplayMemberPath = "Group";
			
			TableTemplate element= null;
			
			var rc = new RelayCommand((x) => {
				control.DialogResult = true;
				element = new TableTemplate(){ Alias=control.tbName.Text, Group=control.cbGroup.Text, ElementTemplate="", ItemsTemplate="" };
				control.Close();
			});
			control.Buttons=new[]{control.CancelButton,new Button(){ Content = "okay", Command = rc, IsDefault = true}};
		restart:
			var value = control.ShowDialog();
			if (!(value.HasValue && value.Value)) return;
			if (string.IsNullOrEmpty(element.Alias)) {
				control.Title="supply a name for your template!";
				goto restart;
			}
			Model.Reader.Model.Templates.Add(element);
			Model.Reader.Model.Templates= Model.Reader.Model.Templates;
			RefreshTemplates();
			PushState(element);
		}
		void Event_RemoveTemplate(object sender, RoutedEventArgs e)
		{
			if (Model.LastTemplate==null) return;
			string t = (Model.LastTemplate as TableTemplate).Alias;
			const string format = @"You are about to delete the template ""{0}"".\nAre you sure?";
			control = new CreateTemplateDialog(){
				Title="New Template",
				ResizeMode=ResizeMode.CanResizeWithGrip,
				Content = new TextBlock(){ Text=string.Format(format,t) }
			};
			control.Buttons=new[]{
				control.CancelButton,
				new Button(){
					Content = "okay",
					Command = new RelayCommand((x) => { control.DialogResult = true; control.Close(); }),
					IsDefault = true
				}
			};
			var value = control.ShowDialog();
			if (!(value.HasValue && value.Value)) return;
			
			Model.Reader.Model.Templates.Templates.Remove(Model.LastTemplate as TableTemplate);
			Model.Reader.Model.Templates= Model.Reader.Model.Templates;
			
			RefreshTemplates();
		}
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		void Event_CreateView_Form(object sender, System.Windows.RoutedEventArgs e)
		{
			ModernDialog.ShowMessage("This feature is not yet available","Information",MessageBoxButton.OK);
		}
	}
}
