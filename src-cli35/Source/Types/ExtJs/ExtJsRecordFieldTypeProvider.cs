/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
#region Using
using System;
using System.Xml.Serialization;
#endregion

namespace Generator.Elements.Types
{
	public class ExtJsRecordFieldTypeProvider : EnumProvider
	{
		public override string Name
		{
			get {
				return "ExtJs 1.6 Record Field Type";
			}
		}
		public override Array Types
		{
			get {
				return ExtJsRecordFieldType.GetValues(typeof(ExtJsRecordFieldType));
			}
		}

		/// <summary>always true.</summary>
		public override bool CanDoFromNative
		{
			get {
				return true;
			}
		}

		public override TypeCode ToNative(string name)
		{
			switch (name) {
				case "Boolean":
					return TypeCode.Boolean;
				case "Float":
					return TypeCode.Double;
				case "Int":
					return TypeCode.Int64;
				case "Date":
					return TypeCode.DateTime;
				case "Auto":
				case "String":
				default:
					return TypeCode.String;
			}
		}
		
		public override string ProvideTypeCode(TypeCode toConvert)
		{
			switch (toConvert) {
				case TypeCode.Byte:
				case TypeCode.SByte:
				case TypeCode.Int16:
//				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
					return ExtJsRecordFieldType.Int.ToString();
				case TypeCode.Boolean:
					return ExtJsRecordFieldType.Boolean.ToString();
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return ExtJsRecordFieldType.Float.ToString();
				case TypeCode.DateTime:
					return ExtJsRecordFieldType.Date.ToString();
				case TypeCode.String:
					return ExtJsRecordFieldType.String.ToString();
				default:
					return ExtJsRecordFieldType.Auto.ToString();
			}
		}
		
		public Array RecordFieldType
		{
			get {
				return ExtJsFieldType.GetValues(typeof(ExtJsRecordFieldType));
			}
		}
	}

}
