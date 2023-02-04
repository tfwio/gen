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

	public enum ColumnModeType
	{
		/// <summary>
		/// create a double-quotation field around the key, and value
		/// </summary>
		DQuoteKey,
		/// <summary>
		/// create a single-quotation field around the key, and value
		/// </summary>
		SQuoteKey,
		/// <summary>
		/// create a double-quotation field around the value
		/// </summary>
		DQuote,
		/// <summary>
		/// create a single-quotation field around the value
		/// </summary>
		SQuote,
	}
	public class ColumnInfo
	{
		public bool ForceEscape {
			get { return _forceEscape; }
			set { _forceEscape = value; }
		} bool _forceEscape = false;
		
		public Type ColumnType {
			get { return _type; } set { _type = value; }
		} Type _type = typeof(string);
		
		public bool HasColumnType { get { return _type!=null; } }
		
		public void ToTable(DataTable table)
		{
			if (HasColumnType) table.Columns.Add(Name,ColumnType);
			else table.Columns.Add(Name,typeof(string));
		}
		public void ToColTable(DataSet ds, string tableName) { ToColTable(ds.Tables[tableName]); }
		public void ToColTable(DataTable table)
		{
			DataRowView rv = table.DefaultView.AddNew();
			rv["name"] = Name;
			rv["show"] = IsVisible;
			rv["iskey"] = this.IsId;
			rv["width"] = string.IsNullOrEmpty(WidthAttribute) ? -1: Width.Value;
		}
		
		public string Name = string.Empty, Info = string.Empty, Reformat = null, TableName = string.Empty;
		
		public bool IsRootTable { get { return TableName == string.Empty; } }
		public bool IsVisible = true, IsId = false;
		public int? Width = null;
		
		public string WidthAttribute {
			get {
				if (Width.HasValue) return string.Format(" width=\"{0}\"",Width.Value);
				return string.Empty;
			}
		}
		
		public ColumnInfo(string name) :
			this(name,null,null,true,false)
		{
		}
		
		public ColumnInfo(string name, string reformat) :
			this(name,reformat,null,true,false)
		{
		}
		
		public ColumnInfo(string name, string reformat, int? width, bool isVisible) :
			this(name, reformat,width,isVisible,false)
		{
		}
		
		public ColumnInfo(string name, string reformat, int? width, bool isVisible, bool isId)
		{
			Name = name;
			IsVisible = isVisible;
			IsId = isId;
			Reformat = reformat;
			Width = width;
		}
		
	}
}
