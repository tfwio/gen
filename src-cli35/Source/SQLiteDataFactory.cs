/*
 * Created by SharpDevelop.
 * User: oIo
 * Date: 2/10/2011
 * Time: 9:52 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
#region Using
using System;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;

#endregion

namespace Generator.Data
{
	public class SQLiteDataFactory : BasicDataFactory<SQLiteConnection,SQLiteCommand,SQLiteDataAdapter,SQLiteParameter>
	{
		public class SQLiteDataContext : AccessDataFactory.AccessDataContext
		{
			public SQLiteDataContext(string source) : base(source,null)
			{
			}
		}
		
		public override void Create(string filename)
		{
			base.Create(filename);
			DataContext = new SQLiteDataContext(filename);
		}
		public Array GetDataNativeItemTypes(){ throw new NotImplementedException(); }
	}
}
