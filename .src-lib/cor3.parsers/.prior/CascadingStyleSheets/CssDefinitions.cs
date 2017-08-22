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
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Linq;
namespace System.Cor3.Parsers.CascadingStyleSheets
{
	/// <summary>
	/// Sections defined in the CSS Grammar Appendix
	/// include a Lexical Scanner (Demonstration Example)
	/// which may have the ability to parse definitions within
	/// a CSS Text-Document.  Scanner contains definitions
	/// which are not implemented that are titled as follows.
	/// <para><a href="http://www.w3.org/TR/css3-background/">CSS Backgrounds and Borders Module Level 3</a></para>
	/// </summary>
	/// <remarks>
	/// <ul>
	/// <li>
	/// Scanner-Functions
	/// <ul>
	/// <li>stylesheet</li>
	/// <li>import</li>
	/// <li>media</li>
	/// <li>media_list</li>
	/// <li>medium</li>
	/// <li>page</li>
	/// <li>psuedo_page</li>
	/// <li>operator</li>
	/// <li>combinator</li>
	/// <li>unary_operator</li>
	/// <li>property</li>
	/// <li>ruleset</li>
	/// <li>selector</li>
	/// <li>simple_selector</li>
	/// <li>class</li>
	/// <li>element_name</li>
	/// <li>attrib</li>
	/// <li>psuedo</li>
	/// <li>declaration</li>
	/// <li>prio</li>
	/// <li>expr</li>
	/// <li>term</li>
	/// <li>function</li>
	/// <li>hexcolor</li>
	/// </ul>
	/// </li>
	/// </ul>
	/// <para>
	/// furthermore in reference material <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a>
	/// deontes further clarification of such as the following remarks.
	/// <ol>
	/// <li>String Definitions; a seeming foundation for the following.</li>
	/// <li>Symbol Definitions; using String Definitions as their basis.</li>
	/// <li>3: Scanner-Functions; The parser application.</li>
	/// </ol>
	/// </para>
	/// </remarks>
	public class CssDefinitions : CommonTextDefinitions
	{
		// G.2 Lexical scanner
		
		#region SYMBOLS
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symS = @"{strS}".Replace("{strS}",strS);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCdo = @"<!--";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCdc = @"-->";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symIncludes = @"~=";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symDashmatch = @"|=";
		//
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symString = @"{strString}".Replace("{strString}",strString);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symBadString = @"{strBadString}".Replace("{strBadString}",strBadString);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symIdent = @"{strIdent}".Replace("{strIdent}",strIdent);
		//
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symHash = @"#{strName}"
			.Replace("{strName}",strName);
		//
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symImport = @"@import\s*({DblAndSingleQSpStr})\s*;*"
			.Replace("{DblAndSingleQSpStr}",DblAndSingleQSpStr);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symPage = @"@page";
		/// <summary>Keyframe Property definitions are found in <a href="http://www.w3.org/TR/css3-animations">CSS Animations</a>.
		/// <para>Syntacticly, they work similarly to @media elements in that they extend the block-level by +1
		/// which means that there are definitions inside the main block with attributes blocked at block-level 2.</para>
		/// </summary>
		public static readonly string symKeyframes = @"@keyframes";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		/// <remarks>
		/// Media elements (http://www.w3.org/TR/CSS2/media.html) are documented elsewhere
		/// in documentation (i'll find it soon enough), and are of the most
		/// complex for the parser to interpret given that we could
		/// be dealing with print, screen, mobile and other qualifying identifiers
		/// dependant upon the rules that qualify.
		/// <para>
		/// A media section can also be defined in HTML, such as follows:
		/// <example>
		/// <code>&lt;LINK REL="stylesheet" TYPE="text/css" MEDIA="print, handheld" HREF="foo.css"&gt;</code>
		/// </example>
		/// </para>
		/// <para>
		/// Additionally, a media element will contain it's own CSS definitions for
		/// the qualified device, within a curly block.  EG: {…}.
		/// </para>
		/// <para>
		/// A @media construct may also be interpreted of an import statement.
		/// See the provided link above for more info.
		/// </para>
		/// </remarks>
		public static readonly string symMedia = @"@media";
		/// <summary>
		/// See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a>, or <a href="http://www.w3.org/International/questions/qa-css-charset.en.php">Declaring character encodings in CSS</a>.
		/// </summary>
		/// <example>
		/// An example usage in CSS is as follows:
		/// <code>@charset "UTF-8";</code>
		/// </example>
		public static readonly string symCharset = @"@charset";
		
