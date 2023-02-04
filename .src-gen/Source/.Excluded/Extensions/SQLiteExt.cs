#region Using
/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 07/18/2011
 * Time: 07:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Data.SQLite;

using Generator.Core.Entities.Types;

#endregion
namespace Generator.Extensions
{
	static public class SQLiteExt
	{
		// not used
		static public string[] GetTypeNames()
		{
			return TypeAffinity.GetNames(typeof(System.Data.SQLite.TypeAffinity));
		}
		// not used
		static public System.TypeCode GetNativeType(TypeAffinity aff)
		{
			switch (aff)
			{
				case TypeAffinity.Text:
				case TypeAffinity.Blob:
					return TypeCode.String;
				case TypeAffinity.DateTime:
					return TypeCode.DateTime;
				case TypeAffinity.Double:
				case TypeAffinity.Int64:
					return TypeCode.Double;
				case TypeAffinity.Null:
					return TypeCode.DBNull;
				case TypeAffinity.Uninitialized:
				case TypeAffinity.None:
				default:
					return TypeCode.Empty;
			}
		}
		// not used
		static public string NativeTStr(TypeAffinity typeRef)
		{
			if (typeRef==TypeAffinity.Blob) return "String";
			else if (typeRef==TypeAffinity.DateTime) return "DateTime";
			else if (typeRef==TypeAffinity.Double) return "Double";
			else if (typeRef==TypeAffinity.Int64) return "Int64";
			else if (typeRef==TypeAffinity.None) return "DBNull";
			else if (typeRef==TypeAffinity.Null) return "DBNull";
			else if (typeRef==TypeAffinity.Uninitialized) return "Empty";
			else if (typeRef==TypeAffinity.Text) return "String";
			else return "Empty";
		}
		// not used
		static public string ClientTStr(this DataRowView row, string fname)
		{
			return "";
		}
	}
}
