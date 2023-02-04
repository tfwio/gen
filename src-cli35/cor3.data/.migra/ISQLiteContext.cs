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
using System.Data.SQLite;

namespace System.Cor3.Data.Context
{
	public interface ISQLiteContext
	{
		DataSet InsertCategory(string q);
		SQLiteDataAdapter InsertCategoryAdapter(DbOp op, string query, SQLiteConnection connection);
		int InsertCategoryFillOperation(SQLiteDataAdapter A, DataSet D, string tablename);
		DataSet DeleteCategory(string q);
		SQLiteDataAdapter DeleteCategoryAdapter(DbOp op, string query, SQLiteConnection connection);
		int DeleteCategoryFillOperation(SQLiteDataAdapter A, DataSet D, string tablename);
		DataSet UpdateCategory(string q);
		SQLiteDataAdapter UpdateCategoryAdapter(DbOp op, string query, SQLiteConnection connection);
		int UpdateCategoryFillOperation(SQLiteDataAdapter A, DataSet D, string tablename);
		DataSet SelectCategory(string q);
		SQLiteDataAdapter SelectCategoryAdapter(DbOp op, string query, SQLiteConnection connection);
		int SelectCategoryFillOperation(SQLiteDataAdapter A, DataSet D, string tablename);
		DataSet Select(string q);
		SQLiteDataAdapter a_sel(DbOp op, string query, SQLiteConnection connection);
		int f_sel(SQLiteDataAdapter A, DataSet D, string tablename);
		DataSet Insert(string q);
		SQLiteDataAdapter a_ins(DbOp op, string query, SQLiteConnection connection);
		int f_ins(SQLiteDataAdapter A, DataSet D, string tablename);
		DataSet Delete(string q);
		SQLiteDataAdapter a_del(DbOp op, string query, SQLiteConnection connection);
		int f_del(SQLiteDataAdapter A, DataSet D, string tablename);
		DataSet Update(string q);
		SQLiteDataAdapter a_upd(DbOp op, string query, SQLiteConnection connection);
		int f_upd(SQLiteDataAdapter A, DataSet D, string tablename);
	}
}