		/// <summary>
		/// See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a>, but is not there – so see <a href="http://www.w3.org/TR/css3-fonts/">CSS Fonts Module Level 3</a>.
		/// </summary>
		public static readonly string symFontFace = @"@font-face";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a> as defined in <a href="http://www.w3.org/TR/css3-namespace/">CSS3 Namespace</a></summary>
		public static readonly string symNamespace = @"@namespace";
		//
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symImportant = @"\u0033({w}|{comment})*important";
		//
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUnitEm = @"{strNum}EM"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUnitEx = @"{strNum}EX"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUnitPx = @"{strNum}PX"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUnitCm = @"{strNum}CM"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUnitMm = @"{strNum}MM"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUnitIn = @"{strNum}IN"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUnitPt = @"{strNum}PT"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUnitPc = @"{strNum}PC"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUnitDeg = @"{strNum}DEG"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUnitRad = @"{strNum}RAD"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUnitGrad = @"{strNum}GRAD"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUnitMs = @"{strNum}MS"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUnitS = @"{strNum}S"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUnitHz = @"{strNum}HZ"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUnitKhz = @"{strNum}KHZ"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUnitDimension = @"{strIdent}"
			.Replace("{strNum}",strNum)
			.Replace("{strIdent}",strIdent);
		// 
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symPercentage = @"{strNum}%"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symNumber = @"{strNum}"
			.Replace("{strNum}",strNum);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUriString = @"url(""{strW}{strString}{strW}"")"
			.Replace("{strW}",strW)
			.Replace("{strString}",strString);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symUriUrl = @"url(""{strW}{strUrl}{strW}"")"
			.Replace("{strUrl}",strUrl)
			.Replace("{strW}",strW);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symBadUri = @"{strBadUri}"
			.Replace("{strBadUri}",strBadUri);
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symFunction = @"{strIdent}\("
			.Replace("{strIdent}",strIdent);
		// seems to be a unicode checking impl
		// also, I think that the usage of \u0 is not as I've implemented
		// it's probably going to be just a regular zero.
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharA = @"a|\u0{0,4}(\u41|\u61)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharC = @"c|\u0{0,4}(\u43|\u63)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharD = @"d|\u0{0,4}(\u44|\u64)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharE = @"e|\u0{0,4}(\u45|\u65)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharG = @"g|\u0{0,4}(\u47|\u67)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharH = @"h|\u0{0,4}(\u48|\u68)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharI = @"i|\u0{0,4}(\u49|\u69)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharK = @"k|\u0{0,4}(\u4B|\u6B)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharL = @"l|\u0{0,4}(\u4C|\u6C)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharM = @"m|\u0{0,4}(\u4D|\u6D)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharN = @"n|\u0{0,4}(\u4E|\u6E)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharO = @"o|\u0{0,4}(\u4F|\u6F)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharP = @"p|\u0{0,4}(\u50|\u70)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharR = @"r|\u0{0,4}(\u52|\u72)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharS = @"s|\u0{0,4}(\u53|\u73)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharT = @"t|\u0{0,4}(\u54|\u74)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharU = @"u|\u0{0,4}(\u55|\u75)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharX = @"x|\u0{0,4}(\u58|\u78)(\r\n|[ \t\r\n\f])?";
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string symCharZ = @"z|\u0{0,4}(\u5A|\u7A)(\r\n|[ \t\r\n\f])?";
		#endregion
		
		// url\((('[^'\r\n\f]*')|("[^"\r\n\f]*")|([^\r\n\f\)]*))\)
		/// <summary>
		/// I'm not sure that the CSS Spec matches URL string that haven't quotations,
		/// so I provide one here.
		/// </summary>
		public const string UrlString = @"url\((('[^'\r\n\f]*')|(""[^""\r\n\f]*"")|([^\r\n\f\)]*))\)";
		
