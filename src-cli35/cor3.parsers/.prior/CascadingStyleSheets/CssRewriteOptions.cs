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
	/// Though this class is not implemented, it targets
	/// common options that the CssParser should be aware of.
	/// </summary>
	[Serializable]
	public class CssRewriteOptions
	{
		/// <summary>
		/// (default is True).
		/// If set to TRUE, all reformatting options are ignored in attempt
		/// to retain the CSS Syntax-Format provided by the input file
		/// or CssFragment input(s).
		/// </summary>
		[System.ComponentModel.DefaultValue(true)]
		public bool IsSameAsInput {
			get { return isSameAsInput; }
			set { isSameAsInput = value; }
		} bool isSameAsInput;
		/// <summary>
		/// The number of Tag Definitions per line. (Default is -1)
		/// <para>By default (value of -1) this option is ignored.</para>
		/// </summary>
		/// <description>
		/// <h3>CSS Example</h3>
		/// <para>
		/// In the following CSS, <tt>DT, DL and ML</tt> would be our target.
		/// </para>
		/// <pre>
		/// DT,DL,ML { attribute: value; }</pre>
		/// <para>If set, this setting would restrict the number of entities per line.</para>
		/// </description>
		[System.ComponentModel.DefaultValue(-1)]
		public int MaxCharsPerTagDefinitionLine {
			get { return maxCharsPerTagDefinitionLine; }
			set { maxCharsPerTagDefinitionLine = value; }
		} int maxCharsPerTagDefinitionLine;
	
		/// <summary>
		/// <para>If set to TRUE, css will be formatted as follows
		/// <pre>
		/// HTML,BODY,.Etc {
		/// 	attribute1: value;
		/// 	attributeN: value;
		/// }</pre></para>
		/// <para>If set to FALSE, css will be formatted as follows
		/// <pre>
		/// HTML,BODY,.Etc
		/// {
		/// 	attribute1: value;
		/// 	attributeN: value;
		/// }</pre></para>
		/// </summary>
		[System.ComponentModel.DefaultValue(false)]
		public bool BraceOnSameLineAsDefinitions {
			get { return braceOnSameLineAsDefinitions; }
			set { braceOnSameLineAsDefinitions = value; }
		} bool braceOnSameLineAsDefinitions;
	}
}
