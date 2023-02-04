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
	public interface IQueryContext1
	{
		DataSet InsertCategory(string q);
		DataSet SelectCategory(string q);
		DataSet UpdateCategory(string q);
		DataSet DeleteCategory(string q);
		DataSet Insert(string q);
		DataSet Select(string q);
		DataSet Delete(string q);
		DataSet Update(string q);
		void Initialize();
		DataSet Category { get; set; }
		DataSet Data { get; set; }
	}
}
