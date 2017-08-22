#region Using
using System;
using System.ComponentModel;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
#endregion
namespace Generator.Core.Markup
{
	public interface IMarkupTemplate<BaseClass>
	{
//		protected BaseClass Element { get;set; }
		string ElementTemplate { get; }
		string ItemsTemplate { get; }
	}
	
	/// <summary>
	/// Provides the foundation for the TableTemplate class.
	/// </summary>
	public class MarkupTemplate<BaseClass> :
		IMarkupTemplate<BaseClass>
	{
		#region Defalult Constants
		internal const string groupDefault = "Default";
		internal const string groupTemplate = "Template";
		internal const string groupAssembly = "Assembly";
		#endregion
		#region default props
		string alias, name = "new test item";
		[XmlAttribute,CategoryAttribute(groupDefault)]
		public string Name { get { return name; } set { name = value; } }
		
		/// <summary>
		/// Depending how this strategy is used, our Alias could be a TableTemplate
		/// or target tag-name.
		/// </summary>
		[XmlAttribute,Category(groupDefault)]
		public string Alias { get { return alias; } set { alias = value; } }
		
		BaseClass element;
		[XmlIgnore,Category(groupDefault)]
		protected BaseClass Element { get { return element; } set { element = value; } }
		
		string groupName,Template = string.Empty, tags = string.Empty;
		
		[XmlAttribute,Category(groupDefault)]
		public string Group { get { return groupName; } set { groupName = value; } }
		
		[XmlAttribute,Category(groupDefault),DefaultValue("")]
		public string Tags { get { return tags; } set { tags = value; } }
		
		string itemsTemplate;
		/// <summary>
		/// Otherwise known as the field template
		/// </summary>
		[XmlElement]
		public string ItemsTemplate
		{
			get { return itemsTemplate; }
			set { itemsTemplate = value; }
		}
		
		[Browsable(false)]
		virtual public string ElementTemplate { get { return Template; } set { Template=value; } }
		#endregion
		
		public MarkupTemplate()
		{
		}
		public MarkupTemplate(BaseClass value)
		{
			element = value;
		}
		
		virtual protected void FromRowValues(DataRowView row)
		{
			FromRowValues(row.Row);
		}
		
		static public T RowValue<T>(DataRow row, string field)
		{
			if (row[field]==DBNull.Value) return (T)(Object)null;
			return (T)row[field];
		}
		virtual protected void FromRowValues(DataRow row)
		{
			this.Alias = RowValue<string>(row,"Alias");
			this.ElementTemplate =  RowValue<string>(row,res.elmTpl);
			this.ItemsTemplate =  RowValue<string>(row,res.itmTpl);
			this.Tags =  RowValue<string>(row,"Tags");
			this.Group =  RowValue<string>(row,"Group");
			this.Name =  RowValue<string>(row,"Name");
		}
		virtual protected void ToRow(DataRowView row)
		{
			row["Name"] = this.Name;
			row["Alias"] = this.Alias;
			row["Group"] = this.Group;
			row["Tags"] = this.Tags;
			row[res.elmTpl] = this.Element;
			row[res.itmTpl] = this.ElementTemplate;
		}
		
		public void ToTable(DataSet ds, string tableName)
		{
			ToTable(ds.Tables[tableName]);
		}
		virtual public void ToView(DataView view)
		{
			ToTable(view.Table);
		}
		virtual public void ToTable(DataTable table)
		{
			DataRowView drv = table.DefaultView.AddNew();
			ToRow(drv);
			table.Rows.Add(drv.Row);
			table.AcceptChanges();
		}
	}
}
