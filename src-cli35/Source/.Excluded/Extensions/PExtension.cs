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
using System.Data.SqlClient;
using System.Text.RegularExpressions;
#endregion

namespace Generator.Extensions
{
	static public class PExtension
	{
		#region Not used
		
		// Not referenced
		static public string parseDataSetFields(this string input, DataSet ds, string table, params string[] fields)
		{
			foreach (DataColumn col in ds.Tables[table].Columns)
			{
				string newField = "{dataset-{t}:{f}}".Replace("{t}",table).Replace("{f}",col.ColumnName);
				//				if (input.Contains(newField))
				//					input.Replace(newField,ds.Tables[table][col.ColumnName]
			}
			return null;
		}
		
		/// <summary>
		/// note that the dataset contains a single table named as a parameter.
		/// each field asked for is returned.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="ds"></param>
		/// <param name="table"></param>
		/// <param name="fields"></param>
		/// <returns></returns>
		static public string listDataTables(this string input, DataSet ds, string table, params string[] fields)
		{
			List<string> listRows = new List<string>();
			string newstring = "rows for {table}: {rows}".Replace("{table}",table);
			if (!ds.Tables.Contains(table)) return "no table exists with the name {}".Replace("{}",table);
			else
			{
				List<string> listCols = new List<string>();
				foreach (DataRowView row in ds.Tables[table].DefaultView)
				{
					foreach (string field in fields) listCols.Add(field);
				}
				listRows.Add(newstring.Replace("{rows}",string.Join(", ",listCols.ToArray())));
			}
			return null;
		}

		#endregion
	}
}
