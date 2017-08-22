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
	/// <summary>EOL mode is (thus-far) used in CssParserOptions
	/// to prevent errors from loading to Rich Text Format
	/// Editors (WinForms and WPF).
	/// <para>Rich Text components are known to automatically
	/// convert input streams as they parse the text-content.</para>
	/// </summary>
	/// <remarks>See if RTF.Read()|.Load()|.Open() (which-ever)
	/// has an option to specify an end-of-line mode and either
	/// point it out in the parser's documentation or demos.</remarks>
	public enum EolMode
	{
		CRLF,
		LFCR,
		CR,
		LF,
	}
	public interface ITextRange
	{
		string Substring(string input);
		long Position { get; set; }
		long Length { get; set; }
	}
}
