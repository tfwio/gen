using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Generator.Elements.Types
{
	/// <summary>
	/// TypeCode (NativeType) to ExtJsRecordFieldType.
	/// </summary>
	public class ExtJsRecordFieldTypeConverter : GeneratorTypeConverter
	{
		
		public override bool CanConvert<T>()
		{
			if (typeof(T) == typeof(ExtJsRecordFieldType)) return true;
			return base.CanConvert<T>();
		}
		/// <summary>
		/// Convert to TypeCode
		/// <para>ints are translated to Int64, Floats default to Double.</para>
		/// </summary>
		public TypeCode Convert(ExtJsRecordFieldType input, TypeCode defaultType)
		{
			return Convert(input,defaultType,defaultType,TypeCode.Int64,TypeCode.Double);
		}
		/// <summary>
		/// Convert to TypeCode from ExtJsRecordFieldType.
		/// </summary>
		public TypeCode Convert(ExtJsRecordFieldType input, TypeCode autoType, TypeCode defaultType, TypeCode defaultInt, TypeCode defaultFloat)
		{
			switch (input) {
				case ExtJsRecordFieldType.Boolean:
					return TypeCode.Boolean;
				case ExtJsRecordFieldType.Date:
					return TypeCode.DateTime;
				case ExtJsRecordFieldType.Float:
					return TypeCode.Double;
				case ExtJsRecordFieldType.Int:
					return TypeCode.Int64;
				case ExtJsRecordFieldType.String:
					return TypeCode.String;
				case ExtJsRecordFieldType.Auto:
					return autoType;
				default:
					return defaultType;
			}
		}
		
		/// <summary>
		/// input should be ExtJsRecordFieldType 
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public override TypeCode Convert(string input)
		{
			ExtJsRecordFieldType outType = ExtJsRecordFieldType.Auto;
			bool canDo = Enum.TryParse<ExtJsRecordFieldType>(input, out outType);
			if (!canDo) outType = ExtJsRecordFieldType.Auto;
			
			return this.Convert(outType,TypeCode.Object);
		}
		public override string Convert(TypeCode input)
		{
			switch (input) {
				case TypeCode.Boolean:
					return ExtJsRecordFieldType.Boolean.ToString();
				case TypeCode.Byte:
				case TypeCode.SByte:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
					return ExtJsRecordFieldType.Int.ToString();
				case TypeCode.Char:
				case TypeCode.String:
					return ExtJsRecordFieldType.Int.ToString();
				case TypeCode.DateTime:
					return ExtJsRecordFieldType.Date.ToString();
				case TypeCode.Single:
				case TypeCode.Double:
					return ExtJsRecordFieldType.Date.ToString();
				case TypeCode.Object:
				case TypeCode.DBNull:
					return ExtJsRecordFieldType.Auto.ToString();
				case TypeCode.Decimal:
					return ExtJsRecordFieldType.Boolean.ToString();
			}
			throw new NotImplementedException();
		}
	}
}
