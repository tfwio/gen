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
using System.Data;

namespace System.Cor3.Data
{
	using IDbCommand = System.Data.IDbCommand;
	using IDbConnection = System.Data.IDbConnection;
	using IDbDataAdapter = System.Data.IDbDataAdapter;
	
	#region Enum
	/// <summary>
	/// These flags are/were used as a result per
	/// query parser.
	/// <para>Parser would attempt to detect
	/// the type of queries embedded in the script.
	/// </para>
	/// <para>Also, this is used for common Engine-Specific
	/// flags per query execution context.</para>
	/// </summary>
	[Flags]
	public enum DbOp
	{
		Undefined = 0,
		Delete,
		Insert,
		Select,
		Update
	}
	#endregion
	
	#region IFace
	public interface IDataAbstraction<TConnection,TCommand,TAdapter,TParameter> : IDbAbstraction
		where TConnection:IDbConnection
		where TCommand:IDbCommand
		where TAdapter:IDbDataAdapter
		where TParameter:IDbDataParameter
	{
		DataSet GlobalData { get; }
		TConnection Connection { get; }
		TAdapter Adapter { get; }
		DictionaryList<string,string> QueryParams { get; }
	}
	public interface IDbAbstraction
	{
		// for example: ‘d:/dev/adb.mdb’
		string DataSource { get; }
		string ConnectionString { get; }
		int LastRecordsAffected { get; set; }
	}
	#endregion
	
	#region RowParamCaller
	/// <summary>
	/// This is a base class—containing delegates or callbacks for use in it's
	/// derived classes such as the <see cref="DataAbstract<TConnection,TCommand,TAdapter,TParameter>" />
	/// class.
	/// </summary>
	public abstract class RowParamCaller<TConnection,TCommand,TAdapter,TParameter>
	{
		#region delegate
		public delegate void		CBQueryParameters(string query, params TParameter[] dataParams);
		/// <summary>
		/// Provides IDbDataAdapter for SQL Query.
		/// </summary>
		public delegate TAdapter	CBRowParam(DbOp op, string query, TConnection connection);
		/// <summary>
		/// used to parameterize a command.
		/// </summary>
		public delegate TCommand	CBParams(StatementType ExecutionContext, TCommand cmd);
		/// <summary>
		/// Database Operation.  I may as well have left this blank.  I forgot!
		/// </summary>
		public delegate DataSet		CBDataOp(CBDataFill cbf, CBRowParam cbr, CBParams cbp);
		public delegate int			CBDataFill(TAdapter A, DataSet D, string tablename);
		#endregion
	}
	
	#endregion
	
	#region DataAbstractBuilder
	/// <summary>
	/// An abstract class which provides properties to it's derived classes (such as
	/// <see cref="DataAbstract<TConnection,TCommand,TAdapter,TParameter>" />).
	/// </summary>
	public abstract class DataAbstractBuilder<TConnection,TCommand,TAdapter,TParameter> :
		RowParamCaller<TConnection,TCommand,TAdapter,TParameter>
		where TConnection:IDbConnection
		where TCommand:IDbCommand, new()
		where TAdapter:IDbDataAdapter,IDisposable,new()
		where TParameter:IDbDataParameter
	{
		abstract public string DataSource { get; }
		abstract public string ConnectionString { get; }
		abstract public int LastRecordsAffected { get; set; }
	}
	#endregion
	
