/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 5/25/2011
 * Time: 1:20 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace System.Cor3.Parsers
{
	/// <summary><pre>[\u0000-\u001F] (000-031) Control-Characters
	/// [\u0020-\u002F] (032-047) {space}!"#$%&amp;'()*+,-./
	/// [\u0030-\u0039] (048-057) 0-9
	/// [\u003A-\u0040] (058-064) :;&lt;=&gt;?@
	/// [\u0041-\u005A] (065-090) A-Z
	/// [\u005B-\u0060] (091-096) [\]^_`
	/// [\u0061-\u007A] (097-122) a-z
	/// [\u007B-\u007E] (123-126) {|}~</pre></summary>
	public enum AnsiCharClassType
	{
		/// <summary><tt>[\u0000-\u001F] (000-031) Control-Characters</tt></summary>
		Control,
		/// <summary><tt>[\u0020-\u002F] (032-047) {space}!"#$%&amp;'()*+,-./</tt></summary>
		Operator0,
		/// <summary><tt>[\u0030-\u0039] (048-057) 0-9</tt></summary>
		Digit,
		/// <summary><tt>[\u003A-\u0040] (058-064) :;&lt;=&gt;?@</tt>/// </summary>
		Operator1,
		/// <summary><tt>[\u0041-\u005A] (065-090) A-Z</tt></summary>
		Uppercase,
		/// <summary><tt>[\u005B-\u0060] (091-096) [\]^_`</tt></summary>
		Operator2,
		/// <summary><tt>[\u0061-\u007A] (097-122) a-z</tt></summary>
		Lowercase,
		/// <summary><tt>[\u007B-\u007E] (123-126) {|}~</tt></summary>
		Operator3,
		/// <summary>
		/// If not classified by other types, the character is assumed to be unicode.
		/// <para>
		/// For CSS Parser, we only need to check against qualifying operators,
		/// so unicode or otherwise unknown chars are considered 'safe'.
		/// </para>
		/// </summary>
		UnicodeOrUnknown
	}
}
