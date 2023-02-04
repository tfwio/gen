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
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml.Serialization.Configuration;
using System.Xml.Serialization.Advanced;

namespace System.Cor3.Data.Context
{

	public abstract class QueryContext : IQueryContext1
	{
		public delegate DataSet TSqlExecute(string q);
		//;
		internal string datafile = null;
		
		[XmlIgnore]
		abstract public QueryContextInfo ContextData { get; }

		internal DataSet category = null;
		[XmlIgnore]
		public DataSet Category {
			get { return category; }
			set { category = value; }
		}
		public abstract DataSet InsertCategory(string q);
		public abstract DataSet SelectCategory(string q);
		public abstract DataSet UpdateCategory(string q);
		public abstract DataSet DeleteCategory(string q);

		internal DataSet data = null;
		[XmlIgnore] public DataSet Data {
			get { return data; }
			set { data = value; }
		}
		public abstract DataSet Insert(string q);
		public abstract DataSet Select(string q);
		public abstract DataSet Delete(string q);
		public abstract DataSet Update(string q);

		public abstract void Initialize();
		
	}


}
