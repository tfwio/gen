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
using System.Globalization;
using System.Text.RegularExpressions;
#endregion

namespace Generator.Extensions
{
	static public class XHelpers
	{
		const string replaceField_ifNull = "Null";
		const string    getString_ifNull = "Null";
		// --------------------------------------------------------------------
		static public string ReplaceField(this string input, DataRowView row, string format, string field, string ifNull)
		{
			return input.Replace(
				"%{tag}%".Replace("{tag}",field),
				row.GetString(format,field,ifNull)
			);
		}
		// 
		static public string ReplaceField(this string input, DataRowView row, string field)
		{
			return input.ReplaceField(row,"{0}",field,replaceField_ifNull);
		}
		// --------------------------------------------------------------------
		static public string GetString(this DataRowView row, string format, string field, string valueIfNull)
		{
			return string.Format(
				CultureInfo.CurrentCulture,
				format,
				row[field] == DBNull.Value ? valueIfNull : row[field]
			);
		}
		static public string GetString(this DataRowView row, string field, string ifNull) { return row.GetString("{0}",field,ifNull); }
		static public string GetString(this DataRowView row, string field) { return row.GetString(field,getString_ifNull); }
		// --------------------------------------------------------------------
		static public DataTable FindTable(this DataSet ds, string table)
		{
			if (ds.Tables.Contains(table)) return ds.Tables[table];
			else return null;
		}
		// --------------------------------------------------------------------
		static public string ToBool(this string input)
		{
			if (string.IsNullOrEmpty(input)) return false.ToString();
			else if (input.ToLower().Equals("true")) return true.ToString();
			else if (input.ToLower().Equals("false")) return false.ToString();
			else if (input.ToLower().Equals("yes")) return true.ToString();
			else if (input.ToLower().Equals("no")) return false.ToString();
			return false.ToString();
		}
	}
}
