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
using System.Cor3.Data.Engine;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;

#endregion

namespace Generator.Data
{
	public class SqlDataFactory : BasicDataFactory<SqlConnection,SqlCommand,SqlDataAdapter,SqlParameter>
	{
		public class SQLDataContext : QueryDataContext
		{
			public string connection;
			public string catalog;
			public SQLDataContext(string connection, string catalog)
			{
				this.connection = connection;
				this.catalog = catalog;
			}
		}
		
		public override void Create(string filename)
		{
			base.Create(filename);
			DataContext = new SQLDataContext(@"VAIO\SqlExpress","prime");
		}
		public Array GetDataNativeItemTypes(){ throw new NotImplementedException(); }
		#region Server-Connection/Schema
		static SqlServerDb NewSqlServerDb= new SqlServerDb(@"VAIO\SqlExpress","prime");
		/// <summary>SqlServer specific translation.</summary>
		/// <param name="c">SqlServerDataConnection.</param>
		/// <param name="ds">A Dataset (possibly) containing schema-information.</param>
		/// <param name="name">The name of the table.</param>
		static public void TableToDataset(SqlConnection c, DataSet ds, string name)
		{
			DataTable table = c.GetSchema(name);
			table.TableName = name;
			ds.Tables.Add(table);
		}
		public const string db_sql2005			= "mssql2005";
		static public DataSet GetSqlServerSchemas(SqlDataFactory.SQLDataContext info)
		{
			DataSet ds = new DataSet(db_sql2005);
			bool check = false;
			using (SqlServerDb sqldb = NewSqlServerDb)
			{
				using (SqlConnection c = sqldb.Connection) {
					try { c.Open(); c.Close(); } catch { check = true; }
				} if (check) { ErrorMessage.Show("Couldn't do it"); return null; }
				
				// retrieves the schema information
				using (SqlConnection c = sqldb.Connection)
				{
					using (SqlDataAdapter a = new SqlDataAdapter())
					{
						c.Open();
						try {
							TableToDataset(c,ds,Gen.Strings.Schema_Indexes);
							TableToDataset(c,ds,Gen.Strings.Schema_Tables);
							TableToDataset(c,ds,Gen.Strings.Schema_Views);
							TableToDataset(c,ds,Gen.Strings.Schema_Procedures);
							TableToDataset(c,ds,Gen.Strings.Schema_Columns);
							TableToDataset(c,ds,Gen.Strings.Schema_DataTypes);
							TableToDataset(c,ds,Gen.Strings.Schema_Restrictions);
							TableToDataset(c,ds,Gen.Strings.Schema_ReservedWords);
						} catch (Exception) {
							
							throw;
						};
						c.Close();
					}
				}
			}
			return ds;
		}
		#endregion
	}
}
