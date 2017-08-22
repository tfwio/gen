/* oio * 01/21/2014 * Time: 09:09
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Generator;
using Generator.Elements;
namespace GeneratorTool
{
	/// <summary>
	/// Nonstatic
	/// </summary>
	public class WriterTplModel : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string n)
		{
			var handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(n));
		}

		#endregion
		

		#region General Properties
		internal TemplateUtil util;
		
		public bool IsInitialized {
			get { return isInitialized; }
			set { isInitialized = value; this.OnPropertyChanged("IsInitialized"); }
		} bool isInitialized = false;
		
		public string SelectedGroup {
			get {
				return selectedGroup;
			}
			set {
				selectedGroup = value;
				this.GetRows(value);
				OnPropertyChanged("SelectedGroup");
			}
		} string selectedGroup;
		public TemplateElement SelectedRow {
			get {
				return selectedRow;
			}
			set {
				selectedRow = value;
				OnPropertyChanged("SelectedRow");
			}
		} TemplateElement selectedRow;

		public List<string> GroupNames {
			get { return groupNames; }
			set { groupNames = value; OnPropertyChanged("GroupNames"); }
		} internal List<string> groupNames;
		
		public List<TemplateElement> Rows {
			get { return rows; }
			set { rows = value; OnPropertyChanged("Rows"); }
		} internal List<TemplateElement> rows;
		
		public void Initialize(string path)
		{
			if (util != null) Clear();
			util = new TemplateUtil(path);
			groupNames = util.GetGroups();
			IsInitialized = true;
		}
		#endregion
		
		public void LoadDatabase(string file)
		{
			if (!string.IsNullOrEmpty(file)) return;
			Initialize(file);
		}
		
		#region Methods
		internal void Clear()
		{
			if (util != null) {
				util.Templates.Clear();
				if (groupNames != null) groupNames.Clear();
				if (rows != null) rows.Clear();
				util = null;
				GC.Collect();
				// this probably happens automatically, but I'm not sure.
			}
		}
		internal void GetRows(string tableName)
		{
			rows = util.Templates.Where(t => t.Table == tableName).ToList();
		}
		#endregion
	}
}






