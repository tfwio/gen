using System;

// StringBuilder
// important TODOS:

namespace System.Cor3.Parsers.Html
{
	/// <summary>
	/// The flags in this enumeration indicate the parser's ability to re-write
	/// content given the parser's preferences (unless otherwise specified).
	/// That is, the enumeration is used as a tool for re-writing HTML syntax.
	/// </summary>
	[Flags]
	public enum BlockWriteInfo
	{
		/// <summary>
		/// Used when the block requesting a value is Null.
		/// </summary>
		None = 0,
		/// <summary>
		/// Treat unknown elements as blocks.
		/// </summary>
		Unknown = 0xF00,
		/// <summary>
		/// Insert Line-Terminators before and after the tag as well as before and after the child-content.
		/// <code language="xml">
		/// <pre>&lt;div&gt;
		/// @content
		/// &lt;/div&gt;</pre>
		/// </code>
		/// </summary>
		Block = 0x100,
		/// <summary>
		/// Insert Line-Terminators before and after the tag.
		/// </summary>
		InlineBlock = 0x300, // 0x300
		/// <summary>No SPACE characters before or after the tag.
		/// </summary>
		Inline = 0x200,
		/// <summary>Insert one space character inserted before the tag.</summary>
		SpacePre = 0x01,
		/// <summary>Insert one space character inserted after the tag.</summary>
		SpacePost = 0x02,
		/// <summary>Insert one space character before and after the tag.</summary>
		SpaceWrap = 0x03,
		/// <summary>Insert one space character inserted before the tag's content.</summary>
		SpacePreContent = 0x04,
		/// <summary>Insert one space character inserted after the tag's content.</summary>
		SpacePostContent = 0x08,
		/// <summary>Insert space character before and after tag's content.</summary>
		SpaceWrapContent = 0x0C,
		/// <summary>Insert line-break before the Element.</summary>
		LinePre = 0x10,
		/// <summary>Insert line-break after the Element.</summary>
		LinePost = 0x20,
		/// <summary>Insert line-break before and after the Element.</summary>
		LineWrap = 0x30,
		/// <summary>Insert a line-break before Elmeent's content.</summary>
		LinePreContent = 0x40,
		/// <summary></summary>
		LinePostContent = 0x80,
		LineContentWrap = 0xC0, // 0xC0
		CleanWhitespacePre = 0x1000,
		CleanWhitespacePost = 0x2000,
		CleanWhitespace = 0x3000,
	}
}
