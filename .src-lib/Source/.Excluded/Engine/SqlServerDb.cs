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
 * User: oIo
 * Date: 11/15/2010 – 2:49 AM
 */
using System;
using System.Collections.Generic;
using System.Cor3.Data;
using System.Data.SqlClient;

namespace System.Cor3.Data.Engine
{
	public class SqlServerDb : SqlDbA
	{
		/// <summary>
		/// the input string consists of comma delimited values for:
		/// Datasource (name), Table (name)
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		static public SqlServerDb FromString(string input)
		{
			string[] dss = input.Split(',');
			List<string> list = new List<string>();
			foreach (string s in dss) list.Add(s.Trim());
			// we could just unmark SqlDbA from being abstract and all.
			SqlServerDb db = new SqlServerDb(dss.Length >= 1 ? list[0] : "", dss.Length >= 2 ? list[1] : "");
			return db;
			
		}
		public SqlServerDb():base()
		{
			
		}
		public SqlServerDb(string source, string tablename) : base(source,tablename)
		{
			
		}
	}
}
