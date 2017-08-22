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
using System.Cor3.Data.Engine;
using System.Data;

#endregion

namespace Generator.Data
{
	public abstract class BasicDataFactory<TConnection,TCommand,TAdapter,TParameter> : IDataFactory<TConnection,TCommand,TAdapter,TParameter>
		where TConnection:IDbConnection
		where TCommand:IDbCommand, new()
		where TAdapter:IDbDataAdapter,IDisposable,new()
		where TParameter:IDbDataParameter
	{
		public QueryDataContext DataContext { get;set; }
		public DataAbstract<TConnection,TCommand,TAdapter,TParameter> Database { get;set; }
		
		virtual public Array GetWebFormTypes(){
			return TypeCode.GetValues(typeof(Generator.Elements.Types.WebFormTypes));
		}
		virtual public void BindToData(){}
		virtual public void Create() {  }
		virtual public void Create(string filename)
		{
			throw new NotImplementedException();
		}
		
		virtual public Array GetDataNativeItemTypes()
		{
			throw new NotImplementedException();
		}
		
		virtual public void GetSchemas()
		{
			throw new NotImplementedException();
		}
	}
}
