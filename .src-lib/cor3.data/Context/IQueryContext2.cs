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
 * Date: 11/30/2011
 * Time: 14:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Cor3.Data.Engine;
using System.Data;
using System.Data.SQLite;
using System.Xml;
using System.Xml.Serialization;

namespace System.Cor3.Data.Context
{
//	public class QueryContextCollection
//	{
//		List<IQueryContext2> items = new List<IQueryContext2>();
//		
//		static List<IQueryContext2> FromFile(string filename)
//		{
//			List<IQueryContext2> list = new List<IQueryContext2>();
//			XmlReader reader = new XmlTextReader(filename);
//			XPathNavigator nav;
//			
//		}
//		
//		public QueryContextCollection()
//		{
//		}
//	}
	
	public interface IQueryContext
	{
		QueryDatabaseContext ContextInfo { get; }
	}
	/// <summary>
	/// IQueryContext is a prototype for automation of database orientated
	/// procedures.
	/// </summary>
	public interface IQueryContext2 : IQueryContext1, IQueryContext
	{
		//		DataSet Data { get; set; }
		//		DataSet Categories { get; set; }
		/// <summary>
		/// The name of the table.
		/// </summary>
		[XmlIgnore]
		string TableName { get; }
		/// <summary>
		/// The default row (title) for the table
		/// </summary>
		string TableAlias { get; }
		/// <summary>
		/// Grouping column-names designated for grouping in the
		/// order you would like grouping applied.
		/// </summary>
		string[] TableGroups { get; }
		/// <summary>
		/// A default (Content) table-column designated for a primary
		/// text-editor.
		/// </summary>
		string TableContent { get; }
		/// <summary>
		/// A list of all editable table-fields to be supplied to the
		/// dataset for the main query.
		/// </summary>
		string[] Fields { get; }
	}
}