	#region DataAbstract
	/// <summary>
	/// This is the main (abstract) base-class for <see cref="System.Cor3.Data.Engine" />
	/// classes and data-execution-contexts.
	/// </summary>
	/// <remarks>
	/// as of 2012-04-23, measures have been taken to prevent DOUBLE-INSERTIONS
	/// which was caused by improper (deriving) class impl(s) and frankly, not
	/// documenting much of the materials built on this set of classes.
	/// </remarks>
	public abstract class DataAbstract<TConnection,TCommand,TAdapter,TParameter> :
		DataAbstractBuilder<TConnection,TCommand,TAdapter,TParameter>,
	IDataAbstraction<TConnection,TCommand,TAdapter,TParameter>,
	IDisposable
		where TConnection:IDbConnection
		where TCommand:IDbCommand, new()
		where TAdapter:IDbDataAdapter,IDisposable,new()
		where TParameter:IDbDataParameter
	{
		/// <summary>
		/// Set/Get the database-engine used for operations.
		/// </summary>
		internal protected abstract string data_id { get; }

		#region abstract
		/// <summary>
		/// The default <see cref="DataSet"/> for default usage when
		/// operations are performed.
		/// </summary>
		abstract public DataSet GlobalData { get; protected set; }
		/// <summary>
		/// Provides the <see cref="DataAbstract<TConnection,TCommand,TAdapter,TParameter>.TConnection" /> to methods during operations.
		/// <para>Note that the connection is disposed after each execution</para>
		/// </summary>
		abstract public TConnection Connection { get; }
		/// <summary>
		/// Provides the <see cref="DataAbstract<TConnection,TCommand,TAdapter,TParameter>.TAdapter" /> to methods during operations.
		/// <para>Note that the connection is disposed after each execution</para>
		/// </summary>
		abstract public TAdapter Adapter { get; }
		/// <summary>
		/// (Obsoletion Status)
		/// <para>
		/// It seems that this class is no longer used.
		/// </para>
		/// <para>
		/// It was once a part of the original Database-Class Operations, and
		/// is no longer actively used (however it's here because a few old projects
		/// haven't been deleted yet which MAY make use of this.
		/// </para>
		/// </summary>
		abstract public DictionaryList<string, string> QueryParams { get; }
		#endregion
		#region IDisposable
		virtual public void Dispose()
		{
			if (GlobalData!=null)
			{
				GlobalData.Tables.Clear();
				GlobalData.Clear();
				GlobalData.Dispose();
				GlobalData = null;
			}
		}
		#endregion

		#region Fill
		/// <summary>
		/// A default fill operation.
		/// <seealso cref="RowParamCaller<TConnection,TCommand,TAdapter,TParameter>.CBRowParam" />
		/// </summary>
		/// <param name="A"></param>
		/// <param name="D"></param>
		/// <param name="tablename"></param>
		/// <returns></returns>
		virtual public int DefaultFill(TAdapter A, DataSet D, string tablename)
		{
			return A.Fill(D);
		}
		#endregion
		#region adapt
		virtual public TAdapter DefaultSelectAdapter(DbOp op, string query, TConnection connection)
		{
			return new TAdapter(){ SelectCommand=new TCommand(){ CommandText=query, Connection=connection} };
		}
		#endregion
		#region ins
		/// <summary>
		/// 
		/// </summary>
		/// <param name="query"></param>
		/// <param name="S">delegate(DbOp op, string query, TConnection connection)</param>
		/// <returns></returns>
		public DataSet Insert(string query, CBRowParam S)
		{
			return Insert(query: query, S: S, Fill: DefaultFill);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="table"></param>
		/// <param name="query"></param>
		/// <param name="S">delegate(DbOp op, string query, TConnection connection)</param>
		/// <returns></returns>
		public DataSet Insert(string table, string query, CBRowParam S)
		{
			return Insert(table,query,S,DefaultFill);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="query"></param>
		/// <param name="S">delegate(DbOp op, string query, TConnection connection)</param>
		/// <param name="Fill"></param>
		/// <returns></returns>
		public DataSet Insert(string query, CBRowParam S, CBDataFill Fill)
		{
			return Insert(null,query,S,Fill);
		}
		/// <summary>
		/// This executes two separate queries if they're present, however it explicity
		/// sets the insert operation's query.
		/// <para>The actual select statement has to be set up through the RowParamCallback,
		/// or through using InsertSelect(table,queryinsert,queryselect,…</para>
		/// </summary>
		/// <param name="table">Applied to DataSet.Table.Name property.</param>
		/// <param name="query">insert statement</param>
		/// <param name="S">delegate(DbOp op, string query, TConnection connection)</param>
		/// <param name="Fill">delegate(TAdapter A, DataSet D, string tablename)</param>
		/// <returns></returns>
		public DataSet Insert(string table, string query, CBRowParam S, CBDataFill Fill)
		{
			DataSet ds = new DataSet(data_id??"");
			using (TConnection C = this.Connection)
			{
				using (TAdapter A = S(DbOp.Insert, query, C))
				{
					C.Open();
					try
					{
						if ((A.InsertCommand != null) && !string.IsNullOrEmpty(A.InsertCommand.CommandText)) A.InsertCommand.ExecuteNonQuery();
					}
					catch (Exception e) { System.Diagnostics.Debug.Assert(false,e.Message,e.Source); }
					try
					{
						if ((A.SelectCommand != null) && !string.IsNullOrEmpty(A.SelectCommand.CommandText))
						{
							A.SelectCommand.ExecuteNonQuery();
							Fill(A,ds,table);
						}
					}
					catch (Exception e) { System.Diagnostics.Debug.Assert(false,e.Message,e.Source); }
					C.Close();
				}
			}
//			MessageBox.Show();
//			ds.Tables[0].Rows.Count
			return ds;
		}
		
		/// <summary>
		/// See Insert function for reference.
		/// </summary>
		/// <returns></returns>
		public DataSet InsertSelect(string table, string queryInsert, string querySelect, CBRowParam S, CBDataFill Fill)
		{
			DataSet ds = new DataSet(data_id??"");
			using (TConnection C = this.Connection)
			{
				using (TAdapter A = S(DbOp.Insert, queryInsert, C))
				{
					C.Open();
					try
					{
						if (A.InsertCommand != null) A.InsertCommand.ExecuteNonQuery();
					}
					catch { // (Exception e) {
						System.Diagnostics.Debug.Assert(false,"Insert stement failed.");
					}
					try
					{
						if ((A.SelectCommand != null) && !string.IsNullOrEmpty(A.SelectCommand.CommandText))
						{
							A.SelectCommand.ExecuteNonQuery();
							Fill(A,ds,table);
						}
						Fill(A,ds,table);
					}
					catch (Exception e) {
						System.Diagnostics.Debug.Assert(false,e.Message,e.Source);
					}
					C.Close();
				}
			}
			return ds;
		}
		#endregion
		#region del
		/// <summary>
		/// 
		/// </summary>
		/// <param name="table"></param>
		/// <param name="query"></param>
		/// <param name="S">delegate(DbOp op, string query, TConnection connection)</param>
		/// <param name="Fill">delegate(TAdapter A, DataSet D, string tablename)</param>
		/// <returns></returns>
		public DataSet Delete(string table, string query, CBRowParam S, CBDataFill Fill)
		{
			DataSet ds = new DataSet(data_id??"");
			using (TConnection C = this.Connection)
			{
				using (TAdapter A = S(DbOp.Delete,query,C))
				{
					C.Open();
					if (A.DeleteCommand != null) A.DeleteCommand.ExecuteNonQuery();
					try
					{
						if ((A.SelectCommand != null) && !string.IsNullOrEmpty(A.SelectCommand.CommandText))
						{
							A.SelectCommand.ExecuteNonQuery();
							Fill(A,ds,table);
						}
					}
					catch(Exception error)
					{
						#if DEBUG
						System.Diagnostics.Debug.Print("Select Command from within a DELETE command.");
						throw error;
						#endif
					}
					C.Close();
				}
			}
			return ds;
		}

		#endregion
		#region upd
		/// <summary>
		/// 
		/// </summary>
		/// <param name="query"></param>
		/// <param name="S">delegate(DbOp op, string query, TConnection connection)</param>
		/// <returns></returns>
		public DataSet Update(string query, CBRowParam S)
		{
			return Update(query,S,DefaultFill);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="table"></param>
		/// <param name="query"></param>
		/// <param name="S">delegate(DbOp op, string query, TConnection connection)</param>
		/// <returns></returns>
		public DataSet Update(string table, string query, CBRowParam S)
		{
			return Update(table,query,S,DefaultFill);
		}
		public DataSet Update(string query, CBRowParam S, CBDataFill Fill)
		{
			return Update(null,query,S,Fill);
		}
		/// <summary>
		/// Executes the provided Comands, which may include a INSERT Command.
		/// If no insert command is present, the Data-Fill operation is ignored.
		/// </summary>
		/// <param name="table"></param>
		/// <param name="query"></param>
		/// <param name="S">delegate(DbOp op, string query, TConnection connection)</param>
		/// <param name="Fill">delegate(TAdapter A, DataSet D, string tablename)</param>
		/// <returns></returns>
		public DataSet Update(string table, string query, CBRowParam S, CBDataFill Fill)
		{
			//Logger.LogG("DataAbstract.Update",query);
			DataSet ds = new DataSet(data_id??"");
			using (TConnection C = this.Connection)
			{
				using (TAdapter A = S(DbOp.Update,query,C))
				{
					C.Open();
					try
					{
						A.UpdateCommand.ExecuteNonQuery();
						Fill(A,ds,table);
					}
					catch (Exception e)
					{
						#if DEBUG
						System.Diagnostics.Debug.Print("Update ERROR: '{0}'",A.UpdateCommand.CommandText);
						foreach (IDbDataParameter p in A.UpdateCommand.Parameters)
							System.Diagnostics.Debug.Print("Param: '{0}', ToString = \"{1}\"", p.ParameterName, p);
						//Logger.Warn("DataAbstract.Update: ERROR!","\n--------------\n{0}\n--------------\n",e.ToString());
						throw e;
						#endif
					}
					try
					{
						if ((A.SelectCommand != null) && !String.IsNullOrEmpty(A.SelectCommand.CommandText) )
						{
							A.SelectCommand.ExecuteNonQuery();
							Fill(A,ds,table);
						}
					}
					catch (Exception e)
					{
						#if DEBUG
						System.Diagnostics.Debug.Print("Select command-text: '{0}'",A.SelectCommand.CommandText);
						//Logger.Warn("DataAbstract.Select (Via Update Method): ERROR!","\n--------------\n{0}\n--------------\n",e.ToString());
						throw e;
						#endif
					}
					C.Close();
				}
			}
			return ds;
		}
		#endregion
		#region sel
		/// <summary>
		/// 
		/// </summary>
		/// <param name="query"></param>
		/// <param name="S">delegate(DbOp op, string query, TConnection connection)</param>
		/// <returns></returns>
		public DataSet Select(string query, CBRowParam S)
		{
			return Select(query,S,DefaultFill);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="table"></param>
		/// <param name="query"></param>
		/// <param name="S">delegate(DbOp op, string query, TConnection connection)</param>
		/// <returns></returns>
		public DataSet Select(string table, string query, CBRowParam S)
		{
			return Select(table,query,S,DefaultFill);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="query"></param>
		/// <param name="S">delegate(DbOp op, string query, TConnection connection)</param>
		/// <param name="Fill">delegate(TAdapter A, DataSet D, string tablename)</param>
		/// <returns></returns>
		public DataSet Select(string query, CBRowParam S, CBDataFill Fill)
		{
			return Select(null,query,S,Fill);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="table"></param>
		/// <param name="query"></param>
		/// <param name="S">delegate(DbOp op, string query, TConnection connection)</param>
		/// <param name="Fill">delegate(TAdapter A, DataSet D, string tablename)</param>
		/// <returns></returns>
		public DataSet Select(string table, string query, CBRowParam S, CBDataFill Fill)
		{
			DataSet ds = new DataSet(data_id??"");
			using (TConnection C = this.Connection)
				using (TAdapter A = S(DbOp.Select,query,C))
			{
				C.Open();
				A.SelectCommand.ExecuteNonQuery();
				Fill(A,ds,table);
				C.Close();
			}
			return ds;
		}
		
		#endregion
		
		public DataAbstract()
		{
		}
		public DataAbstract(string source)
		{
		}
		public DataAbstract(string source, string table)
		{
		}
	}
	#endregion
	
}
#region Big Comment
/*
	public class DataUtils
	{
		class DataOperation {
			DbOp operation = DbOp.Undefined;
			string initialTable, source, query;
			string del, ins, upd, sel;
			public DataOperation(string ins, string upd, string sel, string del, string source)
			{
				this.ins = ins;
				this.upd = upd;
				this.sel = sel;
				this.del = del;
			}
			public DataOperation(string ins, string upd, string sel, string del, string source, string table)
			{
				this.ins = ins;
				this.upd = upd;
				this.sel = sel;
				this.del = del;
			}
			public DataOperation(DbOp o, string query, string source, string table) : this(o,query,source)
			{
				this.initialTable = table;
			}
			public DataOperation(DbOp o, string query, string source)
			{
				this.operation = o;
				this.source = source;
				this.query  = query;
				switch (o)
				{
					case DbOp.Insert: this.ins = query; break;
					case DbOp.Delete: this.del = query; break;
					case DbOp.Update: this.upd = query; break;
					case DbOp.Select: this.sel = query; break;
				}
			}
		}
		static int op_select = DbOp.Select;
		static int op_insert = DbOp.Insert;
		static int op_update = DbOp.Update;
		static int op_delete = DbOp.Delete;
		
		bool CheckOp(DbOp o, DbOp match) { return CheckOp((int)o, (int)match); }
		bool CheckOp(int o, int match) { if (o & match == match) { return true; } return false; }
		delegate void Parameterize(int op, SqlCommand cmd);
		delegate void Parameterize(int op, SQLiteCommand cmd);
		delegate void Parameterize(int op, OleDbCommand cmd);
//		delegate void Parameterize(MySQLCommand cmd);
		static public DataSet GetData(
				DatabaseType t,
				DbOp o,
				string table,
				string initialrecord,
				string query,
				Parameterize getParams
				)
		{
			DataSet ds = new DataSet("System.Cor3.Data.GetData");
			ds.Tables.Add(table);
			int op = (int)o;
			if (t == DatabaseType.SqlServer)
			{
				SqlDbA dclass = new SqlDbA();
				using (SqlConnection C = dclass.Connection) {
					using (SqlDataAdapter A = dclass.Adapter) {

						if (CheckOp(o,op_select)) { A.SelectCommand = new SqlCommand(query,C); Parameterize(A.getParams); }
						if (CheckOp(o,op_update)) { A.UpdateCommand = new SqlCommand(query,C); Parameterize(A.getParams); }
						if (CheckOp(o,op_delete)) { A.DeleteCommand = new SqlCommand(query,C); Parameterize(A.getParams); }
						if (CheckOp(o,op_insert)) { A.InsertCommand = new SqlCommand(query,C); Parameterize(A.getParams); }
						
						C.Open();
						if (CheckOp(o,op_select)) { A.SelectCommand = new SqlCommand(query,C); Parameterize(A.getParams); }
						if (CheckOp(o,op_update)) { A.UpdateCommand = new SqlCommand(query,C); Parameterize(A.getParams); }
						if (CheckOp(o,op_delete)) { A.DeleteCommand = new SqlCommand(query,C); Parameterize(A.getParams); }
						if (CheckOp(o,op_insert)) { A.InsertCommand = new SqlCommand(query,C); Parameterize(A.getParams); }
						C.Close();
						
					}
				}
				dclass = null;
			}
			else if (t == DatabaseType.SQLite)
			{
				SqlLite sclass = new SqlLite();
				
				sclass = null;
			}
			else if (t == DatabaseType.OleAccess)
			{
				Access10 aclass = new Access10();
				
				aclass = null;
			}
		}
	}*/
#endregion
#region Little Comment
//		public delegate DataSet exec(string query, paramSettings parameterize);

//		public exec ExecuteInsert;
//		public exec ExecuteUpdate;
//		public exec ExecuteDelete;
//		public exec ExecuteSelect;
//
//		public void ConfigureCommands(exec del, exec upd, exec ins, exec sel)
//		{
//			ExecuteInsert = ins;
//			ExecuteUpdate = upd;
//			ExecuteDelete = del;
//			ExecuteSelect = sel;
//		}
#endregion
