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
using System.Cor3.Data.Map.Types;
using System.Data;

namespace System.Cor3.Data
{

	public abstract class QueryContext : IQueryContext1
	{
		public delegate DataSet TSqlExecute(string q);
		//;
		internal string datafile = null;

		internal DataSet category = null;
		public DataSet Category {
			get { return category; }
			set { category = value; }
		}
		public abstract DataSet InsertCategory(string q);
		public abstract DataSet SelectCategory(string q);
		public abstract DataSet UpdateCategory(string q);
		public abstract DataSet DeleteCategory(string q);

		internal DataSet data = null;
		public DataSet Data {
			get { return data; }
			set { data = value; }
		}
		public abstract DataSet Insert(string q);
		public abstract DataSet Select(string q);
		public abstract DataSet Delete(string q);
		public abstract DataSet Update(string q);

		public abstract void Initialize();
		
	}

	public abstract class QueryContext<TConnection, TCommand, TAdapter, TParameter> :
		QueryContext,
		IDisposable, IQueryContext2
		where TConnection : IDbConnection
		where TCommand : IDbCommand, new()
		where TAdapter : IDbDataAdapter, IDisposable, new()
		where TParameter : IDbDataParameter
	{
		public class ContextClass : DatabaseContext<TConnection, TCommand, TAdapter, TParameter>
		{
			public ContextClass() : base()
			{
			}
		}

		public DatabaseContext ContextInfo {
			get { return Context; }
		}
		public ContextClass Context { get; set; }

		public string TableContent { get { return ContextInfo.TableContent; } }
		public string TableAlias { get { return ContextInfo.TableAlias; } set { ContextInfo.TableAlias = value; } }
		public string TableName { get { return ContextInfo.TableName; } }
		public string[] Fields { get { return ContextInfo.TableFields; } }
		public string[] TableGroups { get { return ContextInfo.TableGroups; } }

//		public delegate TAdapter RowParamCallback(DbOp o, string q, TConnection c);
//		public delegate int TFillCallback(TAdapter A, DataSet D, string tablename);
		public delegate void TCommandParamsCallback(TCommand C);
		public delegate void TQueryInfoCallback(TCommand C, string table, string field, DataRowView row);

		#region Context
		public abstract TAdapter InsertCategoryAdapter(DbOp op, string query, TConnection connection);
		public abstract int InsertCategoryFillOperation(TAdapter A, DataSet D, string tablename);

		public abstract TAdapter DeleteCategoryAdapter(DbOp op, string query, TConnection connection);
		public abstract int DeleteCategoryFillOperation(TAdapter A, DataSet D, string tablename);

		public abstract TAdapter UpdateCategoryAdapter(DbOp op, string query, TConnection connection);
		public abstract int UpdateCategoryFillOperation(TAdapter A, DataSet D, string tablename);

		public abstract TAdapter SelectCategoryAdapter(DbOp op, string query, TConnection connection);
		public abstract int SelectCategoryFillOperation(TAdapter A, DataSet D, string tablename);
		#endregion

		#region House

		public abstract TAdapter a_sel(DbOp op, string query, TConnection connection);
		public abstract int f_sel(TAdapter A, DataSet D, string tablename);

		public abstract TAdapter a_ins(DbOp op, string query, TConnection connection);
		public abstract int f_ins(TAdapter A, DataSet D, string tablename);

		public abstract TAdapter a_del(DbOp op, string query, TConnection connection);
		public abstract int f_del(TAdapter A, DataSet D, string tablename);

		public abstract TAdapter a_upd(DbOp op, string query, TConnection connection);
		public abstract int f_upd(TAdapter A, DataSet D, string tablename);

		#endregion

		public QueryContext(string db)
		{
			datafile = db;
		}
		/// <summary>
		/// here, we'll clean up and info from the datasets.
		/// </summary>
		void IDisposable.Dispose()
		{
			category.Tables.Clear();
			category.Clear();
			category.Dispose();
			category = null;

			data.Tables.Clear();
			data.Clear();
			data.Dispose();
			data = null;
		}
		#region no
		#if no
		public class Qub
		{
			#region Callback
			TSqlExecute InsertCategoryCB;
			SQLiteDb.RowParamCallback InsertCategoryAdapterCB;
			SQLiteDb.DataFillCallback InsertCategoryFillOperationCB;

			TSqlExecute DeleteCategoryCB;
			SQLiteDb.RowParamCallback DeleteCategoryAdapterCB;
			SQLiteDb.DataFillCallback DeleteCategoryFillOperationCB;

			TSqlExecute UpdateCategoryCB;
			SQLiteDb.RowParamCallback UpdateCategoryAdapterCB;
			SQLiteDb.DataFillCallback UpdateCategoryFillOperationCB;

			TSqlExecute SelectCategoryCB;
			SQLiteDb.RowParamCallback SelectCategoryAdapterCB;
			SQLiteDb.DataFillCallback SelectCategoryFillOperationCB;
			#endregion

		}
		#endif
		#endregion
		
	}
}
