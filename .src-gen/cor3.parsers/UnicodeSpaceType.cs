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
	/// <summary>
	/// ICU Space Types: U+2000-U+2009, U+200A, U+205F
	/// </summary>
	public enum UnicodeSpaceType
	{
		/// <summary>
		/// En Quad (U+2000)
		/// </summary>
		EnQuad = 0x2000,
		/// <summary>
		/// Em Quad (U+2001)
		/// </summary>
		EmQuad = 0x2001,
		/// <summary>
		/// En Space (U+2002)
		/// </summary>
		EnSpace = 0x2002,
		/// <summary>
		/// Em Space (U+2003)
		/// </summary>
		EmSpace = 0x2003,
		/// <summary>
		/// Three-Per-Em Space (U+2004)
		/// </summary>
		ThreePerEmSpace = 0x2004,
		/// <summary>
		/// Four-Per-Em Space (U+2005)
		/// </summary>
		FourPerEmSpace = 0x2005,
		/// <summary>
		/// Six-Per-Em Space (U+2006)
		/// </summary>
		SixPerEmSpace = 0x2006,
		/// <summary>
		/// Figure Space (U+2007)
		/// </summary>
		FigureSpace = 0x2007,
		/// <summary>
		/// Punctuation Space (U+2008)
		/// </summary>
		PunctuationSpace = 0x2008,
		/// <summary>
		/// Thin Space (U+2009)
		/// </summary>
		ThinSpace = 0x2009,
		/// <summary>
		/// Hair Space (U+200A)
		/// </summary>
		HairSpace = 0x200A,
		/// <summary>
		/// Mathematical Space (U+205F)
		/// </summary>
		MathematicalSpace = 0x205F,
	}
}
