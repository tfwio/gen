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
 * Created by SharpDevelop.
 * User: oio
 * Date: 11/30/2011
 * Time: 14:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Cor3.Data.Engine;
using System.Data;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml.Serialization.Configuration;
using System.Xml.Serialization.Advanced;

namespace System.Cor3.Data.Context
{
	/// <summary>However useful, the parameterization method(s) used here is a bit
	/// out-dated (compared to other class-designs i've been working on).
	/// <para>Parameterization should in the future be provided by a callback
	/// method or Func&lt;bool,IDbCommand&gt; named Parameterize (or such)
	/// or the via callback with similar parameters, and for usfulness,
	/// return the IDbCommand being parameterized.</para>
	/// <para>Example: IDbCommand callback(IDbCommand cmd, bool hasPrimaryKey)</para>
	/// </summary>
	/// <remarks>
	/// This class is a little old and is a bit of OVERKILL!!!  But it does
	/// do it's job increasingly well in fact.
	/// Of 2012-04, many bugs have been cleaned up.
	/// <para>Note that all of the DataFill callbacks provided by this class
	/// (exception being SelectFill) return a value of -1.  This is to generally
	/// to prevent complex commands, however if you add a SELECT command,
	/// the data-fill operation MUST be supplied, otherwise no data-fill
	/// operation is invoked.</para>
	/// <para>2012-04: Debug-Console is now default output mechanism.</para>
	/// <para>2012-04: CONSOLE (debug) info is secondary if present.</para>
	/// </remarks>
	/// <seealso cref="QueryDatabaseContext" />
	/// <seealso cref="QueryContextInfo" />
	/// <seealso cref="QueryContext{TConnection,TCommand,TAdapter,TParameter}" />
	public abstract class QueryContext<TConnection, TCommand, TAdapter, TParameter> :
		
		QueryContext, IDisposable, IQueryContext2
		
