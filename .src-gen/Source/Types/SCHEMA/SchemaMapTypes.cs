/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
#region Using
using System;
using System.Data.OleDb;
#endregion

namespace Generator.Elements.Types
{
	// note also System.Data.Common.StorageType
	// note also System.Data.DbType
	// note also System.Data.FunctionId
	// note also System.Data.SqlDbType (of course)
	// note also System.Data.StatementType
	// note also System.Data.ValueType
	// note also System.Data.Odbc.OdbcType
	public enum SchemaMapTypes
	{
		 /// 2
		@Short = OleDbType.SmallInt,
		///3,
		@Long = OleDbType.Integer,
		///4,
		@Single = OleDbType.Single,
		///5,
		@Double = OleDbType.Double,
		///6,
		@Currency = OleDbType.Currency,
		///7
		@DateTime = 7,//OleDbType.Date,  // I know that Date maps correctly to 7, but just in case…
		///11
		@Bit = OleDbType.Boolean,// is this correct? Guess so.
		///17
		@Byte = OleDbType.UnsignedTinyInt,
		///72
		@GUID = OleDbType.Guid,
		///128
		@BigBinary = OleDbType.Binary, //204 (Ole.Binary is 128)
		///128
		@LongBinary = OleDbType.Binary, //205
		///128
		@VarBinary = OleDbType.Binary, //204
		///130
		@LongText = OleDbType.WChar,//203
		///130
		@VarChar = OleDbType.WChar,//202
		///131
		@Decimal = OleDbType.Decimal,
	}
}
