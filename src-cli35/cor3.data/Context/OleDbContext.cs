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
using System.Data.OleDb;

namespace System.Cor3.Data.Context
{
	//	public delegate void rowFilter(SqlManager m, DataRowView row, string field);
	public class OleDbContext: QueryContext<OleDbConnection,OleDbCommand,OleDbDataAdapter,OleDbParameter> {
		
		#region Category
		public override DataSet InsertCategory(string q)
		{
			category.Tables.Clear();
			using (Access10 db = new Access10(datafile))
				category = db.Delete(ContextInfo.CategoryName,q,InsertCategoryAdapter,InsertCategoryFillOperation);
			return category;
		}
		public override OleDbDataAdapter InsertCategoryAdapter(DbOp op, string query, OleDbConnection connection)
		{
			return new OleDbDataAdapter(query,connection);
		}
		public override int InsertCategoryFillOperation(OleDbDataAdapter A, DataSet D, string tablename)
		{
			return 0;
		}
		
		public override DataSet DeleteCategory(string q)
		{
			category.Tables.Clear();
			using (Access10 db = new Access10(datafile))
				category = db.Delete(Context.TableName,q,DeleteCategoryAdapter,DeleteCategoryFillOperation);
			return category;
		}
		public override OleDbDataAdapter DeleteCategoryAdapter(DbOp op, string query, OleDbConnection connection)
		{
			return new OleDbDataAdapter(query,connection);
		}
		public override int DeleteCategoryFillOperation(OleDbDataAdapter A, DataSet D, string tablename)
		{
			return 0;
		}
		
		public override DataSet UpdateCategory(string q)
		{
			category.Tables.Clear();
			using (Access10 db = new Access10(datafile))
				category = db.Delete(Context.CategoryName,q,UpdateCategoryAdapter,UpdateCategoryFillOperation);
			return category;
		}
		public override OleDbDataAdapter UpdateCategoryAdapter(DbOp op, string query, OleDbConnection connection)
		{
			return new OleDbDataAdapter(query,connection);
		}
		public override int UpdateCategoryFillOperation(OleDbDataAdapter A, DataSet D, string tablename)
		{
			return 0;
		}
		
		public override DataSet SelectCategory(string q)
		{
			category.Tables.Clear();
			category.Tables.Add(Context.CategoryName);
			using (Access10 db = new Access10(datafile))
				category = db.Select(Context.CategoryName,q,SelectCategoryAdapter,SelectCategoryFillOperation);
			return category;
		}
		public override OleDbDataAdapter SelectCategoryAdapter(DbOp op, string query, OleDbConnection connection)
		{
			return new OleDbDataAdapter(query,connection);
		}
		public override int SelectCategoryFillOperation(OleDbDataAdapter A, DataSet D, string tablename)
		{
			A.Fill(D);
			return 0;
		}
		#endregion
		#region House
		public override DataSet Select(string q)
		{
			if (data==null) data=new DataSet();
			data.Tables.Clear();
			using (Access10 db = new Access10(datafile))
				data = db.Select(Context.TableName,q,AdapterSelect,FillSelect);
			return data;
		}
		public override OleDbDataAdapter  AdapterSelect(DbOp op, string query, OleDbConnection connection) { return new OleDbDataAdapter(query,connection); }
		public override int FillSelect(OleDbDataAdapter A, DataSet D, string tablename) { A.Fill(D); return 0; }
		
		public override DataSet Insert(string q)
		{
			data.Tables.Clear();
			using (Access10 db = new Access10(datafile))
			{
				data = db.Insert(Context.TableName,q,AdapterInsert,FillInsert);
			}
			return data;
		}
		public override OleDbDataAdapter AdapterInsert(DbOp op, string query, OleDbConnection connection)
		{
			OleDbDataAdapter A = new OleDbDataAdapter(null,connection);
			A.InsertCommand = new OleDbCommand(query,connection);
			return A;
		}
		public override int FillInsert(OleDbDataAdapter A, DataSet D, string tablename)
		{
			return 0;
		}
		
		public override DataSet Delete(string q)
		{
			data.Tables.Clear();
			using (Access10 db = new Access10(datafile))
				data = db.Delete(Context.TableName,q,AdapterDelete,FillDelete);
			return data;
		}
		public override OleDbDataAdapter AdapterDelete(DbOp op, string query, OleDbConnection connection)
		{
			OleDbDataAdapter A = new OleDbDataAdapter(null,connection);
			A.DeleteCommand = new OleDbCommand(query,connection);
			return A;
		}
		public override int FillDelete(OleDbDataAdapter A, DataSet D, string tablename)
		{
			return 0;
		}
		
		public override DataSet Update(string q)
		{
			data.Tables.Clear();
			using (Access10 db = new Access10(datafile))
				data = db.Update(Context.TableName,q,AdapterUpdate,FillUpdate);
			return data;
		}
		public override OleDbDataAdapter AdapterUpdate(DbOp op, string query, OleDbConnection connection)
		{
			OleDbDataAdapter A = new OleDbDataAdapter(query,connection);
			A.UpdateCommand = new OleDbCommand(query,connection);
			return A;
		}
		public override int FillUpdate(OleDbDataAdapter A, DataSet D, string tablename)
		{
			return -1;
		}
		#endregion

		public OleDbContext() : this(null)
		{
		}
		public OleDbContext(QueryContextInfo qdc) : base(qdc,null)
		{
		}
		public OleDbContext(QueryContextInfo qdc,string file) : base(qdc,file)
		{
			this.Initialize();
			this.Context.Generator = "ole";
		}
		public override void Initialize()
		{
//			throw new NotImplementedException();
		}
	}
}
