using System;

namespace Generator
{
	public static class NullableTypeUtility
	{
		#region NativeNullType Helper
		/// <summary>
		/// This is for handling a DataName pointer for GoLang.
		/// At the moment, we're not sure how to handle some types
		/// since I'm not in the mood to think adequatly ;)
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
    static public string GetNativeNullableGoType(string input)
    {
      switch (input)
      {
        case "int": // target native go types
        case "int32":
        case "int64":
        case "float":
        case "float32":
        case "float64":
          return string.Format("*{0}",input);
        case "string":
        case "DBNull":
        case "Empty":
        case "object":
          return input;
        default:
          return string.Format("*{0}",input);
      }
    }
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
