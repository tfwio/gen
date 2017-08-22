/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 07/18/2011
 * Time: 07:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
#region Using
using System;
using System.Cor3.Data;
using System.Cor3.Data.Engine;
using System.Data;
using System.Data.SQLite;

using Generator.Elements.Types;

#endregion

namespace Generator.Extensions
{
	
	static public class AccessDataExtension
	{
		static readonly string[] AceTypeNames = Gen.Strings.TypeNames_Ace.Split(',');
		static public string[] AceStringNames(this AccessDataTypes src) { return AceTypeNames; }
		
		static public TypeCode AceGetNativeTypeCode(this AccessDataTypes typeRef)
		{
			if (typeRef==AccessDataTypes.Number) return TypeCode.Double;
			else if (typeRef==AccessDataTypes.AutoIncr) return TypeCode.Int32;
			else if (typeRef==AccessDataTypes.Currency) return TypeCode.Decimal;
			else if (typeRef==AccessDataTypes.DateTime) return TypeCode.DateTime;
			else if (typeRef==AccessDataTypes.Memo) return TypeCode.String;
			else if (typeRef==AccessDataTypes.Hyperlink) return TypeCode.String;
			else if (typeRef==AccessDataTypes.Ole) return TypeCode.Object;
			else if (typeRef==AccessDataTypes.Text) return TypeCode.String;
			else if (typeRef==AccessDataTypes.YesNo) return TypeCode.Boolean;
			else return TypeCode.Empty;
		}
		static public string AceStrTypeCode(this DataRowView row, string fname)
		{
			int value = int.Parse(row[fname].ToString());
			AccessDataTypes adt = (AccessDataTypes)value;
			// there are the few redundant types
			if (adt==AccessDataTypes.Number) return "Double";
			else if (adt==AccessDataTypes.AutoIncr) return "Int32";
			else if (adt==AccessDataTypes.Currency) return "Decimal";
			else if (adt==AccessDataTypes.DateTime) return "DateTime";
			else if (adt==AccessDataTypes.Memo) return "String";
			else if (adt==AccessDataTypes.Text) return "String";
			else if (adt==AccessDataTypes.Hyperlink) return "String";
			else if (adt==AccessDataTypes.Ole) return "Object";
			else if (adt==AccessDataTypes.YesNo) return "Boolean";
			else return string.Empty;
		}
		static public string AceStrAceType(this DataRowView row, string fname)
		{
			int value = int.Parse(row[fname].ToString());
			// there are the few redundant types
			if (value==(int)AccessDataTypes.Number) return "Number";
			else if (value==(int)AccessDataTypes.AutoIncr) return "AutoIncr";
			else if (value==(int)AccessDataTypes.Currency) return "Currency";
			else if (value==(int)AccessDataTypes.DateTime) return "DateTime";
			else if (value==(int)AccessDataTypes.Memo) return "Memo";
			else if (value==(int)AccessDataTypes.Text) return "Text";
			else if (value==(int)AccessDataTypes.Hyperlink) return "Hyperlink";
			else if (value==(int)AccessDataTypes.Ole) return "Ole";
			else if (value==(int)AccessDataTypes.YesNo) return "YesNo";
			else return string.Empty;
		}
	}
}
