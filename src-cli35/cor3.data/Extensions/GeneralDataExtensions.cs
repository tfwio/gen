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
#if NET35
/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
using System;
using System.Collections.Generic;
using System.Cor3.Data.Engine;
using System.Data;

namespace System.Cor3.Data
{
	static public class GeneralDataExtensions
	{
		
		/**
		 * 
		 * Are any of the following extensions used?
		 * 
		 */
		
		
		static public List<string> getqueryinsertkeys(this DataRowView row, string Key)
		{
			List<string> list = getrowfields(row), list1 = new List<string>();
			foreach (string v in list)
				if (v!=Key)
					list1.Add(
						"@key = @value"
						.Replace("@key",v.QBrace())
						.Replace("@value", "@" + ( v=="id" ? "xid" : v.CamelClean() ) )
					);
			list.Clear(); list = null;
			return list1;
		}
		static public List<string> getqueryinsertkeys(this DataRowView row)
		{
			return getqueryinsertkeys(row,"id");
		}
		// note that we would use a key in stead of constant field like this.
		static public List<string> getbracelist(this DataRowView row) { return getbracelist(row,"id"); }
		static public List<string> getbracelist(this DataRowView row, string key)
		{
			List<string> list = getrowfields(row), list1 = new List<string>();
			foreach (string v in list) if (v!=key) list1.Add(v.QBrace());
			list.Clear(); list = null;
			return list1;
		}
		// note that we would use a key in stead of constant field like this.
		static public List<string> getfieldlist(this DataRowView row) { return getfieldlist(row,"id"); }
		static public List<string> getfieldlist(this DataRowView row, string key)
		{
			List<string> list = getrowfields(row), list1 = new List<string>();
			foreach (string v in list) if (v!=key) list1.Add("@" + ( v=="id" ? "xid" : v.CamelClean() ) );
			list.Clear(); list = null;
			return list1;
		}
		static public List<string> getrowfields(this DataRowView row)
		{
			List<string> list = new List<string>();
			foreach (DataColumn col in row.Row.Table.Columns)
			{
				list.Add(col.ColumnName);
			}
			return list;
		}
		
		/// <summary>
		/// Provide row info for a specific version of the Table the row belongs to
		/// (if the row is attached to a table).
		/// </summary>
		/// <param name="row"></param>
		/// <param name="Key"></param>
		/// <param name="Version"></param>
		/// <returns></returns>
		static public object GetRowInfo(this DataRowView row, string Key, DataViewRowState Version)
		{
			int rowIndex = -1;
			object returnValue = null;
			using (DataView View = new DataView(row.Row.Table,"","",Version))
			{
				for (int i = 0; i < View.Count; i++)
				{
					if (row.Equals(View[i]))
					{
						rowIndex = i;
						break;
					}
				}
				System.Diagnostics.Debug.Assert(rowIndex==-1, "No matching record was found in the table.");
				returnValue = row[Key];
			}
			return returnValue;
		}

	}
}
#endif
