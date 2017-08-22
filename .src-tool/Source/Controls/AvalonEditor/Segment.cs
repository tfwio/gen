/*
 * oio * 4/24/2012 * 10:11 AM
 */
#region Using
using System;
using System.Cor3.Parsers;
using IRange = ICSharpCode.AvalonEdit.Document.ISegment;
#endregion

namespace GeneratorTool.Controls
{
	static class Extender { static public IRange GetSegment(this TextRange input) { return new Segment(input); } }
	class Segment : IRange
	{
		TextRange range;
		int IRange.Offset { get { return range.Position32; } }
		int IRange.Length { get { return range.Length32; } }
		int IRange.EndOffset { get { return (int)range.EndPosition; } }
		public Segment(TextRange range)
		{
			this.range = range;
		}
		~Segment()
		{
			this.range = default(TextRange);
		}
	}
}
