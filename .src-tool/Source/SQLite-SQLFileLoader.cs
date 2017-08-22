/* oio : 03/10/2014 00:34 */
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using GeneratorTool.Views;
namespace GeneratorTool.SQLiteUtil
{
	class SQLFileLoader : IBindCommands, INotifyPropertyChanged
	{
		#region IBindCommands implementation
		
		public class LoadSqlFileCmd : BasicCommand {
		
			public SQLFileLoader Loader { get; set; }
		
			// we can always load a new file.
		
			public override bool CanExecute(object parameter) { return true; }
		
			protected override void OnExecute(object parameter)
			{
				Console.WriteLine("Load SQL clicked");
				var ed = parameter as GeneratorTool.Controls.Editor;
				Loader.Load();
				if (Loader.IsLoaded) ed.Load(Loader.SqlFile);
			}
		
		} static public ICommand LoadSqlFileCommand;
		
		public class SaveSqlFileCmd : BasicCommand {
		
			public SQLFileLoader Loader { get; set; }
		
			// we can always load a new file.
		
			public override bool CanExecute(object parameter) { return true; }
		
			protected override void OnExecute(object parameter)
			{
				var ed = parameter as GeneratorTool.Controls.Editor;
				Loader.Save(ed.Text);
			}
		
		} static public ICommand SaveSqlFileCommand;
		
		public class SaveSqlFileAsCmd : BasicCommand {
		
			public SQLFileLoader Loader { get; set; }
		
			// we can always load a new file.
		
			public override bool CanExecute(object parameter) { return true; }
		
			protected override void OnExecute(object parameter)
			{
				var ed = parameter as GeneratorTool.Controls.Editor;
				Loader.SaveAs(ed.Text);
			}
		
		} static public ICommand SaveSqlFileAsCommand;
		
		public void UpdateBindings(UserControl control)
		{
			Console.WriteLine("SQLFileLoader.UpdateBindings");
			LoadSqlFileCommand   = new LoadSqlFileCmd { Loader=this };
			SaveSqlFileCommand   = new SaveSqlFileCmd { Loader=this };
			SaveSqlFileAsCommand = new SaveSqlFileAsCmd { Loader=this };
			
			control.CommandBindings.Add(new CommandBinding(LoadSqlFileCommand));
			control.CommandBindings.Add(new CommandBinding(SaveSqlFileCommand));
			control.CommandBindings.Add(new CommandBinding(SaveSqlFileAsCommand));
		}
		
		#endregion
		
		public SQLFileLoader()
		{
		}
		
		#region FileDialogs
		static public readonly OpenFileDialog OfdSql = new OpenFileDialog() { Filter = "SQL File (*.sql)|*.sql|All files|*" };
		static public readonly SaveFileDialog SfdSql = new SaveFileDialog() { Filter = "SQL File (*.sql)|*.sql|All files|*" };
		static public readonly SaveFileDialog SfdSqlAs = new SaveFileDialog() { Filter = "SQL File (*.sql)|*.sql|All files|*" };
		
		#endregion
		
		#region DataFile loader and property
		
		public string SqlFile {
			get { return sqlFile; }
			set { sqlFile = value; OnProperty(SqlFile); }
		} string sqlFile;
		
		public bool IsLoaded {
			get { return isLoaded; }
			set { isLoaded = value; OnProperty("IsLoaded"); }
		} bool isLoaded = false;
		
		public void Load()
		{
			bool? value = OfdSql.ShowDialog();
			if (!(value.HasValue && value.Value)) return;
			Load(OfdSql.FileName);
		}
		
		public void Load(string file)
		{
			SqlFile = file;
			IsLoaded = File.Exists(SqlFile);
		}
		
		public void Save(string text)
		{
			bool? value = false;
			
			// show file-dialog if no file has previously been loaded
			if (IsLoaded != true) {
				
				value = SfdSql.ShowDialog();
				// if loaded file exists, set our SqlFile
				if (File.Exists(SfdSql.FileName)) this.SqlFile = SfdSql.FileName;
				
				
			}
			// Otherwise, just save the file
			File.WriteAllText(SqlFile, text);
		}
		
		/// <summary>
		/// Here, we are always using a file-dialog to save the file.
		/// </summary>
		/// <param name="text"></param>
		public void SaveAs(string text)
		{
			bool? value = SfdSqlAs.ShowDialog();
			
			// If FileDialog points to file, write text.
			if (value.HasValue && value.Value) {
				SqlFile = SfdSql.FileName;
				IsLoaded = true;
				File.WriteAllText(SqlFile,text);
				return;
			}
		}
		
		#endregion
		
		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;
		
		void OnProperty(string prop)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
		#endregion
	}
}


