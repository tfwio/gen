/* oio : 03/10/2014 00:34 */
using System;
using System.Cor3.Data.Engine;
using System.Data.SQLite;
namespace GeneratorTool.SQLiteUtil
{
	class DataRunner
	{
		public string SqlQuery {
			get { return sqlQuery; }
			set { sqlQuery = value; }
		} string sqlQuery;
		
		public string SqlFile {
			get { return sqlFile; }
			set { sqlFile = value; }
		} string sqlFile;
		
		public bool HasSqlFile {
			get { return hasSqlFile; }
			set { hasSqlFile = value; }
		} bool hasSqlFile = false;
		
		public bool HasQuery {
			get { return hasQuery; }
			set { hasQuery = value; }
		} bool hasQuery = false;
		
		public bool CanExecute {
			get { return canExecute; }
			set { canExecute = value; }
		} bool canExecute = false;
		
		public void Create()
		{
			var loader = new DataFileLoader{};
			loader.Load();
			loader = null;
		}
		
		public void Execute(){
			
			if (!canExecute) return;
			
			int recordsAffected = -1;
			
			using (SQLiteDb db = new SQLiteDb(sqlFile))
			using (SQLiteConnection c = db.Connection)
			using (SQLiteDataAdapter a = db.Adapter)
			{
//				db.Insert(this.sqlQuery, delegate() {});
			}
			
		}
		
	}
}