		#region Entity Defs
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a>
		/// hexidecimal number characters
		/// </summary>
		public static readonly string strH = "[a-fA-F]";
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a>
		/// non-ascii characters
		/// </summary>
		public static readonly string strNonAscii = "[\u00F0-\u0179]";
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a>
		/// hash characters.  I know not what these are.
		/// </summary>
		public static readonly string strHash = "\u0023";
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strUnicode = "\\{strHGroup}{1-6}(\r\n|[ \t\r\n\f])?"
			.Replace("{strHGroup}",strH);
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strEscape = "{strUnicode}|\\[^\r\n\f0-9a-fA-F]"
			.Replace("{strUnicode}",strUnicode);
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strNmstart = "[_a-zA-Z]|{strNonAsciiGroup}|{strEscape}"
			.Replace("{strNonAsciiGroup}",strNonAscii)
			.Replace("{strEscape}",strEscape);
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strNmchar = "[_a-zA-Z0-9-]|{strNonAsciiGroup}|{strEscape}"
			.Replace("{strNonAsciiGroup}",strNonAscii)
			.Replace("{strEscape}",strEscape);
		
		/// <summary>A Double-Quotation-string and escape sequence<br />See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strString1 = @"""([^\n\r\f""]|\{strNl}|{strEscape})*"""
			.Replace("{strNl}",strNl)
			.Replace("{strEscape}",strEscape);
		
