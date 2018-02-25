using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

using Generator.Elements.Basic;
using Generator.Elements.Types;
using Generator.Parser;

#if TREEV
using System.Windows.Forms;
#endif
namespace Generator.Elements
{
	/// <summary>
	/// a field element
	/// </summary>
	public partial class FieldElement : DataMapElement
	{
		[XmlIgnore] public TableElement Parent {
			get { return parent; }
			set { parent = value; OnPropertyChanged("Parent"); }
		} TableElement parent;
		
		static public FieldElement Clone(FieldElement element)
		{
			var field = new FieldElement();
			field.BaseClass = element.BaseClass;
			field.BlockAction = element.BlockAction;
			field.CodeBlock = element.CodeBlock;
			field.ConnectionParameters = element.ConnectionParameters;
			field.DataName = element.DataName;
			field.DataType = element.DataType;
			field.DataTypeNative = element.DataTypeNative;
			field.DefaultValue = element.DefaultValue;
			field.Description = element.Description;
			field.FormatString = element.FormatString;
			field.FormType = element.FormType;
			field.IsArray = element.IsArray;
			field.IsNullable = element.IsNullable;
			field.IsPrimary = false;
			field.MaxLength = element.MaxLength;
//			field.Parent = element.Parent;
			field.Tags = element.Tags;
//			field.ToolTip = element.ToolTip;
			field.Transform = element.Transform;
			field.UseFormat = element.UseFormat;
			return field;
		}
		
		//Dictionary<string,Func<object,string>> Utils = new Dictionary<string, Func<object, string>>();
		
		#region Properties
		
		/// <summary>
		/// This is going to be our Engine Specific Type such as Ole, Access, SqlServer, etc.
		/// There currently is no way of telling what our specific type is at this point, however
		/// that is due to change.
		/// </summary>
		
		[XmlAttribute/*,DefaultValue("Text")*/] public string DataType {
			get { return dataType; }
			set { dataType = value; OnPropertyChanged("DataType"); }
		} string dataType = "Text";
		
		
		[XmlAttribute, DefaultValue("")]                public string DataTypeNative { get { return dataTypeNative; } set { dataTypeNative = value; OnPropertyChanged("DataTypeNative"); } } string dataTypeNative;
		[XmlAttribute]                                  public string DataName       { get { return dataName; } set { dataName = value; OnPropertyChanged("DataName"); } } string dataName;
		[XmlAttribute]                                  public string FormatString   { get { return formatString; } set { formatString = value; OnPropertyChanged("FormatString"); } } string formatString;
		[XmlAttribute, DefaultValue("")]                public string BaseClass      { get { return baseClass; } set { baseClass = value; OnPropertyChanged("BaseClass"); } } string baseClass;
		[XmlAttribute,DefaultValue(-1)]                 public int    MaxLength      { get { return maxLength; } set { maxLength = value; OnPropertyChanged("MaxLength"); } } int maxLength = -1;
		[DefaultValue(false),XmlAttribute]              public bool   UseFormat      { get { return useFormat; } set { useFormat = value; OnPropertyChanged("UseFormat"); } } bool useFormat;
		[DefaultValue(true),XmlAttribute]               public bool   IsNullable     { get { return isNullable; } set { isNullable = value; OnPropertyChanged("IsNullable"); } } bool isNullable = true;
		[XmlAttribute]                                  public bool   IsArray        { get { return isArray; } set { isArray = value; OnPropertyChanged("IsArray"); } } bool isArray;
		[XmlIgnore]                                     public bool   IsPrimary      { get { return parent.PrimaryKey == dataName; } set { if (value) Parent.PrimaryKey = dataName; OnPropertyChanged("IsPrimary"); } }
		[XmlAttribute]                                  public string Description    { get { return description; } set { description = value; OnPropertyChanged("Description"); } } string description;
		[XmlAttribute/*,DefaultValue("DBNull.Value")*/] public string DefaultValue   { get { return defaultValue; } set { defaultValue = value; OnPropertyChanged("DefaultValue"); } } string defaultValue = "DBNull.Value";
		[XmlAttribute]                                  public string BlockAction    { get { return blockAction; } set { blockAction = value; OnPropertyChanged("BlockAction"); } } string blockAction;
		[XmlAttribute]                                  public string CodeBlock { get { return codeBlock; } set { codeBlock = value; OnPropertyChanged("CodeBlock"); } } string codeBlock;
		[XmlAttribute]                                  public string FormType { get { return formType; } set { formType = value; OnPropertyChanged("FormType"); } } string formType;
		/// <summary>Used internally by the parser</summary>
		[XmlIgnore]                                     public DataViewElement View { get { return view; } set { view = value; OnPropertyChanged("View"); } } DataViewElement view;
		/// <summary>Used internally by the parser</summary>
		[XmlIgnore]                                     public DataViewLink Link { get { return link; } set { link = value; OnPropertyChanged("Link"); } } DataViewLink link;
		
