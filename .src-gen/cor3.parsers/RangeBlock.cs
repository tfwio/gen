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
	/// It seems that this is obsoleted by the <see cref="ParserBlock" />,
	/// and the notion of writing a parser however useful.
	/// </summary>
	/// <description>
	/// <para>The class is bound to a regular expression by the constructor which creates an (privat) set of Ranges.  That's about it.</para>
	/// <para>It seems to me that this class was intended to be inherited by a respactive parser.</para>
	/// </description>
	public struct RangeBlock
	{
		/// <summary>
		/// The default RegexOptions; <tt>RegexOptions.Multiline</tt>.
		/// </summary>
		static public readonly RegexOptions DefaultRegexOptions = RegexOptions.Multiline;
		
		List<TextRange> Ranges;
		System.Text.RegularExpressions.Regex Expressions;
		/// <summary>
		/// Uses the defaut RegexOptions.
		/// </summary>
		/// <param name="expression">The string expression used to create a <see cref="Regex" /> <strong>Expressions</strong>.  Apparently there is only one expression with a pluralized name—go figure.</param>
		RangeBlock(string expression) : this(expression,DefaultRegexOptions) {}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="expression">The string expression used to create a <see cref="Regex" /> <strong>Expressions</strong>.  Apparently there is only one expression with a pluralized name—go figure.</param>
		/// <param name="options">RegexOptions used for the created Regular Expression.</param>
		RangeBlock(string expression, RegexOptions options)
		{
			Ranges = new List<TextRange>();
			Expressions = new Regex(expression,options);
		}
	}
}
