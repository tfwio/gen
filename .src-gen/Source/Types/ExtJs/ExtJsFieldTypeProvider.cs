/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
#region Using
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace Generator.Elements.Types
{
	public class ExtJsFieldTypeProvider : EnumProvider
	{
		
		public override string Name { get { return "ExtJs 1.6 Field Type"; } }
		
		public override Array Types { get { return FieldType; } }
		
		public override TypeCode ToNative(string name)
		{
			switch (name)
			{
				case "CheckboxField":
				case "ComboBox":
				case "CompositeField":
				case "DateField":
				case "DisplayField":
				case "Drop":
				case "DropDownField":
				case "FileUploadField":
				case "Hidden":
				case "HtmlEditor":
				case "MultiCombo":
				case "MultiSelect":
				case "NumberField":
				case "SelectBox":
				case "SliderField":
				case "TextArea":
				case "TextField":
				case "TimeField":
				case "TriggerField":
					default: return TypeCode.String;
			}
		}
		
		public override bool CanDoToNative {
			get {
				return false;
			}
		}
		
		public override bool CanDoFromNative {
			get {
				return true;
			}
		}
		
		public Array FieldType
		{
			get {
				return ExtJsFieldType.GetValues(typeof(ExtJsFieldType));
			}
		}
		
		public override string ProvideTypeCode(TypeCode toConvert)
		{
			throw new NotImplementedException();
		}
	}
}
