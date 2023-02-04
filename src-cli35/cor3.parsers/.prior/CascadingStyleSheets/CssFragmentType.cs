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
	/// Undetermined, CommentBlock, Charset, Namespace, Import,
	/// Media, MediaDefinitionBlock, FontFace, Definition, DefinitionAttribute,
	/// DefinitionAttributeCommentBlock, DefinitionCommentBlock, WhiteSpace
	/// </summary>
	/// <remarks>
	/// Note that in a few cases, browsers implement support for some erronious contexts.
	/// This <oblique>may</oblique> be per CSS specification.
	/// <para>If not all-ready, will be used to classify a <see cref="CssFragment"/>.</para>
	/// </remarks>
	public enum CssFragmentType
	{
		/// <summary>
		/// Has yet to be determined.
		/// </summary>
		Undetermined,
		/// <summary>
		/// Either a single or double quoted string.
		/// <para>
		/// The reason we include this is to prevent errors such as if there is a curly-brace inside a string,
		/// which happens when I use templates that use '{root-path}' or variables in curlies.
		/// Now apparently this will still throw an exception if the url isn't quoted, but maybe
		/// the url-finder will prevent that—we will see.
		/// </para>
		/// </summary>
		StringQuoted,
		/// <summary>
		/// A block is about the same as Undetermined since it is not associated
		/// with a context.
		/// </summary>
		Block,
		/// <summary>
		/// Comment Block.
		/// <para>
		/// Depending on the ParserStateType (completely imaginary),
		/// the comment-block may or may not be actually a DefinitionCommentBlock
		/// or even MediaCommentBlock.  Maybe it's a FontFaceCommentBlock!
		/// Rediculous but true.
		/// </para>
		/// </summary>
		CommentBlock,
		/// <summary>
		/// @charset … ;
		/// </summary>
		Charset,
		/// <summary>
		/// @namespace [name] "…";
		/// </summary>
		Namespace,
		/// <summary>
		/// @import "…"; or @import url("…") or @import '…'; or @import url('…'); or @import url(…);
		/// </summary>
		Import,
		/// <summary>
		/// A complex CSS fragment, similar to the media and font-face elements where
		/// we have a block containing Property definitions.
		/// </summary>
		Keyframes,
		/// <summary>
		/// first block after the @keyframes property-definitions.
		/// </summary>
		KeyframesBlock,
		/// <summary>
		/// Same as Definition within the @keyframes block.
		/// </summary>
		KeyframesBlockDefinition,
		/// <summary>
		/// A complex CSS fragment.
		/// </summary>
		Media,
		/// <summary>
		/// first block after the @media property-definitions.
		/// </summary>
		MediaBlock,
		/// <summary>
		/// Same as Definition within the @media block.
		/// </summary>
		MediaBlockDefinition,
		/// <summary>
		/// Definition Fragment: <tt>@font-face { … }</tt>
		/// </summary>
		FontFace,
		/// <summary>
		/// A standard CSS 'Property' definition fragment.
		/// Not to be confused with DefinitionFragment, which is marked when a set of
		/// CSS-properties are separated (fragmented) by a comment block.
		/// </summary>
		Definition,
		/// <summary>
		/// A standard definition fragment.
		/// <para>If a set of definition-terms is broken by comments, we would mark the Definition as a fragment for future reference.</para>
		/// </summary>
		DefinitionFragment,
		/// <summary>
		/// In the case that a comment separates definition-fragments, we mark it as such.</para>
		/// </summary>
		DefinitionFragmentComment,
		/// <summary>
		/// Attribute name (in parenting definition block)
		/// </summary>
		DefinitionAttribute,
		/// <summary>
		/// Attribute value (in parenting definition block)
		/// </summary>
		DefinitionAttributeValue,
		/// <summary>
		/// (Not supported) A comment within an attribute block.
		/// <para>Note that there are two parts to an attribute: the property-name and the property-value.</para>
		/// </summary>
		DefinitionAttributeCommentBlock,
		/// <summary>
		/// A comment within a definition fragment.
		/// </summary>
		DefinitionCommentBlock,
		/// <summary>
		/// CRLF, LFCR, CR, LF
		/// </summary>
		Breaker,
		/// <summary>
		/// url(…)|url('…')|url("…")
		/// </summary>
		Url,
		/// <summary>
		/// Simply put—eh?
		/// </summary>
		Whitespace,
	}
}
