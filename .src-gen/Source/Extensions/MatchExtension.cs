/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 07/18/2011
 * Time: 07:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
#region Using
using System;
using System.Collections.Generic;
using System.Cor3.Data.Engine;
using System.Cor3.Parsers;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
#endregion

namespace Generator.Extensions
{

	static public class MatchExtension
	{
		static public Regex m_expression = new Regex(@"{([\w\n_ -]*):?([^}]*)}",RegexOptions.Multiline|RegexOptions.ECMAScript);
		internal const string mformat = "match=‘{v}’, index=‘{i}’ length=‘{l}’, method-count=‘{method}’, param-count=‘{count}’";
		internal const string mformat1 = "method = “{method}”, params = “{params}”";

		static public string		MValue(this Match m, int g) { return matchValue(m,g); }
		static public int			MIndex(this Match m, int g) { return matchIndex(m,g); }
		static public int			MLength(this Match m, int g) { return matchLength(m,g); }
		static public string		MFormat(this Match m, int g, string input, char splitter)
		{
			return input
				.Replace("{v}",m.MValue(g))
				.Replace("{i}",m.MIndex(g).ToString())
				.Replace("{l}",m.MLength(g).ToString())
				.Replace("{method-count}",m.MMethodArray().Count.ToString())
				.Replace("{method}",m.MMethod())
				.Replace("{params-count}",m.MParamsArray().Count.ToString())
				.Replace("{params}",m.MParams())
				;
		}
		static public List<string>	MMethodArray(this Match m) { return new List<string>(m.MValue(1).Split('-')); }
		static public string		MMethod(this Match m) { return string.Join("-",m.MMethodArray().ToArray()); }
		static public List<string>	MParamsArray(this Match m) { return new List<string>(m.MValue(2).Split(',')); }
		static public string		MParams(this Match m) { return string.Join(",",m.MParamsArray().ToArray()); }
		static public MatchCollection getmatches(this string input) { return m_expression.Matches(input); }
		static public string		matchValue(Match m, int g) { return m.Groups[g].Captures[0].Value; }
		static public int			matchIndex(Match m, int g) { return m.Groups[g].Captures[0].Index; }
		static public int			matchLength(Match m, int g) { return m.Groups[g].Captures[0].Value.Length; }
	}
}
