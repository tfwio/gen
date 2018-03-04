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
	public class AccessDataFactory : BasicDataFactory<OleDbConnection,OleDbCommand,OleDbDataAdapter,OleDbParameter>{
		
		public const string ole_ace12			= "ace12";
		public class AccessDataContext : QueryDataContext
		{
			public string source;
			public string name;
			public AccessDataContext(string source, string name)
			{
				this.source = source;
				this.name = name;
			}
		}
		
		public override void Create(string filename)
		{
			this.DataContext = new AccessDataContext(filename,null);
			Database = new Access10(filename);
		}
		public Array GetDataNativeItemTypes(){ throw new NotImplementedException(); }
		
		#region Schema Info
		
		static public DataSet GetSchemas(AccessDataContext info)
		{
			DataSet ds = new DataSet(ole_ace12);
			using (Access10 acedb = new Access10(info.source))
			{
				bool failedConnecting = false;
				using (OleDbConnection c = acedb.Connection)
				{
					try { c.Open(); c.Close(); } catch { failedConnecting = true; }
				}
				if (failedConnecting)
				{
					ErrorMessage.Show("Couldn't do it");
					return null;
				}
				using (OleDbConnection c = acedb.Connection)
					using (OleDbDataAdapter a = new OleDbDataAdapter())
				{
					c.Open();
					try {
						Tds(c,ds);
					} catch (Exception) {
						throw;
					};
					c.Close();
				}
			}
			return ds;
		}
		static void Tds(OleDbConnection c, DataSet ds)
		{
			TableToDataset(c,ds,Gen.Strings.Schema_Indexes);
			TableToDataset(c,ds,Gen.Strings.Schema_Tables);
			TableToDataset(c,ds,Gen.Strings.Schema_Views);
			TableToDataset(c,ds,Gen.Strings.Schema_Procedures);
			TableToDataset(c,ds,Gen.Strings.Schema_Columns);
			TableToDataset(c,ds,Gen.Strings.Schema_DataTypes);
			TableToDataset(c,ds,Gen.Strings.Schema_Restrictions);
			TableToDataset(c,ds,Gen.Strings.Schema_ReservedWords);
		}
		/// <summary>Access (OleDB) specific translation</summary>
		/// <param name="c">SqlServerDataConnection.</param>
		/// <param name="ds">A Dataset (possibly) containing schema-information.</param>
		/// <param name="name">The name of the table.</param>
		static public void TableToDataset(OleDbConnection c, DataSet ds, string name)
		{
			DataTable table = c.GetSchema(name);
			table.TableName = name;
			ds.Tables.Add(table);
		}
		#endregion
	}

}
