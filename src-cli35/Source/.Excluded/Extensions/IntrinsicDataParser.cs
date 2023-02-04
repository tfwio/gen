/**
 * oIo * 2/15/2011 11:12 AM
 **/
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using Generator.DatabaseBindingExtension;

namespace Generator.Classes
{
	static public class IntrinsicDataParserExtension
	{
//		static string RegexOr(this string input, params string[] values) { return string.Concat(input,string.Concat); }
		static string RegexName(this string input, string name)
		{
			return string.Format("(?<{0}>{1})",name,input);
		}
	}
	//tDriver,customerContacts,order,voucher,rateFlat,rateFlatType,transports,driver_map,viewDriverTransports
	public class IntrinsicDataParser
	{
		const string expDescriptorRegion	= "#region[^][\n]*";
		const string expDescriptorEndregion	= "#endregion[^][\n]*";
		
		const string expCommentBlock		= @"/\*[^\]";
		const string expCommentLine			= @"//.*\n";
		
		const string expDescriptorRoot		= @"internal|protected|static|const";
		const string expDescriptorPragma	= @"unsafe"; //checked|unchecked
		
		static readonly string expDescriptorType = string.Join("|",TypeCode.GetNames(typeof(TypeCode)));
	}
}
