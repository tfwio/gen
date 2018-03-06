#region User/License
// oio * 8/18/2012 * 11:45 PM

// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.Collections.Generic;
using System.Cor3.Data;
using System.Cor3.Data.Engine;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using Generator.Elements;

//using (SQLiteConnection connection = new SQLiteConnection(connectionstring))
//	using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query,connection))
//{
//	connection.Open();
//
//	connection.Close();
//}
namespace Generator
{
	public class TemplateUtil
	{
		#region static/readonly
		const string sql_select_templates = "select * from [templates];";
		public static readonly string[] template_table_fields = new string[9]{"container","row","grouphead","groupfoot","head","foot","note","table","fields"};
		const string sql_create_templates = @"DROP TABLE IF EXISTS ""templates"";
CREATE TABLE ""templates"" (
id		INTEGER PRIMARY KEY AUTOINCREMENT,
admin		INTEGER DEFAULT 0,
title		VARCHAR DEFAULT NULL,
container	VARCHAR DEFAULT NULL,
[grouphead]		VARCHAR DEFAULT NULL,
[groupfoot]		VARCHAR DEFAULT NULL,
[head]		VARCHAR DEFAULT NULL,
[foot]		VARCHAR DEFAULT NULL,
'row'		VARCHAR DEFAULT NULL,
'note'		VARCHAR DEFAULT NULL,
'table'		VARCHAR DEFAULT NULL,
'fields'		VARCHAR DEFAULT NULL);",
		sql_update_row = @"UPDATE [@table] SET
[@field] = @value
WHERE [@key] = @keyvalue;",
		sql_insert_row = @"
insert into [templates] (
[table],
[title]
) VALUES (
@table,
@title
); ",
			sql_delete_row = @"DELETE FROM ""templates"" where [id] = @xid;";
		#endregion
		
		public TemplateUtil()
		{
		}
		
		public TemplateUtil(string path)
		{
			Initialize(path);
		}
		
		public List<string> GetGroups()
		{
			return templates.Select(t=>t.Table).Distinct().ToList();
		}
		
		#region Templates
		/// <summary>
		/// Callback: Retrieves the DataAdapter for the respective call.
		/// </summary>
		/// <param name="o"></param>
		/// <param name="query"></param>
		/// <param name="C"></param>
		/// <returns></returns>
		static SQLiteDataAdapter SelectTemplatesAdapter(DbOp o, string query, SQLiteConnection C)
		{
			return new SQLiteDataAdapter(query,C);
		}
		
		//		public Dictionary<string, TemplateElement> Templates {
		//			get { return templates; }
		//		} Dictionary<string,TemplateElement> templates = new Dictionary<string,TemplateElement>();
		public List<TemplateElement> Templates {
			get { return templates; }
		} List<TemplateElement> templates = new List<TemplateElement>();
		
		/// <summary>
		/// Loads the application's templates.
		/// In the future the application should be set up to call on this method to re-load the templates.
		/// </summary>
		void Initialize(string datafile_sqlite)
		{
			if (CheckPathForErrors(datafile_sqlite)) return;
			TemplateElement ele;
			Templates.Clear();
			using (SQLiteDb db = new SQLiteDb(datafile_sqlite))
			using (DataSet ds = db.Select("templates",sql_select_templates,SelectTemplatesAdapter))
			using (DataView v = ds.GetDataView("templates"))
				foreach (DataRowView rv in v)
				{
					Templates.Add(TemplateElement.FromRowView(rv));
				}
		}
		
		#endregion
		
		#region Static Actions
		/// <summary>
		/// CREATE BLANK SQLITE DATABASE
		/// </summary>
		/// <param name="path"></param>
		static public void CreateTemplatesTable(string path)
		{
			if (System.IO.File.Exists(path))
			{
				#if NCORE
				Console.Clear();
				Console.Error.Write("File Exists...\n\"{0}\"",path);
				#else
				System.Windows.Forms.MessageBox.Show(
					string.Format("File Exists...\n\"{0}\"",path),
					"Error",
					System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Error
				);
				#endif
				return;
			}
			using (System.IO.FileStream fs = System.IO.File.Create(path,0))
			{
			}
			using (SQLiteDb db = new SQLiteDb(path))
			using (DataSet ds = db.Insert("templates",sql_create_templates,SelectTemplatesAdapter))
			using (DataView v = ds.GetDataView("templates"))
				foreach (DataRowView rv in v)
				{
					//				Templates.Add(TemplateElement.FromRowView(rv));
				}
		}
		/// <summary>
		/// SEND TEMPLATE TO SQLITE DATABASE
		/// </summary>
		/// <param name="path"></param>
		static public void UpdateTemplateRow(string path, string tableName, TemplateElement element, string fieldName, string newValue)
		{
			string query = sql_update_row
				.Replace("@field", fieldName)
				.Replace("@keyvalue", element.Id.ToString())
				.Replace("@key",TemplateElement.col_id)
				.Replace("@table",tableName)
				//				.Replace("@value", newValue)
				;
			using (SQLiteDb db = new SQLiteDb(path))
			using (SQLiteConnection c = db.Connection)
			using (SQLiteDataAdapter a = new SQLiteDataAdapter(null,c))
			using (a.UpdateCommand = new SQLiteCommand(query,c))
			{
				c.Open();
				a.UpdateCommand.Parameters.AddWithValue("@value",newValue);
				a.UpdateCommand.ExecuteNonQuery();
				Debug.Print("{0}",query);
				c.Close();
			}
			
		}
		public bool InsertRow(string path, TemplateElement element)
		{
			bool haserror = false;
			using (SQLiteDb db = new SQLiteDb(path))
			using (SQLiteConnection c = db.Connection)
			using (SQLiteDataAdapter a = new SQLiteDataAdapter(null,c))
			using (a.InsertCommand = new SQLiteCommand(sql_insert_row,c))
			{
				try {
					c.Open();
					a.InsertCommand.Parameters.AddWithValue("@table",element.Table);
					a.InsertCommand.Parameters.AddWithValue("@title",element.Title);
					a.InsertCommand.ExecuteNonQuery();
					c.Close();
				} catch {
					haserror=true;
				}
			}
			return haserror;
		}
		public bool DeleteRow(string path, TemplateElement element)
		{
			bool haserror = false;
			using (SQLiteDb db = new SQLiteDb(path))
			using (SQLiteConnection c = db.Connection)
			using (SQLiteDataAdapter a = new SQLiteDataAdapter(null,c))
			using (a.DeleteCommand = new SQLiteCommand(sql_delete_row,c))
			{
				try {
					c.Open();
					a.DeleteCommand.Parameters.AddWithValue("@xid",element.Id);
					a.DeleteCommand.ExecuteNonQuery();
					c.Close();
				} catch {
					haserror=true;
				}
			}
			return haserror;
		}
		#endregion
		
		#region Check For Errors
		/// <summary>
		/// returns true on error.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		bool CheckPathForErrors(string path)
		{
			return !System.IO.File.Exists(path);
		}
		
		#endregion
		
	}
}
