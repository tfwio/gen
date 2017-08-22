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
using System.Data.SQLite;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Data.OleDb;
using System.Data.SqlClient;
namespace System.Cor3.Data.Context
{
	[XmlRoot]
	public class DatabaseContextCollection : SerializableClass<DatabaseContextCollection>
	{
		string _name = "database-context-collection";
		[XmlAttribute] public string Name { get { return _name; } set { _name = value; } }
		
		
		QueryContextInfo[] list = new QueryContextInfo[0];
		[XmlElement(typeof(QueryContextInfo))]
		public QueryContextInfo[] Contexts { get { return list; } set { list = value; } }
		
		[XmlIgnore]
		public QueryContextInfo[] Parameters {
			get { return contexts; }
			set { contexts = value; }
		} QueryContextInfo[] contexts;
//
//		[XmlElement(Type=typeof(DatabaseContextSerializer))]
////		[XmlElement("Parameters")]
//		public DatabaseContextSerializer XmlParameters {
//			get {
//				if (Parameters == null)
//					return null;
//				else {
//					return new DatabaseContextSerializer(Parameters);
//				}
//			}
//			set {
//				contexts = value.Parameters;
//			}
//		}
		public DatabaseContextCollection()
		{
		}
		public DatabaseContextCollection(params QueryContextInfo[] contexts)
		{
			list = contexts;
		}

	}

}
