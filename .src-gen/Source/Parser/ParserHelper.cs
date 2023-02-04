/* oIo : 11/15/2010 ? 2:33 AM */
#region Using
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using Generator.Core;
using Generator.Elements;
using Generator.Elements.Types;
using Generator.Core.Markup;
using SqlDbType = System.Data.SqlDbType;

#endregion

/*
 * This is designed to be imported into another project as an include.
 */
namespace Generator.Core
{
	class ParserHelper
	{
		internal static bool FieldReferencesKey(string input) { return Contains("FieldValuesNK",input); }
		internal static bool Contains(string Key, string input) { return input.Contains(Key); }
		public static string ReplaceValues(TableElement table, string tableTemplate)
		{
			table.InitializeDictionary();
			return table.Reformat(tableTemplate);
		}
		internal static string ReplaceFieldValues(string input, params string[] values)
		{
			string cFieldOut = string.Join(",",values);
			string fieldOut = string.Join(string.Empty,values);
			return input
				.ReplaceP("FieldValues",fieldOut)
				.ReplaceP("FieldValuesNK",fieldOut)
				.ReplaceP("FieldValues,Cdf",cFieldOut)
				.ReplaceP("FieldValuesNK,Cdf",cFieldOut);
		}
		public static List<string> GetParamStrings(TableElement table, string fieldTemplate, bool noKey)
		{
			List<string> paramStrings = new List<string>();
			int counter=0;
			foreach (FieldElement field in table.Fields)
			{
				string fieldGen = fieldTemplate;
				bool isPrimary = table.PrimaryKey==field["DataName"].ToString();
				if (noKey & isPrimary) continue;
				fieldGen = fieldGen
					.ReplaceP("FieldIndex",counter++)
					.ReplaceP("IsPrimary",isPrimary)
					.ReplaceP("PrimaryKey",table.PrimaryKey);
				
				fieldGen = field.Replace(fieldGen);
				paramStrings.Add(fieldGen);
			}
			return paramStrings;
		}
	}
}
