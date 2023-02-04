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
using System.Text.RegularExpressions;

namespace System.Cor3.Parsers
{
	/// <summary>
	/// <para>A complex block would check to see if the blocking mechanism requires additional
	/// rules other then a simple indexing routine.</para>
	/// <para>a good example of a complex routine would be CSS Comment
	/// where we need to check for erronious end-points.</para>
	/// <para>CSS Comment Regular-Expression: <pre>\/\*[^*]*\*+([^/*][^*]*\*+)*\/</pre></para>
	/// <para>the above expression broken into logical groups.</para>
	/// <pre>
	/// token: \/\*
	/// the above matches '/*'
	/// token: [^*]*\*+
	/// the above matches any character up to the first encountered '*'
	/// token: ([^/*][^*]*\*+)*
	/// find a '*' with no '/' or '*' and no '*' before it.
	/// token (the final character to match: \/</pre>
	/// </summary>
	public struct ParserBlock
	{
		/// <summary>
		/// If (default) TRUE, parser knows to look beyond the end of line boundary.
		/// If set to FALSE, the parser stops looking when it encounters the end of line.
		/// </summary>
		[System.ComponentModel.DefaultValue(true)]
		public bool IsMultiline {
			get { return isMultiline; }
			set { isMultiline = value; }
		} bool isMultiline;
		
		/// <summary>Used to provided the beginning of a block segment.
		/// Our usage of the ParserBlock has thus far (strictly) been
		/// of a CurlyBrace-Begin; EG: '<tt>{</tt>'.
		/// </summary>
		public string BlockBegin {
			get { return blockBegin; }
			set { blockBegin = value; }
		} string blockBegin;
		
		/// <summary>Used to provided the beginning of a block segment.
		/// Our usage of the ParserBlock has thus far (strictly) been
		/// of a CurlyBrace-End; EG: '<tt>}</tt>'.
		/// </summary>
		public string BlockEnd {
			get { return blockEnd; }
			set { blockEnd = value; }
		} string blockEnd;
		
		/// <summary>
		/// Default is null.
		/// specific exceptions per character-range and syntax which
		/// would otherwise throw a false-positive error.
 		/// </summary>
 		/// <description><para>In ANSI C double-quoted string definition, we have
 		/// a doubl-quotation special character which would otherwise break
 		/// finding the end of the string if encountered. Each time a
 		/// forward-slash is encountered in a c-string, we would check
 		/// to see if the following character would be a double-quotation.</para>
 		/// <para>For CSS Comments, we have a rather complex regular expression.
 		/// Generally, the expression searches for the false-positive's termination
 		/// error just the same—but this reminds us of the complexity of readig in-side
 		/// a string.  The First character of any double-quoted C string definition
 		/// is the double-quotation-mark.
 		/// </para>
 		/// </description>
		public string BlockDisqualify {
			get { return blockDisqualify; }
			set { blockDisqualify = value; }
		} string blockDisqualify;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="start">The starting tag such as '{'</param>
		/// <param name="end">The ending tag such as '}'</param>
		public ParserBlock(string start, string end) : this(start,end,null)
		{
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="start">The starting tag such as '{'</param>
		/// <param name="disqualify">Exceptional characters.</param>
		/// <param name="end">The ending tag such as '}'</param>
		public ParserBlock(string start, string end, string disqualify)
		{
			this.blockBegin = start;
			this.blockDisqualify = disqualify;
			this.blockEnd = end;
			this.isMultiline = true;
		}
		//bool IsComplex;
	}
}
