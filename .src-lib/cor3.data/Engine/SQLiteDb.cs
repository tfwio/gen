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
		const string table = "sqlitetable";
		string databaseFile { get;set; }
		
		public SQLiteDb database { get;set; }
		public SQLiteConnection Connection { get;set; }
		public SQLiteDataAdapter Adapter { get;set; }
		
		public SQLiteQuery(string sqliteDatabaseFilePath)
		{
			databaseFile = sqliteDatabaseFilePath;
			database = new SQLiteDb(sqliteDatabaseFilePath);
		}
		
		public bool HasError = false;
		public Exception Error = null;
		
		public void ExecuteInsert(string query, Action<SQLiteCommand> setParams)
		{
			using (Connection = database.Connection)
			using (Adapter = database.Adapter)
			using (Adapter.InsertCommand = new SQLiteCommand(query))
			{
				Connection.Open();
				try {
					if (setParams != null) setParams(Adapter.InsertCommand);
					Adapter.InsertCommand.ExecuteNonQuery();
				}
				catch (Exception e)
				{
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
			using (Adapter.UpdateCommand = new SQLiteCommand(query))
			{
				Connection.Open();
				try {
					if (setParams != null) setParams(Adapter.UpdateCommand);
					Adapter.UpdateCommand.ExecuteNonQuery();
				}
				catch (Exception e)
				{
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
			using (Adapter.DeleteCommand = new SQLiteCommand(query))
			{
				Connection.Open();
				try {
					if (setParams != null) setParams(Adapter.DeleteCommand);
					Adapter.DeleteCommand.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					HasError = true;
					Error = e;
				}
				finally {
					Connection.Close();
				}
			}
		}
	
		public DataSet ExecuteSelect(string query, string tablename)
		{
			DataSet ds = new DataSet();
			ds.Tables.Add(tablename);
			
			using (Connection = database.Connection)
			using (Adapter = database.Adapter)
			using (Adapter.SelectCommand = new SQLiteCommand(query))
			{
				Connection.Open();
				try {
					Adapter.SelectCommand.ExecuteNonQuery();
					Adapter.Fill(ds,table);
				}
				catch (Exception e)
				{
					HasError = true;
					Error = e;
				}
				finally {
					Connection.Close();
				}
			}
			return ds;
		}
		
		/// <summary>
		/// when disposed, we clear everything
		/// </summary>
		public void Dispose()
		{
			this.HasError = false;
			this.Error  = null;
			this.databaseFile = null;
			database.Dispose();
			database = null;
		}
	}
	/// <summary>
	/// see: http://www.connectionstrings.com/sqlite
	/// </summary>
	public class SQLiteDb : DataAbstract<SQLiteConnection, SQLiteCommand, SQLiteDataAdapter, SQLiteParameter>
	{
		#region SQLite Keywords
		
		const string kw = @"ABORT
ACTION
ADD
AFTER
ALL
ALTER
ANALYZE
AND
AS
ASC
ATTACH
AUTOINCREMENT
BEFORE
BEGIN
BETWEEN
BY
CASCADE
CASE
CAST
CHECK
COLLATE
COLUMN
COMMIT
CONFLICT
CONSTRAINT
CREATE
CROSS
CURRENT_DATE
CURRENT_TIME
CURRENT_TIMESTAMP
DATABASE
DEFAULT
DEFERRABLE
DEFERRED
DELETE
DESC
DETACH
DISTINCT
DROP
EACH
ELSE
END
ESCAPE
EXCEPT
EXCLUSIVE
EXISTS
EXPLAIN
FAIL
FOR
FOREIGN
FROM
FULL
GLOB
GROUP
HAVING
IF
IGNORE
IMMEDIATE
IN
INDEX
INDEXED
INITIALLY
INNER
INSERT
INSTEAD
INTERSECT
INTO
IS
ISNULL
JOIN
KEY
LEFT
LIKE
LIMIT
MATCH
NATURAL
NO
NOT
NOTNULL
NULL
OF
OFFSET
ON
OR
ORDER
OUTER
PLAN
PRAGMA
PRIMARY
QUERY
RAISE
REFERENCES
REGEXP
REINDEX
RELEASE
RENAME
REPLACE
RESTRICT
RIGHT
ROLLBACK
ROW
SAVEPOINT
SELECT
SET
TABLE
TEMP
TEMPORARY
THEN
TO
TRANSACTION
TRIGGER
UNION
UNIQUE
UPDATE
USING
VACUUM
VALUES
VIEW
VIRTUAL
WHEN
WHERE";
		#endregion
		
		/// <inheritdoc/>
		protected internal override string data_id { get { return "SqlLite"; } }
		protected DataSet globalData = null;
		/// <inheritdoc/>
		public override DataSet GlobalData { get { return globalData; } protected set { globalData = value; } }
		
		DictionaryList<string,string> parameters = new DictionaryList<string,string>();
		public override DictionaryList<string,string> QueryParams { get { return parameters; } }
		
		#region Constantly
		// New=False;Compress=True
		const string cstring_attach = @"Data Source=$(AttachDbFileName);Version=3;";
		// Data Source=OOO\SQL2005EXPRESS
		// AttachDbFileName="d:\dev\lip_data.mdf"
		//		const string cstring_attach = @"
		//	Data Source=$(DataSource);
		//	AttachDbFilename=""$(AttachDbFileName)"";
		//	Integrated Security=True;
		//	Connect Timeout=30";
		//	User=$(UserName);
		//		const string cstring = @"
		//	Integrated Security=SSPI;
		//	Persist Security Info=False;
		//	Database=$(InitialCatalog);
		//	Server=$(DataSource)
		//";
		#endregion
		
		protected string dsource=string.Empty, dconnect=cstring_attach, dcatalog, username;
		int lastRecordsEffected = -1;
		/// <inheritdoc/>
		public override int LastRecordsAffected { get { return lastRecordsEffected; } set { lastRecordsEffected = value; } }
		
		protected string DataCatalog { get { return dcatalog; } set { dcatalog=value; } }
		/// <inheritdoc/>
		public override string DataSource { get { return dsource; }  }
		//		protected bool LoadFile = false;
		/// <inheritdoc/>
		public override string ConnectionString {
			get {
				return
					//LoadFile ?
					cstring_attach
					//					.Replace("$(InitialCatalog)",DataCatalog)
						.Replace("$(AttachDbFileName)",DataSource)
						;
				//				cstring
				//					.Replace("$(UserName)",username)
				//					.Replace("$(InitialCatalog)",DataCatalog)
				//					.Replace("$(DataSource)",DataSource);
			}
		}
		/// <inheritdoc/>
		public override int DefaultFill(SQLiteDataAdapter A, DataSet D, string tablename)
		{
			return A.Fill(D,tablename);
		}
		public SQLiteDb()
		{
			//			globalData = new DataSet(data_id);
		}
		public SQLiteDb(string source) : this()
		{
			this.dsource = source;
			//			this.dcatalog = table;
		}
		
		
		/// <inheritdoc/>
		public override SQLiteConnection Connection { get { return new SQLiteConnection(ConnectionString); } }
		/// <inheritdoc/>
		public override SQLiteDataAdapter Adapter { get { return new SQLiteDataAdapter(); } }
	}
}
