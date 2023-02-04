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
using System.Data.SQLite;

namespace System.Cor3.Data.Context
{
	public class SQLiteContext : QueryContext<SQLiteConnection, SQLiteCommand, SQLiteDataAdapter, SQLiteParameter>
	{
		#region Category
		public override DataSet InsertCategory(string q)
		{
			category.Tables.Clear();
			using (SQLiteDb db = new SQLiteDb(datafile))
				category = db.Delete(Context.CategoryName,q,InsertCategoryAdapter,InsertCategoryFillOperation);
			return category;
		}
		public override SQLiteDataAdapter InsertCategoryAdapter(DbOp op, string query, SQLiteConnection connection)
		{
			return new SQLiteDataAdapter(query,connection);
		}
		public override int InsertCategoryFillOperation(SQLiteDataAdapter A, DataSet D, string tablename)
		{
			return 0;
		}
	
		public override DataSet DeleteCategory(string q)
		{
			category.Tables.Clear();
			using (SQLiteDb db = new SQLiteDb(datafile))
				category = db.Delete(Context.CategoryName,q,DeleteCategoryAdapter,DeleteCategoryFillOperation);
			return category;
		}
		public override SQLiteDataAdapter DeleteCategoryAdapter(DbOp op, string query, SQLiteConnection connection)
		{
			return new SQLiteDataAdapter(query,connection);
		}
		public override int DeleteCategoryFillOperation(SQLiteDataAdapter A, DataSet D, string tablename)
		{
			return 0;
		}
	
		public override DataSet UpdateCategory(string q)
		{
			category.Tables.Clear();
			using (SQLiteDb db = new SQLiteDb(datafile))
				category = db.Delete(Context.CategoryName,q,UpdateCategoryAdapter,UpdateCategoryFillOperation);
			return category;
		}
		public override SQLiteDataAdapter UpdateCategoryAdapter(DbOp op, string query, SQLiteConnection connection)
		{
			return new SQLiteDataAdapter(query,connection);
		}
		public override int UpdateCategoryFillOperation(SQLiteDataAdapter A, DataSet D, string tablename)
		{
			return 0;
		}
	
		public override DataSet SelectCategory(string q)
		{
			category.Tables.Clear();
			category.Tables.Add(Context.CategoryName);
			using (SQLiteDb db = new SQLiteDb(datafile))
				category = db.Select(Context.CategoryName,q,SelectCategoryAdapter,SelectCategoryFillOperation);
			return category;
		}
		public override SQLiteDataAdapter SelectCategoryAdapter(DbOp op, string query, SQLiteConnection connection)
		{
			return new SQLiteDataAdapter(query,connection);
		}
		public override int SelectCategoryFillOperation(SQLiteDataAdapter A, DataSet D, string tablename)
		{
			A.Fill(D);
			return 0;
		}
		#endregion
		#region House
		/// <inheritdoc/>
		public override DataSet Select(string q) { if (data==null) data=new DataSet(); data.Tables.Clear(); using (SQLiteDb db = new SQLiteDb(datafile)) data = db.Select(Context.TableName,q,AdapterSelect,FillSelect); return data; }
		/// <inheritdoc/>
		public override SQLiteDataAdapter  AdapterSelect(DbOp op, string query, SQLiteConnection connection) { return new SQLiteDataAdapter(query,connection); }
		/// <inheritdoc/>
		public override int FillSelect(SQLiteDataAdapter A, DataSet D, string tablename) { A.Fill(D); return 0; }
	
		/// <inheritdoc/>
		public override DataSet Insert(string q) { data.Tables.Clear(); using (SQLiteDb db = new SQLiteDb(datafile)) data = db.Insert(Context.TableName,q,AdapterInsert,FillInsert); return data; }
		public DataSet Insert(string qins, string qsel) { data.Tables.Clear(); using (SQLiteDb db = new SQLiteDb(datafile)) { data = db.InsertSelect(Context.TableName,qins,qsel,AdapterInsert,FillInsert); } return data; }
		/// <inheritdoc/>
		public override SQLiteDataAdapter AdapterInsert(DbOp op, string query, SQLiteConnection connection) { SQLiteDataAdapter A = new SQLiteDataAdapter(null,connection); A.InsertCommand = new SQLiteCommand(query,connection); return A; }
		/// <inheritdoc/>
		public override int FillInsert(SQLiteDataAdapter A, DataSet D, string tablename) { return 0; }
		
		/// <inheritdoc/>
		public override DataSet Delete(string q) { data.Tables.Clear(); using (SQLiteDb db = new SQLiteDb(datafile)) data = db.Delete(Context.TableName,q,AdapterDelete,FillDelete); return data; }
		/// <inheritdoc/>
		public override SQLiteDataAdapter AdapterDelete(DbOp op, string query, SQLiteConnection connection) { SQLiteDataAdapter A = new SQLiteDataAdapter(null,connection); A.DeleteCommand = new SQLiteCommand(query,connection); return A; }
		/// <inheritdoc/>
		public override int FillDelete(SQLiteDataAdapter A, DataSet D, string tablename) { return 0; }
	
		/// <inheritdoc/>
		public override DataSet Update(string q) { data.Tables.Clear(); using (SQLiteDb db = new SQLiteDb(datafile)) data = db.Update(Context.TableName,q,AdapterUpdate,FillUpdate); return data; }
		/// <inheritdoc/>
		public override SQLiteDataAdapter AdapterUpdate(DbOp op, string query, SQLiteConnection connection) { SQLiteDataAdapter A = new SQLiteDataAdapter(null,connection); A.UpdateCommand = new SQLiteCommand(query,connection); return A; }
		/// <inheritdoc/>
		public override int FillUpdate(SQLiteDataAdapter A, DataSet D, string tablename) { return -1; }
		#endregion
		
		public SQLiteContext() : this(null)
		{
		}
		public SQLiteContext(QueryContextInfo dbc): this (dbc,dbc.tableFileName)
		{
		}
		public SQLiteContext(QueryContextInfo dbc,string path) : base(dbc,path)
		{
			Initialize();
			this.Context.Generator = "sqlite";
		}
		public override void Initialize()
		{
//			throw new NotImplementedException();
		}
	}
}
