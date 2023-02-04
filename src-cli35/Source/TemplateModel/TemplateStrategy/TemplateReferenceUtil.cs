/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 5/25/2011
 * Time: 5:39 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
#region Using
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Generator.Parser;
#endregion

namespace Generator.Core.Markup
{
	/// <summary>
	/// Description of TemplateReferenceTool.
	/// </summary>
	static public class TemplateReferenceUtil
	{
		internal const string regex_quickMatch = @"\$\([^\)]*\)";
		internal const string regex_delimitedMatch = @"\$\((\w+)\:([^\)]*)\)";
		internal const string regex_fieldMatch = @"\$\((Table\w*|Field\w*)\:(?<innerElement>[^\)]+)\)";
		internal const string regex_ioMatch = @"\$\((Directory\w*|File\w*)\:(?<innerElement>[^\)]+)\)";
		
		internal const string regex_FieldAndIOMatch = @"\$\((set\w*|print\w*|begin\w*|end\w*|Directory\w*|Table\w*|Field\w*)\:(?<innerElement>[^\)]+)\)";
		/// <summary>
		/// Finds Tag-Regions (EG: “$(tag-name:varN,varN+1,…)within
		/// a template matching all templates called by a single template.
		/// </summary>
		/// <returns>
		/// The regular expression matches.
		/// </returns>
		/// <remarks>
		/// This regular expression is obsoleted by function: ‘ListTagsAndFiles’
		/// </remarks>
		[Obsolete]
		static public MatchCollection ListTags(string input) { return ParseUtil.Match(regex_fieldMatch,input); }
		
		
		
		/// <summary>
		/// Runs regular expression on the input (see: <see cref="regex_FieldAndIOMatch"/>)
		/// </summary>
		/// <remarks>
		/// though not support yet, the regular expression is designed to support
		/// ‘(Table|FIeld|Directory|File)\w*’ input
		/// </remarks>
		/// <param name="input">the text sent to the regular expression query.</param>
		/// <returns>
		/// The regular expression matches.
		/// </returns>
		static public MatchCollection ListTagsAndFiles(string input) { return ParseUtil.Match(regex_FieldAndIOMatch,input); }
	
		/// <summary>
		/// This call is used to find references to any Table* and Field* elements in tag ‘$([tag])’
		/// </summary>
		/// <param name="input"></param>
		/// <returns>
		/// A list of QuickMatch elements which contain information about the location within the input text.
		/// </returns>
		static public List<QuickMatch> GetReferences(string input)
		{
			var matches = new List<QuickMatch>();
			var mc = ListTagsAndFiles(input);
			
			if (mc==null) return matches;
	
			foreach (Match match in mc) matches.Add(new QuickMatch(match));
			
			return matches;
		}
	}
}