		/// <summary>A Single-Quotation-string and escape sequence<br />See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strString2 = @"'([^\n\r\f']|\{strNl}|{strEscape})*'"
			.Replace("{strNl}",strNl)
			.Replace("{strEscape}",strEscape);
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strBadString1 = @"""([^\n\r\f""]|\{strNl}|{strEscape})*""\?"
			.Replace("{strNl}",strNl)
			.Replace("{strEscape}",strEscape);
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strBadString2 = @"'([^\n\r\f']|\{strNl}|{strEscape})*'\?"
			.Replace("{strNl}",strNl)
			.Replace("{strEscape}",strEscape);
		
		// badcomment1
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strBadComment1 = @"\/\*[^*]*\*+([^/*][^*]*\*+)*";
		
		// badcomment2
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strBadComment2 = @"\/\*[^*]*(\*+[^/*][^*]*)*";
		
		// baduri1
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strBadUri1 = @"url\({strW}([!#$%&*-\[\]-~]|{strNonAscii}|{strEscape})*{strW}"
			.Replace("{strNonAscii}",strNonAscii)
			.Replace("{strW}",strW)
			.Replace("{strEscape}",strEscape);
		
		// baduri2
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strBadUri2 = @"url\({strW}{strString}{strW}"
			.Replace("{strString}",strString)
			.Replace("{strW}",strW);
		
		// baduri3
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strBadUri3 = @"url\({strW}{strBadString}"
			.Replace("{strBadString}",strBadString)
			.Replace("{strW}",strW);
		
		// comment
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strComment = @"\/\*[^*]*\*+([^/*][^*]*\*+)*\/";
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strIdent = @"-?{strNmstart}{strNmchar}*"
			.Replace("{strNmchar}",strNmchar)
			.Replace("{strNmstart}",strNmstart);
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strName = "{strNmchar}+"
			.Replace("{strNmchar}",strNmchar);
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strNum = "[0-9]+|[0-9]*\u002E[0-9]+";
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strString = "{strString1}|{strString2}"
			.Replace("{strString1}",strString1)
			.Replace("{strString2}",strString2);
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strBadString = "{strBadString1}|{strBadString2}"
			.Replace("{strBadString1}",strBadString1)
			.Replace("{strBadString2}",strBadString2);
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strBadComment = "{strBadComment1}|{strBadComment2}"
			.Replace("{strBadComment1}",strBadComment1)
			.Replace("{strBadComment2}",strBadComment2);
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strBadUri = "{strBadUri1}|{strBadUri2}|{strBadUri3}"
			.Replace("{strBadUri1}",strBadUri1)
			.Replace("{strBadUri2}",strBadUri2)
			.Replace("{strBadUri3}",strBadUri3);
		
		// check this guy
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strUrl = @"([!#$%&*-~]|{strNonAscii}|{strEscape})*"
			.Replace("{strNonAscii}",strNonAscii)
			.Replace("{strEscape}",strEscape);
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strS = @"[ \t\r\n\f]";
		
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strW = @"{strS}?"
			.Replace("{strS}",strS);
		
		// new line
		/// <summary>See CSS2 Reference: <a href="http://www.w3.org/TR/2004/CR-CSS21-20040225/grammar.html#q2">Appendix G. Grammar of CSS 2.1</a></summary>
		public static readonly string strNl = "\n|\r\n|\r|\f";
		#endregion
		
		#region CSS Level-3 IDS
		/// <summary>
		/// <para>
		/// These may not actualy be level-3 reference materials,
		/// however were taken from "Cascading Style Sheets (CSS) Snapshot 2010": <a href="http://www.w3.org/TR/CSS/#properties">Section 4.1: Property Index</a>.
		/// </para>
		/// </summary>
		/// <remarks>
		/// This content shall in the future be contained in a Sqlite3 Database
		/// file complete with reference documentation and additional (perhaps
		/// browser-specific) tags.
		/// </remarks>
		public static readonly string CssProperties =
			"background-attachment|background-color|background-image|" +
			"background-position|background-repeat|" +
			"background|" +
			"border-collapse|" +
			"border-color|" +
			"border-spacing|" +
			"border-style|" +
			"border-top|border-right|border-bottom|border-left|" +
			"border-top-color|border-right-color|border-bottom-color|border-left-color|" +
			"border-top-style|border-right-style|border-bottom-style|border-left-style|" +
			"border-top-width|border-right-width|border-bottom-width|border-left-width|" +
			"border-width|" +
			"border|" +
			"bottom|" +
			"caption-side|" +
			"clear|" +
			"clip|" +
			"color|" +
			"content|" +
			"counter-increment|" +
			"counter-reset|" +
			"cursor|" +
			"direction|" +
			"display|" +
			"empty-cells|" +
			"float|" +
			"font-family|" +
			"font-size|" +
			"font-style|" +
			"font-variant|" +
			"font-weight|" +
			"font|" +
			"height|" +
			"left|" +
			"letter-spacing|" +
			"line-height|" +
			"list-style-image|" +
			"list-style-position|" +
			"list-style-type|" +
			"list-style|" +
			"margin-right|margin-left|" +
			"margin-top|margin-bottom|" +
			"margin|" +
			"max-height|" +
			"max-width|" +
			"min-height|" +
			"min-width|" +
			"opacity|" +
			"orphans|" +
			"outline-color|" +
			"outline-style|" +
			"outline-width|" +
			"outline|" +
			"overflow|" +
			"padding-top|padding-right|padding-bottom|padding-left|" +
			"padding|" +
			"page-break-after|" +
			"page-break-before|" +
			"page-break-inside|" +
			"position|" +
			"quotes|" +
			"right|" +
			"table-layout|" +
			"text-align|" +
			"text-decoration|" +
			"text-indent|" +
			"text-transform|" +
			"top|" +
			"unicode-bidi|" +
			"vertical-align|" +
			"visibility|" +
			"white-space|" +
			"windows|" +
			"width|" +
			"word-spacing|" +
			"z-index";

		#endregion

		#region Known Selectors
		
		public static readonly string knownSelectorsStr =
			":link|" +
			":visited|" +
			":hover|" +
			":active|" +
			":focus" +
			// 6.6.4: UI element states
			":enabled|" +
			":disabled|" +
			":checked|" +
			":intermediate|" +
			// 6.6.5: Structural psuedo-classes
			":root|" +
			":nth-child|" +
			":nth-last-child|" +
			":nth-of-type|" +
			":nth-last-of-type|" +
			":first-child|" +
			":last-child|" +
			":first-of-type|" +
			":last-of-type|" +
			":only-child|" +
			":only-of-type|" +
			":empty|" +
			"::first-line|" +
			"::first-letter|" +
			"::before|" +
			"::after";

		#endregion

		#region Attribute/Value Recognition
		// TODO: Error Checking would provide ROW/COL information and description of the error.
		/// <summary>
		/// Attrubute/Value Pair Expression.
		/// <para>The expression should pick up on non-terminating values at the end of a definition-block.</para>
		/// <para>Note that error-checking doesn't happen at attribute or value level.</para>
		/// <para>Error checking is perhaps something that CSS21 reference might guide us toward in the future.</para>
		/// </summary>
		public static readonly string strAttributeValue = @"[\s\n]*(?<attr>[^:][\w-]*)\s*:\s*(?<value>[^;}]*)(;|})?";
		/// <summary>
		/// (not from css21 appendix)
		/// <para>@"({strComment})|({strAttributeValue})"</para>
		/// <para>Differs between comment blocks and the attribute/value tag.</para>
		/// </summary>
		public static readonly string symAttributeValue = @"({strComment})|({strAttributeValue})"
			.Replace("{strComment}",strComment)
			.Replace("{strAttributeValue}",strAttributeValue);

		#endregion

		#region Regular Expression Definitions
		/// <summary>
		/// The following's results are thus far the only set of results using names.
		/// <para>This is the last (main) expression to run after all other expressions are
		/// executed, so we're going to execute within the boundaries of the curly braces.</para>
		/// <para>After all main sections have been provided, we would of course validate
		/// names and values (when we're able).</para>
		/// <para>note that the (?&lt;value&gt;…) result is going to have to be trimmed.</para>
		/// </summary>
		static public readonly Regex AttributeValuePairExpr = new Regex(symAttributeValue,RegexOptions.Multiline|RegexOptions.Compiled|RegexOptions.IgnoreCase);
		/// <summary>
		/// CRLF, LFCR, CR, LF, FF
		/// </summary>
		static public readonly Regex BreakerExpr = new Regex("\r\n|\n\r|\r|\n|\f",RegexOptions.Multiline|RegexOptions.Compiled);
		/// <summary>
		/// Comment expression
		/// </summary>
		static public readonly Regex CommentExpr = new Regex(CssDefinitions.strComment,RegexOptions.Multiline|RegexOptions.Compiled);
		
		/**
		 * The following elements are terminated by a colon
		 */

		#region Colon Delimited: @charset, @namespace and @import
		
		/// <summary>terminated by a colon</summary>
		static public readonly Regex CharsetExpr = new Regex(CssDefinitions.symCharset,RegexOptions.Multiline|RegexOptions.Compiled);
		/// <summary>terminated by a colon</summary>
		static public readonly Regex NamespaceExpr = new Regex(CssDefinitions.symNamespace,RegexOptions.Multiline|RegexOptions.Compiled);
		/// <summary>terminated by a colon</summary>
		static public readonly Regex ImportExpr = new Regex(CssDefinitions.symImport,RegexOptions.Multiline|RegexOptions.Compiled);
		
		#endregion
		
		/// <summary>
		/// The @font-face Property is similar to the @media element in that it contains a curly-block, however
		/// is a standard 1-level property with attributes.  It's terminated by the end of it's curly-block.
		/// </summary>
		static public readonly Regex FontFaceExpr = new Regex(CssDefinitions.symFontFace,RegexOptions.Multiline|RegexOptions.Compiled);
		
		/// <summary>The @keyframe element is a curly-block region beginning (out of the block) with a '@keyframe' tag.
		/// <para>The next word defines the keyframe's name (probably using the class-name spec which allows for more then
		/// alpha-numeric characters in key-frame names).</para>
		/// </summary>
		static public readonly Regex KeyframeExpr = new Regex(CssDefinitions.symKeyframes,RegexOptions.Multiline|RegexOptions.Compiled);
		/// <summary>
		/// The @media element is a curly-block region beginning (out of the block) with a '@media' tag.
		/// </summary>
		static public readonly Regex MediaExpr = new Regex(CssDefinitions.symMedia,RegexOptions.Multiline|RegexOptions.Compiled);
		/// <summary>
		/// single or double quoted strings.
		/// </summary>
		static public readonly Regex QuotedStringExpr = new Regex(CommonTextDefinitions.DblAndSingleQSpStr,RegexOptions.Multiline|RegexOptions.Compiled|RegexOptions.IgnoreCase);
		/// <summary>
		/// single-quoted, double-quoted and just the url in parenthesis.
		/// </summary>
		static public readonly Regex UrlExpr = new Regex(CssDefinitions.UrlString,RegexOptions.Multiline|RegexOptions.Compiled|RegexOptions.IgnoreCase);

		#endregion
		
	}
}
