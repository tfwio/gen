using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using Generator.Elements.Basic;
namespace Generator.Elements
{
	public class DataViewLink : IDataView, INotifyPropertyChanged
	{
		#region PropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		protected internal void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion
		#region Properties
		[XmlAttribute("db")]
		public string Database {
			get {
				return database;
			}
			set {
				database = value;
				OnPropertyChanged("Database");
			}
		}

		string database;

		[XmlAttribute("table")]
		public string Table {
			get {
				return table;
			}
			set {
				table = value;
				OnPropertyChanged("Table");
			}
		}

		string table;

		[XmlAttribute("fields")]
		public string Fields {
			get {
				return fields;
			}
			set {
				fields = value;
				OnPropertyChanged("LinkItems");
			}
		}

		string fields;

		/// <summary>
		/// get|set; The alias for the primary table in the view.
		/// </summary>
		[XmlAttribute("as")]
		public string Alias {
			get {
				return alias;
			}
			set {
				alias = value;
				OnPropertyChanged("Alias");
			}
		}

		string alias;

		/// <summary>
		/// The name of the table being linked to including the field: alias.field.
		/// Parsed with a <tt>string.Split('.',...)</tt>.
		/// </summary>
		[XmlAttribute("on")]
		public string On {
			get {
				return _on;
			}
			set {
				_on = value;
				OnPropertyChanged("On");
			}
		}

		string _on;

		[XmlAttribute("from")]
		public string From {
			get {
				return _from;
			}
			set {
				_from = value;
				OnPropertyChanged("From");
			}
		}

		string _from;

		#endregion
		#region Field Utility
		[XmlIgnore]
		public List<string> TableFieldArray {
			get {
				return new List<string>(Fields.Split(','));
			}
		}

		[XmlIgnore]
		public List<string> tablefieldarray {
			get {
				return new List<string>(Fields.ToLower().Split(','));
			}
		}

		string Act(bool ic, string input)
		{
			return ic ? input.ToLower() : input;
		}

		public bool HasField(TableElement table, FieldElement field, bool ignoreCase = true)
		{
			List<string> fields = ignoreCase ? tablefieldarray : TableFieldArray;
			bool returnValue = fields.Contains(field.DataName);
			fields = null;
			return returnValue;
		}
	#endregion
	}
}


