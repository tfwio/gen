using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

using Generator.Elements.Basic;
using Generator.Elements.Types;

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
		
		
		[XmlAttribute, DefaultValue("")] public string DataTypeNative {
			get { return dataTypeNative; } set { dataTypeNative = value; OnPropertyChanged("DataTypeNative"); }
		} string dataTypeNative;
		[XmlAttribute] public string DataName {
			get { return dataName; } set { dataName = value; OnPropertyChanged("DataName"); }
		} string dataName;
		[XmlAttribute] public string FormatString {
			get { return formatString; } set { formatString = value; OnPropertyChanged("FormatString"); }
		} string formatString;
		[XmlAttribute, DefaultValue("")] public string BaseClass {
			get { return baseClass; } set { baseClass = value; OnPropertyChanged("BaseClass"); }
		} string baseClass;
		[XmlAttribute,DefaultValue(-1)] public int MaxLength {
			get { return maxLength; } set { maxLength = value; OnPropertyChanged("MaxLength"); }
		} int maxLength = -1;
		[DefaultValue(false),XmlAttribute] public bool UseFormat {
			get { return useFormat; } set { useFormat = value; OnPropertyChanged("UseFormat"); }
		} bool useFormat;
		
		[DefaultValue(true),XmlAttribute] public bool IsNullable {
			get { return isNullable; } set { isNullable = value; OnPropertyChanged("IsNullable"); }
		} bool isNullable = true;
		
		[XmlAttribute]
		public bool IsArray {
			get { return isArray; } set { isArray = value; OnPropertyChanged("IsArray"); }
		} bool isArray;
		
		[XmlIgnore]
		public bool IsPrimary {
			get { return parent.PrimaryKey == dataName; }
			set { if (value) Parent.PrimaryKey = dataName; OnPropertyChanged("IsPrimary"); }
		}
		
		[XmlAttribute]
		public string Description {
			get { return description; }
			set { description = value; OnPropertyChanged("Description"); }
		} string description;
		
		[XmlAttribute/*,DefaultValue("DBNull.Value")*/]
		public string DefaultValue {
			get { return defaultValue; }
			set { defaultValue = value; OnPropertyChanged("DefaultValue"); }
		} string defaultValue = "DBNull.Value";
		
		[XmlAttribute]
		public string BlockAction {
			get { return blockAction; }
			set { blockAction = value; OnPropertyChanged("BlockAction"); }
		} string blockAction;
		
		[XmlAttribute]
		public string CodeBlock {
			get { return codeBlock; }
			set { codeBlock = value; OnPropertyChanged("CodeBlock"); }
		} string codeBlock;
		
		[XmlAttribute]
		public string FormType {
			get { return formType; }
			set { formType = value; OnPropertyChanged("FormType"); }
		} string formType;

		/// <summary>
		/// Used internally by the parser
		/// </summary>
		[XmlIgnore]
		public DataViewElement View {
			get { return view; }
			set { view = value; OnPropertyChanged("View"); }
		} DataViewElement view;
		/// <summary>
		/// Used internally by the parser
		/// </summary>
		[XmlIgnore]
		public DataViewLink Link {
			get { return link; }
			set { link = value; OnPropertyChanged("Link"); }
		} DataViewLink link;
		
		#endregion

		#region This[String Key]
		/// <summary>
		/// This is to be used carefully, it Generates the entire (Params) Dictionary
		/// to simply reference a single element-value within.
		/// </summary>
		public object this[string Key]
		{
			get {
				return !Contains(Key) ? null : Params[Key];
			}
		}

		/// <summary>
		/// A helper method used to transform our internal variable dictionary
		/// for parser addons.
		/// </summary>
		[XmlIgnore]
		public Action<DICT<string,object>> Transform { get;set; }
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
		/// <seealso cref="Generator.Elements.Parser.TemplateFactory" />
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
				fparams.Add("DataName",					DataName);
				fparams.Add("dataname",					DataName.ToLower());
				// fixed 2012-08-22
				// fparams.Add("DataNameC",				dataName.ToStringCapitolize());
				fparams.Add("DataNameC",				DataName.ToStringCapitolize());
				fparams.Add("CleanName,Nodash",			DataName.Clean());
				fparams.Add("FriendlyName",				DataName.Clean());
				fparams.Add("CleanName",				DataName.Replace("-","_"));
				fparams.Add("DataNameX",				DataName.Replace("-","_").ReplaceId());
				fparams.Add("FriendlyNameC",			DataName.Clean().ToStringCapitolize());
				#endregion
				#region DataAlias (for links and views)
				try
				{
					fparams.Add("DataAlias",null);
					if (View!=null) fparams["DataAlias"] = string.Concat(View.Alias,".",DataName);
					if (Link!=null) fparams["DataAlias"] = string.Concat(Link.Alias,".",DataName);
					if (View==null && Link==null) fparams["DataAlias"] = fparams["DataName"];
					fparams.Add("dataalias",(fparams["DataAlias"] as string).ToLower());
					fparams.Add("DataAliasC",(fparams["DataAlias"] as string).ToStringCapitolize());
					fparams.Add("CleanAlias,Nodash",(fparams["DataAlias"] as string).Clean());
					fparams.Add("FriendlyAlias",(fparams["DataAlias"] as string).Clean());
					fparams.Add("CleanAlias",(fparams["DataAlias"] as string).Replace("-","_"));
					fparams.Add("FriendlyAliasC",(fparams["DataAlias"] as string).Clean().ToStringCapitolize());
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
				fparams.Add("DataType",					DataType??string.Empty);
				fparams.Add("datatype",					(DataType??string.Empty).ToLower());
				// Converted to a standard Native Type such as 'string'
				fparams.Add("DataTypeNative",			DataTypeNative??string.Empty);
				fparams.Add("DataTypeNativeF",			DataTypeNative??string.Empty);
				fparams.Add("datatypenative",			(DataTypeNative??string.Empty).ToLower());
				#endregion
				//
				// FIXME: We need to convert flash types also from Access/Ace Types.
				//		  This means that we do need to know what type our origin is
				//		  at this point.
				fparams.Add("FlashDataType",	As3TypeProvider.SystemTypeToFlashType(this.DataTypeNative));
//				fparams.Add("FlashDataType",	TypeInfo.SqlTypeStrToFlashType(DataType));
				// -------------------------
				// Formatting
				// -------------------------
				#region Formatting
				fparams.Add("FormatString",		FormatString);
				//
				fparams.Add("MaxLMAX",			MaxLength == -1 ? "MAX" : MaxLength.ToString());
				fparams.Add("nmax",				MaxLength == -1 ? "" : string.Format("({0})",MaxLength));
				fparams.Add("smax",				MaxLength == -1 ? "(MAX)" : string.Format("({0})",MaxLength));
				fparams.Add("MaxLength",		MaxLength);
				//
				fparams.Add("CodeBlock",		CodeBlock??String.Empty);
				fparams.Add("BlockAction",		BlockAction??String.Empty);
				// TODO: ADDED FIELD TYPE NAMES
				fparams.Add("FormType",			FormType??String.Empty);
				fparams.Add("FormTypeClean",	(FormType??String.Empty).Clean());
				fparams.Add("formtype",			(FormType??String.Empty).ToLower());
				//
				fparams.Add("IsString",			false);
				fparams.Add("IsBool",			false);
				fparams.Add("IsNum",			false);
				// -------------------------
				// IS-DATA-TYPE Properties
				// -------------------------
				switch (DataTypeNative) {
					case "String":
					case "Char":
						fparams["IsString"] = true;
						break;
					default:
						fparams["IsString"] = false;
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
				NativeTypeProvider.TypeCodeStringToNativeDictionary(DataTypeNative,fparams);
				//
				#region disabled
				#if no
				
				switch (DataTypeNative) {
					case "Single":
						fparams.Add("Native","float");
						break;
					case "Double":
						fparams.Add("Native","double");
						break;
					case "Decimal":
						fparams.Add("Native","Decimal");
						break;
					case "Boolean":
						fparams.Add("Native","bool");
						break;
						//					case "BigInt":		fparams.Add("Native","Int64"); break;
					case "DateTime":
						fparams.Add("Native","DateTime");
						break;
					case "DBNull":
						fparams.Add("Native","DBNull");
						break;
					case "Empty":
						fparams.Add("Native","Empty");
						break;
					case "Int16":
						fparams.Add("Native","short");
						break;
					case "Int32":
						fparams.Add("Native","int");
						break;
					case "Int64":
						fparams.Add("Native","long");
						break;
					case "UInt16":
						fparams.Add("Native","ushort");
						break;
					case "UInt32":
						fparams.Add("Native","uint");
						break;
					case "UInt64":
						fparams.Add("Native","ulong");
						break;
					case "String":
						fparams.Add("Native","string");
						break;
					default:
						fparams.Add("Native",DataTypeNative.ToLower());
						break;
				}
				#endif
				#endregion
				//
				#region CustomTypes
				fparams.Add("NativeNullType",	NullableTypeUtility.GetNativeNullType(fparams["Native"].ToString()));
				fparams.Add("NativeNullValue",	NullableTypeUtility.IsNativeNullable(fparams["Native"].ToString()) ? ".Value" : "");
				//
				if ( ((bool)fparams["IsString"] ) == true ) {
					fparams.Add("SqlFormat",	"'{0}'");
					fparams.Add("Format",		@"""{0}""");
					fparams.Add("fmax",			MaxLength == -1 ? "(max)" : string.Format("({0})",MaxLength));
				} else if ( ((bool)fparams["IsBool"] )==true ) {
					fparams.Add("Format",string.Empty);
					fparams.Add("fmax",string.Empty);
				} else if ( ((bool)fparams["IsNum"] )==true ) {
					if (UseFormat) fparams.Add("fmax",MaxLength==-1 ? "" : string.Format("({0})",MaxLength));
				}
				//
				switch (DataType) {
					case "Char":
					case "NChar":
					case "NText":
					case "NVarChar":
					case "Text":
					case "VarChar":
						fparams.Add("max", MaxLength == -1 ? "(max)" : string.Format("({0})",MaxLength));
						break;
					default:
						fparams.Add("max", MaxLength == -1 ? "" : string.Format("({0})",MaxLength));
						break;
				}
				#endregion
				//
				fparams.Add("UseFormat",	UseFormat);
				fparams.Add("IsNullable",	IsNullable);
				fparams.Add("DataTypeNullable",	IsNullable ? NullableTypeUtility.GetNativeNullType(fparams["Native"].ToString()) : fparams["Native"]);
				fparams.Add("Description",	Description);
				fparams.Add("DefaultValue",	DefaultValue);
				
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

		/// <summary>
		/// Used at the UI-level;  Should this be in our TREEV block?
		/// </summary>
		[XmlIgnore] public string ToolTip
		{
			get {
				return string.Format(
					"Name: “{0}”\nType: “{1}{2}”",
					this.DataName,
					this.DataType,
					this["fmax"]
				);
			}
		}

		public FieldElement(string n, string t)
		{
			DataType = t;
			DataName = n;
		}
		
		public FieldElement()
		{
		}

		public string Replace(string Input, Action<DICT<string,object>> action=null)
		{
			Transform = action;
			DICT<string,object> prams = Params;
			Transform = null;
			
			string rv = string.Copy(Input);
			foreach (KeyValuePair<string,object> entry in prams) {
				rv = rv
					.Replace(
						string.Format("$({0})",entry.Key),
						string.Format("{0}",entry.Value)
					)
					.Replace(
						string.Format("$({0})",entry.Key.CapitolN()),
						string.Format("{0}",entry.Value.ToStringCapitolize())
					);
			}
			return rv;
		}

	}
}
