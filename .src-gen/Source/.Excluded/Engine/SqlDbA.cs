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
using System;
using System.Collections.Generic;
using System.Cor3.Data;
using System.Data;
using System.Data.SqlClient;

namespace System.Cor3.Data.Engine
{
	/// <summary>
	/// <para>
	/// This (no longer abstract) class targets SqlServer.
	/// </para>
	/// <para>
	/// This abstract class is one founding aspect of the template engine,
	/// and is used to connect to a data source.
	/// </para>
	/// <para>
	/// • as noted, this class is usually wrapped by a generated database
	/// template featuring methods that connect to and encapsulates data
	/// automating conversion to a DotNet native type is there is one, 
	/// leaving the template author or class-wrapper to specialize in any
	/// additional markup so that useful results can be automated quickly.
	/// </para>
	/// <para>
	/// currently, I'm looking to implement a strategy for making more advanced
	/// combined queries, such as a “master view” which is a table of origin
	/// mapped to several other supplementary data from correlating tables
	/// (table relationships).  while it can be noted that these related tables
	/// can be defined in the schematics of the actual database (thinking of table
	/// key definitions in mysql and sqlserver), the idea here is that we get the
	/// desired results in a automated way to a DotNet native Type encapsulated
	/// in something similar to a DataModel (as used by MVC or MVVM applications).
	/// The only restriction that we are dealing with here, would be that we are 
	/// working in DotNet Framework v2.0.
	/// </para>
	/// </summary>
	public class SqlDbA : DataAbstract<SqlConnection, SqlCommand, SqlDataAdapter, SqlParameter>
	{
		#region Constantly
		// Data Source=OOO\SQL2005EXPRESS
		// AttachDbFileName="d:\dev\lip_data.mdf"
		const string cstring_attach = @"
	Data Source=$(DataSource);
	AttachDbFilename=""$(AttachDbFileName)"";
	Integrated Security=True;
	Connect Timeout=30";
//	User=$(UserName);
		const string cstring = @"
	Integrated Security=SSPI;
	Persist Security Info=False;
	Database=$(InitialCatalog);
	Server=$(DataSource)
";
		#endregion

		/// <inheritdoc/>
		protected internal override string data_id { get { return "SqlDbA"; } }
		
		protected DataSet globalData = null;
		
		/// <inheritdoc/>
		public override DataSet GlobalData { get { return globalData; } protected set { globalData = value; } }
		
		DictionaryList<string,string> parameters = new DictionaryList<string,string>();
		
		/// <inheritdoc/>
		public override DictionaryList<string,string> QueryParams { get { return parameters; } }

		protected string dsource=string.Empty, dconnect=cstring, dcatalog, username;
		int lastRecordsEffected = -1;
		/// <inheritdoc/>
		public override int LastRecordsAffected { get { return lastRecordsEffected; } set { lastRecordsEffected = value; } }

		protected string DataCatalog { get { return dcatalog; } set { dcatalog=value; } }
		/// <inheritdoc/>
		public override string DataSource { get { return dsource; }  }
		protected bool LoadFile = false;
		
		/// <inheritdoc/>
		public override string ConnectionString {
			get {
				return LoadFile ?
				cstring_attach
					.Replace("$(InitialCatalog)",DataCatalog)
					.Replace("$(AttachDbFileName)",DataSource) :
				cstring
					.Replace("$(UserName)",username)
					.Replace("$(InitialCatalog)",DataCatalog)
					.Replace("$(DataSource)",DataSource);
			}
		}
		
		/// <inheritdoc/>
		public override int DefaultFill(SqlDataAdapter A, DataSet D, string tablename)
		{
			return A.Fill(D,tablename);
		}
		
		public SqlDbA()
		{
//			 globalData = new DataSet(data_id);
		}
		public SqlDbA(string source, string table) : this()
		{
			this.dsource = source;
			this.dcatalog = table;
		}

		public override SqlConnection Connection { get { return new SqlConnection(ConnectionString); } }
		public override SqlDataAdapter Adapter { get { return new SqlDataAdapter(); } }
		
	}


}
