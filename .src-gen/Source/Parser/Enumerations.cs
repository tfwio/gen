using System;
using System.Xml.Serialization;

namespace Generator.Parser
{
	#region Referenced Enumerations
	public enum NsTypes { Global, AdapterTypes, DatabaseTypes, SqlTypes, TableTypes, FieldTypes }
	public enum TemplateType { TableTemplate,FieldTemplate,TemplatePreview, }
	#endregion
	
	#region NOT REFERENCED
	public enum FieldCategory { Database, Table, Field, Global, }
	public enum NsTableTypes { TableName, TableNameC, TableCleanName, TableCleanNameC, TableNameClean, tablenameclean, TableNameCClean, TableType, tabletype, }
	public enum NsSqlTypes { None, Delete, Insert, Select, Update, }
	public enum NsAdapter { AdapterNs,AdapterT,AdapterNsT, CommandNs,CommandT,CommandNsT, ConnectionT,ConnectionNsT,ConnectionNs, ReaderNslReaderT,ReaderNsT }
	public enum NsDbTypes { DbType, FriendlyName }
	public enum NsFieldTypes { FieldValues, [XmlEnum("FieldValues,Cdf")] FieldValues_Cdf, FieldValuesNK, [XmlEnum("FieldValuesNK,Cdf")] FieldValuesNK_Cdf }
	#endregion
}
