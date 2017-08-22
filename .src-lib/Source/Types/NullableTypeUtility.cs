using System;

namespace Generator
{
	public static class NullableTypeUtility
	{
		#region NativeNullType Helper
		static public string GetNativeNullType(string input)
		{
			switch (input)
			{
				case "string":
				case "DBNull":
				case "Empty":
				case "object":
					return input;
				default:
					return string.Format("{0}?",input);
			}
		}
		static public bool IsNativeNullable(string input)
		{
			switch (input)
			{
				case "string":
				case "DBNull":
				case "Empty":
				case "object":
					return false;
				default:
					return true;
			}
		}
		#endregion

		static public bool IsNullable(string type)
		{
			TypeCode code= (TypeCode)TypeCode.Parse(typeof(TypeCode),type);
			return IsNullable(code);
//			return false;
		}
		static public bool IsNullable(TypeCode code)
		{
			switch (code) {
				case TypeCode.Boolean:
					return true;
				case TypeCode.Byte:
					return true;
				case TypeCode.Char:
					return true;
				case TypeCode.DateTime:
					return true;
					// case TypeCode.DBNull: return true;
				case TypeCode.Decimal:
					return true;
				case TypeCode.Double:
					return true;
					// case TypeCode.Empty: return true;
				case TypeCode.Int16:
					return true;
				case TypeCode.Int32:
					return true;
				case TypeCode.Int64:
					return true;
				case TypeCode.SByte:
					return true;
				case TypeCode.Single:
					return true;
				case TypeCode.UInt16:
					return true;
				case TypeCode.UInt32:
					return true;
				case TypeCode.UInt64:
					return true;
				case TypeCode.Object:
					return false;
				case TypeCode.String:
					return false;
				default:
					return false;
			}
		}
	}
}
