/* oio : 03/10/2014 00:34 */
using System;
using System.ComponentModel;
using System.Data.SQLite;
using System.IO;
using Microsoft.Win32;
using GeneratorTool.Views;

namespace GeneratorTool.SQLiteUtil
{
	/// <summary>
	/// There are three methods exposed here.
	/// Two overloads of Load, one triggers a OpenFileDialog, the other takes a FileName parameter.
	/// A Create method creates a blank SQLite database and sets it as the datafile.
	/// </summary>
	class DataFileLoader : INotifyPropertyChanged
	{
		#region Commands
		
		public class LoadDatabaseCommand : BasicCommand {
		
			DataFileLoader Loader { get; set; }
		
			// we can always load a new file.
		
			public override bool CanExecute(object parameter) { return true; }
		
			protected override void OnExecute(object parameter)
			{
				Loader.Load();
			}
		
		}
		public class CreateDatabaseCommand : BasicCommand {
		
			DataFileLoader Loader { get; set; }
		
			public override bool CanExecute(object parameter) { return true; }
		
			protected override void OnExecute(object parameter)
			{
				Loader.Create();
			}
		
		}
		#endregion
		
		#region FileDialogs
		static public readonly OpenFileDialog OFD_DatabaseFile = new OpenFileDialog(){
			Filter =
			  "SQLite Database (*.db)|*.db|" +
				"SQlite Database (*.db3)|*.db3|" +
			  "SQlite Database (*.dbx)|*.dbx|" +
				"SQLite3 Database (*.sqlite)|*.sqlite|" +
				"All files|*",
			DefaultExt = "SQLite Database (*.db)|*.db",
		};
		static public readonly SaveFileDialog SFD_DatabaseFile = new SaveFileDialog(){
			Filter =
				"SQlite Database (*.db3)|*.db3|" +
				"SQLite Database (*.db)|*.db|" +
				"SQlite Database (*.dbx)|*.dbx|" +
				"SQLite3 Database (*.sqlite)|*.sqlite|" +
				"All files|*"
			};
		#endregion
		
		#region DataFile loader and property
		
		public string DataFile {
			get { return dataFile; } set { dataFile = value; OnProperty(DataFile); }
		} string dataFile;
		
		public bool IsLoaded {
			get { return isLoaded; }
			set { isLoaded = value; OnProperty("IsLoaded");  }
		} bool isLoaded = false;
		
		
		#endregion
		
		public void Create()
		{
			bool? value = SFD_DatabaseFile.ShowDialog();
			if (value.HasValue && value.Value)
			{
				Load(SFD_DatabaseFile.FileName);
				SQLiteConnection.CreateFile(SFD_DatabaseFile.FileName);
			}
		}
		
		public void Load()
		{
			if (!OFD_DatabaseFile.ShowDialog().Value) { return; }
			Load(OFD_DatabaseFile.FileName);
		}
		
		public void Load(string file)
		{
			DataFile = file;
			IsLoaded = File.Exists(DataFile);
		}
		
		#region INotifyPropertyChanged implementation
		
		public event PropertyChangedEventHandler PropertyChanged;
		void OnProperty(string prop)
		{
			if (PropertyChanged!=null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
		
		#endregion
		
	}
}
					