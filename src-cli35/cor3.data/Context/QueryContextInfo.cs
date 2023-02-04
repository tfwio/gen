#region User/License
// Copyright (c) 2005-2013 tfwroble
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
/*
 * User: oio
 * Date: 11/30/2011
 * Time: 14:46
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Serialization;

namespace System.Cor3.Data.Context
{
	[Serializable]
	public class QueryContextCollection : SerializableClass<QueryContextCollection>
	{
		public List<QueryContextInfo> Info { get;set; }
	}
	
	[Serializable]
	public class QueryContextInfo
	{
		[XmlAttribute] public string Generator = "Unspecified";
		
		#region Table
		
		[XmlIgnore] public DataRowView TableRow { get; set; }

		// Table Information \\
		
		/// <summary>
		/// If supplied, represents identification for the table.
		/// <para>if not supplied, provides the value for the ‘TableName’.</para>
		/// </summary>
		[XmlAttribute] public string TablePrj {
			get { return this.tablePrj ?? TableName; } set {
				tablePrj = value;
			}
		} internal string tablePrj = null;
		
		public event EventHandler TablePrjChanged;
		
		/// <summary>
		/// If supplied, represents identification for the table.
		/// <para>if not supplied, provides the value for the ‘TableName’.</para>
		/// </summary>
		[XmlAttribute] public string TableFile {
			get { return this.tableFileName; } set { tableFileName = value; }
		} internal string tableFileName = null;
		
		/// <summary>
		/// If supplied, represents identification for the table.
		/// <para>if not supplied, provides the value for the ‘TableName’.</para>
		/// </summary>
		[XmlAttribute] public string TableAlias {
			get { return this.tableAlias ?? TableName; } set { tableAlias = value; }
		} internal string tableAlias = null;
		
		/// <summary>
		/// The name of the table.
		/// <para>REQUIRED</para>
		/// </summary>
		[XmlAttribute]	public string TableName {
			get { return tableName; } set { tableName = value; }
		} string tableName = string.Empty; // required
		
		/// <summary>
		/// In memory (UI), represents the currently editable field.
		/// <para>REQUIRED</para>
		/// </summary>
		[XmlAttribute]	public string TablePk {
			get { return tablePk; } set { tablePk = value; }
		} string tablePk = string.Empty; // required
		
		/// <summary>
		/// Designates a table-field as NAME or KEY—useful for a ui application.
		/// </summary>
		
		[XmlAttribute]	public string TableTitle {
			get { return tableTitle; } set { tableTitle = value; }
		} string tableTitle = string.Empty; // required
		
		/// <summary>
		/// Designates a table-field as VALUE—useful for a ui application.
		/// <para>REQUIRED</para>
		/// </summary>
		
		[XmlAttribute] public string TableContent {
			get { return tableContent; } set { tableContent = value; }
		} string tableContent = string.Empty; // required

		// ————————————————————————————————————————————————————
		// Category Information \\
		// ————————————————————————————————————————————————————
		#endregion
		
		#region Category
		
		/// <summary>
		/// In memory (UI), represents the currently designated TABLE
		/// name of the table we are relying on for category
		/// (GROUP) information.
		/// <para>
		/// • Maybe I was wrong and this stores the currently selected
		/// category in memory?
		/// </para>
		/// </summary>

		[XmlAttribute] public string TableCategory {
			get { return tableCategory; } set { tableCategory = value; }
		} string tableCategory=null; // required to enable category grouping

		/// <summary>
		/// 
		/// </summary>

		[XmlAttribute] public CategoryMode CategoryMode
		{
			get
			{
				return categoryMode;
			}
			set
			{
				categoryMode = value;
			}
		} CategoryMode categoryMode = CategoryMode.None;
		
		/// <summary>
		/// 
		/// </summary>
		
		[XmlAttribute] public string CategoryName {
			get { return categoryName; } set { categoryName = value; }
		} string categoryName=null;
		
		/// <summary>
		/// the title of the category.
		/// <para>• (REQUIRED) If you supplied a Category Name.</para>
		/// </summary>
		
		[XmlAttribute] public string CategoryTitle {
			get { return categoryTitle; } set { categoryTitle = value; }
		} string categoryTitle=null;
		
		/// <summary>
		/// The Primary Key for the category table
		/// <para>• (REQUIRED) If you supplied a Category Name.</para>
		/// </summary>
		
		[XmlAttribute] public string CategoryPk {
			get { return categoryPk; } set { categoryPk = value; }
		} string categoryPk=null;
		
		#endregion
		
		#region Sort
		
		/// <summary>
		/// </summary>
		[XmlAttribute] public string SqlSort {
			get { return sqlSort; } set { sqlSort = value; }
		} string sqlSort = null;
		
		/// <summary>
		/// SqlOrderMode.
		/// Undefined by default—relys on the query's default ascending/descending.
		/// </summary>
		[XmlAttribute] public SqlOrderMode SqlSortOrder {
			get { return sqlSortOrder; }
			set { sqlSortOrder = value; }
		} SqlOrderMode sqlSortOrder = SqlOrderMode.Undefined;
		
		#endregion
		
		#region Fields
		// ————————————————————————————————————————————————————
		public bool ContainsField(string Key)
		{
			foreach (string field in TableFields)
			{
				if (field==Key) return true;
			}
			return false;
		}
		// ————————————————————————————————————————————————————
		
		/// <summary>
		/// TableFields
		/// <para>The field names in the data-table.  Named after each 'field' within the database's table</para>
		/// </summary>
		[XmlIgnore] public string[] TableFields {
			get { return tableFields; }
			set { tableFields = value; }
		} [XmlAttribute] public string TableFieldsAttribute
		{
			get { return TableFields==null ? null : string.Join(",",TableFields); }
			set { if (value!=null) TableFields = string.IsNullOrEmpty(value) ? null : value.Split(','); }
		} string[] tableFields = new string[0];
		
		/// <summary>
		/// Table Category-Fields
		/// <para>comma delimited set of table categories you can assign a table. </para>
		/// </summary>
		[XmlIgnore] public string[] TableCategoryFields {
			get { return tableCategoryFields; }
			set { tableCategoryFields = value; }
		} [XmlAttribute] public string TableCategoryFieldsAttribute
		{
			get { return TableCategoryFields==null ? null : string.Join(",",TableCategoryFields); }
			set { if (value!=null) TableCategoryFields = string.IsNullOrEmpty(value) ? new string[0] : value.Split(','); }
		} string[] tableCategoryFields = new string[0];

		/// <summary>
		/// TableInsertStamp
		/// <para>
		/// Contain/force fields required for insert queries, keeping in mind the primarykey (functions in here somewhere)
		/// </para>
		/// </summary>
		[XmlIgnore] public string[] TableInsertStamp {
			get { return tableInsertStamp; }
			set { tableInsertStamp = value; }
		}  [XmlAttribute("insertstamp")] public string TableInsertStampAttribute {
			get { return tableInsertStamp==null ? null : string.Join(",",TableInsertStamp); }
			set { if (value!=null) TableInsertStamp = string.IsNullOrEmpty(value) ? new string[0] : value.Split(','); }
		} string[] tableInsertStamp = new string[0];
		
		/// <summary>
		/// TableCategoryInsertStamp
		/// <para>if included, forces these fields and forces them to INSERT queries at the least.</para>
		/// </summary>
		[XmlIgnore] public string[] TableCategoryInsertStamp {
			get { return tableCategoryInsertStamp; }
			set { tableCategoryInsertStamp = value; }
		}  [XmlAttribute("catstamp")] public string TableCategoryInsertStampAttribute {
			get { return tableCategoryInsertStamp==null ? null : string.Join(",",TableCategoryInsertStamp); }
			set { if (value!=null) TableCategoryInsertStamp = string.IsNullOrEmpty(value) ? new string[0] : value.Split(','); }
		} string[] tableCategoryInsertStamp = new string[0];

		#endregion

		#region Groups
		public bool ContainsGroup(string Key)
		{
			foreach (string field in TableGroups)
			{
				if (field==Key) return true;
			}
			return false;
		}

		/// <summary>
		/// TableGroups
		/// </summary>

		/// <summary>
		/// If used, Embedded are functions bind to create a
		/// DataSet containing Groups?  If not, something not to far
		/// off.
		/// </summary>
		[XmlIgnore] public string[] TableGroups {
			get { return tableGroups; }
			set { tableGroups = value; }
		} string[] tableGroups = new string[0];

		/// <summary>
		/// this Attribute is strictly for serialization.
		/// I don't know why I would write something like that.
		/// </summary>
		[XmlAttribute] public string TableGroupsAttribute
		{
			get { return string.Join(",",TableGroups); }
			set { if (value!=null) TableGroups = string.IsNullOrEmpty(value) ? new string[0] : value.Split(','); }
		}

		#endregion

		public void SetContext(QueryContextInfo clone)
		{
			this.SqlSort = clone.SqlSort;
			this.SqlSortOrder = clone.SqlSortOrder;
			this.tableAlias = clone.tableAlias;
			this.TablePk = clone.TablePk;
			this.TableFieldsAttribute = clone.TableFieldsAttribute;
			this.TableCategory = clone.TableCategory;
			this.TableContent = clone.TableContent;
			this.TableGroupsAttribute = clone.TableGroupsAttribute;
			this.TableName = clone.TableName;
			this.TableTitle = clone.TableTitle;
			this.TableInsertStamp = clone.TableInsertStamp;
			this.CategoryName = clone.CategoryName;
			this.CategoryPk = clone.CategoryPk;
			this.CategoryMode = clone.categoryMode;
			this.CategoryTitle = clone.CategoryTitle;
			this.Generator = clone.Generator; // < \\
		}
		
		static public QueryContextInfo Create(QueryContextInfo clone)
		{
			QueryContextInfo context = new QueryContextInfo();
			context.SetContext(clone);
			return context;
		}

		public QueryContextInfo()
		{
		}
	}

}