		where TConnection : IDbConnection
		where TCommand : IDbCommand, new()
		where TAdapter : class,IDbDataAdapter, IDisposable, new()
		where TParameter : IDbDataParameter
	{
		/// <summary>
		/// A callbaack used to provide parameters to the (IDbCommand) <strong>TCommand</strong>.
		/// </summary>
		public delegate void TCommandParamsCallback(TCommand C);
		/// <summary>
		/// (document me again)
		/// A callback used to automate the generation of a query for execution.
		/// This is (probably) handled of or into <see cref="QueryContextInfo" />.
		/// </summary>
		public delegate void TQueryInfoCallback(TCommand C, string table, string field, DataRowView row);
//		static public IQueryContext2 Create()
//		{
//			return new QueryContext();
//		}
		/// <summary>
		/// Database-Context.
		/// Use for data-execution.
		/// </summary>
		[XmlIgnore] public DatabaseContext<TConnection, TCommand, TAdapter, TParameter> Context { get; set; }
		/// <summary>
		/// Context-Info.
		/// </summary>
		[XmlIgnore] public QueryDatabaseContext ContextInfo { get { return Context as QueryDatabaseContext; } }
		/// <summary>
		/// Query Context-Info.
		/// </summary>
		[XmlIgnore] public override QueryContextInfo ContextData { get { return Context as QueryContextInfo; } }
		
		/// <summary>
		/// A field marked as content.
		/// This field is automated with a paradigm requiring in many cases a field to associate with Content, and another associated with a Title or Alias.
		/// </summary>
		public string TableContent { get { return ContextInfo.TableContent; } }
		/// <summary>
		/// A field to be marked as alias.
		/// <para>This field is automated with a paradigm requiring in many cases a field to associate with Content, and another associated with a Title or Alias.</para>
		/// <para>Perhaps marked so that resulting rows can be referenced by alias (name) if needed.</para>
		/// </summary>
		/// <remarks>todo: supply adequate documentation for this field.</remarks>
		public string TableAlias { get { return ContextInfo.TableAlias; } set { ContextInfo.TableAlias = value; } }
		/// <summary>
		/// The name of the table as in the Database.
		/// </summary>
		public string TableName { get { return ContextInfo.TableName; } }
		
		/// <summary>
		/// 
		/// </summary>
		public string[] Fields { get { return ContextInfo.TableFields; } }
		public string[] TableGroups { get { return ContextInfo.TableGroups; } }
		
		
		#region Category Select Statements
		public abstract TAdapter InsertCategoryAdapter(DbOp op, string query, TConnection connection);
		public abstract int InsertCategoryFillOperation(TAdapter A, DataSet D, string tablename);
		
		public abstract TAdapter DeleteCategoryAdapter(DbOp op, string query, TConnection connection);
		public abstract int DeleteCategoryFillOperation(TAdapter A, DataSet D, string tablename);
		
		public abstract TAdapter UpdateCategoryAdapter(DbOp op, string query, TConnection connection);
		public abstract int UpdateCategoryFillOperation(TAdapter A, DataSet D, string tablename);
		
		public abstract TAdapter SelectCategoryAdapter(DbOp op, string query, TConnection connection);
		public abstract int SelectCategoryFillOperation(TAdapter A, DataSet D, string tablename);
		#endregion
		
		#region TQueryInfoCallback
		public DataAbstract<TConnection, TCommand, TAdapter, TParameter>.CBRowParam AdapterSelectAction { get; set; }
		public DataAbstract<TConnection, TCommand, TAdapter, TParameter>.CBRowParam AdapterInsertAction { get; set; }
		public DataAbstract<TConnection, TCommand, TAdapter, TParameter>.CBRowParam AdapterDeleteAction { get; set; }
		public DataAbstract<TConnection, TCommand, TAdapter, TParameter>.CBRowParam AdapterUpdateAction { get; set; }
		public virtual TAdapter AdapterSelect(DbOp op, string query, TConnection connection) { if (this.AdapterSelectAction!=null) return this.AdapterSelectAction.Invoke(op,query,connection); return (TAdapter)null; }
		/// <summary>
		/// This method is responsible for Parameterizing the Command.
		/// </summary>
		/// <param name="op"></param>
		/// <param name="query"></param>
		/// <param name="connection"></param>
		/// <returns></returns>
		public virtual TAdapter AdapterInsert(DbOp op, string query, TConnection connection) { if (this.AdapterInsertAction!=null) return this.AdapterInsertAction.Invoke(op,query,connection); return (TAdapter)null; }
		/// <summary>
		/// This method is responsible for Parameterizing the Command.
		/// </summary>
		/// <param name="op"></param>
		/// <param name="query"></param>
		/// <param name="connection"></param>
		/// <returns></returns>
		public virtual TAdapter AdapterDelete(DbOp op, string query, TConnection connection) { if (this.AdapterDeleteAction!=null) return this.AdapterDeleteAction.Invoke(op,query,connection); return (TAdapter)null; }
		/// <summary>
		/// This method is responsible for Parameterizing the Command.
		/// </summary>
		/// <param name="op"></param>
		/// <param name="query"></param>
		/// <param name="connection"></param>
		/// <returns></returns>
		public virtual TAdapter AdapterUpdate(DbOp op, string query, TConnection connection) { if (this.AdapterUpdateAction!=null) return this.AdapterUpdateAction.Invoke(op,query,connection); return (TAdapter)null; }
		#endregion
		#region TQueryInfoCallback
		/// <summary>
		/// Responsible for filling the dataset with the result-set.
		/// </summary>
		public DataAbstract<TConnection, TCommand, TAdapter, TParameter>.CBDataFill FillSelectAction { get; set; }
		/// <summary>
		/// Responsible for filling the dataset with the result-set.
		/// </summary>
		public DataAbstract<TConnection, TCommand, TAdapter, TParameter>.CBDataFill FillInsertAction { get; set; }
		/// <summary>
		/// Responsible for filling the dataset with the result-set.
		/// </summary>
		public DataAbstract<TConnection, TCommand, TAdapter, TParameter>.CBDataFill FillDeleteAction { get; set; }
		/// <summary>
		/// Responsible for filling the dataset with the result-set.
		/// </summary>
		public DataAbstract<TConnection, TCommand, TAdapter, TParameter>.CBDataFill FillUpdateAction { get; set; }
		/// <summary>
		/// Responsible for filling the dataset with the result-set.
		/// </summary>
		/// <returns>In stead of using the Data-Fill operation's result (the number of records affected),
		/// we count the number of records actually returned to the dataset.
		/// Some IDbDataAdapter providers do not always use the suggested semantics.</returns>
		/// <param name="A"></param>
		/// <param name="D"></param>
		/// <param name="tablename"></param>
		public virtual int FillSelect(TAdapter A, DataSet D, string tablename) { if (FillSelectAction!=null) return this.FillSelectAction.Invoke(A,D,tablename); else A.Fill(D); return D.Tables[TableName].Rows.Count; }
		/// <summary>
		/// Responsible for filling the dataset with the result-set.
		/// </summary>
		/// <returns>always -1.</returns>
		/// <param name="A"></param>
		/// <param name="D"></param>
		/// <param name="tablename"></param>
		public virtual int FillInsert(TAdapter A, DataSet D, string tablename) { if (FillInsertAction!=null) return this.FillInsertAction.Invoke(A,D,tablename); return -1; }
		/// <summary>
		/// Responsible for filling the dataset with the result-set.
		/// </summary>
		/// <returns>always -1.</returns>
		/// <param name="A"></param>
		/// <param name="D"></param>
		/// <param name="tablename"></param>
		public virtual int FillDelete(TAdapter A, DataSet D, string tablename) { if (FillDeleteAction!=null) return this.FillDeleteAction.Invoke(A,D,tablename); return -1; }
		/// <summary>
		/// Responsible for filling the dataset with the result-set.
		/// </summary>
		/// <returns>always -1.</returns>
		/// <param name="A"></param>
		/// <param name="D"></param>
		/// <param name="tablename"></param>
		/// <returns></returns>
		public virtual int FillUpdate(TAdapter A, DataSet D, string tablename) { if (FillUpdateAction!=null) return this.FillUpdateAction.Invoke(A,D,tablename); return -1; }
		#endregion

		public QueryContext(QueryContextInfo dbc): this (dbc,dbc.TableFile)
		{
		}
		public QueryContext() : this(null)
		{
		}
		public QueryContext(QueryContextInfo dqc, string db)
		{
			this.Context = new DatabaseContext<TConnection, TCommand, TAdapter, TParameter>();
			if (dqc!=null) this.Context.SetContext(dqc);
			datafile = db;
		}
		
		/// <summary>
		/// here, we'll clean up and info from the datasets.
		/// </summary>
		void IDisposable.Dispose()
		{
			#if DEBUG
			Logger.LogG("QueryContext","Disposing -- clearing data");
			#elif CONSOLE
			System.Diagnostics.Debug.Print("QueryContext","Disposing -- clearing data");
			#endif
			try
			{
				if (category!=null) this.ClearCategoryTable();
				if (data!=null) this.ClearDataTable();
			}
			catch (Exception error) {
				#if DEBUG
				System.Diagnostics.Debug.Print("Error Disposing QueryContext\n{0}",error.ToString());
				#endif
			}
		}
		
		/// <summary>
		/// Clear/Dispose <strong>category</strong> DataSet.
		/// </summary>
		internal void ClearCategoryTable()
		{
			if (category==null) return;
			if (category.Tables!=null) category.Tables.Clear();
			category.Clear(); category.Dispose(); category = null;
		}
		/// <summary>
		/// Clear all tables and Dispose the DataSet <strong>data</strong>.
		/// </summary>
		internal void ClearDataTable()
		{
			if (data==null) return;
			if (data.Tables!=null) category.Tables.Clear();
			data.Clear(); data.Dispose(); data = null;
		}
		#region no
		#if no
		/// <summary>
		/// This class isn't compiled due to pragma assertions therefore
		/// is not in any way implemented.
		/// </summary>
		public class Qub
		{
			#region Callback
			TSqlExecute InsertCategoryCB;
			SQLiteDb.CBRowParam InsertCategoryAdapterCB;
			SQLiteDb.CBDataFill InsertCategoryFillOperationCB;
			
			TSqlExecute DeleteCategoryCB;
			SQLiteDb.CBRowParam DeleteCategoryAdapterCB;
			SQLiteDb.CBDataFill DeleteCategoryFillOperationCB;
			
			TSqlExecute UpdateCategoryCB;
			SQLiteDb.CBRowParam UpdateCategoryAdapterCB;
			SQLiteDb.CBDataFill UpdateCategoryFillOperationCB;
			
			TSqlExecute SelectCategoryCB;
			SQLiteDb.CBRowParam SelectCategoryAdapterCB;
			SQLiteDb.CBDataFill SelectCategoryFillOperationCB;
			#endregion
			
		}
		#endif
		#endregion
		
	}
}
