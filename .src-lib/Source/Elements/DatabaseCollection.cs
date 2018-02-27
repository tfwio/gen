using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using Generator.Parser;
using Generator;

namespace Generator.Elements
{
	//
	[XmlRoot("DatabaseCollection"/*,Namespace=DatabaseCollection.const_ns*/)] //,Namespace="http://w3.tfw.co/xmlns/2011/dbscheme")]
	public partial class DatabaseCollection : SerializableClass<DatabaseCollection>, INotifyPropertyChanged
	{
		#region PropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		virtual protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion
		
		static public readonly object queryContainer = "queryContainer";
		const string const_ns = "http://w3.tfw.co/xmlns/2011";
		const string const_ns_ttl = "dbscheme";
		public const string ref_asm_node = "References";

		[XmlIgnore] public DatabaseCollection Parent { get; set; }

    #region Method: Rechild
    /// <summary>
    /// The role of this action is to parent each of the elements so as to enable
    /// reverse-lookups.
    /// <para>Thus far, it appears that the Rechild method is executed when a configuration file
    /// is loaded (such as databaseconfiguration file).</para>
    /// </summary>
    internal static void Rechild(DatabaseCollection element)
		{
			for (int i = 0; i < element.Databases.Count; i++) {
				element.Databases[i].Parent = element;
				Rechild(element,element.Databases[i]);
			}
		}
    /// <summary>
    /// The role of this action is to parent each of the elements so as to enable
    /// reverse-lookups.
    /// <para>Thus far, it appears that the Rechild method is executed when a configuration file
    /// is loaded (such as databaseconfiguration file).</para>
    /// </summary>
    internal static void Rechild(DatabaseCollection parent, DatabaseElement child)
		{
			child.Children.Clear();
			for (int i = 0; i < child.Items.Count; i++) {
				
				child.Items[i].Parent = child;
				Rechild(child,child.Items[i]);
				child.Children.Add(child.Items[i]);
			}
			for (int i = 0; i < child.Views.Count; i++)
			{
				child.Views[i].Parent = child;
				child.Children.Add(child.Views[i]);
			}
		}
		/// <summary>
		/// The role of this action is to parent each of the elements so as to enable
		/// reverse-lookups.
		/// <para>Thus far, it appears that the Rechild method is executed when a configuration file
		/// is loaded (such as databaseconfiguration file).</para>
		/// </summary>
		internal static void Rechild(DatabaseElement parent, TableElement child)
		{
			for (int i = 0; i < child.Fields.Count; i++) {
				child.Fields[i].Parent = child;
			}
		}
		
		/// <summary>
		/// The role of this action is to parent each of the elements so as to enable
		/// reverse-lookups.
		/// <para>Thus far, it appears that the Rechild method is executed when a configuration file
		/// is loaded (such as databaseconfiguration file).</para>
		/// </summary>
		public void Rechild()
		{
			Rechild(this);
		}
		#endregion
		
		[XmlAttribute]
		public string DateModified
		{
			get {
				return string.Format("yyyy/MM/dd", DateTime.Now);
			}
		}

		#region ReferenceAssemblyCollection Elements (conditional pragma)
		#if ASMREF
		ReferenceAssemblyCollection itemsAsm;
		[XmlElement("Assemblies")]
		public ReferenceAssemblyCollection Assemblies
		{
			get {
				return itemsAsm;
			} set {
				itemsAsm = value;
			}
		}

		ReferenceAssemblyElement FindAsmName(string Key)
		{
			if (itemsAsm==null) return null;
			if (Assemblies.Assemblies.Count==0) return null;
			foreach (ReferenceAssemblyElement aref in this.Assemblies.Assemblies) if (aref.Name==Key) return aref;
			return null;
		}
		public bool HasAssembly(string Key)
		{
			return FindAsmName(Key)!=null;
		}
		#endif
		#endregion

		#region DatabaseElement-Collection Utilities
		public override XmlSerializerNamespaces SerializableNamespaces
		{
			get {
				var xsn = new XmlSerializerNamespaces();
				xsn.Add(const_ns_ttl,const_ns);
				return xsn;
			}
		}

		public DatabaseElement this[string Key]
		{
			get {
				return Find(Key);
			}
		}

		DatabaseElement Find(string Key)
		{
			if (items==null) return null;
			if (items.Count==0) return null;
			foreach (DatabaseElement db in this.Databases) if (db.Name==Key) return db;
			return null;
		}

		public bool Contains(string Key)
		{
			return Find(Key)!=null;
		}

		#endregion

		#region DatabaseElement Elements
		List<DatabaseElement> items;
		[XmlElement("Database")]
		public List<DatabaseElement> Databases
		{
			get {
				return items;
			} set {
				items = value; OnPropertyChanged("Databases");
			}
		}
		#endregion

		#region QueryElement Elements (Sql, Query)
		List<QueryElement> sqlItems;
		[XmlElement("Sql")]
		public List<QueryElement> Sql
		{
			get {
				return sqlItems;
			} set {
				sqlItems = value; OnPropertyChanged("Sql");
			}
		}

		List<QueryElement> queries;
		[XmlElement("Query")]
		public List<QueryElement> Queries
		{
			get {
				return queries;
			} set {
				queries = value; OnPropertyChanged("Queries");
			}
		}
		#endregion
#if controlutil
		/// <summary>
		/// <para>Attempts to load a file as a Database Collection.</para>
		/// <para>If no file is selected (DialogResult.Cancel) a new collection is created and loaded.</para>
		/// </summary>
		/// <returns></returns>
		static public DatabaseCollection LoadSafe()
		{
			string Filename = ControlUtil.FGet("xdata|*.xdata|Xml Document|*.xml|All Types|*.xdata;*.xml|All Files|*;");
			if (Filename==string.Empty) return new DatabaseCollection();
			return Load(Filename);
		}
#endif
		public DatabaseCollection()
		{
		}

		/// <summary></summary>
		/// <param name="dbs">(IDbConfiguration) where the List of
		/// ‘Databases’ finds the template, and sends the IDbConfiguration
		/// through to the Table.ConvertInput method.
		/// </param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public string ConvertInput(Generator.Export.Intrinsic.IDbConfiguration4 dbs, string tableName)
		{
			// I suppose that this loop just checks weather the table exists.
			foreach (DatabaseElement elm in Databases) {
				if ( elm[tableName] == null ) continue;
				if ( elm.Contains(tableName) ) {
				  return TemplateFactory.Generate( dbs );
				}
			}
			return string.Format( Gen.Messages.DatabaseCollection_ConvertInput_TableNotFound , tableName.ToUpper() );
		}

	}
	
}
