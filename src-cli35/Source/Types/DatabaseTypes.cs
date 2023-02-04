using System;

namespace Generator.Elements.Types
{
	/// <summary>
	/// See Generator.Elements.Types
	/// </summary>
	public enum DatabaseType
	{
		Unspecified,
		ClassObject,
		SqlServer,
		#if USEMYSQL
		MySql,
		#endif
		//OleAccess_2007,
		OleDb,
		OleAccess,
		SQLite
	}
	
}