		#endregion

		#region This[String Key]
		
		/// <summary>This is to be used carefully, it Generates the entire (Params) Dictionary to simply reference a single element-value within.</summary>
		public object this[string Key]
		{
			get {
				return !Contains(Key) ? null : Params[Key];
			}
		}
		/// <summary>A helper method used to transform our internal variable dictionary for parser addons.</summary>
		[XmlIgnore] public Action<DICT<string,object>> Transform { get;set; }
		
		/// <summary>
		/// THE Dictionary of KeyValuePair of Values contained within this Field.
		/// <remarks>
		/// Try not to use more then one copy of this property.
		/// <para>
		/// Inevitably (in the future), this element is going to have to be able to add additional
		/// ENTITY's perhaps provided by the likes of combonations of XSLT, XSD, DTD, …
		/// </para>
		/// <para>
		/// That could as easly mean providing a serialization provision such as
		/// implemented by the old “host” project and the ‘Serial’ class
		/// that adds 'Serialization.Types' to a specific element bound to Serialization.
		/// </para>
		/// </remarks>
		/// <seealso cref="Generator.Parser.TemplateFactory" />
		/// </summary>
		[XmlIgnore] public DICT<string,object> Params
		{
			get
			{
				//
				var fparams = new DICT <string,object>();
				// we're gearing towards usage such as this using reflection
				// as opposed to this particular static method.
				GeneratorTypeProvider.GetTypes<GeneratorDateTimeFieldProvider>(input: fparams);
				// -------------------------
				// DataName
				// -------------------------
				#region DataName
				fparams.Add(StrKeys.DataName,					DataName);
				fparams.Add(StrKeys.dataname,					DataName.ToLower());
				// fixed 2012-08-22
				// fparams.Add(StrKeys.DataNameC,			dataName.ToStringCapitolize());
				fparams.Add(StrKeys.DataNameC,				DataName.ToStringCapitolize());
				fparams.Add(StrKeys.CleanName_Nodash,	DataName.Clean());
				fparams.Add(StrKeys.FriendlyName,			DataName.Clean());
				fparams.Add(StrKeys.CleanName,				DataName.Replace("-","_"));
				fparams.Add(StrKeys.DataNameX,				DataName.Replace("-","_").ReplaceId());
				fparams.Add(StrKeys.FriendlyNameC,		DataName.Clean().ToStringCapitolize());
				#endregion
				#region DataAlias (for links and views)
				try
				{
					fparams.Add(StrKeys.DataAlias,null);
					if (View!=null) fparams["DataAlias"] = string.Concat(View.Alias,".",DataName);
					if (Link!=null) fparams["DataAlias"] = string.Concat(Link.Alias,".",DataName);
					if (View==null && Link==null) fparams["DataAlias"] = fparams["DataName"];
					fparams.Add(StrKeys.dataalias,(fparams["DataAlias"] as string).ToLower());
					fparams.Add(StrKeys.DataAliasC,(fparams["DataAlias"] as string).ToStringCapitolize());
					fparams.Add(StrKeys.CleanAlias_Nodash,(fparams["DataAlias"] as string).Clean());
					fparams.Add(StrKeys.FriendlyAlias,(fparams["DataAlias"] as string).Clean());
					fparams.Add(StrKeys.CleanAlias,(fparams["DataAlias"] as string).Replace("-","_"));
					fparams.Add(StrKeys.FriendlyAliasC,(fparams["DataAlias"] as string).Clean().ToStringCapitolize());
				}
				catch (Exception error)
				{
					Logger.Warn("Field Parser Error",error.Message);
				}
				#endregion
				// -------------------------
				// DataType
				// -------------------------
				#region DataType
				fparams.Add(StrKeys.DataType,					DataType??string.Empty);
				fparams.Add(StrKeys.datatype,					(DataType??string.Empty).ToLower());
				// Converted to a standard Native Type such as 'string'
				fparams.Add(StrKeys.DataTypeNative,		DataTypeNative??string.Empty);
				fparams.Add(StrKeys.DataTypeNativeF,	DataTypeNative??string.Empty);
				fparams.Add(StrKeys.datatypenative,		(DataTypeNative??string.Empty).ToLower());
				#endregion
				//
				// FIXME: We need to convert flash types also from Access/Ace Types.
				//		  This means that we do need to know what type our origin is
				//		  at this point.
				fparams.Add(StrKeys.FlashDataType,	As3TypeProvider.SystemTypeToFlashType(this.DataTypeNative));
//				fparams.Add(StrKeys.FlashDataType,	TypeInfo.SqlTypeStrToFlashType(DataType));
				// -------------------------
				// Formatting
				// -------------------------
				#region Formatting
				fparams.Add(StrKeys.FormatString,		FormatString);
				//
				fparams.Add(StrKeys.MaxLMAX,		 MaxLength == -1 ? "MAX" : MaxLength.ToString());
				fparams.Add(StrKeys.nmax,				 MaxLength == -1 ? "" : string.Format("({0})",MaxLength));
				fparams.Add(StrKeys.smax,				 MaxLength == -1 ? "(MAX)" : string.Format("({0})",MaxLength));
				fparams.Add(StrKeys.MaxLength,	 MaxLength);
				//
				fparams.Add(StrKeys.CodeBlock,	 CodeBlock??String.Empty);
				fparams.Add(StrKeys.BlockAction, BlockAction??String.Empty);
				// TODO: ADDED FIELD TYPE NAMES
				fparams.Add(StrKeys.FormType,			FormType??String.Empty);
				fparams.Add(StrKeys.FormTypeClean,	(FormType??String.Empty).Clean());
				fparams.Add(StrKeys.formtype,			(FormType??String.Empty).ToLower());
				//
				fparams.Add(StrKeys.IsString,		false);
				fparams.Add(StrKeys.IsBool,			false);
				fparams.Add(StrKeys.IsNum,			false);
				// -------------------------
				// IS-DATA-TYPE Properties
				// -------------------------
				switch (DataTypeNative) {
					case "String":
					case "Char":
						fparams[StrKeys.IsString] = true;
						break;
					default:
						fparams[StrKeys.IsString] = false;
						break;
				}
				#endregion
				//
				#region DataTypeNative
				switch (DataTypeNative) {
					case "Boolean":
					case "DateTime":
					case "DBNull":
					case "Empty":
					case "String":
						fparams["IsNum"] = false;
						break;
					default:
						fparams["IsNum"] = true;
						break;
				}
				//
				switch (DataTypeNative) {
					case "Boolean":
						fparams["IsBool"] = (true);
						break;
					default:
						fparams["IsBool"] = false;
						break;
				}
				#endregion
				//
				DataTypeNative.TypeCodeStringToNativeDictionary(fparams);
				//
				#region disabled
				#if no
				switch (DataTypeNative) {
					case "Single":
						fparams.Add(StrKeys.Native,"float");
						break;
					case "Double":
						fparams.Add(StrKeys.Native,"double");
						break;
					case "Decimal":
						fparams.Add(StrKeys.Native,"Decimal");
						break;
					case "Boolean":
						fparams.Add(StrKeys.Native,"bool");
						break;
						//					case "BigInt":		fparams.Add(StrKeys.Native,"Int64"); break;
					case "DateTime":
						fparams.Add(StrKeys.Native,"DateTime");
						break;
					case "DBNull":
						fparams.Add(StrKeys.Native,"DBNull");
						break;
					case "Empty":
						fparams.Add(StrKeys.Native,"Empty");
						break;
					case "Int16":
						fparams.Add(StrKeys.Native,"short");
						break;
					case "Int32":
						fparams.Add(StrKeys.Native,"int");
						break;
					case "Int64":
						fparams.Add(StrKeys.Native,"long");
						break;
					case "UInt16":
						fparams.Add(StrKeys.Native,"ushort");
						break;
					case "UInt32":
						fparams.Add(StrKeys.Native,"uint");
						break;
					case "UInt64":
						fparams.Add(StrKeys.Native,"ulong");
						break;
					case "String":
						fparams.Add(StrKeys.Native,"string");
						break;
					default:
						fparams.Add(StrKeys.Native,DataTypeNative.ToLower());
						break;
				}
				#endif
				#endregion
				//
				#region CustomTypes
				fparams.Add(StrKeys.NativeNullType,	NullableTypeUtility.GetNativeNullType(fparams["Native"].ToString()));
				fparams.Add(StrKeys.NativeNullValue,	NullableTypeUtility.IsNativeNullable(fparams["Native"].ToString()) ? ".Value" : "");
				fparams.Add(StrKeys.NativeNullTypeGo,  NullableTypeUtility.GetNativeNullableGoType(fparams["DataTypeNative"].ToString()));
				//
				if ( ((bool)fparams["IsString"] ) == true ) {
					fparams.Add(StrKeys.SqlFormat,	"'{0}'");
					fparams.Add(StrKeys.Format,		@"""{0}""");
					fparams.Add(StrKeys.fmax,			MaxLength == -1 ? "(max)" : string.Format("({0})",MaxLength));
				} else if ( ((bool)fparams["IsBool"] )==true ) {
					fparams.Add(StrKeys.Format,string.Empty);
					fparams.Add(StrKeys.fmax,string.Empty);
				} else if ( ((bool)fparams["IsNum"] )==true ) {
					if (UseFormat) fparams.Add(StrKeys.fmax,MaxLength==-1 ? "" : string.Format("({0})",MaxLength));
				}
				//
				switch (DataType) {
					case "Char":
					case "NChar":
					case "NText":
					case "NVarChar":
					case "Text":
					case "VarChar":
						fparams.Add(StrKeys.max, MaxLength == -1 ? "(max)" : string.Format("({0})",MaxLength));
						break;
					default:
						fparams.Add(StrKeys.max, MaxLength == -1 ? "" : string.Format("({0})",MaxLength));
						break;
				}
				#endregion
				//
        fparams.Add(StrKeys.UseFormat,  UseFormat);
				fparams.Add(StrKeys.IsNullable,	IsNullable);
				fparams.Add(StrKeys.DataTypeNullable,	IsNullable ? NullableTypeUtility.GetNativeNullType(fparams["Native"].ToString()) : fparams["Native"]);
				fparams.Add(StrKeys.Description,	Description);
				fparams.Add(StrKeys.DefaultValue,	DefaultValue);
				
				if (Transform!=null) Transform(fparams);
				
				return fparams;
			}
		}

