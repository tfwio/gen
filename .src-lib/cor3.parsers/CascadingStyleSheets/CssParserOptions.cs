/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 5/25/2011
 * Time: 1:20 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace System.Cor3.Parsers.CascadingStyleSheets
{
	/// <summary>
	/// Some parser options.
	/// </summary>
	public class CssParserOptions
	{
		/// <summary>USE WITH CAUTION!
		/// <para>If set to true, Comments will be ignored if they are between
		/// Definition Terms, if set to false, the comment will be ignored
		/// by the parser and Defintions will be merged.</para>
		/// <para>This would classify as a re-write rule</para>
		/// </summary>
		public bool CleanDefinitionFragmentedComments {
			get { return cleanDefinitionFragmentedComments; }
			set { cleanDefinitionFragmentedComments = value; }
		} bool cleanDefinitionFragmentedComments = false;
		
		
		/// <summary>If set to true, white-space is cleaned from definition term-space.  Default is TRUE.</summary>
		public bool CleanDefinitionWhiteSpace {
			get { return cleanDefinitionWhiteSpace; }
			set { cleanDefinitionWhiteSpace = value; }
		} bool cleanDefinitionWhiteSpace = true;
		
		/// <summary>
		/// (Default is CR) If <see cref="AutoConvertEndL" /> is set, this Property
		/// will be referenced for what kind of line-terminator should
		/// be used.</summary>
		/// <remarks>It might prove helpful to check like MSVS and ICSharpCode does for inconsistent EOLs and ask the user.
		/// I personyally don't like the popup window.  It would be nice to see a option categorized into
		/// text-editor settings, or simply text|stream|text-stream.
		/// </remarks>
		public EolMode EndOfLineTerminator {
			get { return endOfLineTerminator; }
			set { endOfLineTerminator = value; }
		} EolMode endOfLineTerminator = EolMode.CR;
		/// <summary>
		/// (Default: true; <oblique>though it shouldn't be</oblique>)<br />
		/// If TRUE, parser converts all end-of-lines after loading
		/// the buffer.  (targeting RTF controls which will not render
		/// properly without their expected line-terminators).
		/// </summary>
		public bool AutoConvertEndL {
			get { return autoConvertEndL; }
			set { autoConvertEndL = value; }
		} bool autoConvertEndL = true;
		
		/// <summary>(default: TRUE)
		/// Begin parsing the CSS content when the new CssParser is created.
		/// </summary>
		[System.ComponentModel.DefaultValue(true)]
		public bool ParseOnConstructor {
			get { return parseOnConstructor; }
			set { parseOnConstructor = value; }
		} bool parseOnConstructor = true;
		
		/// <summary>
		/// Weather the parser throws an exception if the file or content
		/// to be paresed is null or empty.
		/// </summary>
		[System.ComponentModel.DefaultValueAttribute(true)]
		public bool IgnoreEmptyFileException {
			get { return ignoreEmptyFileException; }
			set { ignoreEmptyFileException = value; }
		} bool ignoreEmptyFileException = true;
		
		/// <summary>
		/// <para>Clears all <see cref="CssParserReference" /> Lists with an exception
		/// to the main parser-result-set, <see cref="CssParserReference.Leveled" />.</para>
		/// <para>This does not necessarily have any bearing on
		/// <see cref="CleanAfterParse" />'s value.</para>
		/// </summary>
		[System.ComponentModel.DefaultValue(true)]
		public bool CleanAfterParse {
			get { return cleanAfterParse; }
			set { cleanAfterParse = value; }
		} bool cleanAfterParse = true;
	
		/// <summary>
		/// <para>Adds Urls and Quoted Strings to the final List of CssFragments
		/// for reference.  Note that url and string expressions defy the
		/// prevolent logic of this parser.</para>
		/// <para>/// The reason we include this is to prevent errors such as if there is a curly-brace inside a string,
		/// which happens when I use templates that use '{root-path}' or variables in curlies.
		/// Now apparently this will still throw an exception if the url isn't quoted, but maybe
		/// the url-finder will prevent that—we will see.</para>
		/// </summary>
		[System.ComponentModel.DefaultValue(true)]
		public bool LevelUrlsAndStrings {
			get { return levelUrlsAndStrings; }
			set { levelUrlsAndStrings = value; }
		} bool levelUrlsAndStrings = true;
	
	
	}
}
