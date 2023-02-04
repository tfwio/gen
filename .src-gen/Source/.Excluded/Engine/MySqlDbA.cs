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
#if USEMYSQL
/*
 * User: oIo
 * Date: 11/15/2010 – 2:49 AM
 */
using System;
using System.Cor3.Data.Engine;
using System.Cor3.Data;

using MySql.Data.MySqlClient;

namespace System.Cor3.Data.Engine
{
	/// <summary>
	/// this abstraction is intended for mysql databsae connections.
	/// </summary>
	public abstract class MySqlDbA : IDataAbstraction<MySqlConnection, MySqlCommand, MySqlDataAdapter/*, MySqlParameters*/>,IDbAbstraction
		
	{
		DICT_List<string,string> parameters = new DICT_List<string,string>();
		public DICT_List<string,string> QueryParams { get { return parameters; } }

		int lastRecordsEffected = -1;
		int IDbAbstraction.LastRecordsAffected { get { return lastRecordsEffected; } set { lastRecordsEffected = value; } }

		const string cstring = cstrings.MICROSOFT_ACE_OLEDB_12_0;
		string dsource=string.Empty, dconnect=string.Empty;

		string IDbAbstraction.DataSource { get { return dsource; }  }
		string IDbAbstraction.ConnectionString { get { return DataSource; } }

		protected override int DefaultFill(SQLiteDataAdapter A, DataSet D, string tablename)
		{
			return A.Fill(D,tablename);
		}
		
		protected abstract string DataSource { get; }
		protected abstract string ConnectionString { get; }

		abstract public MySqlConnection Connection { get; }
		abstract public MySqlDataAdapter Adapter { get; }
	}
}
#endif