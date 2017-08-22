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
		/// Runs regular expression on the input (regex_FieldAndIOMatch)
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
		/// <param name="collection"></param>
		/// <param name="templateName"></param>
		/// <returns></returns>
//		static public List<QuickMatch> GetReferences(TemplateCollection collection, string templateName)
//		{
//			Logger.LogG("TemplateReferenceUtil","GetReferences");
//			List<QuickMatch> xlist = new List<QuickMatch>();
//	
//			// part 1: root tags
//			// UNDONE: What is a root tag?  TableTemplate?
//			List<QuickMatch> list = null;
//			if (templateName==null) { list = new List<QuickMatch>(); }
//			else if (templateName==string.Empty) { list = new List<QuickMatch>(); }
//			else
//			{
//				Logger.LogG("TemplateReferenceUtil","GetReferences as we have a templateName");
//				list = collection[templateName].GetReferences();
//			}
//			// end of part one
//			
//			
//			// begin part two: find embedded template tags.
//			Logger.LogG("TemplateReferenceUtil","GetReferences will follow for referenced templates.");
//			foreach (QuickMatch match in collection[templateName].GetReferences())
//			{
//				List<QuickMatch> listB = GetReferences(collection, collection[match.Params[0]].Alias);
//				foreach (QuickMatch qm in listB) list.Add(qm);
//			}
//			Logger.LogG("TemplateReferenceUtil","GetReferences result: {0} Matches found.",list.Count);
//			return list;
//		}
	
		/// <summary>
		/// This call is used to find references to any Table* and Field* elements in tag ‘$([tag])’
		/// </summary>
		/// <param name="input"></param>
		/// <returns>
		/// A list of QuickMatch elements which contain information about the location within the input text.
		/// </returns>
		static public List<QuickMatch> GetReferences(string input)
		{
			List<QuickMatch> matches = new List<QuickMatch>();
			MatchCollection mc = ListTagsAndFiles(input);
			Logger.LogM("ref","matches {0}",matches.Count);
			
			if (mc==null) return matches;
	
			foreach (Match match in mc) matches.Add(new QuickMatch(match));
			
			return matches;
		}
	}
}
