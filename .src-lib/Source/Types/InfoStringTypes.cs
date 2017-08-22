/*
 * oIo — 11/17/2010 — 8:50 PM
 */
#region Using
using System;
#endregion
namespace Generator.Elements.Types
{
	public enum InfoStringTypes
	{
		None,
		#if USEMYSQL
		MySql,
		#endif
		Microsoft,
	}
}
