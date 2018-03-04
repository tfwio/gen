/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 07/18/2011
 * Time: 07:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
#region Using
using System;
using System.Collections.Generic;
using System.Cor3.Data.Engine;
using System.Cor3.Parsers;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

using Generator.Data;
#endregion
#if WPF4
using MessageBox = System.Windows.MessageBox;
#elif !NCORE
using MessageBox = System.Windows.Forms.MessageBox;
#endif
namespace Generator.Extensions
{
	[Obsolete]
	public class SchemaProviderClass
	{
	}
	static public class SchemaExtension
	{
		public const string ole_ace12			= "ace12";
		public const string db_sql2005			= "mssql2005";
		
		static public DataSet GetAccessSchemas(AccessDataFactory.AccessDataContext info) { return AccessDataFactory.GetSchemas(info); }
		static public DataSet GetSqlServerSchemas(SqlDataFactory.SQLDataContext info) { return SqlDataFactory.GetSqlServerSchemas(info); }

		/**
		 * the only extension method in the entire class
		 */
		#region Extension: String.SFilter
		static public string SFilter(this string input, string wid, string table)
		{
			return "{word-id} = '{col-id}'"
				.Replace(oldValue: "{col-id}", newValue: table)
				.Replace(oldValue: "{word-id}", newValue: wid)
				;
		}

		#endregion
		
		#region Unused
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <param name="ds"></param>
		/// <param name="table"></param>
		/// <returns></returns>
		static public List<string> SchemaGetTableCols(this string input, DataSet ds, string table)
		{
			List<string> list = new List<string>();
			foreach (DataColumn col in ds.Tables[table].DefaultView) list.Add(col.ColumnName);
			return list;
		}
		#endregion
	}
}
