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

namespace System.Cor3.Parsers.CascadingStyleSheets
{
	/// <summary>
	/// Contains various List&lt;<see cref="CssFragment" />&gt; sections
	/// for the parser to manage.
	/// </summary>
	public class CssParserReference
	{
		/// <summary>
		/// Clears all but Leveled content and line-break positions.
		/// </summary>
		public void Clean(/*CssParserOptions options*/)
		{
			this.Comment.Clear();
			this.Charset.Clear();
			this.Namespace.Clear();
			this.Import.Clear();
			this.Media.Clear();
			this.FontFace.Clear();
			this.Definition.Clear();
			this.BlockLevels.Clear();
//			if (!options.LevelUrlsAndStrings)
//			{
			this.Quoted.Clear();
			this.Url.Clear();
//			}
//			this.Leveled.Clear();
			
		}
		/// <summary>
		/// Clears all lists.
		/// </summary>
		public void Clear()
		{
			this.Clean();
			this.Breaker.Clear();
			this.Leveled.Clear();
		}

		/// <summary>
		/// Root fragment; Contains all text.
		/// </summary>
		public CssFragment Root = TextRange.Empty;
		//
		public List<CssFragment> Quoted = new List<CssFragment>();
		public List<CssFragment> Url = new List<CssFragment>();
		//
		public List<CssFragment> Breaker = new List<CssFragment>();
		public List<CssFragment> Comment = new List<CssFragment>();
		public List<CssFragment> Charset = new List<CssFragment>();
		public List<CssFragment> Namespace = new List<CssFragment>();
		public List<CssFragment> Import = new List<CssFragment>();
		/// <summary>
		/// Known @media elements are: "print", "handheld", and even the following
		/// "only screen and (max-width: {digit}{digit-type})" has been seen. (note
		/// that our {digit} and {digit-type} are imaginary text such as "800px"
		/// and such.
		/// <para>
		/// Within the "@media only screen and (…)" sections, attributes are
		/// defined in the paranthesies such as:
		/// <code>
		/// min-width: {dimsnsion} ; | max-width: {d} ; |
		/// max-device-width: {d} ; | min-device-width: {d} ; |
		/// device-width: {d} ; | device-height: {d} ; |
		/// width: {d} ; | height: {d} ; |
		/// orientation: 'landscape' | 'portrait' ;</code>
		/// </para>
		/// </summary>
		public List<CssFragment> Media = new List<CssFragment>();
		public List<CssFragment> Keyframes = new List<CssFragment>();
		public List<CssFragment> FontFace = new List<CssFragment>();
		public List<CssFragment> Definition = new List<CssFragment>();
		/// <summary>
		/// BlockLevels are used to section the Definition into ranges,
		/// as well as any child-blocks.
		/// <para>
		/// if one block element is contained within another, then
		/// the particular fragment will contain a child-fragment
		/// pointing to the particular block element.
		/// </para>
		/// </summary>
		public List<CssFragment> BlockLevels = new List<CssFragment>();
		public List<CssFragment> Attributes = new List<CssFragment>();
		public List<CssFragment> Leveled = new List<CssFragment>();
		
		public CssParserReference()
		{
		}
	}
}
