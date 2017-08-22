/*
 * User: oIo
 * Date: 11/15/2010 ? 2:49 AM
 */
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace Generator.Elements.Types
{
  
  public static class NativeTypeProvider
  {
    static public string NativeTypeCode(this Type input, string nullvalue="_NULL_")
    {
      TypeCode t=TypeCode.Empty;
      return Enum.TryParse<TypeCode>(input.Name, out t) ? t.ToString() : nullvalue;
    }
    static public string TypeN(this object input)
    {
      return input.GetType().Name;
    }
    static public string TypeC(this object input)
    {
      return input.GetType().NativeTypeCode();
    }
    static public void TypeCodeStringToNativeDictionary(this string DataTypeNative, IDictionary<string,object> fparams)
    {
      TypeCode code = TypeCode.Empty;
      bool converted = Enum.TryParse(DataTypeNative, out code);
      string result = "Empty";
      result = converted ? TypeCodeToNativeString(code) : "Empty";
      fparams.Add("NativeType",result);
      fparams.Add("Native",result);
      #region No
      #if No
      switch (DataTypeNative) {
        case "Single":
          fparams.Add("Native","float");
          break;
        case "Double":
          fparams.Add("Native","double");
          break;
        case "Decimal":
          fparams.Add("Native","decimal");
          break;
        case "Boolean":
          fparams.Add("Native","bool");
          break;
          //case "BigInt":		fparams.Add("Native","Int64"); break;
        case "DateTime":
          fparams.Add("Native","DateTime");
          break;
        case "DBNull":
          fparams.Add("Native","DBNull");
          break;
        case "Empty":
          fparams.Add("Native","Empty");
          break;
        case "Int16":
          fparams.Add("Native","short");
          break;
        case "Int32":
          fparams.Add("Native","int");
          break;
        case "Int64":
          fparams.Add("Native","long");
          break;
        case "UInt16":
          fparams.Add("Native","ushort");
          break;
        case "UInt32":
          fparams.Add("Native","uint");
          break;
        case "UInt64":
          fparams.Add("Native","ulong");
          break;
        default:
          fparams.Add("Native",DataTypeNative.ToLower());
          break;
      }
      #endif
      #endregion
    }
    static public NativeTypes TypeCodeToNativeTypes(this TypeCode typeCode)
    {
      switch (typeCode) {
        case TypeCode.Single:
          return NativeTypes.@float;
        case TypeCode.Double:
          return NativeTypes.@double;
        case TypeCode.Decimal:
          return NativeTypes.@decimal;
        case TypeCode.Boolean:
          return NativeTypes.@bool;
          //case "BigInt":		fparams.Add("Native","Int64"); break;
        case TypeCode.DateTime:
          return NativeTypes.@DateTime;
        case TypeCode.DBNull:
          return NativeTypes.@DBNull;
        case TypeCode.Empty:
          return NativeTypes.@Empty;
        case TypeCode.Int16:
          return NativeTypes.@short;
        case TypeCode.Int32:
          return NativeTypes.@int;
        case TypeCode.Int64:
          return NativeTypes.@long;
        case TypeCode.UInt16:
          return NativeTypes.@ushort;
        case TypeCode.UInt32:
          return NativeTypes.@uint;
        case TypeCode.UInt64:
          return NativeTypes.@ulong;
        default:
          return NativeTypes.@object;
      }
    }
    static public string TypeCodeToNativeString(this TypeCode typeCode)
    {
      switch (typeCode) {
        case TypeCode.Boolean:
          return "bool";
        case TypeCode.Byte:
          return "byte";
        case TypeCode.Char:
          return "char";
          //case "BigInt":		fparams.Add("Native","Int64"); break;
        case TypeCode.DateTime:
          return "DateTime";
        case TypeCode.DBNull:
          return "DBNull";
        case TypeCode.Decimal:
          return "decimal";
        case TypeCode.Double:
          return "double";
        case TypeCode.Empty:
          return "Empty";
        case TypeCode.Int16:
          return "short";
        case TypeCode.Int32:
          return "int";
        case TypeCode.Int64:
          return "long";
//				case "BigInt": // this shouldn't be here, but i used it so it is.
//					return "long";
        case TypeCode.SByte:
          return "ubyte";
        case TypeCode.Single:
          return "float";
        case TypeCode.String:
          return "string";
        case TypeCode.UInt16:
          return "ushort";
        case TypeCode.UInt32:
          return "uint";
        case TypeCode.UInt64:
          return "ulong";
        default:
          return "object";
      }
    }
    
  }
  ///
  public enum NativeTypes {
    ///
    @bool,
    ///
    @byte,
    ///
    @char,
    ///
    @DateTime,
    ///
    @DBNull,
    ///
    @decimal,
    ///
    @double,
    ///
    @Empty,
    ///
    @short,
    ///
    @int,
    ///
    @long,
    ///
    @object,
    ///
    @sbyte,
    ///
    @float,
    ///
    @string,
    ///
    @ushort,
    ///
    @uint,
    ///
    @ulong,
  }
}
