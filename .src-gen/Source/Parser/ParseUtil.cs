/*
 * User: oIo
 * Date: 11/15/2010 ? 2:33 AM
 */
using System;
using System.Collections.Generic;
using System.Cor3.Parsers;

using System.Text.RegularExpressions;
using SqlDbType = System.Data.SqlDbType;
namespace Generator.Parser
{
	public static class ParseUtil
	{
		const char boo = 'a';
//		internal const string regex_fieldMatch = @"\$\((set\w*|print\w*|begin\w*|end\w*|Table\w*|Field\w*)\:(?<innerElement>[^\)]+)\)";
	//		const  string regex_fieldmatch = @"\$\((Table\w*|Field\w*)\:(?<innerElement>[^\)]+)\)";
		const  string regex_first_word = @"[^\s\t]*";
		const  string regex_break_content = @"(([^\n])*[^\n][^\n]\n?)*";
		//[^\\]{0}"([^\\]{0}[^"])*"
		const  string regex_string_section = @"@?[^\\]{0}$(ToFind)([^\\]{0}[^$(ToFind)])*$(ToFind)";
		// not tested
		const  string regex_char_section = @"$(ToFind)([^$(ToFind)])*$(ToFind)";
		static string ExpressionDoubleQuoteString { get { return regex_string_section.Replace("$(ToFind)",@""""); } }
		static string ExpressionSingleQuoteString { get { return regex_char_section.Replace("$(ToFind)","'"); } }
		//const string regex_break_conte = @"(([^\n][\n])*[^\n][^\n])*";
	
		static public MatchCollection Match(string regex, string input)
		{
			var r = new Regex(regex,RegexOptions.Multiline);
			if (!r.IsMatch(input)) return null;
			MatchCollection mc = r.Matches(input);
			r = null;
			return mc;
		}
		static public List<TextRange> MatchRanges(string regex, string input)
		{
			MatchCollection mc = Match(regex,input);
			if (mc==null) return null;
			List<TextRange> list = new List<TextRange>();
			foreach (Match m in mc) list.Add(new TextRange(m.Index,m.Length));
			if (list.Count==0) { list.Clear(); list = null; return null; }
			return list;
		}
		static public MatchCollection MatchSectionGroups(string input) { return Match(regex_break_content,input); }
		static public MatchCollection MatchFirstWord(string input) { return Match(regex_first_word,input); }
	
		static public List<TextRange> GetSectionGroups(string input)
		{
			MatchCollection mc = MatchSectionGroups(input);
			if (mc==null) return null;
			List<TextRange> list = new List<TextRange>();
			foreach (Match m in mc)
			{
				if (m.Length > 0) { list.Add(TextRange.FromMatch(m)); }
			}
			return list;
		}
		/// <returns>A text range with an index to the first word found, otherwise ‘TextRange.Empty’</returns>
		static public TextRange GetFirstWord(string input)
		{
			TextRange tr = TextRange.Empty;
			MatchCollection mc = MatchFirstWord(input);
			if (mc==null) return tr;
			foreach (Match ma in mc)
			{
				if (ma.Length==0) continue;
				else if (ma.Length > 0) { tr = TextRange.FromMatch(ma); break; }
			}
			mc = null;
			return tr;
		}
	}
}
