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
//#if NET35
/*
 * User: oIo
 * Date: 11/15/2010 – 2:49 AM
 */
using System.Collections.Generic;
using System.Data;
using IDbCommand = System.Data.IDbCommand;
using IDbConnection = System.Data.IDbConnection;
using IDbDataAdapter = System.Data.IDbDataAdapter;

namespace System.Cor3.Data
{
	static public class QueryStringExtension
	{
		static public string SqlTrim(this string input)
		{
			return input.Replace(@"'",@"''");
		}

		static public string MakeStatement(this string input, string format, params string[] values)
		{
			return string.Concat(input,string.Format(format,values));
		}

		internal const string query_fmt_inner_join = "INNER JOIN {0} ON {1} = {2}";
		internal const string query_from = "FROM {0}";
		internal const string query_order_by = "ORDER BY {0}{1}";
		internal const string query_order = "ORDER BY {0}";
		internal const string query_select = "SELECT {0}";
		internal const string query_oEqu = " = {0}";
		internal const string query_oLess = " < {0}";
		internal const string query_oLEqu = " <= {0}";
		internal const string query_oGrea = " > {0}";
		internal const string query_oGEqu = " >= {0}";
		internal const string query_oEqual = " = {0}";
		
		internal const string query_Is = "IS {0}";
		internal const string query_Like = "LIKE {0}";
		internal const string query_Where = "WHERE {0}";
		internal const string query_And = "AND {0}";
		
		internal const string field_brace = "[{0}]";
		internal const string field_squote = "'{0}'";
		internal const string field_dquote = @"""{0}""";
		internal const string field = @"@{0}";
	
		static public string HtmlEscape(this string input)					{ return Uri.EscapeDataString(input); }
		static public string HtmlUnescape(this string input)				{ return Uri.UnescapeDataString(input); }
		/// <summary>
		/// first strips '@' from the beginning of the string if it's there,
		/// then applies the amp once again.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		static public string QField(this string input)						{ return string.Format(field.TrimStart('@'),input); }
		static public string QBrace(this string input)						{ return string.Format(field_brace,input); }
		static public string QCurly(this string input)						{ return string.Format("{{{0}}}",input); }
		static public string QInnerJoin(this string input, string tableRef, string srcField, string refField)
		{
			return input.MakeStatement(query_fmt_inner_join,tableRef,srcField,refField);
		}
		static public string QFrom(this string input, string statement)		{ return input.MakeStatement(query_from,statement); }
	
		static public string QOrderBy(this string input, string statement)	{ return input.QOrderBy(statement,false); }
		static public string QOrderBy(this string input, string statement, bool desc)
		{
			return input.MakeStatement(query_order_by,statement, desc ? " DESC":string.Empty);
		}
	
		static public string QOrder(this string input, params string[] statements)
		{
			string query = string.Empty;
			List<string> list = new List<string>();
			foreach (string statement in statements) list.Add(statement);
			query = string.Format(query_order,string.Join(", ",list.ToArray()));
			list.Clear();
			list = null;
			return string.Concat(input,query);
		}
	
		static public string QSelect(this string input, string statement)	{ return input.MakeStatement(query_select,statement); }
		static public string QLess(this string input, string statement)		{ return input.MakeStatement(query_oLess,statement); }
		static public string QLEqual(this string input, string statement)	{ return input.MakeStatement(query_oLEqu,statement); }
		static public string QGreater(this string input, string statement)	{ return input.MakeStatement(query_oGrea,statement); }
		static public string QEqual(this string input, string statement)	{ return input.MakeStatement(query_oEqu,statement); }
		static public string QGEqual(this string input, string statement)	{ return input.MakeStatement(query_oGEqu,statement); }
		static public string QIs(this string input, string statement)		{ return input.MakeStatement(query_Is,statement); }
		static public string QLike(this string input, string statement)		{ return input.MakeStatement(query_Like,statement); }
		static public string QWhere(this string input, string statement)	{ return input.MakeStatement(query_Where,statement); }
		static public string QAnd(this string input, string statement)		{ return input.MakeStatement(query_Where,statement); }
	
	}
}
//#endif