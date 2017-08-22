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
	/// Describes a CSS Tag or Selector statement.
	/// Note that usage of this enumeration will be as needed—or in
	/// other words—just because these tags are defined does not
	/// mean that the parser supports them all.
	/// </summary>
	[Flags]
	public enum CssTagDescriptor
	{
		/// <summary>
		/// References HTML ELEMENT references such as
		/// HTML, BODY, DIV, …
		/// </summary>
		Element,
		/// <summary>
		/// <para>EG: :before, :after</para>
		/// </summary>
		Selector,
		/// <summary>
		/// Special selectors implemented by a browser.
		/// <para>EG: ::-moz-something-or-other</para>
		/// <para>EG: ::-webkit-poop</para>
		/// <para>EG: ::webkit-again</para>
		/// <para>Note that some selectors (CSS2) are known to use two semi-colons howver
		/// they are interpreted by the parser by their text.  Selectors supported by a browser
		/// don't change too often, however often enough for us to externalize support for such
		/// to a db (or customizable resource file).</para>
		/// </summary>
		BrowserSelector,
		/// <summary>
		/// EG: <tt>input[type=text]</tt>
		/// </summary>
		Operator,
		/// <summary>
		/// EG: <tt>input[type=text]</tt>
		/// </summary>
		Brace,
		/// <summary>
		/// Some selectors (and perhaps some other statements) use
		/// functions or a syntax which allows for parameterization on selectors,
		/// indexing values, so fourth…
		/// <para>basically if there is a use of "(…)", we have a function.</para>
		/// <para>EG: <tt>table tr:nth-child(1n+2)</tt></para>
		/// </summary>
		Function,
	}
}
