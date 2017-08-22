/* oio * 3/13/2014 * Time: 6:04 PM
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using Generator;
using Generator.Elements;
using Microsoft.Win32;
using GeneratorTool.Views.Content;

namespace GeneratorTool.Views
{
	/// <summary>
	/// Interaction logic for WriterTemplateControl.xaml
	/// </summary>
	public partial class WriterTemplateControl : UserControl
	{
		static readonly ResourceGenerator ResGen = new ResourceGenerator();
		#region Registry
		
		const string regpath = @"Software\tfoxo\template-tool";
		const string reg_template_viewer_db 	= "Template Viewer Database";
		
		public string LastOpenDatabase {
			get { return Reg.GetKeyValueString(regpath,reg_template_viewer_db); }
			set { Reg.SetKeyValueString(regpath,reg_template_viewer_db,value); }
		}
		#endregion
		#region Static Properties
		
		static readonly OpenFileDialog Ofd = new OpenFileDialog(){ Filter="SQLite Database (sqlite,db3)|*.sqlite;*.db3;*.db" }; 
		static readonly SaveFileDialog Sfd = new SaveFileDialog(){ Filter="SQLite Database (sqlite,db3)|*.sqlite;*.db3;*.db" };
		const string ResxFilter = "Resource File (*.resx)|*.resx";
		static readonly SaveFileDialog SaveResxFileDialog = new SaveFileDialog() { Filter=ResxFilter }; 
		static readonly OpenFileDialog LoadResxFileDialog = new OpenFileDialog() { Filter=ResxFilter };
		
		static public readonly RoutedCommand GroupUpdatedCommand = new RoutedCommand("Group changed",typeof(WriterTemplateControl));
		static public readonly RoutedCommand RowUpdatedCommand = new RoutedCommand("Row changed",typeof(WriterTemplateControl));
		static public readonly RoutedCommand FieldUpdatedCommand = new RoutedCommand("Field changed",typeof(WriterTemplateControl));
		static public readonly RoutedCommand SyntaxUpdatedCommand = new RoutedCommand("Syntax changed",typeof(WriterTemplateControl));
		static public readonly RoutedCommand SaveResourcesCommand = new RoutedCommand("Save resx resource",typeof(WriterTemplateControl));
		static public readonly RoutedCommand AutoSaveResourcesCommand = new RoutedCommand("AutoSave resx resource",typeof(WriterTemplateControl)){InputGestures={ new KeyGesture(Key.G,ModifierKeys.Control) }};
		
		#endregion
		
		#region Model
		
		WriterTplModel Model {
			get { return model; }
			set { model = value; }
		} WriterTplModel model = new WriterTplModel();
		
		#endregion
		
		public WriterTemplateControl()
		{
			InitializeComponent();
			var cols = new List<string>(TemplateElement.ColumnNames);
			cols.Remove("id");
			cols.Remove("admin");
			cbTplField.ItemsSource = cols;
		}
		void ClearEditor() { editor.Text = string.Empty; }
		
		void SelectionToView()
		{
			if (model.SelectedRow == null) { ClearEditor(); return; }
			else if (model.SelectedGroup == null) { ClearEditor(); return; }
			else if (cbTplField.SelectedValue == null) { ClearEditor(); return; }
			try {
				string selectedField = cbTplField.SelectedValue as string;
				editor.Text = model.SelectedRow.GetKeyValue(selectedField) as string;
			} catch {
				ModernDialog.ShowMessage("Error showing content.","Error",MessageBoxButton.OK);
			}
		}
		
		
		#region Overrides
		
		public override void BeginInit()
		{
			base.BeginInit();
			CommandBindings.Add(new CommandBinding(GroupUpdatedCommand,Event_GroupUpdatedCmd));
			CommandBindings.Add(new CommandBinding(RowUpdatedCommand,Event_RowUpdatedCmd));
			CommandBindings.Add(new CommandBinding(FieldUpdatedCommand,Event_FieldUpdatedCmd));
			CommandBindings.Add(new CommandBinding(SyntaxUpdatedCommand,Event_SyntaxUpdatedCmd));
		}
		
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			cbTplField.SelectedIndex = 0;
			editor.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save,Event_SaveToRow));
			CommandBindings.Add(new CommandBinding(SaveResourcesCommand,Event_SaveResxFile));
			CommandBindings.Add(new CommandBinding(AutoSaveResourcesCommand,Event_AutoSaveResxFile));
		}
		
		#endregion
		
		#region Local Event Handlers
		
		void LoadDatabase(object send, RoutedEventArgs args)
		{
			var val = Ofd.ShowDialog();
			if (!(val.HasValue && val.Value)) return;
			InitializeData();
		}
		void InitializeData()
		{
			string grp = cbTplGroup.Text, ttl = cbTplName.Text;
			Model.Initialize(Ofd.FileName);
			cbTplGroup.Text=null;
			cbTplGroup.ItemsSource = model.GroupNames;
			if (grp!=null) cbTplGroup.Text = grp;
			if (ttl!=null) cbTplName.Text = ttl;
		}
		void Event_SyntaxUpdatedCmd(object sender, RoutedEventArgs e)
		{
			editor.SyntaxHighlighting = cbEditorFmt.SelectedValue as ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition;
		}
		
		void Event_GroupUpdatedCmd(object sender, RoutedEventArgs e)
		{
			model.SelectedGroup = cbTplGroup.SelectedItem as string;
			cbTplName.ItemsSource = model.Rows;
			if (model.rows.Count >= 1) cbTplName.SelectedIndex = 0;
		}
		void Event_RowUpdatedCmd(object sender, RoutedEventArgs e)
		{
			model.SelectedRow = cbTplName.SelectedValue as TemplateElement;
			SelectionToView();
		}
		void Event_FieldUpdatedCmd(object sender, RoutedEventArgs e)
		{
			SelectionToView();
		}
		
		NewWriterTemplateControl control;
		void Event_CreateRow(object sender, RoutedEventArgs e)
		{
			control = new NewWriterTemplateControl(){ DataContext=Model,Title="New Template Row", ResizeMode=ResizeMode.CanResizeWithGrip, };
			TemplateElement element= null;
			var rc = new RelayCommand((x) => {
				control.DialogResult = true;
				element = new TemplateElement(){ Title=control.tbName.Text, Table=control.cbGroup.Text };
				control.Close();
			});
			control.Buttons=new[]{control.CancelButton,new Button(){ Content = "okay", Command = rc, IsDefault = true}};
			//			control.cbGroup.ItemsSource = model.GroupNames;
			var value = control.ShowDialog();
			if (!(value.HasValue && value.Value)) return;
			//			ModernDialog.ShowMessage("We have our template-element","Success",MessageBoxButton.OK);
			var fname = Ofd.FileName;
			Model.util.InsertRow(Ofd.FileName,element);
			InitializeData();
			cbTplGroup.Text = element.Table;
			cbTplName.Text = element.Title;
		}
		void Event_DeleteRow(object sender, RoutedEventArgs e)
		{
			var element= cbTplName.SelectedItem as TemplateElement;
			string n = element.Table;
			if (element==null) { ModernDialog.ShowMessage("Nothing was selected or Null.","Error",MessageBoxButton.OK); return; }
			
			var result = ModernDialog.ShowMessage("You are about to permanantly delete the selected record.\nAre you sure you want to continue?","Confirm Deletion",MessageBoxButton.OKCancel);
			if (result!=MessageBoxResult.OK) return;
			
			if (Model.util.DeleteRow(Ofd.FileName,element))
			{
				ModernDialog.ShowMessage("There was an error executing the SQL Query.","Error",MessageBoxButton.OK);
				return;
			}
			cbTplName.Text = null;
			InitializeData();
			if (Model.GroupNames.Contains(n)) cbTplGroup.Text = n; else cbTplGroup.Text = null;
			//			cbTplName.Text = element.Title;
		}
		void Event_SaveToRow(object sender, RoutedEventArgs e)
		{
			string selectedField = cbTplField.SelectedValue as string;
			if (selectedField==null)
			{
				ModernDialog.ShowMessage("Nothing to do; Nothing is loaded","Error",MessageBoxButton.OK);
				return;
			}
			else if (Model.util.Templates==null)
			{
				ModernDialog.ShowMessage("Nothing to do; Nothing is loaded","Error",MessageBoxButton.OK);
				return;
			}
			//			Debug.Print("Row, id: ‘{0}’, name: {1}", StrRow.Id.Value, StrRow.Title);
			Model.SelectedRow.SetValue(cbTplField.SelectedValue as string,editor.Text);
			TemplateUtil.UpdateTemplateRow(Ofd.FileName,"templates",model.SelectedRow,selectedField,editor.Text);
		}
		
		void Event_ButtonContextMenu(object sender, RoutedEventArgs e) { (sender as Button).ContextMenu.IsOpen = true; }
		
		bool? CsDesigner(string content, string title="generating..."){
			
			var dlg = new ModernDialog(){ Content=content,Title=title };
			var rc = new RelayCommand((x) => { dlg.DialogResult = true; dlg.Close(); });
			dlg.Buttons = new[]{ dlg.CancelButton,new Button(){ Content = "okay", Command = rc, IsDefault = true}};
			return dlg.ShowDialog();
		}
		
		void Event_SaveResxFile(object sender, RoutedEventArgs e)
		{
			var value = SaveResxFileDialog.ShowDialog();
			if (!(value.HasValue && value.Value)) return ;
			StaticResourceGenerator.SaveResource(Model,SaveResxFileDialog.FileName,false,tNamespace.Text);
		}
		void Event_AutoSaveResxFile(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(SaveResxFileDialog.FileName))
			{
				Event_SaveResxFile(sender,e);
				return;
			}
			StaticResourceGenerator.SaveResource(Model,SaveResxFileDialog.FileName,false,tNamespace.Text);
		}
		#endregion
	
	}
}