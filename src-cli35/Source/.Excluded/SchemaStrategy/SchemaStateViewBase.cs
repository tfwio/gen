using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;

using Generator.Elements;
using Generator.Data;
using Generator.Extensions;

namespace Generator.Core.Schemas
{
	/// <summary>
	/// when disposed, we clear the views and not the tables.
	/// </summary>
	public class SchemaStateViewBase : IDisposable
	{
		public DataTable tableTypes, tableColumns, tableTables;
		public DataView viewTypes, viewCols, viewTables;
		
		public SchemaStateViewBase(DataSet dataSet, string initialTable)
		{
			tableColumns = dataSet.FindTable(Strings.Schema_Columns);
			tableTables  = dataSet.FindTable(Strings.Schema_Tables);
			tableTypes   = dataSet.FindTable(Strings.Schema_DataTypes);
			
			string colTableFilter = "TABLE_NAME = '{colname}'".Replace("{colname}",initialTable);
			
			viewCols = new DataView(tableColumns,string.Empty,string.Empty,DataViewRowState.CurrentRows|DataViewRowState.ModifiedCurrent);
			viewTables = new DataView(tableTables,string.Empty,string.Empty,DataViewRowState.CurrentRows|DataViewRowState.ModifiedCurrent);
			viewTypes = new DataView(tableTypes,string.Empty,string.Empty,DataViewRowState.CurrentRows|DataViewRowState.ModifiedCurrent);
			
			FilterCols = "".SFilter("TABLE_NAME",initialTable);
		}
		
		public string FilterCols	{ set { viewCols.RowFilter = value; } }
		public string FilterTables	{ set { viewTables.RowFilter = value; } }
		public string FilterTypes	{ set { viewTypes.RowFilter = value; } }
		
		void unload()
		{
			viewTypes.Dispose();
			viewCols.Dispose();
			viewTables.Dispose();
		}
	
		public void Dispose()
		{
			unload();
		}
	}
}
