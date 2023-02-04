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

namespace System.Cor3.Data
{
	public interface IQueryBasicContext<TConnection, TCommand, TAdapter, TParameter>
		where TConnection : IDbConnection, IDisposable
		where TCommand : IDbCommand,new()
		where TAdapter : IDbDataAdapter, IDisposable,new()
		where TParameter : IDbDataParameter
	{
		void Initialize();

		string TableContent { get; }
		string TableName { get; }
		string[] Fields { get; }
		
		DatabaseContext<TConnection,TCommand,TAdapter,TParameter> Context { get; set; }
	}
}
