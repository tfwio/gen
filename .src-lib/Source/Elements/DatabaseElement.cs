using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

using Generator.Elements.Basic;
using Generator.Elements.Types;

#if TREEV
#endif
namespace Generator.Elements
{
	public class ExElement : DataMapElement, INotifyPropertyChanged
	{
		/*XmlElement here probably isn't serialized*/
		[XmlAttribute] public string Name {
			get { return name; } set { name = value; OnPropertyChanged("Name"); }
		} [XmlElement("name")] string name;
		/*XmlElement here probably isn't serialized*/
		[XmlAttribute] public string Expression {
			get { return expression; } set { expression = value; OnPropertyChanged("Expression"); }
		} [XmlElement("name")] string expression;
		
		[XmlAttribute] public string Ex {
			// build string.RegexReplace(// on @attr.replace
			// unil last replace(,emty)
			get { return expression; } set { expression = value; OnPropertyChanged("CalculatedExpression"); }
		}
		
		List<ExElement> children;
		public List<ExElement> Children {
			get { return children; }
			set { children = value;OnPropertyChanged("Expression"); }
		}
		public ExElement()
		{
		}
		
	}
	public partial class ExpressionElement : DataMapElement, INotifyPropertyChanged
	{
		public ExpressionElement()
		{
		}
	}
	public partial class DatabaseElement : DataMapElement, INotifyPropertyChanged
	{
		public void Remove(DatabaseChildElement child)
		{
			if (child is TableElement) items.Remove(child as TableElement);
			if (child is DataViewElement) views.Remove(child as DataViewElement);
			children.Remove(child);
		}
		public void Insert(int index, DatabaseChildElement child)
		{
			if (child is TableElement) items.Insert(index,child as TableElement);
			if (child is DataViewElement) views.Insert(index,child as DataViewElement);
			children.Insert(index,child);
		}
		
		[XmlIgnore]
		public DatabaseCollection Parent {
			get {
				return parent;
			}
			set {
				parent = value; OnPropertyChanged("Parent");
			}
		} DatabaseCollection parent;
		#region Properties
		
		#region Connection Type
		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute]
		public ConnectionType ConnectionType
		{ get { return connectionType; } set { connectionType = value; OnPropertyChanged("ConnectionType"); }
		} ConnectionType connectionType;
		#endregion
		
		/*XmlElement here probably isn't serialized*/
		[XmlAttribute] public string Name {
			get { return name; } set { name = value; OnPropertyChanged("Name"); }
		} [XmlElement("info")] string name;
		
		[XmlAttribute] public string PrimaryKey {
			get { return primaryKey; } set { primaryKey = value; OnPropertyChanged("PrimaryKey"); }
		} string primaryKey;
		
		/// <summary>
		/// This doesn't appear to be implemented.
		/// </summary>
		[XmlElement] public List<UniqueKey> Keys;
		
		/// <summary>
		/// A (XmlElement) collection of TableElement Items.
		/// </summary>
		[XmlElement("Table")]
		public List<TableElement> Items {
			get { return items; }
			set { items = value; OnPropertyChanged("Items"); }
		} List<TableElement> items;
		
		[XmlIgnore] public List<DatabaseChildElement> Children {
			get { return children; }
			set { children = value; OnPropertyChanged("Children"); }
		} List<DatabaseChildElement> children = new List<DatabaseChildElement>();
		
		/// <summary>
		/// A (XmlElement) collection of DataViewElement Views.
		/// </summary>
		[XmlElement("View")]
		public List<DataViewElement> Views {
			get { return views; }
			set { views = value; OnPropertyChanged("Views"); }
		} List<DataViewElement> views;
//		/// <summary>
//		/// </summary>
//		public List<DataViewElement> Views { get { return views; } set { views = value;  OnPropertyChanged("Views"); } }
//		List<DataViewElement> views;

		#endregion

		private TableElement Find(string Key) {
			if (items==null) return null;
			if (items.Count==0) return null;
			foreach (TableElement table in this.Items) if (table.Name==Key) return table;
			return null;
		}

		#region public: Methods
		/// <summary>
		/// Checks to see if the TableElement is in the DatabaseCollection.
		/// </summary>
		/// <param name="Key">The name of the table</param>
		/// <returns>
		/// True if the table exists in the DatabaseElement checking.
		/// </returns>
		public bool Contains(string Key) { return Find(Key)!=null; }
		/// <summary>
		/// Returns the table if it exists, otherwise NULL.
		/// </summary>
		public TableElement this[string Key] { get { return Find(Key); } }
		#endregion

		#region Constructor
		
		/// <summary>
		/// Creates a new DatabaseElement Element with a given name.
		/// </summary>
		/// <param name="_name">An initial name for the Database.</param>
		public DatabaseElement(string _name)
		{
			Name = _name;
		}
		/// <summary>
		/// Default parameter-less constructor.
		/// </summary>
		public DatabaseElement()
		{
		}
		#endregion
	}
	
}
