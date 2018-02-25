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
    
    // ==========================================
    
    internal const string  PrimaryKey        = "PrimaryKey";
    
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
    internal const string  Link              = "Link";
    internal const string  View              = "View";
    
  }
}


