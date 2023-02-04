/* oIo * 11/15/2010 – 2:33 AM */
using System;

namespace Generator.Elements.Types
{
	public enum ConnectionType
	{
		None,
		OleDb,
		#if USEMYSQL
		MySql,
		#endif
		SQLite,
		Sql,
	}

}
