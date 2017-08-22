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
using System.Data;
using System.Xml.Serialization;

namespace System.Cor3.Data.Context
{
	public interface IDatabaseContext
	{
		string statementSelect();
		string statementCategories();
		string statementTables();
		string statementInsert();
		string statementInsert(params string[] fields);
		string statementDelete();
		string statementDelete(bool asString);
		string statementUpdate(DataRowView row);
		string statementUpdate(DataRowView row, params string[] fields);
		void SetFields(params string[] TableFields);
		[XmlAttributeAttribute()]
		string CategoryName { get; set; }
		[XmlAttributeAttribute()]
		string CategoryTitle { get; set; }
		[XmlAttributeAttribute()]
		string CategoryPk { get; set; }
		[XmlIgnoreAttribute()]
		DataRowView TableRow { get; set; }
		[XmlAttributeAttribute()]
		string TableName { get; set; }
		[XmlAttributeAttribute()]
		string TablePk { get; set; }
		[XmlAttributeAttribute()]
		string TableTitle { get; set; }
		[XmlAttributeAttribute()]
		string TableContent { get; set; }
		[XmlIgnoreAttribute()]
		string[] TableFields { get; set; }
		[XmlAttributeAttribute()]
		string TableCategory { get; set; }
		[XmlAttributeAttribute()]
		string TableFieldsAttribute { get; set; }
		SqlOrderMode OrderMode { get; set; }
		string OrderValue { get; set; }
		[XmlIgnoreAttribute()]
		string SELECT { get; }
		[XmlIgnoreAttribute()]
		string CATEGORIES { get; }
		[XmlIgnoreAttribute()]
		string TABLES { get; }
		event EventHandler TableCategoryChanged;
	}
}
