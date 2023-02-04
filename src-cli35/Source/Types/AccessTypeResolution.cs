/* oIo * 2/10/2011 9:52 PM */
using System;
using Generator.Elements.Types;
using System.Data;
using System.Linq;

using Generator.Extensions;
using Generator.Parser;
namespace Generator.Data
{
	/// <summary>
	/// It seems that this script is a work in progress and has not been 
	/// implemented in any way.
	/// </summary>
	public class AccessTypeResolution : TypeResolutionClass<AccessDataTypes>
	{
		public override string ServiceID {
			get { return ole_ace12; }
		}
		public const string ole_ace12 = "ace12";
		
		public override string GetDefaultValue(DataSet dataSchema, DataRowView rowColumn)
		{
			return rowColumn["COLUMN_DEFAULT"]==DBNull.Value ? "" : rowColumn.GetString("COLUMN_DEFAULT");
		}
		
		#region Ignored
		/// <summary></summary>
//		public override AccessDataTypes GetNativeType(DataSet dataSchema, DataRowView rowColumn)
//		{
//			SchemaNavigator sn = new SchemaNavigator(dataSchema);
//			string typename = rowColumn.GetString("DATA_TYPE");
//			bool hasmax = HasMaxLength(dataSchema,rowColumn);
//			int maxlen = (hasmax) ? GetMaxLength(dataSchema,rowColumn) : -1;
//			
//			switch (typename)
//			{
//				case "3": // autoincr,number
//					return AccessDataTypes.Number;
//				case "6": // currency
//					return AccessDataTypes.Currency;
//				case "7": // datetime
//					return AccessDataTypes.DateTime;
//				case "11": // yesno
//					return AccessDataTypes.YesNo;
//				case "130": // Memo,Hyperlink
//					if (hasmax && (maxlen >0) && (maxlen <= 255))
//					{
//						return AccessDataTypes.Text;
//					}
//					return AccessDataTypes.Memo;
//				case "128": // Ole
//					return AccessDataTypes.Ole;
//			}
//			
//			string output = null;
//			using (DataView v = sn.ViewDataTypes)
//			{
//				foreach (DataRowView row in v)
//					if (row.GetString("NativeDataType")==typename)
//						output = row.GetString("NativeDataType");
//			}
//			return (AccessDataTypes) Enum.Parse(typeof(AccessDataTypes),output);
//		}
		#endregion
		
		/// <summary></summary>
		public override TypeCode GetDataType(DataSet dataSchema, DataRowView rowColumn)
		{
			SchemaNavigator sn = new SchemaNavigator(dataSchema);
			string typename = rowColumn.GetString("DATA_TYPE");
			string output = null;
			using (DataView v = sn.ViewDataTypes)
			{
				foreach (DataRowView row in v)
					if (row.GetString("NativeDataType")==typename)
						output = row.GetString("NativeDataType");
			}
			if (output==null) return TypeCode.Empty;
			output = output.Replace("System.","");
			return (TypeCode) Enum.Parse(typeof(TypeCode),output);
		}
		
		/// <summary></summary>
		public override int GetMaxLength (DataSet dataSchema, DataRowView rowColumn)
		{
			if (!HasMaxLength(dataSchema,rowColumn)) return -1;
			return (int)rowColumn.Row["CHARACTER_MAXIMUM_LENGTH"];
		}
		
		/// <summary></summary>
		public override bool HasMaxLength (DataSet dataSchema, DataRowView rowColumn)
		{
			SchemaNavigator sn = new SchemaNavigator(dataSchema);
			//CHARACTER_MAXIMUM_LENGTH
			bool isNull = rowColumn["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value;
			return !isNull;
		}
		
		public override string GetPrimaryKey(DataSet dataSchema, DataRowView rowColumn)
		{
			SchemaNavigator sn = new SchemaNavigator(dataSchema);
			string tablename = rowColumn.GetString("TABLE_NAME");
			string output = null;
			using (DataView v = sn.ViewIndexes)
			{
//				output = (from u in v where u.TABLE_NAME == tablename select u).FirstOrDefault().GetString("INDEX_NAME");
				foreach (DataRowView row in v)
					if (row.GetString("TABLE_NAME")==tablename)
						output = row.GetString("TABLE_NAME");
			}
			sn = null;
			return output;
		}
	}
}
