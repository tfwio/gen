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
	/// <summary>Cascading Style Sheet Comment Folding Strategy.
	/// <para>(Reliant upon CssParser)</para>
	/// </summary>
	public class CommentBlockFoldingStrategy/* : AbstractFoldingStrategy*/
	{
		public Dictionary<TextRange,bool> dicFoldings;
		
		static int lastDetectedError = 0;
		static readonly char[] eolChars = new char[]{'\r','n'};
		static readonly Regex PragmaRegion = new Regex(@"^[\s\t]*(///)([^\r\n]*)",RegexOptions.Multiline);
	
		#region Static Methods
		/// <summary>
		/// We havent a Match object for Comment syntax here, so we have to parse the resulting matches to text-ranges,
		/// unless something correlates with
		/// </summary>
		/// <param name="document"></param>
		/// <param name="list"></param>
		/// <returns></returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
		static public List<NewFolding> GetFoldingRanges(TextDocument document, List<NewFolding> list)
		{
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
					
					XLog.WriteG("Position: ", "{0}", peek.Position);
					
					peek.ShrinkRight(len);
					
					NewFolding nf = new NewFolding( (int)peek.Position, (int)range.EndPosition);
					
					nf.DefaultClosed = true;
					
					if (peeek.Groups[2].Value==string.Empty) nf.Name = "‘…’";
					else nf.Name= string.Format("Pragma Region “{0}”",peeek.Groups[2].Value.Trim());
	//					Global.statG();
					XLog.WriteC("folding","{0}, {1}",nf.StartOffset,nf.EndOffset);
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
		
		public override IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset)
		{
			// clear existing foldings
			CommentBlockFoldingStrategy bfs = new CommentBlockFoldingStrategy();
			List<NewFolding> newFoldings = new List<NewFolding>(bfs.CreateNewFoldings(document, out firstErrorOffset));
			newFoldings = GetFoldingRanges(document,newFoldings);
			
			firstErrorOffset = lastDetectedError;
			
			return newFoldings;
		}
	
		public CommentBlockFoldingStrategy() : base()
		{
			XLog.WriteM("PragmaFoldingStrategy",".Instance");
	//			RegisterHelper.Registerhelper();
		}
	}
}
