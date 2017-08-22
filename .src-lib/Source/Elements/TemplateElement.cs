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
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Forms;

//using (SQLiteConnection connection = new SQLiteConnection(connectionstring))
//using (SQLiteDataAdapter adapter   = new SQLiteDataAdapter(query,connection))
//{
//	connection.Open();
//	connection.Close();
//}
namespace Generator.Elements
{
	/// <summay>TemplatesModel, PrimaryKey=‘id’</summary>
	/// <remarks></remarks>
	[Serializable]
	public class TemplateElement
	{
		/// <summary></summary>
		public string TableName { get { return table_name; } }
		
		#region static/const
		internal const string table_name = "templates";
		public const string actionName = "list-templates";
		internal const string col_id = "id",col_admin = "admin",col_title = "title",col_table = "table",col_container = "container",col_row = "row",col_head = "head",col_foot = "foot",col_grouphead = "grouphead",col_groupfoot = "groupfoot",col_note = "note";
		internal static readonly Type[] tcoltypes = new Type[]{ typeof(int?), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string) };

		public static string[] ColumnNames {
			get {
				return tcols;
			}
		} static internal protected readonly string[] tcols = { col_id,col_admin,col_table,col_title,col_container,col_row,col_head,col_foot,col_grouphead,col_groupfoot,col_note };
		static internal protected readonly string   tkey  = "id";
		#endregion
		
		/// <summary>adds data to the DataRowView</summary>
		static public TemplateElement FromRowView(DataRowView row)
		{
			TemplateElement model = new TemplateElement();
			if (row==null) return model;
//			Debug.Print(
//				"LOADED TEMPLATE ROW, Type: {0}, Value: {1}",
//				row[col_id].GetType(),
//				row[col_id]
//			);
			if (row[col_id]!=DBNull.Value) model.Id = Convert.ToInt32(row[col_id]);
			if (row[col_admin]!=DBNull.Value) model.Admin = row[col_admin] as string;
			if (row[col_table]!=DBNull.Value) model.Table = row[col_table] as string;
			if (row[col_title]!=DBNull.Value) model.Title = row[col_title] as string;
			if (row[col_container]!=DBNull.Value) model.Container = row[col_container] as string;
			if (row[col_row]!=DBNull.Value) model.Row = row[col_row] as string;
			if (row[col_head]!=DBNull.Value) model.Head = row[col_head] as string;
			if (row[col_foot]!=DBNull.Value) model.Foot = row[col_foot] as string;
			if (row[col_grouphead]!=DBNull.Value) model.Grouphead = row[col_grouphead] as string;
			if (row[col_groupfoot]!=DBNull.Value) model.Groupfoot = row[col_groupfoot] as string;
			if (row[col_note]!=DBNull.Value) model.Note = row[col_note] as string;
			return model;
		}
		
		/// <summary></summary>
		static public implicit operator TemplateElement(DataRowView row) { return FromRowView(row); }
		
		/// <summary>adds data to the DataRowView</summary>
		static public void ToRowView(TemplateElement model, DataRowView row)
		{
			row[col_id] = model.Id;
			row[col_admin] = model.Admin;
			row[col_table] = model.Table;
			row[col_title] = model.Title;
			row[col_container] = model.Container;
			row[col_row] = model.Row;
			row[col_head] = model.Head;
			row[col_foot] = model.Foot;
			row[col_grouphead] = model.Grouphead;
			row[col_groupfoot] = model.Groupfoot;
			row[col_note] = model.Note;
		}
		
		#region Properties
		/// <summary>driverno Field: Name="id" Type="int?", (MAX) </summary>
		[DisplayName(@"")] public int? Id {
			get { return _id; } set { _id = value; }
		} int? _id = default(int);
	
		/// <summary>driverno Field: Name="admin" Type="string", (50) </summary>
		[DisplayName(@"")] public string Admin {
			get { return _admin; } set { _admin = value; }
		} string _admin = default(string);
		
		/// <summary>driverno Field: Name="admin" Type="string", (50) </summary>
		[DisplayName(@"")] public string Table {
			get { return _table; } set { _table = value; }
		} string _table = default(string);
	
		/// <summary>driverno Field: Name="title" Type="string", (50) </summary>
		[DisplayName(@"")] public string Title {
			get { return _title; } set { _title = value; }
		} string _title = default(string);
	
		/// <summary>driverno Field: Name="container" Type="string", (MAX) </summary>
		[DisplayName(@"")] public string Container {
			get { return _container; } set { _container = value; }
		} string _container = default(string);
	
		/// <summary>driverno Field: Name="row" Type="string", (MAX) </summary>
		[DisplayName(@"")] public string Row {
			get { return _row; } set { _row = value; }
		} string _row = default(string);
	
		/// <summary>driverno Field: Name="head" Type="string", (MAX) </summary>
		[DisplayName(@"")] public string Head {
			get { return _head; } set { _head = value; }
		} string _head = default(string);
	
		/// <summary>driverno Field: Name="foot" Type="string", (MAX) </summary>
		[DisplayName(@"")] public string Foot {
			get { return _foot; } set { _foot = value; }
		} string _foot = default(string);
	
		/// <summary>driverno Field: Name="grouphead" Type="string", (MAX) </summary>
		[DisplayName(@"")] public string Grouphead {
			get { return _grouphead; } set { _grouphead = value; }
		} string _grouphead = default(string);
	
		/// <summary>driverno Field: Name="groupfoot" Type="string", (MAX) </summary>
		[DisplayName(@"")] public string Groupfoot {
			get { return _groupfoot; } set { _groupfoot = value; }
		} string _groupfoot = default(string);
	
		/// <summary>driverno Field: Name="note" Type="string", (MAX) </summary>
		[DisplayName(@"")] public string Note {
			get { return _note; } set { _note = value; }
		} string _note = default(string);
	
		#endregion
		
		/// <summary></summary>
		public object GetKeyValue(string key)
		{
			switch (key)
			{
				case "id": return Id;
				case "table": return Table;
				case "admin": return Admin;
				case "title": return Title;
				case "container": return Container;
				case "row": return Row;
				case "head": return Head;
				case "foot": return Foot;
				case "grouphead": return Grouphead;
				case "groupfoot": return Groupfoot;
				case "note": return Note;
				default: return null;
			}
		}
		
		/// <summary></summary>
		public void SetValue(string Key, object Value)
		{
			switch (Key)
			{
				case "id": Id = (int?) Value; break;
				case "admin": Admin = (string) Value; break;
				case "title": Title = (string) Value; break;
				case "table": Table = (string) Value; break;
				case "container": Container = (string) Value; break;
				case "row": Row = (string) Value; break;
				case "head": Head = (string) Value; break;
				case "foot": Foot = (string) Value; break;
				case "grouphead": Grouphead = (string) Value; break;
				case "groupfoot": Groupfoot = (string) Value; break;
				case "note": Note = (string) Value; break;
			}
		}
	}
}
