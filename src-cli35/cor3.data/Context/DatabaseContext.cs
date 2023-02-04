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
using System.Cor3.Data.Engine;
using System.Data;
using System.Runtime.Remoting.Contexts;
using System.Xml.Serialization;

namespace System.Cor3.Data.Context
{

	public class DatabaseContext<TConnection,TCommand,TAdapter,TParameter> : QueryDatabaseContext, IDisposable
		where TConnection:IDbConnection
		where TCommand:IDbCommand, new()
		where TAdapter:class,IDbDataAdapter,IDisposable,new()
		where TParameter:IDbDataParameter
	{
		public QueryContext<TConnection,TCommand,TAdapter,TParameter>.TCommandParamsCallback TableRowParams = null;
		public QueryContext<TConnection,TCommand,TAdapter,TParameter>.TQueryInfoCallback TableAdapterParams = null;
		
		void IDisposable.Dispose()
		{
		}

		public DatabaseContext()
		{
		}
		public DatabaseContext(QueryContextInfo clone)
		{
			SetContext(clone);
		}

	}

}
