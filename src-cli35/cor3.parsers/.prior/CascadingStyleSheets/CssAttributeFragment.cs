/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 5/25/2011
 * Time: 1:20 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Cor3.Parsers.CascadingStyleSheets
{

	/// <summary>
	/// Css Attribute Fragment.
	/// <para>Contains a set of CssValueFragment fragments.</para>
	/// </summary>
	public class CssAttributeFragment : CssFragment
	{
		/// <summary>
		/// Tags, Selectors and such definitions are stored into
		/// a List.
		/// </summary>
		public List<string> Tags {
			get { return tags; }
			set { tags = value; }
		} List<string> tags;
		
		/// <summary>
		/// 
		/// </summary>
		public string TagsAsString {
			get { return tagsAsString; }
			set { SetTagList(tagsAsString = value); }
		} string tagsAsString;
		
		void SetTagList(string tagInput)
		{
			
		}
		
		public CssAttributeFragment(TextRange r) : base(r)
		{
		}
		/// <summary>
		/// </summary>
		/// <param name="parser">CssParser</param>
		/// <param name="fragment">CssFragment</param>
		void GetAttributes(CssFragment fragment, CssParser parser)
		{
			// TODO: CssParserOptions.ReadAttributes
			string sTags, sBlocks;
			if (fragment.FragmentType== CssFragmentType.Definition)
			{
				sTags = fragment.Tag.ToString();
				sBlocks = parser
					.FindNextFragment(CssFragmentType.Block,(int)this.Position)
					.section.Substring(parser.Text);
			}
		}
		
	}
}
