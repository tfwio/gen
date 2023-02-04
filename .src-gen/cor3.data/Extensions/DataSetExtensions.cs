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
 * Created by SharpDevelop.
 * User: oio
 * Date: 6/8/2011
 * Time: 11:20 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Cor3.Data;
using System.Data;

namespace System.Cor3.Data
{
	/// <summary>
	/// This class originated in Prime-MVC-Application.
	/// It provides various extensions for obtaining DataRowView
	/// and DataView objects with less coding.
	/// </summary>
	static public class DataSetExtensions
	{
		/// <summary>Get the first record from the first table in the dataset.</summary>
		/// <param name="d">The dataset</param>
		/// <returns>Null of not table or record is present, and the DataRowView if there is a matching record.</returns>
		static public DataRowView GetFirstRow(this DataSet d)
		{
			return d.GetDataRow(0,0);
		}
		/// <summary>
		/// Get the first row from Table with the index provided by <strong>tableNo</strong> value.
		/// </summary>
		/// <returns>Null of not table or record is present, and the DataRowView if there is a matching record.</returns>
		/// <param name="d">the dataset.</param>
		/// <param name="tableNo">Index of the table we want results from.</param>
		static public DataRowView GetFirstRow(this DataSet d, int tableNo)
		{
			return d.GetDataRow(tableNo,0);
		}
		/// <summary>
		/// Get a DataRowView from Table Index <strong>tableNo</strong> and the record with Index <strong>recNo</strong>.
		/// </summary>
		/// <returns>Null of not table or record is present, and the DataRowView if there is a matching record.</returns>
		/// <param name="d">the dataset.</param>
		/// <param name="tableNo">Index of the desired table<./param>
		/// <param name="recNo">Index of the desired row within the table.</param>
		static public DataRowView GetDataRow(this DataSet d, int tableNo, int recNo)
		{
			if (d.HasRows()) return d.GetDataView(tableNo)[recNo];
			return null;
		}
		/// <summary>
		/// Get a DataRowView from Table named <strong>tableName</strong> and the record with Index <strong>recNo</strong>.
		/// </summary>
		/// <param name="d">the dataset.</param>
		/// <param name="tableName">The name of the table.</param>
		/// <param name="recNo">Record Index.</param>
		/// <returns></returns>
		static public DataRowView GetDataRow(this DataSet d, string tableName, int recNo)
		{
			if (d.HasRows()) return d.GetDataView(tableName)[recNo];
			return null;
		}
		/// <summary>
		/// Get the default view.
		/// </summary>
		/// <returns>The default table view.</returns>
		/// <param name="d">The dataset.</param>
		/// <param name="table">Table Name.</param>
		static public DataView GetDataView(this DataSet d, string table)
		{
			return d.Tables[table].DefaultView;
		}
		/// <summary>
		/// Get the default view.
		/// </summary>
		/// <returns>The default table view.</returns>
		/// <param name="d"></param>
		/// <param name="tableNo"></param>
		static public DataView GetDataView(this DataSet d, int tableNo)
		{
			return d.Tables[tableNo].DefaultView;
		}
		// NOTE:	bugfix 0000001—table isn't always created in what specific case?  assume the case of an exception.
		/// <summary>
		/// Check if the table at index <strong>tableId</strong> has records.
		/// </summary>
		/// <param name="d">the dataset.</param>
		/// <param name="tableId">index of the table.</param>
		/// <returns>True if the table has 1 or more records.</returns>
		static public bool HasRows(this DataSet d, int tableId)
		{
			if (d.Tables.Count==0) return false;
			if (d.Tables[tableId].Rows.Count > 0)
				return true;
			return false;
		}
		/// <summary>
		/// Check if the first table in the dataset has data.
		/// </summary>
		/// <returns>True if the table has 1 or more records.</returns>
		/// <param name="d">the deataset.</param>
		static public bool HasRows(this DataSet d)
		{
			return d.HasRows(0);
		}

	}
}
