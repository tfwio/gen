using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace System.Cor3.Parsers
{
	/// <summary>
	/// ParserTypes are put to use in the <see cref="INIParser" />.
	/// Within the structure are a few simple, common string constants.
	/// </summary>
	/// <remarks>
	/// It has been years since I've looked into the INIParser and
	/// usage of these variables, so you can expect this class to
	/// either be deleted in the future or encapsulated in INIParser's
	/// common-resource or such.
	/// </remarks>
	public struct ParserTypes
	{
		/// <summary>value = "default"</summary>
		public const string DefaultSection = "default";
		/// <summary>value = "line-comment"</summary>
		public const string LineComment = "line-comment";
		/// <summary>value = "block-comment"</summary>
		public const string BlockComment = "block-comment";
		// charstrings "", ''
		/// <summary>value = @"{},[],()"</summary>
		public const string SectionIndexers = @"{},[],()";
	}
}
