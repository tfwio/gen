using System;
namespace Generator.Parser
{
  /* 
   * useful regex-replace 1
   * 
   * regex="StrKeys.([\w,]*)"
   * replace=StrKeys.$1
   *
   * useful regex-replace 2
   * 
   * regex=^      ,([^\n]*)
   * replace=      internal const string $1;
   */
  /// <summary>
  /// This class is here to provide reference to the field tags
  /// usable within TableTemplate.
  /// 
  /// trace usage of each of these tags from code-perspective (IDE)
  /// via some function like a 'Find References' context-method.
  /// </summary>
  static class StrKeys
  {
    
    /// <summary>
    /// Some comment
    /// </summary>
    internal const string Native            = "Native"; // Not properly categorized?
    
    // ==========================================
    
    internal const string  BaseClass         = "BaseClass";
    internal const string  FormatString      = "FormatString";
    
    // ==========================================
    
    internal const string  FieldValues       = "FieldValues";
    internal const string  FieldValuesCdf    = "FieldValues,Cdf";
    internal const string  FieldValuesNK     = "FieldValuesNK";
    internal const string  FieldValuesNKCdf  = "FieldValuesNK,Cdf";
    internal const string  FieldIndex        = "FieldIndex";
    internal const string  IsPrimaryKey      = "IsPrimaryKey";
    
    // dataname
    // ==========================================
    
    internal const string  DataName          = "DataName";
    internal const string  dataname          = "dataname";
    internal const string  DataNameC         = "DataNameC";
    internal const string  CleanName_Nodash  = "CleanName,Nodash";
    internal const string  FriendlyName      = "FriendlyName";
    internal const string  CleanName         = "CleanName";
    internal const string  DataNameX         = "DataNameX";
    internal const string  FriendlyNameC     = "FriendlyNameC";
    
    // data alias for links and views
    // ==========================================
    
    internal const string  DataAlias         = "DataAlias";
    internal const string  dataalias         = "dataalias";
    internal const string  DataAliasC        = "DataAliasC";
    internal const string  CleanAlias_Nodash = "CleanAlias,Nodash";
    internal const string  FriendlyAlias     = "FriendlyAlias";
    internal const string  CleanAlias        = "CleanAlias";
    internal const string  FriendlyAliasC    = "FriendlyAliasC";
    
    // data type
    // ==========================================
    
    internal const string  DataType          = "DataType";
    internal const string  datatype          = "datatype";
    internal const string  DataTypeNative    = "DataTypeNative";
    internal const string  DataTypeNativeF   = "DataTypeNativeF";
    internal const string  datatypenative    = "datatypenative";
    
    // misc?
    // ==========================================
    
    internal const string  FlashDataType     = "FlashDataType";
    
    // ==========================================
    
    internal const string  MaxLMAX           = "MaxLMAX";
    internal const string  nmax              = "nmax";
    internal const string  smax              = "smax";
    internal const string  MaxLength         = "MaxLength";
    internal const string  CodeBlock         = "CodeBlock";
    internal const string  BlockAction       = "BlockAction";
    internal const string  FormType          = "FormType";
    internal const string  FormTypeClean     = "FormTypeClean";
    internal const string  formtype          = "formtype";
    internal const string  IsString          = "IsString";
    internal const string  IsBool            = "IsBool";
    internal const string  IsNum             = "IsNum";
    
    // ==========================================
    
    internal const string  NativeNullType    = "NativeNullType";
    internal const string  NativeNullValue   = "NativeNullValue";
    internal const string  NativeNullTypeGo  = "NativeNullTypeGo";
    
    // ==========================================
    
    internal const string  SqlFormat         = "SqlFormat";
    internal const string  Format            = "Format";
    internal const string  fmax              = "fmax";
    internal const string  max               = "max";
    internal const string  UseFormat         = "UseFormat"; // bool
    internal const string  IsNullable        = "IsNullable"; // bool
    internal const string  DataTypeNullable  = "DataTypeNullable";
    internal const string  Description       = "Description";
    internal const string  DefaultValue      = "DefaultValue";
    
    // things that were not here? we might find these in additional elements (table, database, ...)
    // ==========================================
    
    internal const string  IsArray           = "IsArray"; // (hasn't proven useful);
    
    // TableElement values (there are probably some up there and all...)
    // ==========================================
    
    internal const string  Link                = "Link";
    internal const string  View                = "View";
    internal const string  TableType           = "TableType";
    internal const string  tabletype           = "tabletype";
    internal const string  PK                  = "PK";
    internal const string  pk                  = "pk";
    internal const string  PrimaryKey          = "PrimaryKey";
    internal const string  PrimaryKeyCleanC    = "PrimaryKeyCleanC";
    internal const string  primarykey          = "primarykey";
    internal const string  Name                = "Name";
    internal const string  Table               = "Table";
    internal const string  TableAlias          = "TableAlias";
    internal const string  _TableAlias         = "_TableAlias";
    internal const string  tablealias          = "tablealias";
    internal const string  TableAliasC         = "TableAliasC";
    internal const string  FriendlyTableAlias  = "FriendlyTableAlias";
    internal const string  TableAliasClean     = "TableAliasClean";
    internal const string  tablealiasclean     = "tablealiasclean";
    internal const string  TableAliasCClean    = "TableAliasCClean";
    internal const string  TABLEALIASCLEAN     = "TABLEALIASCLEAN";
    internal const string  TableAliasCName     = "TableAliasCName";
    internal const string  TableAliasCNameC    = "TableAliasCNameC";
    internal const string  TableName           = "TableName";
    internal const string  tablename           = "tablename";
    internal const string  TableNameC          = "TableNameC";
    internal const string  TableNameClean      = "TableNameClean";
    internal const string  tablenameclean      = "tablenameclean";
    internal const string  TableNameCClean     = "TableNameCClean";
    internal const string  TableCleanName      = "TableCleanName";
    internal const string  TableCleanNameC     = "TableCleanNameC";
    internal const string  AdapterNs           = "AdapterNs";
    internal const string  AdapterT            = "AdapterT";
    internal const string  AdapterNsT          = "AdapterNsT";
    internal const string  CommandNs           = "CommandNs";
    internal const string  CommandT            = "CommandT";
    internal const string  CommandNsT          = "CommandNsT";
    internal const string  ConnectionNs        = "ConnectionNs";
    internal const string  ConnectionT         = "ConnectionT";
    internal const string  ConnectionNsT       = "ConnectionNsT";
    internal const string  ParameterT          = "ParameterT";
    internal const string  ReaderNs            = "ReaderNs";
    internal const string  ReaderT             = "ReaderT";
    internal const string  ReaderNsT           = "ReaderNsT";
    internal const string  PKDataName          = "PKDataName";
    internal const string  PKDataType          = "PKDataType";
    internal const string  PKDataTypeNative    = "PKDataTypeNative";
    internal const string  PKNativeNullType    = "PKNativeNullType";
    internal const string  PKNativeNullValue   = "PKNativeNullValue";
    internal const string  PKNativeNullValueGo = "PKNativeNullValueGo";
    internal const string  PKDescription       = "PKDescription";
    internal const string  PKDataNameC         = "PKDataNameC";
    internal const string  PKCleanName         = "PKCleanName";
    internal const string  PKCleanName_Nodash  = "PKCleanName,Nodash";
    internal const string  PKFriendlyName      = "PKFriendlyName";
    internal const string  PKFriendlyNameC     = "PKFriendlyNameC";
    
    
  }
}


