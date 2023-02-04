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
using System.Data;
using System.Data.SQLite;
namespace System.Cor3.Data.Engine
{
	public class SQLiteQuery : IDisposable
	{
//		const string table = "sqlitetable";

		/// <summary>
		/// setup the connection using a datafile (path to the file)
		/// </summary>
		/// <param name="dataFile"></param>
		/// <returns></returns>
		static public implicit operator SQLiteQuery(string dataFile)
		{
			return new SQLiteQuery(dataFile);
		}
		
		#region Properties
		
		string databaseFile { get; set; }
		public SQLiteDb database { get; set; }
		public SQLiteConnection Connection { get; set; }
		public SQLiteDataAdapter Adapter { get; set; }
		
		#endregion

		public SQLiteQuery(string sqliteDatabaseFilePath)
		{
			databaseFile = sqliteDatabaseFilePath;
			database = new SQLiteDb(sqliteDatabaseFilePath);
		}
		
		#region Exception Information
		public bool HasError = false;

		public Exception Error = null;
		#endregion

		#region Query Methods
		public void ExecuteInsert(string query, Action<SQLiteCommand> setParams)
		{
			using (Connection = database.Connection)
				using (Adapter = database.Adapter)
					using (Adapter.InsertCommand = new SQLiteCommand(query)) {
						Connection.Open();
						try {
							if (setParams != null)
								setParams(Adapter.InsertCommand);
							Adapter.InsertCommand.ExecuteNonQuery();
						}
						catch (Exception e) {
							HasError = true;
							Error = e;
						}
						finally {
							Connection.Close();
						}
					}
		}

		public void ExecuteUpdate(string query, Action<SQLiteCommand> setParams)
		{
			using (Connection = database.Connection)
				using (Adapter = database.Adapter)
					using (Adapter.UpdateCommand = new SQLiteCommand(query)) {
						Connection.Open();
						try {
							if (setParams != null)
								setParams(Adapter.UpdateCommand);
							Adapter.UpdateCommand.ExecuteNonQuery();
						}
						catch (Exception e) {
							HasError = true;
							Error = e;
						}
						finally {
							Connection.Close();
						}
					}
		}

		public void ExecuteDelete(string query, Action<SQLiteCommand> setParams)
		{
			using (Connection = database.Connection)
				using (Adapter = database.Adapter)
					using (Adapter.DeleteCommand = new SQLiteCommand(query)) {
						Connection.Open();
						try {
							if (setParams != null)
								setParams(Adapter.DeleteCommand);
							Adapter.DeleteCommand.ExecuteNonQuery();
						}
						catch (Exception e) {
							HasError = true;
							Error = e;
						}
						finally {
							Connection.Close();
						}
					}
		}

		public DataSet ExecuteSelect(string query, string table)
		{
			DataSet ds = new DataSet();
			ds.Tables.Add(table);
			using (Connection = database.Connection)
				using (Adapter = database.Adapter)
					using (Adapter.SelectCommand = new SQLiteCommand(query)) {
						Connection.Open();
						try {
							Adapter.SelectCommand.ExecuteNonQuery();
							Adapter.Fill(ds, table);
						}
						catch (Exception e) {
							HasError = true;
							Error = e;
						}
						finally {
							Connection.Close();
						}
					}
			return ds;
		}
		#endregion
		
		#region Disposal
		/// <summary>
		/// when disposed, we clear everything
		/// </summary>
		public void Dispose()
		{
			this.HasError = false;
			this.Error = null;
			this.databaseFile = null;
			database.Dispose();
			database = null;
		}
		#endregion
	
	}
}


