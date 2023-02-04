/*
 * User: oIo
 * Date: 2/5/2011 – 10:00 PM
 */
#region Using
using System;
using System.Collections.Generic;
#endregion
namespace Generator.Elements.Types
{
	public class TSQLConverter
	{
		
		///
		static public NativeTypes SqlTypeToNativeType(TypeCode code)
		{
			switch (code) {
				case TypeCode.Boolean:
					return NativeTypes.@bool;
				case TypeCode.Byte:
					return NativeTypes.@byte;
				case TypeCode.Char:
					return NativeTypes.@char;
				case TypeCode.DateTime:
					return NativeTypes.@DateTime;
				case TypeCode.DBNull:
					return NativeTypes.DBNull;
				case TypeCode.Single:
					return NativeTypes.@float;
				case TypeCode.Decimal:
					return NativeTypes.@decimal;
				case TypeCode.Double:
					return NativeTypes.@double;
				case TypeCode.Empty:
					return NativeTypes.Empty;
				case TypeCode.Int16:
					return NativeTypes.@short;
				case TypeCode.Int32:
					return NativeTypes.@int;
				case TypeCode.Int64:
					return NativeTypes.@long;
				case TypeCode.Object:
					return NativeTypes.@object;
				case TypeCode.SByte:
					return NativeTypes.@sbyte;
				case TypeCode.String:
					return NativeTypes.@string;
				case TypeCode.UInt16:
					return NativeTypes.@ushort;
				case TypeCode.UInt32:
					return NativeTypes.@int;
				case TypeCode.UInt64:
					return NativeTypes.@long;
			}
			return NativeTypes.Empty;
		}
	}
}
