/*
 * Created by SharpDevelop.
 * User: oIo
 * Date: 2/10/2011
 * Time: 9:52 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
#region Using
using System;
using System.Cor3.Data;
using System.Data;

#endregion

namespace Generator.Data
{
	public interface IDataFactory<TConnection,TCommand,TAdapter,TParameter>
		where TConnection:IDbConnection
		where TCommand:IDbCommand, new()
		where TAdapter:IDbDataAdapter, IDisposable,new()
		where TParameter:IDbDataParameter
	{
		QueryDataContext DataContext { get;set; }
		DataAbstract<TConnection,TCommand,TAdapter,TParameter> Database { get;set; }
		
		void Create();
		void Create(string filename);
		void BindToData();
		void GetSchemas();
		Array GetWebFormTypes();
		Array GetDataNativeItemTypes();
	}
}
