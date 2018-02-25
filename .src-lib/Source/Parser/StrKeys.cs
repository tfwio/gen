using System;
namespace Generator.Parser
{
  /// <summary>
  /// This class is here to provide reference to the field tags
  /// usable within TableTemplate.
  /// </summary>
  static class StrKeys
  {
    // IsArray has no use.
    // , Native            = "Native" // should this be here?
    internal const string
      FieldValues         = "FieldValues"
      , FieldValuesCdf    = "FieldValues,Cdf"
      , FieldValuesNK     = "FieldValuesNK"
      , FieldValuesNKCdf  = "FieldValuesNK,Cdf"
      , FieldIndex        = "FieldIndex"
      , IsPrimaryKey      = "IsPrimaryKey"
      , PrimaryKey        = "PrimaryKey"
      // dataname
      , DataName          = "DataName"
      , dataname          = "dataname"
      , DataNameC         = "DataNameC"
      , CleanName_Nodash  = "CleanName,Nodash"
      , FriendlyName      = "FriendlyName"
      , CleanName         = "CleanName"
      , DataNameX         = "DataNameX"
      , FriendlyNameC     = "FriendlyNameC"
      // data alias for links and views
      , DataAlias         = "DataAlias"
      , dataalias         = "dataalias"
      , DataAliasC        = "DataAliasC"
      , CleanAlias_Nodash = "CleanAlias,Nodash"
      , FriendlyAlias     = "FriendlyAlias"
      , CleanAlias        = "CleanAlias"
      , FriendlyAliasC    = "FriendlyAliasC"
      // data type
      , DataType          = "DataType"
      , datatype          = "datatype"
      , DataTypeNative    = "DataTypeNative"
      , DataTypeNativeF   = "DataTypeNativeF"
      , datatypenative    = "datatypenative"
      // misc?
      , FlashDataType     = "FlashDataType"
      // 
      , MaxLMAX           = "MaxLMAX"
      , nmax              = "nmax"
      , smax              = "smax"
      , MaxLength         = "MaxLength"
      , CodeBlock         = "CodeBlock"
      , BlockAction       = "BlockAction"
      , FormType          = "FormType"
      , FormTypeClean     = "FormTypeClean"
      , formtype          = "formtype"
      , IsString          = "IsString"
      , IsBool            = "IsBool"
      , IsNum             = "IsNum"
      // 
      , NativeNullType    = "NativeNullType"
      , NativeNullValue   = "NativeNullValue"
      , NativeNullTypeGo  = "NativeNullTypeGo"
      // 
      , SqlFormat         = "SqlFormat"
      , Format            = "Format"
      , fmax              = "fmax"
      , max               = "max"
      , UseFormat         = "UseFormat" // bool
      , IsNullable        = "IsNullable" // bool
      , DataTypeNullable  = "DataTypeNullable"
      , Description       = "Description"
      , DefaultValue      = "DefaultValue"
      ;
      
  }
}


