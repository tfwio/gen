/* oIo : 2/10/2011 : 9:52 PM */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Generator.Elements;
using System.Cor3.Parsers;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Generator.Core.Types
{
	public struct SCHEMA_REMAP
	{
		public string Name;
		
		public string TableFrom;
		public string FieldFrom;
		
		public string TableTo;
		public string FieldToMap;
		public string FieldTo;
		
		public SCHEMA_REMAP(string name, string tableFrom, string fieldFrom, string tableTo, string fieldTo, string fieldToMap)
		{
			Name = name;
			
			TableFrom = tableFrom;
			TableTo = tableTo;
			
			FieldFrom = fieldFrom;
			FieldTo = fieldTo;
			FieldToMap = fieldToMap;
		}
		static public List<SCHEMA_REMAP> GetAccessMapTable(params SCHEMA_REMAP[] values)
		{
			List<SCHEMA_REMAP> list = new List<SCHEMA_REMAP>();
			foreach (SCHEMA_REMAP map in values)
				list.Add(map);
			return list;
		}
		static public List<SCHEMA_REMAP> AccessMap()
		{
			// note the NativeDataType (int) field provided here
			//TypeCode,ProviderDbType,NativeDataType
			return GetAccessMapTable(
				// this provides Access's TypeName
				new SCHEMA_REMAP("AccessType","Columns","DATA_TYPE","DataType","TypeName","NativeDataType"),
				// this provides .Net TypeName such as System.Byte[]
				new SCHEMA_REMAP("NativeType","Columns","DATA_TYPE","DataType","DataType","NativeDataType")
			);
		}
	}
}
