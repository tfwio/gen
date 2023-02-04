/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 5/25/2011
 * Time: 1:20 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;

namespace System.Cor3.Parsers.CascadingStyleSheets
{
	/// <summary>
	/// Thus far containing only some default color definitions.
	/// </summary>
	class CommonDefinitions
	{
		internal static readonly Color DefaultBackgroundColor = Color.FromArgb(64,64,64);
		internal static readonly Color DefaultForegroundColor = Color.FromArgb(250,250,250);
	}

	/// <summary>
	/// Contains a few notable comments and Regular Expression String
	/// helper variables.
	/// </summary>
	public class CommonTextDefinitions
	{
		
		static internal readonly char[] chrClean = { '\r','\n',' ','\t','\f' };
		/// <summary>
		/// Match a double quoted string on a single line.
		/// </summary>
		internal const string CDQtSpStr = @"""[^""\r\n\f]*""";
		/// <summary>
		/// match ansi single quoted string without spanning lines.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Str")]
		public const string CSQtSpStr = @"'[^'\r\n\f]*'";
		public static string DblAndSingleQSpStr = "{CDQtSpStr}|{CSQtSpStr}"
			.Replace("{CDQtSpStr}",CDQtSpStr)
			.Replace("{CSQtSpStr}",CSQtSpStr);
		/// <summary>
		/// this is only to be used inside url(…).
		/// It isn't perfect, but find for colourization.
		/// </summary>
		internal const string CPRBraceSpStr = @"[^\r\n\f\)]*";
		
		
		/// <summary>C String Special Characters</summary>
		internal const string CStrSpChr = @"\t\b\r\f\\\'\""\n";
		/// <summary>CS String '@' Special Chars: two (doubly) quotations like Sql Server's single-quote strings.</summary>
		internal const string CSDQtSpChr = @"""""";
		/// <summary>SQL Single Quote String Special Quote</summary>
		/// <description>Note that these string act exactly like @CS-String in the usage of verbatim content.</description>
		internal const string SQLSQtSpChr = @"''";
		/// <summary></summary>
		internal const string MySQLSQtSpChr = @"\`";
		
		// ANSI
		
		// [\u0020-\u002F] (032-047) {space}!"#$%&'()*+,-./
		// [\u0030-\u0039] (048-057) 0-9
		// [\u003A-\u0040] (058-064) :;<=>?@
		// [\u0041-\u005A] (065-090) A-Z
		// [\u005B-\u0060] (091-096) [\]^_`
		// [\u0061-\u007A] (097-122) a-z
		// [\u007B-\u007E] (123-126) {|}~
		
		// todo: csv
		
		// 
		// A-Za-z: \u0041-\u005A\u0061-\u007A
		// A-Za-z0-9: \u0041-\u005A\u0061-\u007A\u0030-\u0039
		
		public static readonly string asciiChars = "\u0041-\u005A\u0061-\u007A";
		public static readonly string asciiDigits = "\u0030-\u0039";
		public static readonly string asciiCharsAndDigits = "{asciiChars}{asciiDigits}"
			.Replace("{asciiChars}",asciiChars)
			.Replace("{asciiDigits}",asciiDigits);
		// NON-ANSI (Unicode)
		// We're only going to go as far as hex: 0x0179
		// [\u007F-\u0179]
	}
}
