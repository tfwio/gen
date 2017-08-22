using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Generator.Elements.Basic;

namespace Generator.Elements
{
	public partial class DataViewElement : DatabaseChildElement, IDataView 
	{
		[XmlIgnore]
		public DatabaseElement Parent {
			get {
				return parent;
			}
			set {
				parent = value;
				OnPropertyChanged("Parent");
			}
		}
		DatabaseElement parent;
		#region Properties
		[XmlAttribute("name")] public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } } string name;
		[XmlAttribute("db")] public string Database { get { return database; } set { database = value; OnPropertyChanged("Database"); } } string database;
		[XmlAttribute("table")] public string Table { get { return table; } set { table = value; OnPropertyChanged("Table"); } } string table;
		[XmlAttribute("fields")] public string Fields { get { return fields; } set { fields = value; OnPropertyChanged("Fields"); } } string fields;
		/// <summary>get|set; The alias for the primary table in the view.</summary>
		[XmlAttribute("as")] public string Alias { get { return alias; } set { alias = value; OnPropertyChanged("Alias"); } } string alias;
		[XmlElement("link")] public List<DataViewLink> LinkItems { get { return linkItems; } set { linkItems = value; OnPropertyChanged("LinkItems"); } } List<DataViewLink> linkItems;
		#endregion
		#region Generate(helpers)
		
		public void GetTables()
		{
			
		}
		
		#endregion
		#region Field Utility
		
		[XmlIgnore] public List<string> TableFieldArray { get { return new List<string>(Fields.Split(',')); } }
		[XmlIgnore] public List<string> tablefieldarray { get { return new List<string>(Fields.ToLower().Split(',')); } }
		
		string Act(bool ic, string input) { return ic? input.ToLower() : input;}
		public bool HasField(TableElement table, FieldElement field, bool ignoreCase=true)
		{
			List<string> fields = ignoreCase?tablefieldarray:TableFieldArray;
			bool returnValue = fields.Contains(field.DataName);
			fields = null;
			return returnValue;
		}
		
		#endregion
		
		public DataViewElement(){}
		public DataViewElement(TreeNode node)
		{
			if (!(node.Tag is DataViewElement))
				throw new ArgumentException("Automated Element was not of type(DataViewElement)");
			var refElement = node.Tag as DataViewElement;
			Database = refElement.Database;
			Table = refElement.Table;
			Name = refElement.Name;
			Alias = refElement.Alias;
			Fields = refElement.Fields;
			linkItems = refElement.LinkItems;
		}
		
	}
	
	
}
