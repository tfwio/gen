/*
 * Created by SharpDevelop.
 * User: oIo
 * Date: 2/10/2011
 * Time: 9:52 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
#region Using
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Generator.Elements;
using System.Cor3.Parsers;
using System.Data;
using System.Text.RegularExpressions;
#endregion

#if WPF4
using System.Windows.Input;
#else
//?
#endif

namespace Generator.Parser
{
	// See SqlTemplateParser.cs
	
	/// <summary>
	/// used in REPLACEMENT_Extension, and SqlTemplateParser indirectly.
	/// </summary>
	public struct REPLACEMENT
	{
		public string OldValue;
		public string NewValue;
		
		public string Replace(string input) { return input.Replace(OldValue,NewValue); }
		
		public REPLACEMENT(string oldValue, string newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}
	}
	
	/// <summary>
	/// The extension contains a single REPLACE method ...
	/// </summary>
	static public class REPLACEMENT_Extension
	{
		// known to be used in SqlTemplateParser
		static public string REPLACE(this string input, params REPLACEMENT[] values)
		{
			if (values==null) return input;
			string output = string.Copy(input);
			foreach (REPLACEMENT replacement in values) output = replacement.Replace(output);
			return output;
		}
	}
}
