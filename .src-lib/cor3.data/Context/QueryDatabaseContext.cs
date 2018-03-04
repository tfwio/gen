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
 * User: oio
 * Date: 11/30/2011
 * Time: 14:46
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;

namespace System.Cor3.Data.Context
{
	/// <summary>
	/// Note that this is my first pass on documenting this class 
	/// quite a little while after writing it.
	/// <para>Documentation is not nearly complete or adequate.</para>
	/// </summary>
	/// <seealso cref="QueryContext{TConnection,TCommand,TAdapter,TParameter}" />
	public class QueryDatabaseContext : QueryContextInfo
	{
		
		// as hard-wired for testing
		// [Obsolete] static public string sqlitedb = @"\newdb.sqlite";
		// ————————————————————————————————————————————————————
		
		public const string master_table = "sqlite_master";
		public const string sql_table_select = "select * from [{0}] where type = 'table';";
		
		// ————————————————————————————————————————————————————
		
		internal const string query_statement_insert = "INSERT INTO @table @fieldset@selector";
		internal const string query_statement_update = "UPDATE @table\nset @fieldset\n@where";
		internal const string query_statement_select = "SELECT @fields\nFROM @table@category@where@order";
		internal const string query_statement_delete = "DELETE FROM @table @where";
		internal const string query_statement1 = "SELECT @fields\nFROM @table@filter@order";
		
		// ————————————————————————————————————————————————————
		
		#region Helper Methods
		// ————————————————————————————————————————————————————
		// we would like a data view for the Categories
		// we would like a data view for the Entries (dependent on default category?)
		/// <summary>
		/// (indirect <see cref="statementSelect()" />)
		/// </summary>
		[XmlIgnore] public string SELECT { get { return statementSelect(); } }
		// ————————————————————————————————————————————————————
		/// <summary>
		/// (indirect <see cref="statementCategories()" />)
		/// </summary>
		[XmlIgnore] public string CATEGORIES { get { return statementCategories(); } }
		// ————————————————————————————————————————————————————
		/// <summary>
		/// (indirect <see cref="statementTables()" />)
		/// </summary>
		[XmlIgnore] public string TABLES { get { return statementTables(); } }
	//		public string UPDATE { get { return statementUpdate(); } }
		// ————————————————————————————————————————————————————
		#endregion

		#region statement(…): categories,tables,insert,delete,update
		/// <summary>
		/// Automation/Generation of a Category-Query based on info supplied to this class.
		/// </summary>
		/// <returns>A SELECT query for categories.</returns>
		public string statementCategories()
		{
			return query_statement_select
				//+"@fields"
				.Replace("@fields","*")
				//+"@table"
				.Replace("@table",CategoryName.QBrace())
				//+"@where"
				.Replace("@where","")
	//				.Replace("@category","".QWhere(CategoryName.QBrace().QEqual("@"+CategoryValue)))
				//+"@category"
				.Replace("@category","")
				//+"@order"
				.Replace("@order","\n".QOrderBy(CategoryTitle.QBrace(), SqlSortOrder== SqlOrderMode.Ascending))
				;
		}
		/// <summary>
		/// Automation/Generation of a standard SQL query based on info supplied to this class.
		/// Why is this named tables?
		/// </summary>
		/// <returns>A SELECT query.</returns>
		public string statementTables()
		{
			return query_statement1
				//+"@fields"
				.Replace("@fields","*")
				//+"@table"
				.Replace("@table",master_table.QBrace())
				//+"@category"
				.Replace("@category","\n".QWhere("type".QBrace().QEqual(master_table.QCurly())))
				//+"@order"
				.Replace("@order","")
				.Replace("@filter","")
				;
		}
		
