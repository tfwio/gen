using System;
using Generator.Core.Entities;

namespace Generator.Core
{
	static class ElementTypeUtility
	{
		/// <summary>
		/// Warning: this method uses System.Windows.Forms.MessageBox.
		/// </summary>
		/// <param name="element"></param>
		static public void GetTableElementDictionary(this TableElement element)
		{
			element.conversionElements.Clear();
			// ac_table
			element.conversionElements.Add("TableType", element.DbType);
			element.conversionElements.Add("tabletype", string.Format("{0}", element.DbType).ToLower());
			// see the primary key related tags below
			element.conversionElements.Add("PK", element.PrimaryKey);
			element.conversionElements.Add("pk", element.PrimaryKey == null ? string.Empty : element.PrimaryKey.ToLower());
			element.conversionElements.Add("PrimaryKey", element.PrimaryKey == null ? string.Empty : element.PrimaryKey);
			element.conversionElements.Add("PrimaryKeyCleanC", element.PrimaryKey == null ? string.Empty : element.PrimaryKey.ToStringCapitolize().Clean());
			element.conversionElements.Add("primarykey", element.PrimaryKey == null ? string.Empty : element.PrimaryKey.ToLower());
			//
			element.conversionElements.Add("Table", element.Name.Replace("-", "_"));
			//
			element.conversionElements.Add("TableName", element.Name);
			element.conversionElements.Add("tablename", element.Name.ToLower());
			//
			element.conversionElements.Add("TableNameC", element.Name.ToStringCapitolize());
			element.conversionElements.Add("TableNameClean", element.FriendlyName);
			element.conversionElements.Add("tablenameclean", element.FriendlyName.ToLower());
			element.conversionElements.Add("TableNameCClean", element.FriendlyName.ToStringCapitolize());
			//
			element.conversionElements.Add("TableCleanName", element.Name.Clean());
			element.conversionElements.Add("TableCleanNameC", element.Name.Clean().ToStringCapitolize());
			//
			element.conversionElements.Add("AdapterNs", element.NsAdapter);
			element.conversionElements.Add("AdapterT", element.TAdapter);
			element.conversionElements.Add("AdapterNsT", element.TypeAdapter);
			//
			element.conversionElements.Add("CommandNs", element.NsCommand);
			element.conversionElements.Add("CommandT", element.TCommand);
			element.conversionElements.Add("CommandNsT", element.TypeCommand);
			//
			element.conversionElements.Add("ConnectionNs", element.NsConnection);
			element.conversionElements.Add("ConnectionT", element.TConnection);
			element.conversionElements.Add("ConnectionNsT", element.TypeConnection);
			//
			element.conversionElements.Add("ParameterT", element.TParameter);
			//
			element.conversionElements.Add("ReaderNs", element.NsReader);
			element.conversionElements.Add("ReaderT", element.TReader);
			element.conversionElements.Add("ReaderNsT", element.TypeReader);
			//
			element.conversionElements.Add("Date", DateTime.Now.ToString("MM/dd/yyyy"));
			element.conversionElements.Add("Time", DateTime.Now.ToString("hh:mm.ss tt"));
			element.conversionElements.Add("DateTime", string.Format("{0:MM/dd/yyyy} {1:hh:mm.ss tt}", DateTime.Now, DateTime.Now));
			//
			if (element.PrimaryKey == null) {
				System.Windows.Forms.MessageBox.Show("Table must provide a primary key", "Please check the table.");
				return;
			} else if (element.PrimaryKey != string.Empty) {
				element.conversionElements.Add("PKDataName", element.PrimaryKeyElement.DataName);
				element.conversionElements.Add("PKDataType", element.PrimaryKeyElement.DataType);
				element.conversionElements.Add("PKDataTypeNative", element.PrimaryKeyElement.DataTypeNative);
				element.conversionElements.Add("PKNativeNullType", NullableTypeUtility.GetNativeNullType(element.PrimaryKeyElement["Native"].ToString()));
				element.conversionElements.Add("PKNativeNullValue", NullableTypeUtility.IsNativeNullable(element.PrimaryKeyElement["Native"].ToString()) ? ".Value" : "");
	//				Add("PKDataTypeNative",		PrimaryKeyElement.DataTypeNative);
				element.conversionElements.Add("PKDescription", element.PrimaryKeyElement.Description);
				//
				element.conversionElements.Add("PKDataNameC", element.PrimaryKeyElement.DataName.ToStringCapitolize());
				element.conversionElements.Add("PKCleanName", element.PrimaryKeyElement.DataName.Replace("-", "_"));
				element.conversionElements.Add("PKCleanName,Nodash", element.PrimaryKeyElement.DataName.Clean());
				element.conversionElements.Add("PKFriendlyName", element.PrimaryKeyElement.DataName.Clean());
				element.conversionElements.Add("PKFriendlyNameC", element.PrimaryKeyElement.DataName.Clean().ToStringCapitolize());
			} else {
				Global.statR(ResourceUtil.ResourceManager.GetString("TableElement_PrimaryKeyNotFound"), element.Name);
			}
		}
		
		static string TypeName(Type t) { return string.Format("{0}.{1}", t.Namespace, t.Name); }
		
		static public string __TypeName(this Type t) { return TypeName(t); }
		
		static string TypeN(this Type t) { return t.Name; }
		
		static public string __TypeN(this Type t) { return TypeN(t); }
		
		static string NsName(Type t) { return t.Namespace; }
		
		static public string __NsName(this Type t) { return NsName(t); }
	
	}
}
