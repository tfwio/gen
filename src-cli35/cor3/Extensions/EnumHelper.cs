using System;
using System.Collections.Generic;
using System.Linq;
namespace System
{
  #if NCORE
  static class HasFlagExtension
  {
    static public bool HasFlag(this Enum value, Enum compare)
    {
      return (Convert.ToUInt64(value) & Convert.ToUInt64(compare)) == Convert.ToUInt64(compare);
    }
  }
  #endif
  /// <summary>
  /// https://stackoverflow.com/questions/15017151/implementation-of-enum-tryparse-in-net-3-5
  /// 
  /// This isn't used but can be useful (in attempt) to build in net35-Client,
  /// though I'm not sure what errors will come after implementing something like this ;)
  /// </summary>
  static class EnumHelper
  {
    /// <summary>
    /// [poster(psubsee2003)'s comment]
    /// 
    /// I dislike using a try-catch to handle any conversion failures or other non-exceptional events as part of the normal flow of my application, so my own Enum.TryParse method for .NET 3.5 and earlier makes use of the Enum.IsDefined() method to make sure the there will not be an exception thrown by Enum.Parse(). You can also include some null checks on value to prevent an ArgumentNullException if value is null.
    /// 
    /// Obviously this method will not reside in the Enum class so you will need a class to include this in that would be appropriate.
    /// 
    /// One limitation is the lack of an enum constraint on generic methods, so you would have to consider how you want to handle incorrect types.  Enum.IsDefined will throw an ArgumentException if TEnum is not an enum but the only other option is a runtime check and throwing a different exception, so I generally do not add an additional check and just let the type checking in these methods handle for me. I'd consider adding IConvertible as another constraint, just to help constrain the type even more.
    /// </summary>
    public static bool TryParse_n3<TEnum>(this string value, out TEnum result)
      where TEnum : struct, IConvertible
    {
      var retValue = value != null && Enum.IsDefined(typeof(TEnum), value);
      result = retValue ? (TEnum)Enum.Parse(typeof(TEnum), value) : default(TEnum);
      return retValue;
    }
    
    public static bool TryParse<T>(this Enum value, out T result) where T : struct, IConvertible
    {
      return value.ToString().TryParse<T>(out result);
    }
    
    public static bool TryParse<T>(this string value, out T result)
      where T : struct, IConvertible
    {
      return TryParse_n3(value, out result);
    }
    // =====================================================================
    
    private static readonly char[] FlagDelimiter = new[] {
      ','
    };

    public static bool TryParseEnum<TEnum>(this string value, out TEnum result) where TEnum : struct
    {
      if (string.IsNullOrEmpty(value)) {
        result = default(TEnum);
        return false;
      }
      var enumType = typeof(TEnum);
      if (!enumType.IsEnum)
        throw new ArgumentException(string.Format("Type '{0}' is not an enum", enumType.FullName));
      result = default(TEnum);
      // Try to parse the value directly
      if (Enum.IsDefined(enumType, value)) {
        result = (TEnum)Enum.Parse(enumType, value);
        return true;
      }
      // Get some info on enum
      var enumValues = Enum.GetValues(enumType);
      if (enumValues.Length == 0)
        return false;
      // probably can't happen as you cant define empty enum?
      var enumTypeCode = Type.GetTypeCode(enumValues.GetValue(0).GetType());
      // Try to parse it as a flag
      if (value.IndexOf(',') != -1) {
        if (!Attribute.IsDefined(enumType, typeof(FlagsAttribute)))
          return false;
        // value has flags but enum is not flags
        // todo: cache this for efficiency
        var enumInfo = new Dictionary<string, object>();
        var enumNames = Enum.GetNames(enumType);
        for (var i = 0; i < enumNames.Length; i++)
          enumInfo.Add(enumNames[i], enumValues.GetValue(i));
        ulong retVal = 0;
        foreach (var name in value.Split(FlagDelimiter)) {
          var trimmedName = name.Trim();
          if (!enumInfo.ContainsKey(trimmedName))
            return false;
          // Enum has no such flag
          var enumValueObject = enumInfo[trimmedName];
          ulong enumValueLong;
          switch (enumTypeCode) {
            case TypeCode.Byte:
              enumValueLong = (byte)enumValueObject;
              break;
            case TypeCode.SByte:
              enumValueLong = (byte)((sbyte)enumValueObject);
              break;
            case TypeCode.Int16:
              enumValueLong = (ushort)((short)enumValueObject);
              break;
            case TypeCode.Int32:
              enumValueLong = (uint)((int)enumValueObject);
              break;
            case TypeCode.Int64:
              enumValueLong = (ulong)((long)enumValueObject);
              break;
            case TypeCode.UInt16:
              enumValueLong = (ushort)enumValueObject;
              break;
            case TypeCode.UInt32:
              enumValueLong = (uint)enumValueObject;
              break;
            case TypeCode.UInt64:
              enumValueLong = (ulong)enumValueObject;
              break;
            default:
              return false;
              // should never happen
          }
          retVal |= enumValueLong;
        }
        result = (TEnum)Enum.ToObject(enumType, retVal);
        return true;
      }
      // the value may be a number, so parse it directly
      switch (enumTypeCode) {
        case TypeCode.SByte:
          sbyte sb;
          if (!SByte.TryParse(value, out sb))
            return false;
          result = (TEnum)Enum.ToObject(enumType, sb);
          break;
        case TypeCode.Byte:
          byte b;
          if (!Byte.TryParse(value, out b))
            return false;
          result = (TEnum)Enum.ToObject(enumType, b);
          break;
        case TypeCode.Int16:
          short i16;
          if (!Int16.TryParse(value, out i16))
            return false;
          result = (TEnum)Enum.ToObject(enumType, i16);
          break;
        case TypeCode.UInt16:
          ushort u16;
          if (!UInt16.TryParse(value, out u16))
            return false;
          result = (TEnum)Enum.ToObject(enumType, u16);
          break;
        case TypeCode.Int32:
          int i32;
          if (!Int32.TryParse(value, out i32))
            return false;
          result = (TEnum)Enum.ToObject(enumType, i32);
          break;
        case TypeCode.UInt32:
          uint u32;
          if (!UInt32.TryParse(value, out u32))
            return false;
          result = (TEnum)Enum.ToObject(enumType, u32);
          break;
        case TypeCode.Int64:
          long i64;
          if (!Int64.TryParse(value, out i64))
            return false;
          result = (TEnum)Enum.ToObject(enumType, i64);
          break;
        case TypeCode.UInt64:
          ulong u64;
          if (!UInt64.TryParse(value, out u64))
            return false;
          result = (TEnum)Enum.ToObject(enumType, u64);
          break;
        default:
          return false;
          // should never happen
      }
      return true;
    }
  }
}