		// ————————————————————————————————————————————————————
		/// <summary>
		/// always returns null and does nothing else.
		/// </summary>
		/// <returns></returns>
		public string statementCategorySelect()
		{
			return null;
		}
		// ————————————————————————————————————————————————————
		/// <summary>
		/// Overload of statementSelect(null,false,false).
		/// </summary>
		/// <returns>see <see cref="statementSelect(string,bool,bool)" /></returns>
		public string statementSelect()
		{
			return statementSelect(null,false,false);
		}
		/// <summary>This method hasn't been implemented.  Just use the overload, and
		/// all will be well.
		/// <para>It simply provides 'ORDER BY' SQL clause using <see cref="SqlSort" /> value.</para>
		/// </summary>
		/// <param name="conditionWhere">the value of your 'where' SQL clause</param>
		/// <param name="ignoreCategories">if true, the method doesn't bother to generate a query for categorized tables (perhaps a reference table).</param>
		/// <param name="ignoreOrder">DOCUMENT ME!!!</param>
		/// <returns></returns>
		public string statementSelect(string conditionWhere, bool ignoreCategories, bool ignoreOrder)
		{
			string order = " ".QOrderBy(SqlSort);
//			string ordr = SqlSort, _orde = "@order";
			//"\n".QOrderBy(this.SqlSort.QBrace(), this.SqlSortOrder == SqlOrderMode.Ascending);
//			string catg = string.Empty;
//			if (!string.IsNullOrEmpty(this.SqlSort))
//			{
////				catg = "\n".QWhere(this.TableCategory.QBrace().QEqual("@"+this.TableCategory.CamelClean()));
//				if (ordr.Contains(","))
//				{
//					foreach (string s in SqlSort.Split(','))
//					{
//						_orde = _orde.Replace(s);
//					}
//				}
//				
//			}
			string catg = "\n".QWhere(this.TableCategory.QBrace().QEqual("@"+this.TableCategory.CamelClean()));
			string categ = string.IsNullOrEmpty(this.TableCategory)?"":"\n".QWhere(this.TableCategory.QBrace().QEqual("@"+this.TableCategory.CamelClean()));
			return query_statement_select
				.Replace("@where",!string.IsNullOrEmpty(conditionWhere) ? "\n".QWhere(conditionWhere) : string.Empty)
				//+"@fields"
				.Replace("@fields","*")
				//+"@table"
				.Replace("@table",TableName.QBrace())
				//+"@category"
				.Replace("@category",ignoreCategories ? "" : categ)
				//+"@order"
				.Replace("@order", ignoreOrder ? "" : string.IsNullOrEmpty(SqlSort)?"":order)
				;
		}
		// ————————————————————————————————————————————————————
		public string statementCategoryInsert()
		{
			return statementInsert(TableFields);
		}
		// ————————————————————————————————————————————————————
		public string statementInsert()
		{
			return statementInsert(TableFields);
		}
		string cultivate()
		{
			string psh = "@where";
			psh = psh.Replace("@where","\nWHERE @where");
			int counter = 0;
			foreach (string field in TableInsertStamp)
			{
				if (counter>=1) psh = psh.Replace("@where","\nAND @where");
				psh = psh.Replace("@where","\n\t"+field.QBrace()+" = @"+field.CamelClean()+"@where");
				counter++;
			}
			return string.Format("SELECT * from {0}{1}",TableName,psh.Replace("@where",""));
		}
		public string statementInsert(params string[] fields)
		{
			List<string> listc = new List<string>();
			List<string> listv = new List<string>();
			List<string> lista = new List<string>();
			foreach (string field in fields)
			{
				listc.Add(string.Format("[{0}]",field));
			}
			foreach (string field in fields)
			{
				listv.Add(string.Format("@{0}",field));
			}
			string skt = (TableInsertStamp.Length>0) ? statementSelect() : "";
			skt = null;
			string ins = query_statement_insert
				//+"@fields"
	//				.Replace("@field","*")
				.Replace("@selector",  skt ?? string.Empty )
				.Replace(
					"@fieldset",
					string.Concat(
						"(\r\n	",
						string.Join(", ",listc.ToArray()),
						")\r\nVALUES (",
						string.Join(", ",listv.ToArray()),
						")"
					)
				)
				//+"@table"
				.Replace("@table",TableName.QBrace())
				//+"@category"
	//				.Replace("@category","".QWhere(CategoryName.QBrace().QEqual("@"+CategoryValue)))
				.Replace("@category","")
				//+"@order"
				.Replace("@order","\n".QOrderBy(CategoryTitle.QBrace(), SqlSortOrder == SqlOrderMode.Ascending))
				;
//			MessageBox.Show(string.Concat(ins,";\n",cultivate()));
			return string.Concat(ins,";\n",cultivate());
		}
		// ————————————————————————————————————————————————————
		public string statementCategoryDelete()
		{
			return statementDelete(false);
		}
		// ————————————————————————————————————————————————————
		public string statementDelete()
		{
			return statementDelete(false);
		}
		public string statementDelete(bool asString)
		{
			return query_statement_delete
				//+"@fields"
	//				.Replace("@field","*")
				//+"@table"
				.Replace("@table",TableName.QBrace())
				//+"@category"
	//				.Replace("@category","".QWhere(CategoryName.QBrace().QEqual("@"+CategoryValue)))
				.Replace("@where"," ".QWhere(TablePk.QBrace()+" ".QEqual(string.Format(asString ? @"'{0}'" : "{0}", TableRow[TablePk]))))
				//+"@order"
	//				.Replace("@order","\n".QOrderBy(CategoryTitle.QBrace(), OrderMode== OrderMode.Ascending))
				;
		}
		// ————————————————————————————————————————————————————
		public string statementUpdate(DataRowView row)
		{
			return statementUpdate(row,TableFields);
		}
		public string statementUpdate(DataRowView row, params string[] fields)
		{
			string fieldparam = "@fieldset";
			string fieldparams = fieldparam;
			string[] list = new string[fields.Length];
			string output = query_statement_update
				//+"@fields"
	//				.Replace("@fieldset", string.Join(",\n	",list))
				//+"@table"
				.Replace("@table",TableName.QBrace())
				//+"@category"
	//				.Replace("@category","".QWhere(CategoryName.QBrace().QEqual("@"+CategoryValue)))
	//				.Replace("@category","")
				//+"@order"
				.Replace("@where","".QWhere(this.TablePk.QBrace().QEqual(row[TablePk].ToString())))
			;
			for (int i=0; i < fields.Length; i++)
			{
				list[i] = string.Format("[{0}] = @{1}",fields[i],fields[i].CamelClean());
			}
			fieldparams = fieldparams.Replace("@fieldset","");
			return output.Replace("@fieldset", string.Join(",\n	",list));
		}
		#endregion

		public QueryDatabaseContext() : base()
		{
		}
		
		public QueryDatabaseContext(QueryContextInfo clone)
		{
			SetContext(clone);
		}
	}

}
