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
	/// As with the BraceFoldingStrategy, we just want regions and don't
	/// care to check for comments just yet.
	/// <para>
	/// I'm going to stick with the guidelines for an old py parser I'd
	/// made based from Regular Expression Evaluations.  Let's see if the
	/// Stack method can help here.
	/// </para>
	/// <para>
	/// The idea would be to provide a list of line segments.
	/// This would indirectly provide a way to get the line number from
	/// character index.
	/// </para>
	/// </summary>
	public class CSPragmaFoldingStrategy/* : AbstractFoldingStrategy*/
	{
		public Dictionary<TextRange,bool> dicFoldings;
		
		static public bool UsePragmaFolding { get; set; }
		static public bool DetectPragmaFolding { get; set; }
		
		#region Private Static Fields/Properties

		static readonly char[] eolChars = new char[]{'\r','n'};

		static readonly Regex PragmaRegion = new Regex(@"^[\s\t]*(#region)([^\r\n]*)|^[\s\t]*(#endregion)([^\r\n]*)",RegexOptions.Multiline);

		static int lastDetectedError = 0;
		#endregion

		#region Static Methods GetFoldingRanges(TextDocument,List<NewFolding>), SortRange(NewFolding,NewFolding)
		static public List<NewFolding> GetFoldingRanges(TextDocument document, List<NewFolding> list)
		{
			if (!DetectPragmaFolding) return new List<NewFolding>();
			Stack<Match> stack = new Stack<Match>();
			foreach (Match m in PragmaRegion.Matches(document.Text))
			{
				if (m.Groups[1].Value=="#region")
				{
					TextRange range = TextRange.FromMatch(m);
					int len = m.Groups[1].Index - m.Groups[0].Index;
					stack.Push(m);
				}
				else
				{
					TextRange range = TextRange.FromMatch(m);
					
					Match peeek = stack.Peek();
					TextRange peek = TextRange.FromMatch(peeek);
					int len = peeek.Groups[1].Index - peeek.Groups[0].Index;
					Logger.Log(MessageType.White,"GetFoldingRanges","{0}",peek.Position);
					peek.ShrinkRight(len);
					Logger.Log(MessageType.White,"GetFoldingRanges","{0}",peek.Position);
					
					NewFolding nf = new NewFolding((int)peek.Position,(int)range.EndPosition);
					nf.Name = "#pragma";
					nf.DefaultClosed = UsePragmaFolding;
					if (peeek.Groups[2].Value==string.Empty) nf.Name = "‘…’";
					else nf.Name= string.Format("#region “{0}”",peeek.Groups[2].Value.Trim());
					Logger.LogG("GetFoldingRanges","folding - {0}, {1}",nf.StartOffset,nf.EndOffset);
					
					if (document.GetLineByOffset(nf.StartOffset)==document.GetLineByOffset(nf.EndOffset)) continue;
					else list.Add( nf );
					stack.Pop();
				}
			}
			list.Sort(SortRange);
			//			if (list.Count >0) list.Remove(list[list.Count-1]);
			return list;
		}
		static int SortRange(NewFolding a, NewFolding b)
		{
			int v = a.StartOffset.CompareTo(b.StartOffset);
			if (v==0)
			{
				return a.EndOffset.CompareTo(b.EndOffset);
			}
			return v;
		}
		#endregion

		/// <inheritdoc/>
		public override IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset)
		{
			Logger.LogM("CSharpPragmaRegionFoldingStrategy",".CreateNewFoldings(TextDocument,out int)");
			// clear existing foldings
			CommentBlockFoldingStrategy bfs = new CommentBlockFoldingStrategy();
			List<NewFolding> newFoldings = new List<NewFolding>(bfs.CreateNewFoldings(document, out firstErrorOffset));
			GetFoldingRanges(document,newFoldings);
			firstErrorOffset = lastDetectedError;
			return newFoldings;
		}
		
		public CSPragmaFoldingStrategy() : base()
		{
			UsePragmaFolding = false;
			DetectPragmaFolding = true;
			Logger.LogM("CSharpPragmaRegionFoldingStrategy",".Instance");
			//			RegisterHelper.Registerhelper();
		}
	}

}
