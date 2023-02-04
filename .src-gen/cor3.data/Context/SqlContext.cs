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
using System.Data.SqlClient;
using System.Data.SQLite;

namespace System.Cor3.Data.Context
{
	//	public delegate void rowFilter(SqlManager m, DataRowView row, string field);
	public class SqlContext: QueryContext<SqlConnection,SqlCommand,SqlDataAdapter,SqlParameter> {
		
		string _source=@"VAIO\SQLEXPRESS", _table = "prime";
		#region Category
		public override DataSet InsertCategory(string q)
		{
			category.Tables.Clear();
			using (SqlDbA db = new SqlDbA(_source,_table))
				category = db.Delete(Context.CategoryName,q,InsertCategoryAdapter,InsertCategoryFillOperation);
			return category;
		}
		public override SqlDataAdapter InsertCategoryAdapter(DbOp op, string query, SqlConnection connection)
		{
			return new SqlDataAdapter(query,connection);
		}
		public override int InsertCategoryFillOperation(SqlDataAdapter A, DataSet D, string tablename)
		{
			return 0;
		}
		
		public override DataSet DeleteCategory(string q)
		{
			category.Tables.Clear();
			using (SqlDbA db = new SqlDbA(_source,_table))
				category = db.Delete(Context.CategoryName,q,DeleteCategoryAdapter,DeleteCategoryFillOperation);
			return category;
		}
		public override SqlDataAdapter DeleteCategoryAdapter(DbOp op, string query, SqlConnection connection)
		{
			return new SqlDataAdapter(query,connection);
		}
		public override int DeleteCategoryFillOperation(SqlDataAdapter A, DataSet D, string tablename)
		{
			return 0;
		}
		
		public override DataSet UpdateCategory(string q)
		{
			category.Tables.Clear();
			using (SqlDbA db = new SqlDbA(_source,_table))
				category = db.Delete(Context.CategoryName,q,UpdateCategoryAdapter,UpdateCategoryFillOperation);
			return category;
		}
		public override SqlDataAdapter UpdateCategoryAdapter(DbOp op, string query, SqlConnection connection)
		{
			return new SqlDataAdapter(query,connection);
		}
		public override int UpdateCategoryFillOperation(SqlDataAdapter A, DataSet D, string tablename)
		{
			return 0;
		}
		
		public override DataSet SelectCategory(string q)
		{
			category.Tables.Clear();
			category.Tables.Add(Context.CategoryName);
			using (SqlDbA db = new SqlDbA(_source,_table))
				category = db.Select(Context.CategoryName,q,SelectCategoryAdapter,SelectCategoryFillOperation);
			return category;
		}
		public override SqlDataAdapter SelectCategoryAdapter(DbOp op, string query, SqlConnection connection)
		{
			return new SqlDataAdapter(query,connection);
		}
		public override int SelectCategoryFillOperation(SqlDataAdapter A, DataSet D, string tablename)
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
			using (SqlDbA db = new SqlDbA(_source,_table))
				data = db.Select(Context.TableName,q,AdapterSelect,FillSelect);
			return data;
		}
		public override SqlDataAdapter  AdapterSelect(DbOp op, string query, SqlConnection connection)
		{
			return new SqlDataAdapter(query,connection);
		}
		public override int FillSelect(SqlDataAdapter A, DataSet D, string tablename)
		{
			A.Fill(D);
			return 0;
		}
		
		public override DataSet Insert(string q)
		{
			data.Tables.Clear();
			using (SqlDbA db = new SqlDbA(_source,_table))
			{
				data = db.Insert(Context.TableName,q,AdapterInsert,FillInsert);
			}
			return data;
		}
		public override SqlDataAdapter AdapterInsert(DbOp op, string query, SqlConnection connection)
		{
			SqlDataAdapter A = new SqlDataAdapter();
			A.InsertCommand = new SqlCommand(query,connection);
			return A;
		}
		public override int FillInsert(SqlDataAdapter A, DataSet D, string tablename)
		{
			return 0;
		}
		
		public override DataSet Delete(string q)
		{
			data.Tables.Clear();
			using (SqlDbA db = new SqlDbA(_source,_table))
				data = db.Delete(Context.TableName,q,AdapterDelete,FillDelete);
			return data;
		}
		public override SqlDataAdapter AdapterDelete(DbOp op, string query, SqlConnection connection)
		{
			SqlDataAdapter A = new SqlDataAdapter();
			A.DeleteCommand = new SqlCommand(query,connection);
			return A;
		}
		public override int FillDelete(SqlDataAdapter A, DataSet D, string tablename)
		{
			return 0;
		}
		
		public override DataSet Update(string q)
		{
			data.Tables.Clear();
			using (SqlDbA db = new SqlDbA(_source,_table))
				data = db.Update(Context.TableName,q,AdapterUpdate,FillUpdate);
			return data;
		}
		public override SqlDataAdapter AdapterUpdate(DbOp op, string query, SqlConnection connection)
		{
			SqlDataAdapter A = new SqlDataAdapter();
			A.UpdateCommand = new SqlCommand(query,connection);
			return A;
		}
		public override int FillUpdate(SqlDataAdapter A, DataSet D, string tablename)
		{
			return -1;
		}
		#endregion
		
		public SqlContext(QueryContextInfo qdc) : base(qdc,null)
		{
			
		}
		public SqlContext() : base(null)
		{
			this.Initialize();
			this.Context.Generator = "sql";
		}
		public override void Initialize()
		{
//			throw new NotImplementedException();
		}
	}

}
