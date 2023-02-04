using Generator.Core.Entities.Types;
using Generator.Data;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace Generator.Schemes
{
	/// <summary>
	/// This is generally a abstract notion here for the purpose of documentation.
	/// </summary>
	abstract public class SchemeProvider<TEnum,TFactory>
		where TEnum:struct
		where TFactory : IDataFactory<OleDbConnection, OleDbCommand, OleDbDataAdapter, OleDbParameter>
	{
		public TFactory Factory { get; set; }

		abstract public string[] TypeNames { get; }

		abstract public TypeCode GetTypeCode(TEnum input);
		//
		//abstract string GetNativeType(AccessDataTypes input);
		//abstract string GetNativeTypeCode(AccessDataTypes input);
		//abstract string GetStringTypeCode(AccessDataTypes input);
	}
	/// <summary>
	/// We need to specify the OleDb dependency dll or COM provider that enables
	/// the possibility of this working.
	/// </summary>
	public class AceScheme : SchemeProvider<AccessDataTypes,AccessDataFactory>
	{
		#region Constants
		public override string[] TypeNames {
			get { return typeNames; }
		} readonly string[] typeNames = Strings.TypeNames_Ace.Split(',');
		#endregion

		#region Type Conversions
		public override TypeCode GetTypeCode(AccessDataTypes typeRef)
		{
			if (typeRef == AccessDataTypes.Number) return TypeCode.Double;
			else if (typeRef == AccessDataTypes.AutoIncr) return TypeCode.Int32;
			else if (typeRef == AccessDataTypes.Currency) return TypeCode.Decimal;
			else if (typeRef == AccessDataTypes.DateTime) return TypeCode.DateTime;
			else if (typeRef == AccessDataTypes.Memo) return TypeCode.String;
			else if (typeRef == AccessDataTypes.Hyperlink) return TypeCode.String;
			else if (typeRef == AccessDataTypes.Ole) return TypeCode.Object;
			else if (typeRef == AccessDataTypes.Text) return TypeCode.String;
			else if (typeRef == AccessDataTypes.YesNo) return TypeCode.Boolean;
			else return TypeCode.Empty;
		}
		#endregion

	}
	#region SqlServer
	/*
	/// <summary>
	/// We need to specify the OleDb dependency dll or COM provider that enables
	/// the possibility of this working.
	/// </summary>
	public class SqlScheme : SchemeProvider<AccessDataTypes>
	{
		#region Constants
		const string typeNamesAce = "AutoIncr,Currency,DateTime,Memo,Number,Ole,Hyperlink,Text,YesNo";
		public override string[] TypeNames
		{
			get { return typeNames; }
		} readonly string[] typeNames = typeNamesAce.Split(',');
		#endregion

		#region Type Conversions
		public override TypeCode GetTypeCode(AccessDataTypes typeRef)
		{
			if (typeRef == AccessDataTypes.Number) return TypeCode.Double;
			else if (typeRef == AccessDataTypes.AutoIncr) return TypeCode.Int32;
			else if (typeRef == AccessDataTypes.Currency) return TypeCode.Decimal;
			else if (typeRef == AccessDataTypes.DateTime) return TypeCode.DateTime;
			else if (typeRef == AccessDataTypes.Memo) return TypeCode.String;
			else if (typeRef == AccessDataTypes.Hyperlink) return TypeCode.String;
			else if (typeRef == AccessDataTypes.Ole) return TypeCode.Object;
			else if (typeRef == AccessDataTypes.Text) return TypeCode.String;
			else if (typeRef == AccessDataTypes.YesNo) return TypeCode.Boolean;
			else return TypeCode.Empty;
		}
	}*/
	#endregion
}
