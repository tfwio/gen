/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
#region Using
using System;
using System.Data;

#endregion

namespace Generator.Elements.Types
{
	public class As3TypeProvider
	{
		/// <summary>
		/// note that it is up to the implementor to test these out and convert them to something
		/// appropriate if one is mapped wrong.
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		static public FlashNativeTypes SqlTypeToFlashType(SqlDbType code)
		{
			switch (code) {
				case SqlDbType.Binary:
					return FlashNativeTypes.Array;
				case SqlDbType.Bit:
					return FlashNativeTypes.Boolean;
				case SqlDbType.Date: // not sure
				case SqlDbType.DateTime:
				case SqlDbType.DateTime2: // not sure
				case SqlDbType.SmallDateTime: // not sure
				case SqlDbType.Time: // not sure
				case SqlDbType.Timestamp: // not sure
				case SqlDbType.DateTimeOffset:
					return FlashNativeTypes.Date;
				case SqlDbType.Image:
					return FlashNativeTypes.UNKNOWN;
				case SqlDbType.Int:
					return FlashNativeTypes.@int;
				case SqlDbType.BigInt:
				case SqlDbType.Decimal:
				case SqlDbType.Float:
				case SqlDbType.Real:
				case SqlDbType.SmallInt:
				case SqlDbType.SmallMoney:
				case SqlDbType.Money:
					return FlashNativeTypes.Number;
				case SqlDbType.Char:
				case SqlDbType.NChar:
				case SqlDbType.NText:
				case SqlDbType.NVarChar:
				case SqlDbType.Text:
				case SqlDbType.VarChar:
					return FlashNativeTypes.String;
				case SqlDbType.Xml:
					return FlashNativeTypes.XML;
//				case SqlDbType.Binary: return FlashNativeTypes.;
				default:
					return FlashNativeTypes.UNKNOWN;
			}
		}
		// FIXME: Replace this semantic with a SystemType converter.
		/// <summary>
		/// This method is currently used within the FieldElement.Params Dictionary
		/// to convert a ‘SqlType’ to a ‘FlashDataType’.
		/// </summary>
		static public string SqlTypeStrToFlashType(string sqlDbType)
		{
			SqlDbType outType = 0;
			bool isParsed = false;
			try {
				outType = (SqlDbType)Enum.Parse(typeof(SqlDbType),sqlDbType);
				isParsed = true;
			} catch (Exception) {
			}
			if (!isParsed) return "UNKNOWN";
			return SqlTypeToFlashType(outType).ToString();
		}
		/* Older */
		/**************************************************************************/
		static public string SystemTypeToFlashType(string typeCode)
		{
			switch (typeCode) {
				case "Object":
					return FlashNativeTypes.Object.ToString();
				case "Boolean":
					return FlashNativeTypes.Boolean.ToString();
				case "Char":
					return FlashNativeTypes.@int.ToString();
				case "SByte":
					return FlashNativeTypes.@uint.ToString();
				case "Byte":
					return FlashNativeTypes.@int.ToString();
				case "Int16":
					return FlashNativeTypes.@int.ToString();
				case "UInt16":
					return FlashNativeTypes.@uint.ToString();
				case "Int32":
					return FlashNativeTypes.@int.ToString();
				case "UInt32":
					return FlashNativeTypes.@uint.ToString();
				case "Int64":
					return FlashNativeTypes.@int.ToString();
				case "UInt64":
					return FlashNativeTypes.@uint.ToString();
				case "Single":
					return FlashNativeTypes.Number.ToString();
				case "Double":
					return FlashNativeTypes.Number.ToString();
				case "Decimal":
					return FlashNativeTypes.Number.ToString();
				case "DateTime":
					return FlashNativeTypes.Date.ToString();
				case "String":
					return FlashNativeTypes.String.ToString();
					//case "DBNull": return FlashNativeTypes.UNKNOWN;
					//case "Empty": return FlashNativeTypes.UNKNOWN;
				default:
					return FlashNativeTypes.UNKNOWN.ToString();
			}
		}
	}
}