		public bool Contains(string Key)
		{
			bool returned = false;
			DICT<string,object> prams = Params;
			returned = prams.ContainsKey(Key);
			prams.Clear();
			prams = null;
			return returned;
		}
		
		#endregion

		/// <summary>Used at the UI-level;  Should this be in our TREEV block?</summary>
		[XmlIgnore] public string ToolTip { get { return string.Format( "Name: “{0}”\nType: “{1}{2}”", this.DataName, this.DataType, this["fmax"] ); } }

		public FieldElement(string n, string t) { DataType = t; DataName = n; }
		
		public FieldElement() { }

		/// <summary>
		/// Generates the field's dictionary and performs string-replace on string-input
    /// against each key-value-pair of the field values stored to dictionary (memory).
		/// </summary>
		/// <param name="Input"></param>
		/// <param name="action">never seems to be used.</param>
		/// <returns></returns>
		public string Replace(string Input, Action<DICT<string,object>> action=null)
		{
		  Transform = action; Transform = null; // < this is never used.
		  DICT<string,object> prams = Params;
		  string rv = string.Copy(Input);
		  foreach (KeyValuePair<string,object> entry in prams)
		  {
		    rv = rv
		      .Replace( string.Format("$({0})", entry.Key), string.Format("{0}",entry.Value) )
		      .Replace( string.Format("$({0})", entry.Key.CapitolN()), string.Format("{0}", entry.Value.ToStringCapitolize()) );
			}
			return rv;
		}
	}
}
