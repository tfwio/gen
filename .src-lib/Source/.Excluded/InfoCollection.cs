/*
 * oIo — 11/17/2010 — 8:50 PM
 */
using System;
using Generator.Elements.Types;

namespace Generator
{

	/// <summary>
	/// However unsure, I believe that this class is designed to modify or 
	/// create table names and expression syntax from a given template for
	/// converting amongst several SQL syntaxes such as MySQL vs T-SQL.
	/// </summary>
	/// <remarks>
	/// this class is not used
	/// <para>
	/// • It is most likeley that this is a class to make it simple to convert
	///   any particular expression to a destination format.
	/// </para>
	/// <para>
	/// • It seems that I also was attempting to cultivate Expression info
	///   from any particular syntax as well.
	/// </para>
	/// </remarks>
	public class InfoCollection
	{
		InfoStringTypes stringMode = InfoStringTypes.Microsoft;
		public InfoStringTypes StringMode { get { return stringMode; } set { stringMode = value; } }
		static public char[]   noIndex  = new   char[]{'[',']','(',')','{','}','=','*','/','+','-','&','|','%','^','#'};

		#region String Filters
		const string table_n1name_fmt = "{0}";
		const string table_n2name_fmt = "{0}.{1}";
		const string table_n3name_fmt = "{0}.{1}.{2}";
		const string table_1name_fmt = "[{0}]";
		const string table_2name_fmt = "[{0}].[{1}]";
		const string table_3name_fmt = "[{0}].[{1}].[{2}]";
		const string table_m1name_fmt = "`{0}`";
		const string table_m2name_fmt = "`{0}`.`{1}`";
		const string table_m3name_fmt = "`{0}`.`{1}`.`{2}`";
		#endregion
		
		string databaseName,tableName,fieldName;
		InfoCollectionViewTypes viewMode;
		string filterName(string input)
		{
			if (input.Trim()=="*") return input;
			if (input.IndexOfAny(noIndex)!=-1) return string.Format(table_1name_fmt,input);
			return input;
		}
		public string DatabaseName { get { return filterName(databaseName); } set { databaseName = value; } }
		public string TableName { get { return filterName(tableName); } set { tableName = value; } }
		public string FieldName { get { return filterName(fieldName); } set { fieldName = value; } }

		public string DisplayString
		{
			get
			{
				switch (ViewMode)
				{
				case InfoCollectionViewTypes.Default: return string.Format(table_n3name_fmt,DatabaseName,TableName,FieldName);
				case InfoCollectionViewTypes.Database: return this.databaseName;
				case InfoCollectionViewTypes.DatabaseTable: return string.Format(table_n2name_fmt,DatabaseName,TableName);
				case InfoCollectionViewTypes.Field: return string.Format(table_n1name_fmt,FieldName);
				case InfoCollectionViewTypes.Table: return string.Format(table_n1name_fmt,TableName);
				case InfoCollectionViewTypes.TableField: return string.Format(table_n2name_fmt,TableName,FieldName);
				default: return string.Format(table_n3name_fmt,DatabaseName,TableName,FieldName);
				}
			}
		}
		
		public InfoCollectionViewTypes ViewMode { get { return viewMode; } set { viewMode = value; } }
		
		public InfoCollection(string input, InfoCollectionViewTypes mode)
		{
			switch (ViewMode = mode)
			{
					case InfoCollectionViewTypes.Database: DatabaseName = input; break;
					case InfoCollectionViewTypes.Table: TableName = input; break;
					case InfoCollectionViewTypes.Field: FieldName = input; break;
			}
		}
		
		public InfoCollection(string field1, string field2, InfoCollectionViewTypes mode)
		{
			switch (viewMode=mode)
			{
				case InfoCollectionViewTypes.DatabaseTable:
					DatabaseName = field1;
					TableName = field2;
					break;
				case InfoCollectionViewTypes.TableField:
					TableName = field1;
					FieldName = field2;
					break;
			}
		}
		public InfoCollection() : this(string.Empty,string.Empty,string.Empty, InfoCollectionViewTypes.Default) {}
		public InfoCollection(string db, string tbl, string fld, InfoCollectionViewTypes mode)
		{
			ViewMode = mode;
			databaseName = db;
			tableName = tbl;
			fieldName = fld;
		}
	}
}
