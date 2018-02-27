/* oio * 01/21/2014 * Time: 09:09 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using Generator;
using Generator.Elements;
using Generator.Core.Markup;
using Microsoft.Win32;

//namespace on.gen {

namespace GeneratorTool
{
	/// <summary>
	/// 
	/// </summary>
	public class GeneratorReader
	{
		public GeneratorModel Model { get; set; }
		
		#region (static) ICommand
		
		public Action InitializeCompleteAction { get; set; }
		public Action LoadCompleteAction { get; set; }
		public Action SaveCompleteAction { get; set; }
		public Action<string> TemplateGeneratedAction { get; set; }
		
		CommandBindingCollection CommandBindings { get; set; }
		
		static public readonly ICommand InitializeConfigurationCommand = new RoutedUICommand(){ Text="Initialize generator configuration-file." };
		static public readonly ICommand LoadConfigurationCommand = new RoutedUICommand(){ Text="Load generator configuration-file.", InputGestures={ new KeyGesture(Key.O, ModifierKeys.Control) } };
		static public readonly ICommand ReloadConfigurationCommand = new RoutedUICommand(){ Text="Reload generator configuration-file.", InputGestures={ new KeyGesture(Key.R, ModifierKeys.Control) } };
		static public readonly ICommand SaveConfigurationCommand = new RoutedUICommand(){ Text="Save generator configuration-file.", InputGestures={ new KeyGesture(Key.S, ModifierKeys.Control|ModifierKeys.Shift) } };
		static public readonly ICommand SaveConfigurationAsCommand = new RoutedUICommand(){ Text="Save generator configuration-file (as).", InputGestures={ new KeyGesture(Key.S, ModifierKeys.Control|ModifierKeys.Shift) } };
		
		#endregion
		
		#region readonly / constants
		
		const string filter = "Generator Configuration|*.generator-config|Xml Document (generator-config)|*.xml";
		readonly OpenFileDialog ofd = new OpenFileDialog { Filter = filter };
		readonly SaveFileDialog sfd = new SaveFileDialog { Filter = filter };
		
		#endregion
		
		public void BindUIElement(UIElement elm)
		{
			CommandBindings = new CommandBindingCollection(
				new List<CommandBinding> {
          new CommandBinding(LoadConfigurationCommand,LoadConfiguration),
          new CommandBinding(ReloadConfigurationCommand,ReloadConfiguration,CanReloadConfiguration),
          new CommandBinding(
						SaveConfigurationCommand,
						SaveConfiguration,
						(object xo, CanExecuteRoutedEventArgs xa) =>
						{
							bool answer = Model == null || string.IsNullOrEmpty(Model.Configuration.datafile) || string.IsNullOrEmpty(Model.Configuration.templatefile);
							if (!answer) {
								xa.CanExecute = true;
								xa.Handled = true;
							}
						}),
					new CommandBinding(
						SaveConfigurationAsCommand,
						SaveConfigurationAs,
						(xo, xa) =>
						{
							bool answer = Model == null ||
								string.IsNullOrEmpty(Model.Configuration.datafile) ||
								string.IsNullOrEmpty(Model.Configuration.templatefile);
							if (!answer) {
								xa.CanExecute = true;
								xa.Handled = true;
							}
						}),
				});
			foreach (CommandBinding binding in CommandBindings)
				if (!elm.CommandBindings.Contains(binding)) {
				elm.CommandBindings.Add(binding);
			}
//			elm.CommandBindings.Add(new CommandBinding(LoadConfigurationCommand,LoadConfiguration));
//			elm.CommandBindings.Add(new CommandBinding(SaveConfigurationCommand,SaveConfiguration));
		}
		
		#region Method: string Generate, IDbConfiguration4 GetConfig
		
		public Generator.Export.Intrinsic.IDbConfiguration4 GetConfig(TableElement table, TableTemplate template) {
			return new TemplateManager(){
				SelectedCollection = Model.Databases,
				SelectedDatabase = table==null ? null : table.Parent,
				SelectedTable = table,
				Templates = Model.Templates,
				SelectedTemplate = template,
				SelectedTemplateGroup = template==null ? null : template.Group,
			};
		}
		
		string GeneratedTemplate { get; set; }
		
		public string Generate(TableElement tableName, TableTemplate templateName)
		{
			Generator.Export.Intrinsic.IDbConfiguration4 config = GetConfig(tableName,templateName);
			GeneratedTemplate = Model.Databases.ConvertInput(config,config.SelectedTable.Name);
			if (TemplateGeneratedAction != null) TemplateGeneratedAction.Invoke(GeneratedTemplate);
			config  = null;
			return GeneratedTemplate;
		}
		
		#endregion
		
		void ConfigLoad()
		{
			Model.FileName = ofd.FileName;
			Model.Configuration = GeneratorConfig.Load(Model.FileName);
			InitializeConfiguration(this, null);
		}

    #region RoutedEvents

    void LoadConfiguration(object o, RoutedEventArgs a)
    {
      if (ofd.ShowDialog().Value)
      {
        Model = new GeneratorModel();
        Model.FileName = ofd.FileName;
        Model.Configuration = GeneratorConfig.Load(Model.FileName);
        InitializeConfiguration(o, a);
        a.Handled = true;
        if (LoadCompleteAction != null) LoadCompleteAction.Invoke();
      }
    }
    void CanReloadConfiguration(object xo, CanExecuteRoutedEventArgs xa) {
      if (Model == null) xa.CanExecute = false;
      else xa.CanExecute = System.IO.File.Exists(Model.FileName);
      xa.Handled = true;
      return;
    }
    void ReloadConfiguration(object o, RoutedEventArgs a)
    {
      if (Model == null) return;
      if (System.IO.File.Exists(Model.FileName))
      {
        Model.Configuration = GeneratorConfig.Load(Model.FileName);
        InitializeConfiguration(o, a);
        a.Handled = true;
        if (LoadCompleteAction != null) LoadCompleteAction.Invoke();
      }
      else
      {
        MessageBox.Show("Nothing to reload.");
      }
    }

    void InitializeConfiguration(object o, RoutedEventArgs a)
		{
//			try {
			Model.Databases = DatabaseCollection.Load(Model.Configuration.datafile);
			Model.Templates = TemplateCollection.Load(Model.Configuration.templatefile);
			// why is this necessary?
			Model.Databases.Rechild();
			a.Handled = true;
//			} catch (Exception e) {
//				throw e;
//			} finally {
			if (InitializeCompleteAction != null)
				InitializeCompleteAction.Invoke();
//			}
		}
		
		BackgroundWorker saveWorker;
		
		void SaveConfiguration(object o, RoutedEventArgs a)
		{
			if (Model==null) {
				a.Handled = true;
				return;
			}
			try {
				if (saveWorker!=null) { saveWorker.Dispose(); saveWorker = null; }
				saveWorker = new BackgroundWorker(){ WorkerSupportsCancellation = false, WorkerReportsProgress = false };
				saveWorker.DoWork += (sender, args) => {
					if (Model == null) return;
					if (Model.Databases != null) {
						Model.Configuration.datafile.BackupFile();
						Model.Databases.Save(Model.Configuration.datafile, Model.Databases);
					}
					if (Model.Templates != null) {
						Model.Configuration.templatefile.BackupFile();
						Model.Templates.Save(Model.Configuration.templatefile, Model.Templates);
					}
					if (Model.Configuration != null) {
						Model.FileName.BackupFile();
						Model.Configuration.Save(Model.FileName, Model.Configuration);
					}
					args.Cancel = true;
				};
				saveWorker.RunWorkerCompleted += (sender, args) => { saveWorker.Dispose(); saveWorker = null;  };
				//ModernDialog.ShowMessage(Model.FileName,"Messagebox",MessageBoxButton.OK);
				saveWorker.RunWorkerAsync();
				a.Handled = true;
			}
			finally {
				if (SaveCompleteAction!=null) SaveCompleteAction.Invoke();
			}
		}
		
		void SaveConfigurationAs(object o, RoutedEventArgs a)
		{
			if (Model==null) {
				a.Handled = true;
				return;
			}
			if (sfd.ShowDialog().Value)
			{
				try {
					if (saveWorker!=null) { saveWorker.Dispose(); saveWorker = null; }
					saveWorker = new BackgroundWorker(){ WorkerSupportsCancellation = false, WorkerReportsProgress = false };
					saveWorker.DoWork += (sender, args) => {
						if (Model == null) return;
						if (Model.Databases != null) {
							Model.Configuration.datafile.BackupFile();
							Model.Databases.Save(Model.Configuration.datafile, Model.Databases);
						}
						if (Model.Templates != null) {
							Model.Configuration.templatefile.BackupFile();
							Model.Templates.Save(Model.Configuration.templatefile, Model.Templates);
						}
						if (Model.Configuration != null) {
							Model.FileName.BackupFile();
							Model.FileName = sfd.FileName;
							Model.Configuration.Save(Model.FileName, Model.Configuration);
						}
						args.Cancel = true;
					};
					saveWorker.RunWorkerCompleted += (sender, args) => { saveWorker.Dispose(); saveWorker = null; };
//					saveWorker.RunWorkerCompleted += (sender, args) => { saveWorker.Dispose(); saveWorker = null; ModernDialog.ShowMessage(Model.FileName,"Messagebox",MessageBoxButton.OK); };
					saveWorker.RunWorkerAsync();
					a.Handled = true;
				}
				finally {
					if (SaveCompleteAction!=null) SaveCompleteAction.Invoke();
				}
			}
		}
		
		#endregion
		
	}
}
