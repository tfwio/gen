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
	/// <summary>
	/// THIS CLASS IS QUITE USEFUL!
	/// Its been a little while since I wrote this.
	/// DOCUMENT ME!!!
	/// </summary>
	public static class DataTemplateExtensions
	{
		/// <summary>
		/// Applies a set of values from a Template to the results of a SQLite Data Operation.
		/// <para>Note that currently this class requires a COMPILER-LEVEL VARIABLE of 'NET35'</para>
		/// </summary>
		/// <remarks>This method is particularly for a personal project or two, however
		/// the methods for this are here to suggest the extension of this namespace
		/// to include more support to data-template operations.</remarks>
		/// <returns>THe template-formatted string.</returns>
		/// <param name="sqliteConnection">SQLiteConnection.</param>
		/// <param name="query">A SQL SELECT query.</param>
		/// <param name="dataFillCallback">A callback to fill generated dataset.</param>
		/// <param name="rowContainer">A Row-level string providing a container for subsequent rows.</param>
		/// <param name="rowContainerTag">The supplied tag such as '{0}' which is used as a string replacement value for the row-container.
		/// The templated row-results be stacked where this token was placed.</param>
		/// <param name="rowTemplate">A template applied to each resulting row.</param>
		/// <param name="rowSeparator">usually an empty sting, or a EOL terminator.</param>
		/// <param name="replaceMethod">User supplied callback to provide row-field specific filters to data results.</param>
		/// <exception cref="ArgumentException">is thrown if any paraemter is null.</exception>
		static public string SQLiteSelect_Tpl(
			// database constructs
			string sqliteConnection,
			string query,
			SQLiteDb.CBRowParam dataFillCallback,
			// template constructs
			string rowContainer,
			string rowContainerTag,
			string rowTemplate,
			string rowSeparator,
			Func<string,DataRowView,string> replaceMethod)
		{
			List<string> list = new List<string>();
			string output = null;
			using (SQLiteDb db = new SQLiteDb(sqliteConnection))
				using (DataSet ds = db.Select("settings",query,dataFillCallback))
					using (DataView v = ds.GetDataView("settings"))
			{
				output = v.DataViewReplace(rowContainer,rowTemplate,rowSeparator,rowContainerTag,replaceMethod);
			}
			return output;
		}
		
		/// <summary>
		/// Local (Private) method to check for input errors.
		/// </summary>
		/// <param name="rv">DataRowView</param>
		/// <param name="rc">Row Container Template</param>
		/// <param name="rt">Row Template</param>
		/// <param name="rs">Row Separator</param>
		/// <param name="rct">Row Container (Replacement) Tag</param>
		static void DataViewReplaceCheck(DataView rv, string rc, string rt, string rs, string rct)
		{
			if (rv==null)	throw new ArgumentException("(DataRowView) row can not be null.","row");
			if (rc==null)	throw new ArgumentException("Container Template was not set","rc");
			if (rt==null)	throw new ArgumentException("Row Template was not set","rt");
			if (rs==null)	throw new ArgumentException("Row Separator was not set","rs");
			if (rct==null)	throw new ArgumentException("Row Container Template was not set","rct");
		}
		/// <summary>
		/// A utility function to replace all data-row values within the matching template.
		/// </summary>
		/// <param name="rowview">The DataView.</param>
		/// <param name="rowContainer">Row-Container Template</param>
		/// <param name="rowTemplate">The Row Template (per DataRowView)</param>
		/// <param name="rowSeparator">Separator used to separate rows.</param>
		/// <param name="rowContainerToken">The token used as replacement value for the complete 'rowTemplate' once all conversions have been made.</param>
		/// <param name="replacementMethod">
		/// A method which replaces the rowTemplate variables given each DataRowView within the DataView.
		/// </param>
		/// <returns>The converted string.</returns>
		static public string DataViewReplace(
				this DataView rowview,
				string rowContainer,
				string rowTemplate,
				string rowSeparator,
				string rowContainerToken,
				Func<string,DataRowView,string> replacementMethod)
		{
			DataViewReplaceCheck(rowview,rowContainer,rowTemplate,rowSeparator,rowContainerToken);
			List<string> list = new List<string>();
			foreach (DataRowView row in rowview)
				list.Add(replacementMethod.Invoke(rowTemplate,row));
			string output = rowContainer.Replace(rowContainerToken,string.Join(rowSeparator,list.ToArray()));
			list.Clear();
			list = null;
			return output;
		}
	
		/// <summary>
		/// A utility function to replace all data-row values within the matching template.
		/// This overload assumes to use '{0}' as the rowContainerToken.
		/// </summary>
		/// <param name="rowview">The DataView.</param>
		/// <param name="rowContainer">Row-Container Template</param>
		/// <param name="rowTemplate">The Row Template (per DataRowView)</param>
		/// <param name="rowSeparator">Separator used to separate rows.</param>
		/// <param name="replacementMethod">
		/// A method which replaces the rowTemplate variables given each DataRowView within the DataView.
		/// </param>
		/// <returns>The converted string.</returns>
		static
			public
			string
			DataViewReplace(
				this DataView rowview,
				string rowContainer,
				string rowTemplate,
				string rowSeparator,
				Func<string,DataRowView,string> replacementMethod)
		{
			return rowview.DataViewReplace(rowContainer,rowTemplate,rowSeparator,"{0}",replacementMethod);
		}
	}
}
#endif