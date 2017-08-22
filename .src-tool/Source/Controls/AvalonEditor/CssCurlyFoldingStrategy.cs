/*
 * oio * 4/22/2012 * 12:11 AM
 */
#region Using
using System;
using System.Collections.Generic;
using System.Cor3.Parsers;
using System.Text.RegularExpressions;

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;

#endregion
namespace GeneratorTool.Controls
{
	/// <summary>
	/// Allows producing foldings from a document based on braces.
	/// </summary>
	// Note how it renders folding to spite comment blocks
	public class CssCurlyFoldingStrategy /*: */
	{
		/// <summary>Gets/Sets the opening brace. The default value is '{'.</summary>
		public char OpeningBrace { get; set; }
		
		/// <summary>Gets/Sets the closing brace. The default value is '}'.</summary>
		public char ClosingBrace { get; set; }
		
		/// <summary>Creates a new BraceFoldingStrategy.</summary>
		public CssCurlyFoldingStrategy()
		{
			this.OpeningBrace = '{';
			this.ClosingBrace = '}';
		}
		
		/// <summary>
		/// Create <see cref="NewFolding"/>s for the specified document.
		/// </summary>
		public override IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset)
		{
			firstErrorOffset = -1;
			return CreateNewFoldings(document);
		}
	
		/// <summary></summary>
		static readonly RegexOptions ropt = RegexOptions.Multiline;
		/// <summary></summary>
		static readonly Regex rxCommentBlock = new Regex(@"\/\*[^*]*\*+([^/*][^*]*\*+)*\/",ropt);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="document"></param>
		/// <returns></returns>
		static List<TextRange> CheckCommentBlocks(ITextSource document)
		{
			List<TextRange> list = new List<TextRange>();
			foreach (Match m in rxCommentBlock.Matches(document.Text))
			{
				list.Add(TextRange.FromMatch(m));
			}
			return list;
		}
		/// <summary>
		/// Create <see cref="NewFolding"/>s for the specified document.
		/// </summary>
		public IEnumerable<NewFolding> CreateNewFoldings(ITextSource document)
		{
			List<NewFolding> newFoldings = new List<NewFolding>();
			
			Stack<int> startOffsets = new Stack<int>();
			int lastNewLineOffset = 0;
			
			char openingBrace = this.OpeningBrace;
			char closingBrace = this.ClosingBrace;
			
			for (int i = 0; i < document.TextLength; i++)
			{
				char c = document.GetCharAt(i);
				if (c == openingBrace) startOffsets.Push(i);
				else if (c == closingBrace && startOffsets.Count > 0)
				{
					int startOffset = startOffsets.Pop();
					// don't fold if opening and closing brace are on the same line
					if (startOffset < lastNewLineOffset)
					{
						newFoldings.Add(new NewFolding(startOffset, i + 1));
					}
				} else if (c == '\n' || c == '\r') { lastNewLineOffset = i + 1; }
			}
			newFoldings.Sort((a,b) => a.StartOffset.CompareTo(b.StartOffset));
			return newFoldings;
		}
	}
}
